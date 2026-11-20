// ============================================================================
// Punto de entrada: acceso (login / registro), navegación entre secciones
// ============================================================================
import { api, setAlPerderSesion } from "./api.js";
import { getApiUrl, setApiUrl, API_URL_POR_DEFECTO, TIPOS_DOCUMENTO } from "./config.js";
import { sesionActual, login, registrar, logout, entrarComoInvitado } from "./auth.js";
import { toast, opciones } from "./ui.js";
import {
  initCliente, cargarPeliculasActivas, initCartelera, initFunciones, buscarFunciones, cargarShop
} from "./cliente.js";
import { initAdmin, abrirAdmin } from "./admin.js";
import { initDashboard, cargarDashboard } from "./dashboard.js";

const authView = document.getElementById("authView");
const appView = document.getElementById("appView");
let appIniciada = false;
let shopCargado = false;
let funcionesCargadas = false;

// ----------------------------------------------------------------------------
// Pantalla de acceso
// ----------------------------------------------------------------------------
function mostrarAcceso() {
  appView.classList.add("d-none");
  authView.classList.remove("d-none");
  verificarApi();
}

async function verificarApi() {
  const estado = document.getElementById("apiStatus");
  estado.className = "api-status";
  estado.querySelector(".txt").textContent = `Verificando API (${getApiUrl()})…`;
  try {
    await api("/api/Catalogo/generos");
    estado.classList.add("ok");
    estado.querySelector(".txt").textContent = `API conectada · ${getApiUrl()}`;
  } catch {
    estado.classList.add("error");
    estado.querySelector(".txt").textContent = `Sin conexión con la API · ${getApiUrl()}`;
  }
}

function mostrarError(id, mensaje) {
  const el = document.getElementById(id);
  el.textContent = mensaje;
  el.classList.toggle("d-none", !mensaje);
}

document.getElementById("regTipoDoc").innerHTML = opciones(TIPOS_DOCUMENTO);

document.getElementById("loginForm").addEventListener("submit", async e => {
  e.preventDefault();
  mostrarError("loginError", "");
  const email = document.getElementById("loginEmail").value.trim();
  const password = document.getElementById("loginPass").value;
  if (!email || !password) {
    mostrarError("loginError", "Completá el correo y la contraseña.");
    return;
  }
  const boton = e.target.querySelector("button[type=submit]");
  boton.disabled = true;
  try {
    const sesion = await login(email, password);
    document.getElementById("loginPass").value = "";
    await mostrarApp(sesion);
    toast(`¡Bienvenido/a! Ingresaste como ${sesion.rol}.`);
  } catch (err) {
    mostrarError("loginError", err.status === 401 ? "Correo o contraseña incorrectos." : err.message);
  } finally {
    boton.disabled = false;
  }
});

document.getElementById("registroForm").addEventListener("submit", async e => {
  e.preventDefault();
  mostrarError("registroError", "");
  const form = e.target;
  if (!form.checkValidity()) {
    form.reportValidity();
    return;
  }
  const fd = new FormData(form);
  if (fd.get("password") !== fd.get("password2")) {
    mostrarError("registroError", "Las contraseñas no coinciden.");
    return;
  }

  const boton = form.querySelector("button[type=submit]");
  boton.disabled = true;
  try {
    const email = fd.get("email").trim();
    await registrar({
      nombreUsuario: fd.get("nombreUsuario").trim(),
      email,
      password: fd.get("password"),
      nombre: fd.get("nombre").trim(),
      apellido: fd.get("apellido").trim(),
      nroDoc: fd.get("nroDoc").trim(),
      idTipoDoc: Number(fd.get("idTipoDoc")),
      fechaNacimiento: fd.get("fechaNacimiento") || null
    });
    form.reset();
    toast("Cuenta creada. Ya podés iniciar sesión.");
    document.getElementById("loginEmail").value = email;
    bootstrap.Tab.getOrCreateInstance(document.getElementById("tab-login")).show();
    document.getElementById("loginPass").focus();
  } catch (err) {
    mostrarError("registroError", err.message);
  } finally {
    boton.disabled = false;
  }
});

document.getElementById("btnInvitado").addEventListener("click", async () => {
  const sesion = entrarComoInvitado();
  await mostrarApp(sesion);
  toast("Entraste como invitado: podés mirar todo, pero para comprar necesitás una cuenta.", "info");
});

document.getElementById("btnConfigApi").addEventListener("click", () => {
  const actual = getApiUrl();
  const nueva = prompt(
    `Dirección de la API (por defecto ${API_URL_POR_DEFECTO}).\nDejala vacía para volver al valor por defecto.`,
    actual
  );
  if (nueva === null) return;
  setApiUrl(nueva.trim());
  verificarApi();
});

// ----------------------------------------------------------------------------
// Aplicación con sesión iniciada
// ----------------------------------------------------------------------------
async function mostrarApp(sesion) {
  authView.classList.add("d-none");
  appView.classList.remove("d-none");

  document.getElementById("userEmail").textContent = sesion.email;
  const rol = document.getElementById("userRol");
  rol.textContent = sesion.rol;
  rol.className = `badge rounded-pill ${sesion.esAdmin ? "badge-red" : "bg-secondary"}`;
  if (sesion.esInvitado) document.getElementById("userEmail").textContent = "";
  document.getElementById("navAdmin").classList.toggle("d-none", !sesion.esAdmin);
  document.getElementById("navDashboard").classList.toggle("d-none", !sesion.esAdmin);

  // El invitado no "sale": vuelve al login para entrar con una cuenta
  const btnLogout = document.getElementById("btnLogout");
  btnLogout.querySelector("span").textContent = sesion.esInvitado ? "Iniciar sesión" : "Salir";
  btnLogout.querySelector("i").className = sesion.esInvitado ? "bi bi-box-arrow-in-right" : "bi bi-box-arrow-right";

  initCliente(sesion);

  if (!appIniciada) {
    appIniciada = true;
    initFunciones();
    if (sesion.esAdmin) initAdmin();
  }

  try {
    await cargarPeliculasActivas();
  } catch (err) {
    toast(err.message, "error");
  }
  await initCartelera();
  cambiarVista("cartelera");
}

function cambiarVista(nombre) {
  const sesion = sesionActual();
  if ((nombre === "admin" || nombre === "dashboard") && !sesion?.esAdmin) nombre = "cartelera";

  document.querySelectorAll(".app-view").forEach(v => v.classList.toggle("d-none", v.id !== `view-${nombre}`));
  document.querySelectorAll(".nav-view").forEach(b => b.classList.toggle("active", b.dataset.view === nombre));

  if (nombre === "funciones" && !funcionesCargadas) { funcionesCargadas = true; buscarFunciones(); }
  if (nombre === "shop" && !shopCargado) { shopCargado = true; cargarShop(); }
  if (nombre === "admin") abrirAdmin();
  if (nombre === "dashboard") { initDashboard(); cargarDashboard(); }

  // En pantallas chicas se cierra el menú desplegable
  const menu = document.getElementById("mainNav");
  if (menu.classList.contains("show")) bootstrap.Collapse.getOrCreateInstance(menu).hide();
  window.scrollTo({ top: 0, behavior: "smooth" });
}

document.querySelectorAll("[data-view]").forEach(b => b.addEventListener("click", e => {
  e.preventDefault();
  cambiarVista(b.dataset.view);
}));

document.getElementById("btnLogout").addEventListener("click", () => {
  logout();
  // Se recarga para limpiar todos los datos de la sesión anterior
  location.reload();
});

// El invitado pidió iniciar sesión (al querer comprar o reseñar)
window.addEventListener("cine:pedir-login", () => {
  logout();
  location.reload();
});

setAlPerderSesion(() => {
  logout();
  toast("Tu sesión venció. Volvé a iniciar sesión.", "error");
  setTimeout(() => location.reload(), 1500);
});

// ----------------------------------------------------------------------------
// Arranque
// ----------------------------------------------------------------------------
const sesionGuardada = sesionActual();
if (sesionGuardada) mostrarApp(sesionGuardada);
else mostrarAcceso();
