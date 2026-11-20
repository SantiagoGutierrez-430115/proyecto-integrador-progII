// ============================================================================
// Dashboard (solo Admin): películas más vendidas, horarios más populares y
// formas de pago más usadas. Datos de api/Dashboard (procedimientos almacenados
// de Script_indicadores_dashboard.sql). Gráficos con Chart.js.
//
// Criterios de diseño: una sola serie por gráfico (sin leyenda, el título la nombra),
// barras finas con puntas redondeadas, grilla tenue, tooltip al pasar el mouse y
// una tabla con los mismos datos debajo de cada gráfico.
// ============================================================================
import { api } from "./api.js";
import { esc, dinero, toast, hoyIso } from "./ui.js";

const COLOR_BARRA = "#e50914";        // carmesí del sitio
const COLOR_BARRA_HOVER = "#ff1e29";
const COLOR_TEXTO = "#e4e4e7";        // tinta principal
const COLOR_TEXTO_SUAVE = "#a1a1aa";  // ejes y etiquetas secundarias
const COLOR_GRILLA = "rgba(255, 255, 255, 0.07)";

const graficos = {};
let iniciado = false;

const entero = n => new Intl.NumberFormat("es-AR").format(n);

export function initDashboard() {
  if (iniciado) return;
  iniciado = true;

  if (window.Chart) {
    Chart.defaults.font.family = "'Plus Jakarta Sans', system-ui, sans-serif";
    Chart.defaults.color = COLOR_TEXTO_SUAVE;
  }

  const form = document.getElementById("filtroDashboard");
  form.querySelector("[name=hasta]").max = hoyIso();
  form.addEventListener("submit", e => {
    e.preventDefault();
    cargarDashboard();
  });
  form.addEventListener("reset", () => setTimeout(cargarDashboard, 0));

  // Atajos de período
  document.querySelectorAll("[data-rango]").forEach(b => b.addEventListener("click", () => {
    const dias = Number(b.dataset.rango);
    form.querySelector("[name=hasta]").value = dias ? hoyIso() : "";
    form.querySelector("[name=desde]").value = dias ? restarDias(dias - 1) : "";
    cargarDashboard();
  }));
}

function restarDias(dias) {
  const d = new Date();
  d.setDate(d.getDate() - dias);
  return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, "0")}-${String(d.getDate()).padStart(2, "0")}`;
}

export async function cargarDashboard() {
  const fd = new FormData(document.getElementById("filtroDashboard"));
  const filtro = { desde: fd.get("desde"), hasta: fd.get("hasta") };
  const top = Number(fd.get("top")) || 10;

  const avisos = document.getElementById("dashboardAviso");
  avisos.classList.add("d-none");

  try {
    const [peliculas, horarios, pagos] = await Promise.all([
      api("/api/Dashboard/peliculas-mas-vendidas", { query: { ...filtro, top } }),
      api("/api/Dashboard/horarios-mas-populares", { query: filtro }),
      api("/api/Dashboard/formas-pago-mas-usadas", { query: filtro })
    ]);

    dibujarKpis(horarios, pagos);
    dibujarPeliculas(peliculas);
    dibujarHorarios(horarios);
    dibujarPagos(pagos);

    if (peliculas.length === 0 && horarios.length === 0 && pagos.length === 0) {
      avisos.textContent = "No hay ventas pagadas en el período elegido.";
      avisos.classList.remove("d-none");
    }
  } catch (err) {
    avisos.textContent = err.message;
    avisos.classList.remove("d-none");
    toast(err.message, "error");
  }
}

// ---------- Indicadores principales ----------
function dibujarKpis(horarios, pagos) {
  const entradas = horarios.reduce((s, h) => s + h.entradasVendidas, 0);
  const recaudacion = horarios.reduce((s, h) => s + h.recaudacion, 0);
  const cantidadPagos = pagos.reduce((s, p) => s + p.cantidadPagos, 0);
  const masPopular = [...horarios].sort((a, b) => b.entradasVendidas - a.entradasVendidas)[0];

  const kpis = [
    { titulo: "Entradas vendidas", valor: entero(entradas), icono: "bi-ticket-perforated" },
    { titulo: "Recaudación por entradas", valor: dinero(recaudacion), icono: "bi-cash-stack" },
    { titulo: "Precio promedio", valor: entradas ? dinero(recaudacion / entradas) : "-", icono: "bi-tag" },
    { titulo: "Horario más elegido", valor: masPopular ? `${masPopular.horario} hs` : "-", icono: "bi-clock", sub: cantidadPagos ? `${entero(cantidadPagos)} pagos registrados` : "" }
  ];

  document.getElementById("dashboardKpis").innerHTML = kpis.map(k => `
    <div class="col-6 col-lg-3">
      <div class="kpi-card h-100">
        <div class="kpi-titulo"><i class="bi ${k.icono} me-1"></i>${esc(k.titulo)}</div>
        <div class="kpi-valor">${esc(k.valor)}</div>
        ${k.sub ? `<div class="kpi-sub">${esc(k.sub)}</div>` : ""}
      </div>
    </div>`).join("");
}

// ---------- Gráfico de barras genérico (una serie) ----------
function grafico(id, { etiquetas, valores, horizontal, formatoEje, tooltip }) {
  graficos[id]?.destroy();
  const canvas = document.getElementById(id);
  if (!window.Chart) {
    canvas.replaceWith(Object.assign(document.createElement("p"), {
      className: "small text-muted-silver", textContent: "No se pudo cargar Chart.js: revisá la tabla de abajo."
    }));
    return;
  }

  const ejeValor = {
    beginAtZero: true,
    grid: { color: COLOR_GRILLA, drawTicks: false },
    border: { display: false },
    ticks: { color: COLOR_TEXTO_SUAVE, padding: 6, callback: v => formatoEje(v) }
  };
  const ejeCategoria = {
    grid: { display: false },
    border: { color: "rgba(255,255,255,0.15)" },
    ticks: { color: COLOR_TEXTO, autoSkip: false }
  };

  graficos[id] = new Chart(canvas, {
    type: "bar",
    data: {
      labels: etiquetas,
      datasets: [{
        data: valores,
        backgroundColor: COLOR_BARRA,
        hoverBackgroundColor: COLOR_BARRA_HOVER,
        borderRadius: 4,
        borderSkipped: "start",   // punta redondeada solo en el extremo, base recta sobre el eje
        maxBarThickness: horizontal ? 22 : 56,
        categoryPercentage: 0.75
      }]
    },
    options: {
      indexAxis: horizontal ? "y" : "x",
      maintainAspectRatio: false,
      animation: { duration: 400 },
      plugins: {
        legend: { display: false },
        tooltip: {
          backgroundColor: "rgba(18, 18, 24, 0.96)",
          borderColor: "rgba(255,255,255,0.12)",
          borderWidth: 1,
          titleColor: COLOR_TEXTO,
          bodyColor: COLOR_TEXTO,
          padding: 10,
          displayColors: false,
          callbacks: { label: ctx => tooltip(ctx.dataIndex) }
        }
      },
      scales: horizontal ? { x: ejeValor, y: ejeCategoria } : { x: ejeCategoria, y: ejeValor }
    }
  });
}

function tabla(id, columnas, filas) {
  document.getElementById(id).innerHTML = filas.length === 0
    ? `<p class="small text-muted-silver mb-0">Sin datos.</p>`
    : `<div class="table-responsive"><table class="table table-cinema table-sm">
        <thead><tr>${columnas.map(c => `<th class="${c.derecha ? "text-end" : ""}">${esc(c.titulo)}</th>`).join("")}</tr></thead>
        <tbody>${filas.map(f => `<tr>${columnas.map(c => `<td class="${c.derecha ? "text-end" : ""}">${esc(c.valor(f))}</td>`).join("")}</tr>`).join("")}</tbody>
      </table></div>`;
}

function ajustarAlto(id, cantidad, porBarra = 34, minimo = 200) {
  document.getElementById(id).parentElement.style.height = `${Math.max(minimo, cantidad * porBarra + 40)}px`;
}

// ---------- Películas más vendidas ----------
function dibujarPeliculas(lista) {
  ajustarAlto("chartPeliculas", lista.length);
  grafico("chartPeliculas", {
    etiquetas: lista.map(p => p.titulo.length > 28 ? p.titulo.slice(0, 27) + "…" : p.titulo),
    valores: lista.map(p => p.entradasVendidas),
    horizontal: true,
    formatoEje: v => entero(v),
    tooltip: i => [`${entero(lista[i].entradasVendidas)} entradas`, `Recaudación: ${dinero(lista[i].recaudacion)}`]
  });
  tabla("tablaPeliculas", [
    { titulo: "#", valor: f => lista.indexOf(f) + 1 },
    { titulo: "Película", valor: f => f.titulo },
    { titulo: "Entradas", valor: f => entero(f.entradasVendidas), derecha: true },
    { titulo: "Recaudación", valor: f => dinero(f.recaudacion), derecha: true }
  ], lista);
}

// ---------- Horarios más populares (en orden de horario, no de ranking) ----------
function dibujarHorarios(lista) {
  const porHora = [...lista].sort((a, b) => a.horario.localeCompare(b.horario));
  grafico("chartHorarios", {
    etiquetas: porHora.map(h => `${h.horario} hs`),
    valores: porHora.map(h => h.entradasVendidas),
    horizontal: false,
    formatoEje: v => entero(v),
    tooltip: i => [
      `${entero(porHora[i].entradasVendidas)} entradas`,
      `${entero(porHora[i].funcionesConVentas)} funciones con ventas`,
      `Recaudación: ${dinero(porHora[i].recaudacion)}`
    ]
  });
  tabla("tablaHorarios", [
    { titulo: "Horario", valor: f => `${f.horario} hs` },
    { titulo: "Entradas", valor: f => entero(f.entradasVendidas), derecha: true },
    { titulo: "Funciones", valor: f => entero(f.funcionesConVentas), derecha: true },
    { titulo: "Recaudación", valor: f => dinero(f.recaudacion), derecha: true }
  ], lista);
}

// ---------- Formas de pago más usadas ----------
function dibujarPagos(lista) {
  ajustarAlto("chartPagos", lista.length, 40, 180);
  grafico("chartPagos", {
    etiquetas: lista.map(p => p.formaPago),
    valores: lista.map(p => p.porcentaje),
    horizontal: true,
    formatoEje: v => `${v}%`,
    tooltip: i => [`${lista[i].porcentaje}% de los pagos`, `${entero(lista[i].cantidadPagos)} pagos`, `Total: ${dinero(lista[i].montoTotal)}`]
  });
  tabla("tablaPagos", [
    { titulo: "Forma de pago", valor: f => f.formaPago },
    { titulo: "Pagos", valor: f => entero(f.cantidadPagos), derecha: true },
    { titulo: "%", valor: f => `${f.porcentaje}%`, derecha: true },
    { titulo: "Monto", valor: f => dinero(f.montoTotal), derecha: true }
  ], lista);
}
