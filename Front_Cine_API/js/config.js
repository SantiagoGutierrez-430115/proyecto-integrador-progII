// ============================================================================
// Configuración del front
// ============================================================================

// Dirección de la API. Por defecto es el perfil "https" de Visual Studio (launchSettings.json).
// Se puede cambiar desde la pantalla de login ("Dirección de la API") sin tocar el código;
// el valor elegido queda guardado en el navegador.
export const API_URL_POR_DEFECTO = "https://localhost:7032";
const CLAVE_API_URL = "cine_api_url";

export function getApiUrl() {
  try {
    return localStorage.getItem(CLAVE_API_URL) || API_URL_POR_DEFECTO;
  } catch {
    return API_URL_POR_DEFECTO;
  }
}

export function setApiUrl(url) {
  try {
    if (url) localStorage.setItem(CLAVE_API_URL, url.replace(/\/+$/, ""));
    else localStorage.removeItem(CLAVE_API_URL);
  } catch { /* navegador sin almacenamiento: se usa el valor por defecto */ }
}

// ----------------------------------------------------------------------------
// Catálogos que la API todavía NO expone por endpoint.
// Son los valores cargados en la base CINE_R (tablas tipos_documentos, salas,
// horarios, tipos_butacas, marcas, tipos_productos, tipos_combos).
// Si se agregan registros en la base, hay que actualizarlos acá (o mejor:
// agregar endpoints en CatalogoController y leerlos desde la API).
// ----------------------------------------------------------------------------
export const TIPOS_DOCUMENTO = [
  { id: 1, nombre: "DNI" },
  { id: 2, nombre: "Pasaporte" },
  { id: 3, nombre: "Documento extranjero" }
];

export const SALAS = [
  { id: 1, nro: 1, tipo: "Tradicional", sucursal: 1 },
  { id: 2, nro: 2, tipo: "3D", sucursal: 1 },
  { id: 3, nro: 1, tipo: "Tradicional", sucursal: 2 },
  { id: 4, nro: 2, tipo: "Premium", sucursal: 2 }
];

export const HORARIOS = [
  { id: 1, hora: "17:00" },
  { id: 2, hora: "19:30" },
  { id: 3, hora: "22:00" }
];

export const TIPOS_BUTACA = [
  { id: 1, nombre: "Común" },
  { id: 2, nombre: "Preferencial" }
];

export const MARCAS = [
  { id: 1, nombre: "Cine Snacks" },
  { id: 2, nombre: "Bebidas del Centro" }
];

export const TIPOS_PRODUCTO = [
  { id: 1, nombre: "Golosinas" },
  { id: 2, nombre: "Bebidas" }
];

export const TIPOS_COMBO = [
  { id: 1, nombre: "Individual" },
  { id: 2, nombre: "Duo" }
];

export function nombreSala(idSala) {
  const s = SALAS.find(x => x.id === idSala);
  return s ? `Sala ${s.nro} · ${s.tipo} · Sucursal ${s.sucursal}` : `Sala #${idSala}`;
}

export function horaDeHorario(idHorario) {
  const h = HORARIOS.find(x => x.id === idHorario);
  return h ? h.hora : `Horario #${idHorario}`;
}
