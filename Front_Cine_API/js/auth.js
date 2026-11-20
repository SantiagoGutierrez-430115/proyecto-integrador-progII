// ============================================================================
// Sesión: login, registro, modo invitado y lectura del token JWT
// ============================================================================
import { api, setToken, getToken, clearToken } from "./api.js";

// Nombres de los claims que genera UsuarioService (ClaimTypes de .NET)
const CLAIM_ID = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
const CLAIM_EMAIL = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
const CLAIM_ROL = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

// El invitado no tiene token: solo una marca en el navegador
const CLAVE_INVITADO = "cine_invitado";

function decodificarToken(token) {
  try {
    let base64 = token.split(".")[1].replace(/-/g, "+").replace(/_/g, "/");
    base64 += "=".repeat((4 - (base64.length % 4)) % 4);
    const bytes = Uint8Array.from(atob(base64), c => c.charCodeAt(0));
    return JSON.parse(new TextDecoder().decode(bytes));
  } catch {
    return null;
  }
}

function esInvitadoGuardado() {
  try { return localStorage.getItem(CLAVE_INVITADO) === "1"; } catch { return false; }
}

function marcarInvitado(valor) {
  try {
    if (valor) localStorage.setItem(CLAVE_INVITADO, "1");
    else localStorage.removeItem(CLAVE_INVITADO);
  } catch { /* sin almacenamiento */ }
}

/**
 * Devuelve la sesión actual o null si no hay.
 * { idUsuario, email, rol, esAdmin, esInvitado }
 */
export function sesionActual() {
  const token = getToken();

  if (token) {
    const claims = decodificarToken(token);
    if (claims && !(claims.exp && claims.exp * 1000 < Date.now())) {
      const rol = Array.isArray(claims[CLAIM_ROL]) ? claims[CLAIM_ROL][0] : claims[CLAIM_ROL];
      return {
        idUsuario: Number(claims[CLAIM_ID]),
        email: claims[CLAIM_EMAIL] || "",
        rol: rol || "Cliente",
        esAdmin: rol === "Admin",
        esInvitado: false
      };
    }
    clearToken();
  }

  if (esInvitadoGuardado()) {
    return { idUsuario: 0, email: "Invitado", rol: "Invitado", esAdmin: false, esInvitado: true };
  }
  return null;
}

export async function login(email, password) {
  const respuesta = await api("/api/Usuario/login", { method: "POST", body: { email, password } });
  if (!respuesta || !respuesta.token) throw new Error("La API no devolvió un token.");
  marcarInvitado(false);
  setToken(respuesta.token);
  return sesionActual();
}

/** Entra sin cuenta: puede ver cartelera, funciones, butacas y candy bar, pero no comprar. */
export function entrarComoInvitado() {
  clearToken();
  marcarInvitado(true);
  return sesionActual();
}

export async function registrar(datos) {
  return api("/api/Usuario/registro", { method: "POST", body: datos });
}

export function logout() {
  clearToken();
  marcarInvitado(false);
}
