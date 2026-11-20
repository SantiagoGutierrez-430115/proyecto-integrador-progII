// ============================================================================
// Utilidades de interfaz: avisos, modales, formatos y escape de HTML
// ============================================================================
import { getApiUrl } from "./config.js";

/** Escapa texto para insertarlo en HTML sin riesgo (comentarios, nombres, etc.). */
export function esc(valor) {
  return String(valor ?? "")
    .replace(/&/g, "&amp;")
    .replace(/</g, "&lt;")
    .replace(/>/g, "&gt;")
    .replace(/"/g, "&quot;")
    .replace(/'/g, "&#39;");
}

const formatoMoneda = new Intl.NumberFormat("es-AR", { style: "currency", currency: "ARS" });
export const dinero = n => formatoMoneda.format(Number(n) || 0);

/** "2026-11-18" -> "18/11/2026" (sin pasar por Date para evitar corrimientos de zona horaria). */
export function fecha(isoFecha) {
  if (!isoFecha) return "-";
  const [a, m, d] = String(isoFecha).substring(0, 10).split("-");
  return `${d}/${m}/${a}`;
}

export function fechaHora(isoFechaHora) {
  if (!isoFechaHora) return "-";
  const d = new Date(isoFechaHora);
  return d.toLocaleString("es-AR", { dateStyle: "short", timeStyle: "short" });
}

export function hoyIso() {
  const d = new Date();
  const mm = String(d.getMonth() + 1).padStart(2, "0");
  const dd = String(d.getDate()).padStart(2, "0");
  return `${d.getFullYear()}-${mm}-${dd}`;
}

export function estrellas(n) {
  const v = Math.max(0, Math.min(5, Math.round(n)));
  return `<span class="stars" title="${v} de 5">${"★".repeat(v)}${"☆".repeat(5 - v)}</span>`;
}

// ---------- Avisos (toasts) ----------
export function toast(mensaje, tipo = "ok") {
  const cont = document.getElementById("toastContainer");
  const icono = tipo === "error" ? "bi-exclamation-octagon text-danger" : tipo === "info" ? "bi-info-circle text-info" : "bi-check-circle text-success";
  const el = document.createElement("div");
  el.className = `toast toast-${tipo} align-items-center`;
  el.setAttribute("role", "status");
  el.innerHTML = `
    <div class="d-flex">
      <div class="toast-body"><i class="bi ${icono} me-2"></i>${esc(mensaje)}</div>
      <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Cerrar"></button>
    </div>`;
  cont.appendChild(el);
  const t = new bootstrap.Toast(el, { delay: tipo === "error" ? 6000 : 3500 });
  el.addEventListener("hidden.bs.toast", () => el.remove());
  t.show();
}

// ---------- Estados de carga / vacío ----------
export function cargando(el, texto = "Cargando…") {
  el.innerHTML = `<div class="empty-state"><div class="spinner-border text-danger mb-2" role="status"></div><div>${esc(texto)}</div></div>`;
}

export function vacio(el, texto, icono = "bi-inbox") {
  el.innerHTML = `<div class="empty-state"><i class="bi ${icono}"></i>${esc(texto)}</div>`;
}

export function errorEn(el, err) {
  el.innerHTML = `<div class="alert alert-danger mb-0"><i class="bi bi-exclamation-triangle me-2"></i>${esc(err.message || err)}</div>`;
}

// ---------- Póster generado (la API no guarda imágenes de películas) ----------
function tonoDe(texto) {
  let h = 0;
  for (const c of String(texto)) h = (h * 31 + c.charCodeAt(0)) % 360;
  return h;
}

/**
 * URL completa de la portada. Puede venir:
 * - como ruta relativa de una imagen subida a la API (ej: /portadas/pelicula-8.jpg), o
 * - como URL externa completa (ej: el póster de Wikipedia cargado por Datos_portadas_trailers.sql).
 */
export function urlPortada(pelicula) {
  const url = pelicula.urlPortada;
  if (!url) return null;
  return /^https?:\/\//i.test(url) ? url : getApiUrl() + url;
}

export function posterHtml(pelicula, extraClase = "") {
  const clasif = pelicula.clasificacion ? `<span class="badge badge-red badge-clasif">${esc(pelicula.clasificacion)}</span>` : "";
  const portada = urlPortada(pelicula);
  const tono = tonoDe(pelicula.titulo);
  const inicial = (pelicula.titulo || "?").trim().charAt(0).toUpperCase();
  const fondo = `background: linear-gradient(160deg, hsl(${tono} 55% 28%), hsl(${(tono + 40) % 360} 60% 10%) 70%, #09090b);`;

  // Con portada se muestra la foto. Debajo queda el póster generado con la inicial:
  // si la imagen no carga (link roto, sin internet), se quita y se ve ese póster.
  return `
    <div class="poster ${portada ? "poster-foto" : ""} ${extraClase}" style="${fondo}">
      <span class="poster-initial">${esc(inicial)}</span>
      ${portada ? `<img src="${esc(portada)}" alt="Portada de ${esc(pelicula.titulo)}" loading="lazy" referrerpolicy="no-referrer" onerror="this.remove()" />` : ""}
      ${clasif}
      <span class="poster-title">${esc(pelicula.titulo)}</span>
    </div>`;
}

/** Id del video a partir de un link de YouTube (la API los guarda como watch?v=ID). */
export function idYoutube(url) {
  const m = /(?:v=|youtu\.be\/|embed\/|shorts\/)([A-Za-z0-9_-]{11})/.exec(url || "");
  return m ? m[1] : null;
}

// ---------- Modales ----------
export function modal(id) {
  return bootstrap.Modal.getOrCreateInstance(document.getElementById(id));
}

export function mostrarInfo(titulo, html) {
  document.getElementById("modalInfoTitulo").textContent = titulo;
  document.getElementById("modalInfoBody").innerHTML = html;
  modal("modalInfo").show();
}

/**
 * Abre el formulario genérico. onSubmit recibe (form) y puede lanzar un error:
 * en ese caso el mensaje se muestra dentro del modal y no se cierra.
 */
export function abrirFormulario({ titulo, html, textoBoton = "Guardar", onSubmit, alAbrir }) {
  const form = document.getElementById("modalFormForm");
  const error = document.getElementById("modalFormError");
  const boton = document.getElementById("modalFormSubmit");

  document.getElementById("modalFormTitulo").textContent = titulo;
  document.getElementById("modalFormBody").innerHTML = html;
  boton.textContent = textoBoton;
  error.classList.add("d-none");

  form.onsubmit = async e => {
    e.preventDefault();
    error.classList.add("d-none");
    if (!form.checkValidity()) {
      form.reportValidity();
      return;
    }
    boton.disabled = true;
    try {
      await onSubmit(form);
      modal("modalForm").hide();
    } catch (err) {
      error.textContent = err.message || String(err);
      error.classList.remove("d-none");
    } finally {
      boton.disabled = false;
    }
  };

  if (alAbrir) alAbrir(form);
  modal("modalForm").show();
}

/** Opciones <option> para un <select>. */
export function opciones(lista, { valor = "id", texto = "nombre", seleccionado, vacia } = {}) {
  let html = vacia !== undefined ? `<option value="">${esc(vacia)}</option>` : "";
  for (const item of lista) {
    const v = item[valor];
    const sel = seleccionado !== undefined && seleccionado !== null && String(seleccionado) === String(v) ? " selected" : "";
    html += `<option value="${esc(v)}"${sel}>${esc(item[texto])}</option>`;
  }
  return html;
}

/** Valor de un campo como número o null si está vacío. */
export function numOrNull(valor) {
  return valor === "" || valor === null || valor === undefined ? null : Number(valor);
}

/** Paginador simple. alCambiar(pagina) se llama al hacer clic. */
export function paginador(el, { pagina, tamanio, total }, alCambiar) {
  const paginas = Math.max(1, Math.ceil(total / tamanio));
  if (paginas <= 1) { el.innerHTML = ""; return; }
  let html = `<ul class="pagination pagination-sm mb-0">`;
  html += `<li class="page-item ${pagina <= 1 ? "disabled" : ""}"><button class="page-link" data-p="${pagina - 1}">&laquo;</button></li>`;
  for (let i = 1; i <= paginas; i++) {
    html += `<li class="page-item ${i === pagina ? "active" : ""}"><button class="page-link" data-p="${i}">${i}</button></li>`;
  }
  html += `<li class="page-item ${pagina >= paginas ? "disabled" : ""}"><button class="page-link" data-p="${pagina + 1}">&raquo;</button></li></ul>`;
  el.innerHTML = html;
  el.querySelectorAll("button[data-p]").forEach(b => b.addEventListener("click", () => alCambiar(Number(b.dataset.p))));
}
