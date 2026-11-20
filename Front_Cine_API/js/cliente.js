// ============================================================================
// Sección de clientes: cartelera, funciones, butacas, compra de entradas,
// reseñas y candy bar.
// ============================================================================
import { api } from "./api.js";
import { nombreSala, horaDeHorario } from "./config.js";
import {
  esc, dinero, fecha, fechaHora, estrellas, toast, cargando, vacio, errorEn,
  posterHtml, modal, opciones, paginador, idYoutube
} from "./ui.js";

// Pide a main.js que muestre la pantalla de login (lo usa el invitado al querer comprar o reseñar)
function pedirLogin() {
  document.querySelectorAll(".modal.show").forEach(m => bootstrap.Modal.getInstance(m)?.hide());
  window.dispatchEvent(new CustomEvent("cine:pedir-login"));
}

let sesion = null;
// Películas activas por id: sirven para mostrar el título en las funciones
// (FuncionDto solo trae el IdPelicula).
const peliculasPorId = new Map();

export function initCliente(sesionActual) {
  sesion = sesionActual;
}

// ----------------------------------------------------------------------------
// Datos compartidos
// ----------------------------------------------------------------------------
export async function cargarPeliculasActivas() {
  // La API pagina de a 100 como máximo
  peliculasPorId.clear();
  let pagina = 1;
  while (true) {
    const r = await api("/api/Pelicula", { query: { pagina, tamanio: 100 } });
    r.items.forEach(p => peliculasPorId.set(p.idPelicula, p));
    if (pagina * r.tamanio >= r.total) break;
    pagina++;
  }
  const lista = [...peliculasPorId.values()].sort((a, b) => a.titulo.localeCompare(b.titulo));
  document.getElementById("filtroFuncionPelicula").innerHTML =
    opciones(lista, { valor: "idPelicula", texto: "titulo", vacia: "Todas" });
  return lista;
}

export function tituloPelicula(idPelicula) {
  return peliculasPorId.get(idPelicula)?.titulo ?? `Película #${idPelicula}`;
}

/** Fecha y hora de inicio de una función como Date local. */
function inicioFuncion(f) {
  return new Date(`${String(f.fecha).substring(0, 10)}T${horaDeHorario(f.idHorario)}:00`);
}

function ordenarFunciones(lista) {
  return lista.sort((a, b) => inicioFuncion(a) - inicioFuncion(b));
}

// ----------------------------------------------------------------------------
// CARTELERA
// ----------------------------------------------------------------------------
let filtrosCartelera = {};

export async function initCartelera() {
  try {
    const [generos, clasificaciones] = await Promise.all([
      api("/api/Catalogo/generos"),
      api("/api/Catalogo/clasificaciones")
    ]);
    document.getElementById("filtroGenero").innerHTML = opciones(generos, { vacia: "Todos los géneros" });
    document.getElementById("filtroClasificacion").innerHTML = opciones(clasificaciones, { vacia: "Toda clasificación" });
  } catch (err) {
    toast(err.message, "error");
  }

  document.getElementById("filtroCartelera").addEventListener("submit", e => {
    e.preventDefault();
    const fd = new FormData(e.target);
    filtrosCartelera = {
      titulo: fd.get("titulo").trim(),
      idGenero: fd.get("idGenero"),
      idClasificacion: fd.get("idClasificacion")
    };
    cargarCartelera(1);
  });

  await cargarCartelera(1);
}

async function cargarCartelera(pagina) {
  const grid = document.getElementById("gridPeliculas");
  cargando(grid, "Cargando cartelera…");
  try {
    const r = await api("/api/Pelicula", { query: { ...filtrosCartelera, pagina, tamanio: 12 } });
    if (r.items.length === 0) {
      vacio(grid, "No hay películas que coincidan con la búsqueda.", "bi-film");
    } else {
      grid.innerHTML = r.items.map(p => `
        <div class="col-6 col-md-4 col-lg-3 animate-fade-up">
          <div class="movie-card">
            ${posterHtml(p)}
            <div class="p-3 d-flex flex-column flex-grow-1">
              <div class="mb-2">
                ${p.formato ? `<span class="chip">${esc(p.formato)}</span>` : ""}
                ${p.generos.slice(0, 2).map(g => `<span class="chip">${esc(g.nombre)}</span>`).join("")}
                ${p.urlTrailer ? `<span class="chip chip-trailer"><i class="bi bi-play-fill"></i> Tráiler</span>` : ""}
              </div>
              <div class="small text-muted-silver mb-3">${p.director ? `Dir. ${esc(p.director)}` : "&nbsp;"}</div>
              <div class="mt-auto d-flex gap-2">
                <button class="btn btn-ghost btn-sm flex-fill" data-detalle="${p.idPelicula}"><i class="bi bi-info-circle"></i> Detalle</button>
                <button class="btn btn-crimson btn-sm flex-fill" data-funciones="${p.idPelicula}"><i class="bi bi-ticket-perforated"></i> Funciones</button>
              </div>
            </div>
          </div>
        </div>`).join("");

      grid.querySelectorAll("[data-detalle]").forEach(b =>
        b.addEventListener("click", () => abrirDetallePelicula(Number(b.dataset.detalle))));
      grid.querySelectorAll("[data-funciones]").forEach(b =>
        b.addEventListener("click", () => abrirDetallePelicula(Number(b.dataset.funciones), true)));
    }
    paginador(document.getElementById("paginacionCartelera"), r, cargarCartelera);
  } catch (err) {
    errorEn(grid, err);
  }
}

// ----------------------------------------------------------------------------
// DETALLE DE PELÍCULA (info + funciones + reseñas)
// ----------------------------------------------------------------------------
export async function abrirDetallePelicula(idPelicula, irAFunciones = false) {
  const body = document.getElementById("modalPeliculaBody");
  document.getElementById("modalPeliculaTitulo").textContent = "";
  cargando(body);
  modal("modalPelicula").show();

  try {
    const [p, funciones, resenias] = await Promise.all([
      api(`/api/Pelicula/${idPelicula}`),
      api(`/api/Funcion/pelicula/${idPelicula}`),
      api(`/api/Resenia/pelicula/${idPelicula}`)
    ]);

    document.getElementById("modalPeliculaTitulo").textContent = p.titulo;
    const proximas = ordenarFunciones(funciones.filter(f => inicioFuncion(f) > new Date()));
    const promedio = resenias.length ? resenias.reduce((s, r) => s + r.puntuacion, 0) / resenias.length : 0;
    const idVideo = idYoutube(p.urlTrailer);

    // Formulario de reseña: solo clientes. El invitado ve una invitación a iniciar sesión.
    let bloqueResenia = "";
    if (sesion.esInvitado) {
      bloqueResenia = `
        <div class="summary-box d-flex flex-wrap align-items-center justify-content-between gap-2">
          <span class="small text-muted-silver"><i class="bi bi-lock me-1"></i>Iniciá sesión con tu cuenta para dejar una reseña.</span>
          <button class="btn btn-crimson btn-sm" data-pedir-login>Iniciar sesión</button>
        </div>`;
    } else if (!sesion.esAdmin) {
      bloqueResenia = `
          <form id="formResenia" class="summary-box">
            <div class="fw-semibold small mb-2">Dejá tu reseña</div>
            <div class="row g-2">
              <div class="col-sm-3">
                <select name="puntuacion" class="form-select form-select-cinema" required>
                  <option value="5">★★★★★</option><option value="4">★★★★</option><option value="3">★★★</option><option value="2">★★</option><option value="1">★</option>
                </select>
              </div>
              <div class="col-sm-7"><input name="comentario" class="form-control form-control-cinema" maxlength="500" placeholder="¿Qué te pareció?" /></div>
              <div class="col-sm-2"><button class="btn btn-crimson w-100">Enviar</button></div>
            </div>
          </form>`;
    }

    body.innerHTML = `
      ${idVideo ? `
        <div class="trailer-box mb-4">
          <div class="ratio ratio-16x9">
            <iframe src="https://www.youtube-nocookie.com/embed/${esc(idVideo)}" title="Tráiler de ${esc(p.titulo)}"
              allow="accelerometer; encrypted-media; gyroscope; picture-in-picture; fullscreen" allowfullscreen loading="lazy"></iframe>
          </div>
          <div class="text-end mt-2">
            <a class="btn btn-ghost btn-sm" href="https://www.youtube.com/watch?v=${esc(idVideo)}" target="_blank" rel="noopener noreferrer">
              <i class="bi bi-youtube text-danger me-1"></i>Ver en YouTube
            </a>
          </div>
        </div>` : ""}
      <div class="row g-4">
        <div class="col-md-4 col-lg-3">${posterHtml(p, "poster-sm")}</div>
        <div class="col-md-8 col-lg-9">
          <div class="mb-2">
            ${p.clasificacion ? `<span class="badge badge-red me-1">${esc(p.clasificacion)}</span>` : ""}
            ${p.formato ? `<span class="chip">${esc(p.formato)}</span>` : ""}
            ${p.generos.map(g => `<span class="chip">${esc(g.nombre)}</span>`).join("")}
          </div>
          <p class="text-white-50">${esc(p.descripcion || "Sin descripción.")}</p>
          <dl class="row small mb-0">
            <dt class="col-sm-3 text-muted-silver">Estreno</dt><dd class="col-sm-9">${fecha(p.fechaLanzamiento)}</dd>
            <dt class="col-sm-3 text-muted-silver">Director</dt><dd class="col-sm-9">${esc(p.director || "-")}</dd>
            <dt class="col-sm-3 text-muted-silver">Elenco</dt><dd class="col-sm-9">${esc(p.actores.map(a => a.nombre).join(", ") || "-")}</dd>
            <dt class="col-sm-3 text-muted-silver">Idiomas</dt><dd class="col-sm-9">${esc(p.idiomas.map(i => i.nombre).join(", ") || "-")}</dd>
            <dt class="col-sm-3 text-muted-silver">País</dt><dd class="col-sm-9">${esc(p.pais || "-")}</dd>
            <dt class="col-sm-3 text-muted-silver">Distribuidor</dt><dd class="col-sm-9">${esc(p.distribuidor || "-")}</dd>
          </dl>

          <h6 class="fw-bold mt-4" id="detalleFunciones"><i class="bi bi-calendar-event text-danger me-1"></i>Próximas funciones</h6>
          ${proximas.length === 0
            ? `<p class="text-muted-silver small">No hay funciones programadas para esta película.</p>`
            : `<div class="d-flex flex-wrap gap-2">${proximas.map(f => `
                <button class="btn btn-ghost btn-sm" data-funcion="${f.idFuncion}">
                  <i class="bi bi-ticket-perforated text-danger"></i> ${fecha(f.fecha)} · ${horaDeHorario(f.idHorario)} · ${esc(nombreSala(f.idSala))}
                </button>`).join("")}</div>`}

          <h6 class="fw-bold mt-4"><i class="bi bi-chat-quote text-danger me-1"></i>Reseñas
            ${resenias.length ? `<span class="ms-2">${estrellas(promedio)}</span> <small class="text-muted-silver">${promedio.toFixed(1)} (${resenias.length})</small>` : ""}
          </h6>
          <div class="mb-3">
            ${resenias.length === 0 ? `<p class="text-muted-silver small">Todavía no hay reseñas.</p>` :
              resenias.map(r => `
                <div class="summary-box mb-2 py-2">
                  <div class="d-flex justify-content-between">${estrellas(r.puntuacion)}<small class="text-muted-silver">${fechaHora(r.fechaResenia)}</small></div>
                  <div class="small mt-1">${esc(r.comentario || "")}</div>
                </div>`).join("")}
          </div>

          ${bloqueResenia}
        </div>
      </div>`;

    body.querySelector("[data-pedir-login]")?.addEventListener("click", pedirLogin);

    body.querySelectorAll("[data-funcion]").forEach(b => b.addEventListener("click", () => {
      const f = proximas.find(x => x.idFuncion === Number(b.dataset.funcion));
      modal("modalPelicula").hide();
      abrirButacas(f);
    }));

    const formResenia = document.getElementById("formResenia");
    if (formResenia) {
      formResenia.addEventListener("submit", async e => {
        e.preventDefault();
        const fd = new FormData(formResenia);
        try {
          await api("/api/Resenia", {
            method: "POST",
            body: { idPelicula, puntuacion: Number(fd.get("puntuacion")), comentario: fd.get("comentario").trim() || null }
          });
          toast("¡Gracias por tu reseña!");
          abrirDetallePelicula(idPelicula);
        } catch (err) {
          toast(err.message, "error");
        }
      });
    }

    if (irAFunciones) document.getElementById("detalleFunciones")?.scrollIntoView({ behavior: "smooth" });
  } catch (err) {
    errorEn(body, err);
  }
}

// ----------------------------------------------------------------------------
// FUNCIONES
// ----------------------------------------------------------------------------
export function initFunciones() {
  const form = document.getElementById("filtroFunciones");
  form.addEventListener("submit", e => {
    e.preventDefault();
    buscarFunciones();
  });
  form.addEventListener("reset", () => setTimeout(buscarFunciones, 0));
}

export async function buscarFunciones() {
  const cont = document.getElementById("listaFunciones");
  const fd = new FormData(document.getElementById("filtroFunciones"));
  const fechaFiltro = fd.get("fecha");
  const idPelicula = fd.get("idPelicula") ? Number(fd.get("idPelicula")) : null;
  const soloProximas = fd.get("soloProximas") === "on";

  cargando(cont, "Buscando funciones…");
  try {
    let funciones;
    if (fechaFiltro) funciones = await api("/api/Funcion/fecha", { query: { fecha: fechaFiltro } });
    else if (idPelicula) funciones = await api(`/api/Funcion/pelicula/${idPelicula}`);
    else funciones = await api("/api/Funcion");

    if (idPelicula) funciones = funciones.filter(f => f.idPelicula === idPelicula);
    if (soloProximas) funciones = funciones.filter(f => inicioFuncion(f) > new Date());
    // Solo se muestran funciones de películas en cartel (activas)
    funciones = ordenarFunciones(funciones.filter(f => peliculasPorId.has(f.idPelicula)));

    if (funciones.length === 0) {
      vacio(cont, "No se encontraron funciones con esos filtros.", "bi-calendar-x");
      return;
    }

    cont.innerHTML = `
      <div class="glass-panel p-2 table-responsive">
        <table class="table table-cinema">
          <thead><tr><th>Película</th><th>Fecha</th><th>Hora</th><th>Sala</th><th></th></tr></thead>
          <tbody>
            ${funciones.map(f => `
              <tr>
                <td class="fw-semibold">${esc(tituloPelicula(f.idPelicula))}</td>
                <td>${fecha(f.fecha)}</td>
                <td>${horaDeHorario(f.idHorario)}</td>
                <td>${esc(nombreSala(f.idSala))}</td>
                <td class="text-end"><button class="btn btn-crimson btn-sm" data-funcion="${f.idFuncion}"><i class="bi bi-grid-3x3-gap me-1"></i>Butacas</button></td>
              </tr>`).join("")}
          </tbody>
        </table>
      </div>`;

    cont.querySelectorAll("[data-funcion]").forEach(b => b.addEventListener("click", () =>
      abrirButacas(funciones.find(f => f.idFuncion === Number(b.dataset.funcion)))));
  } catch (err) {
    errorEn(cont, err);
  }
}

// ----------------------------------------------------------------------------
// BUTACAS Y COMPRA
// ----------------------------------------------------------------------------
const BUTACAS_POR_FILA = 10;

/**
 * Dibuja el mapa de butacas.
 * butacas: [{ idButaca, nroButaca, disponible, premium }]
 */
export function mapaButacasHtml(butacas, seleccionadas = new Set(), soloLectura = false) {
  const ordenadas = [...butacas].sort((a, b) => a.nroButaca - b.nroButaca);
  let filas = "";
  for (let i = 0; i < ordenadas.length; i += BUTACAS_POR_FILA) {
    const letra = String.fromCharCode(65 + Math.floor(i / BUTACAS_POR_FILA));
    const fila = ordenadas.slice(i, i + BUTACAS_POR_FILA).map(b => {
      const clases = ["seat"];
      if (b.premium) clases.push("premium");
      if (!b.disponible) clases.push("occupied");
      if (seleccionadas.has(b.idButaca)) clases.push("selected");
      if (soloLectura) clases.push("readonly");
      const titulo = `Butaca ${b.nroButaca}${b.precio !== undefined ? " · " + dinero(b.precio) : ""}${b.disponible ? "" : " · ocupada"}`;
      return `<button type="button" class="${clases.join(" ")}" data-butaca="${b.idButaca}" title="${esc(titulo)}" ${!b.disponible || soloLectura ? "disabled" : ""}>${b.nroButaca}</button>`;
    }).join("");
    filas += `<div class="seat-row"><span class="row-label">${letra}</span>${fila}</div>`;
  }
  return `
    <div class="screen"></div><div class="screen-label">PANTALLA</div>
    <div class="seat-map"><div class="seat-grid">${filas}</div></div>
    <div class="seat-legend mt-3">
      <span><button class="seat" tabindex="-1"></button> Disponible</span>
      <span><button class="seat premium" tabindex="-1"></button> Preferencial</span>
      ${soloLectura ? "" : `<span><button class="seat selected" tabindex="-1"></button> Seleccionada</span>`}
      <span><button class="seat occupied" tabindex="-1"></button> Ocupada / fuera de servicio</span>
    </div>`;
}

export async function abrirButacas(funcion) {
  const body = document.getElementById("modalButacasBody");
  document.getElementById("modalButacasTitulo").textContent = tituloPelicula(funcion.idPelicula);
  document.getElementById("modalButacasSub").textContent =
    `${fecha(funcion.fecha)} · ${horaDeHorario(funcion.idHorario)} hs · ${nombreSala(funcion.idSala)}`;
  cargando(body, "Cargando butacas…");
  modal("modalButacas").show();
  await dibujarSeleccion(funcion);
}

async function dibujarSeleccion(funcion, compraRealizada = null) {
  const body = document.getElementById("modalButacasBody");
  let butacas;
  try {
    butacas = await api(`/api/funciones/${funcion.idFuncion}/butacas`);
  } catch (err) {
    errorEn(body, err);
    return;
  }

  if (butacas.length === 0) {
    vacio(body, "La sala de esta función todavía no tiene butacas cargadas.", "bi-grid-3x3-gap");
    return;
  }

  // La API no informa el tipo de butaca: las que cuestan más que la tarifa base son preferenciales
  const precioBase = Math.min(...butacas.map(b => b.precio));
  butacas.forEach(b => { b.premium = b.precio > precioBase; });

  const empezo = inicioFuncion(funcion) <= new Date();
  const puedeComprar = !sesion.esAdmin && !sesion.esInvitado && !empezo;
  const seleccion = new Set();
  const disponibles = butacas.filter(b => b.disponible).length;

  body.innerHTML = `
    ${compraRealizada ? `
      <div class="alert alert-success d-flex align-items-center gap-3">
        <i class="bi bi-check-circle-fill fs-3"></i>
        <div><div class="fw-bold">¡Compra realizada!</div>
        Factura Nº <strong>${esc(compraRealizada.nroFactura)}</strong> · Total ${dinero(compraRealizada.total)} · Butacas ${esc(compraRealizada.butacas.join(", "))}</div>
      </div>` : ""}
    <div class="row g-4">
      <div class="col-lg-8">
        <div class="glass-panel p-3" id="contenedorMapa"></div>
      </div>
      <div class="col-lg-4">
        <div class="summary-box">
          <div class="d-flex justify-content-between small text-muted-silver mb-2"><span>Disponibles</span><span>${disponibles} de ${butacas.length}</span></div>
          <div class="d-flex justify-content-between small text-muted-silver mb-3"><span>Precio desde</span><span>${dinero(precioBase)}</span></div>
          <div class="fw-semibold small mb-1">Butacas elegidas</div>
          <div id="resumenButacas" class="small text-muted-silver mb-3">Ninguna</div>
          <div class="d-flex justify-content-between align-items-end mb-3">
            <span class="text-muted-silver">Total</span><span class="total-amount" id="totalCompra">${dinero(0)}</span>
          </div>
          ${sesion.esInvitado
            ? `<button class="btn btn-crimson w-100 py-2" data-pedir-login><i class="bi bi-box-arrow-in-right me-1"></i>Iniciá sesión para comprar</button>
               <p class="small text-muted-silver mt-2 mb-0">Como invitado podés ver la disponibilidad y los precios, pero para comprar entradas necesitás una cuenta.</p>`
            : `<button id="btnComprar" class="btn btn-crimson w-100 py-2" disabled><i class="bi bi-credit-card me-1"></i>Comprar entradas</button>`}
          ${sesion.esAdmin ? `<p class="small text-muted-silver mt-2 mb-0">Los administradores no tienen un cliente asociado, por eso no pueden comprar. Ingresá con una cuenta de cliente.</p>` : ""}
          ${empezo ? `<p class="small text-danger mt-2 mb-0">Esta función ya empezó; no se pueden comprar entradas.</p>` : ""}
          <p class="small text-muted-silver mt-2 mb-0"><i class="bi bi-info-circle"></i> El pago se registra como transferencia.</p>
        </div>
      </div>
    </div>`;

  const contMapa = document.getElementById("contenedorMapa");
  const resumen = document.getElementById("resumenButacas");
  const total = document.getElementById("totalCompra");
  const btnComprar = document.getElementById("btnComprar");
  body.querySelector("[data-pedir-login]")?.addEventListener("click", pedirLogin);

  const redibujar = () => {
    contMapa.innerHTML = mapaButacasHtml(butacas, seleccion, !puedeComprar);
    contMapa.querySelectorAll(".seat-map [data-butaca]").forEach(b => b.addEventListener("click", () => {
      const id = Number(b.dataset.butaca);
      seleccion.has(id) ? seleccion.delete(id) : seleccion.add(id);
      redibujar();
    }));
    const elegidas = butacas.filter(b => seleccion.has(b.idButaca));
    resumen.innerHTML = elegidas.length
      ? elegidas.map(b => `<span class="chip">Nº ${b.nroButaca} · ${dinero(b.precio)}</span>`).join("")
      : "Ninguna";
    total.textContent = dinero(elegidas.reduce((s, b) => s + b.precio, 0));
    if (btnComprar) btnComprar.disabled = !puedeComprar || elegidas.length === 0;
  };
  redibujar();

  btnComprar?.addEventListener("click", async () => {
    const elegidas = butacas.filter(b => seleccion.has(b.idButaca));
    const totalTexto = dinero(elegidas.reduce((s, b) => s + b.precio, 0));
    if (!confirm(`¿Confirmás la compra de ${elegidas.length} entrada(s) por ${totalTexto}?`)) return;

    btnComprar.disabled = true;
    try {
      const r = await api("/api/compras", {
        method: "POST",
        body: { idFuncion: funcion.idFuncion, idsButacas: elegidas.map(b => b.idButaca) }
      });
      toast(`Compra registrada. Factura Nº ${r.nroFactura}.`);
      await dibujarSeleccion(funcion, { ...r, butacas: elegidas.map(b => b.nroButaca) });
    } catch (err) {
      toast(err.message, "error");
      // Si alguien compró antes, se refresca el mapa para mostrar el estado real
      if (err.status === 409) await dibujarSeleccion(funcion);
      else btnComprar.disabled = false;
    }
  });
}

// ----------------------------------------------------------------------------
// CANDY BAR (solo lectura para clientes)
// ----------------------------------------------------------------------------
export async function cargarShop() {
  const gCombos = document.getElementById("gridCombos");
  const gProductos = document.getElementById("gridProductos");
  cargando(gCombos);
  cargando(gProductos);

  try {
    const combos = await api("/api/Combo", { query: { tamanio: 100 } });
    gCombos.innerHTML = combos.items.length === 0
      ? `<div class="col-12">${vacioHtml("No hay combos cargados.")}</div>`
      : combos.items.map(c => `
        <div class="col-md-6 col-lg-4">
          <div class="glass-panel p-3 h-100">
            <div class="d-flex justify-content-between align-items-start mb-2">
              <h6 class="fw-bold mb-0">${esc(c.descripcion)}</h6>
              <span class="badge badge-red">${esc(c.tipoCombo || "")}</span>
            </div>
            <ul class="small text-muted-silver ps-3 mb-3">${c.productos.map(p => `<li>${p.cantidad} × ${esc(p.producto)}</li>`).join("")}</ul>
            <div class="d-flex justify-content-between align-items-end">
              ${c.sumaProductos > c.precio ? `<small class="text-success">Ahorrás ${dinero(c.sumaProductos - c.precio)}</small>` : "<span></span>"}
              <span class="fs-5 fw-800">${dinero(c.precio)}</span>
            </div>
          </div>
        </div>`).join("");
  } catch (err) {
    errorEn(gCombos, err);
  }

  try {
    const productos = await api("/api/ProductoShop", { query: { tamanio: 100 } });
    gProductos.innerHTML = productos.items.length === 0
      ? `<div class="col-12">${vacioHtml("No hay productos cargados.")}</div>`
      : productos.items.map(p => `
        <div class="col-6 col-md-4 col-lg-3">
          <div class="glass-panel p-3 h-100 d-flex flex-column">
            <span class="chip align-self-start">${esc(p.tipoProducto || "")}</span>
            <div class="fw-semibold mt-1">${esc(p.nombre)}</div>
            <small class="text-muted-silver">${esc(p.marca || "")}</small>
            <div class="mt-auto pt-2 d-flex justify-content-between align-items-end">
              <small class="${p.stock > 0 ? "text-muted-silver" : "text-danger"}">${p.stock > 0 ? `Stock: ${p.stock}` : "Sin stock"}</small>
              <span class="fw-800">${dinero(p.precio)}</span>
            </div>
          </div>
        </div>`).join("");
  } catch (err) {
    errorEn(gProductos, err);
  }
}

function vacioHtml(texto) {
  return `<div class="empty-state"><i class="bi bi-inbox"></i>${esc(texto)}</div>`;
}
