// ============================================================================
// Cliente HTTP: todas las llamadas a la API pasan por acá.
// - Agrega el token JWT en el header Authorization.
// - Convierte cualquier respuesta de error de la API en un mensaje legible
//   (texto plano, { mensaje }, o ProblemDetails de validación de ASP.NET).
// ============================================================================
import { getApiUrl } from "./config.js";

const CLAVE_TOKEN = "cine_token";
let alPerderSesion = null;

export class ApiError extends Error {
  constructor(mensaje, status, datos) {
    super(mensaje);
    this.status = status;
    this.datos = datos;
  }
}

export function getToken() {
  try { return localStorage.getItem(CLAVE_TOKEN); } catch { return null; }
}

export function setToken(token) {
  try { localStorage.setItem(CLAVE_TOKEN, token); } catch { /* sin almacenamiento */ }
}

export function clearToken() {
  try { localStorage.removeItem(CLAVE_TOKEN); } catch { /* sin almacenamiento */ }
}

// Se llama cuando la API responde 401 con una sesión iniciada (token vencido o inválido)
export function setAlPerderSesion(fn) {
  alPerderSesion = fn;
}

/**
 * Hace una petición a la API.
 * @param {string} endpoint  ej: "/api/Pelicula"
 * @param {{method?: string, body?: any, query?: object}} opciones
 */
export async function api(endpoint, { method = "GET", body, query } = {}) {
  let url = getApiUrl() + endpoint;

  if (query) {
    const params = new URLSearchParams();
    for (const [clave, valor] of Object.entries(query)) {
      if (valor !== undefined && valor !== null && valor !== "") params.append(clave, valor);
    }
    const qs = params.toString();
    if (qs) url += "?" + qs;
  }

  // FormData (subida de archivos) se manda tal cual: el navegador arma el multipart/form-data
  const esFormData = body instanceof FormData;
  const headers = {};
  if (body !== undefined && !esFormData) headers["Content-Type"] = "application/json";
  const token = getToken();
  if (token) headers["Authorization"] = `Bearer ${token}`;

  let respuesta;
  try {
    respuesta = await fetch(url, {
      method,
      headers,
      body: body === undefined ? undefined : esFormData ? body : JSON.stringify(body)
    });
  } catch {
    throw new ApiError(
      `No se pudo conectar con la API (${getApiUrl()}). Verificá que esté corriendo y que el certificado HTTPS esté aceptado.`,
      0
    );
  }

  const datos = await leerCuerpo(respuesta);

  if (!respuesta.ok) {
    if (respuesta.status === 401 && token && alPerderSesion) alPerderSesion();
    throw new ApiError(mensajeDeError(datos, respuesta.status), respuesta.status, datos);
  }

  return datos;
}

async function leerCuerpo(respuesta) {
  if (respuesta.status === 204) return null;
  const texto = await respuesta.text();
  if (!texto) return null;
  try {
    return JSON.parse(texto);
  } catch {
    return texto;
  }
}

function mensajeDeError(datos, status) {
  if (typeof datos === "string" && datos.trim()) return datos;

  if (datos && typeof datos === "object") {
    if (datos.mensaje) return datos.mensaje;
    // Errores de validación automática de [ApiController] (ProblemDetails)
    if (datos.errors) return Object.values(datos.errors).flat().join(" ");
    if (datos.title) return datos.title;
  }

  const porDefecto = {
    400: "Los datos enviados no son válidos.",
    401: "Tenés que iniciar sesión.",
    403: "No tenés permisos para realizar esta acción.",
    404: "No se encontró lo que buscabas.",
    409: "La operación entra en conflicto con datos existentes.",
    500: "Ocurrió un error interno en el servidor."
  };
  return porDefecto[status] || `Error ${status}`;
}
