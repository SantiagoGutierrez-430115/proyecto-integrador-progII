// ============================================================================
// Sección de soporte y gestión: solo para usuarios con rol Admin.
// (La API también lo controla con [Authorize(Roles = "Admin")]; ocultar la
// sección en el front es solo una comodidad, no la seguridad real.)
// ============================================================================
import { api } from "./api.js";
import {
  SALAS, HORARIOS, TIPOS_BUTACA, MARCAS, TIPOS_PRODUCTO, TIPOS_COMBO,
  nombreSala, horaDeHorario
} from "./config.js";
import {
  esc, dinero, fecha, fechaHora, hoyIso, estrellas, toast, cargando, vacio, errorEn,
  abrirFormulario, mostrarInfo, opciones, numOrNull, paginador, urlPortada
} from "./ui.js";
import { cargarPeliculasActivas, tituloPelicula, mapaButacasHtml, abrirButacas } from "./cliente.js";

const yaCargado = new Set();
const salasOpciones = SALAS.map(s => ({ id: s.id, nombre: nombreSala(s.id) }));
const horariosOpciones = HORARIOS.map(h => ({ id: h.id, nombre: `${h.hora} hs` }));

export function initAdmin() {
  // Cada pestaña se carga la primera vez que se abre
  document.querySelectorAll("[data-admin]").forEach(tab => {
    tab.addEventListener("shown.bs.tab", () => cargarPestania(tab.dataset.admin));
  });

  initClientes();
  initPeliculas();
  initFunciones();
  initButacas();
  initProductos();
  initCombos();
  initAdministradores();
}

export function abrirAdmin() {
  const activa = document.querySelector("[data-admin].active");
  cargarPestania(activa ? activa.dataset.admin : "clientes");
}

function cargarPestania(nombre) {
  if (yaCargado.has(nombre)) return;
  yaCargado.add(nombre);
  ({
    clientes: () => {},
    peliculas: () => cargarPeliculasAdm(1),
    funciones: cargarFuncionesAdm,
    butacas: cargarButacasAdm,
    productos: cargarProductosAdm,
    combos: cargarCombosAdm,
    resenias: cargarReseniasAdm,
    administradores: cargarAdministradores
  })[nombre]?.();
}

const botonIcono = (accion, id, icono, titulo, clase = "btn-ghost") =>
  `<button class="btn ${clase} btn-sm" data-accion="${accion}" data-id="${id}" title="${esc(titulo)}"><i class="bi ${icono}"></i></button>`;

function enlazarAcciones(contenedor, acciones) {
  contenedor.querySelectorAll("[data-accion]").forEach(b =>
    b.addEventListener("click", () => acciones[b.dataset.accion]?.(Number(b.dataset.id))));
}

// ============================================================================
// SOPORTE DE CLIENTES
// ============================================================================
let ultimoFiltroCliente = "";

function initClientes() {
  document.getElementById("formBuscarCliente").addEventListener("submit", e => {
    e.preventDefault();
    ultimoFiltroCliente = new FormData(e.target).get("filtro").trim();
    buscarClientes();
  });
}

async function buscarClientes() {
  const cont = document.getElementById("resultadoClientes");
  cargando(cont, "Buscando clientes…");
  try {
    const r = await api("/api/Clientes", { query: { filtro: ultimoFiltroCliente } });
    cont.innerHTML = `
      <div class="alert alert-success py-2 small"><i class="bi bi-check-circle me-1"></i>${esc(r.mensaje)}</div>
      <div class="table-responsive">
        <table class="table table-cinema">
          <thead><tr><th>Cód.</th><th>Apellido y nombre</th><th>Documento</th><th>Correo</th><th class="text-end">Acciones</th></tr></thead>
          <tbody>${r.clientes.map(c => `
            <tr>
              <td>${c.codCliente}</td>
              <td class="fw-semibold">${esc(c.apellido)}, ${esc(c.nombre)}</td>
              <td>${esc(c.tipoDocumento)} ${esc(c.nroDoc)}</td>
              <td>${esc(c.correoElectronico)}</td>
              <td class="text-end text-nowrap">
                ${botonIcono("editar", c.codCliente, "bi-pencil", "Editar datos")}
                ${botonIcono("reservas", c.codCliente, "bi-journal-text", "Ver reservas")}
              </td>
            </tr>`).join("")}
          </tbody>
        </table>
      </div>`;
    enlazarAcciones(cont, {
      editar: id => editarCliente(r.clientes.find(c => c.codCliente === id)),
      reservas: verReservasCliente
    });
  } catch (err) {
    // 404 = ningún cliente coincide con el filtro: se muestra como aviso, no como error
    if (err.status === 404) {
      cont.innerHTML = `<div class="alert alert-warning mb-0"><i class="bi bi-person-x me-2"></i>${esc(err.message)}</div>`;
    } else {
      errorEn(cont, err);
    }
  }
}

function editarCliente(c) {
  abrirFormulario({
    titulo: `Editar cliente #${c.codCliente}`,
    html: `
      <p class="small text-muted-silver">Solo se actualizan los campos que cambies.</p>
      <div class="row g-2">
        <div class="col-md-6"><label class="form-label">Nombre</label><input name="nombre" class="form-control form-control-cinema" maxlength="100" value="${esc(c.nombre)}" required /></div>
        <div class="col-md-6"><label class="form-label">Apellido</label><input name="apellido" class="form-control form-control-cinema" maxlength="100" value="${esc(c.apellido)}" required /></div>
        <div class="col-md-6"><label class="form-label">Nº de documento (${esc(c.tipoDocumento)})</label><input name="nroDoc" class="form-control form-control-cinema" maxlength="20" value="${esc(c.nroDoc)}" required
          inputmode="numeric" pattern="[0-9]+" title="Solo números, sin puntos ni espacios" oninput="this.value = this.value.replace(/[^0-9]/g, '')" /></div>
        <div class="col-md-6"><label class="form-label">Correo electrónico</label><input type="email" name="correoElectronico" class="form-control form-control-cinema" maxlength="254" value="${esc(c.correoElectronico)}" required /></div>
      </div>`,
    onSubmit: async form => {
      const fd = new FormData(form);
      const r = await api(`/api/Clientes/${c.codCliente}`, {
        method: "PUT",
        body: {
          nombre: fd.get("nombre").trim(),
          apellido: fd.get("apellido").trim(),
          nroDoc: fd.get("nroDoc").trim(),
          correoElectronico: fd.get("correoElectronico").trim()
        }
      });
      toast(r.mensaje);
      buscarClientes();
    }
  });
}

async function verReservasCliente(codCliente) {
  mostrarInfo(`Reservas del cliente #${codCliente}`, `<div id="infoReservas"></div>`);
  const cont = document.getElementById("infoReservas");
  cargando(cont);
  try {
    const r = await api(`/api/Clientes/${codCliente}/reservas`);
    if (r.reservas.length === 0) {
      vacio(cont, r.mensaje, "bi-journal-x");
      return;
    }
    cont.innerHTML = `
      <p class="small text-muted-silver">${esc(r.mensaje)}</p>
      <div class="table-responsive"><table class="table table-cinema">
        <thead><tr><th>Nº</th><th>Reservada</th><th>Película</th><th>Función</th><th>Sala</th><th>Estado</th><th class="text-end">Monto</th></tr></thead>
        <tbody>${r.reservas.map(x => `
          <tr><td>${x.idReserva}</td><td>${fechaHora(x.fechaReserva)}</td><td>${esc(x.pelicula)}</td>
          <td>${fecha(x.fechaFuncion)} ${esc(x.horario)}</td><td>${x.nroSala}</td>
          <td><span class="chip">${esc(x.estadoReserva)}</span></td><td class="text-end">${dinero(x.montoReserva)}</td></tr>`).join("")}
        </tbody></table></div>`;
  } catch (err) {
    errorEn(cont, err);
  }
}

// ============================================================================
// PELÍCULAS
// ============================================================================
let catalogosPelicula = null;
let tituloBuscadoAdm = "";

async function obtenerCatalogosPelicula() {
  if (catalogosPelicula) return catalogosPelicula;
  const nombres = ["paises", "formatos", "clasificaciones", "distribuidores", "directores", "generos", "idiomas", "actores"];
  const listas = await Promise.all(nombres.map(n => api(`/api/Catalogo/${n}`)));
  catalogosPelicula = Object.fromEntries(nombres.map((n, i) => [n, listas[i]]));
  return catalogosPelicula;
}

function initPeliculas() {
  document.getElementById("formBuscarPeliculaAdm").addEventListener("submit", e => {
    e.preventDefault();
    tituloBuscadoAdm = new FormData(e.target).get("titulo").trim();
    cargarPeliculasAdm(1);
  });
  document.getElementById("btnNuevaPelicula").addEventListener("click", () => formularioPelicula(null));
}

async function cargarPeliculasAdm(pagina) {
  const cont = document.getElementById("tablaPeliculasAdm");
  cargando(cont);
  try {
    const r = await api("/api/Pelicula", { query: { titulo: tituloBuscadoAdm, incluirInactivas: true, pagina, tamanio: 15 } });
    if (r.items.length === 0) {
      vacio(cont, "No hay películas que coincidan.", "bi-film");
    } else {
      cont.innerHTML = `
        <div class="table-responsive"><table class="table table-cinema">
          <thead><tr><th>Título</th><th>Estreno</th><th>Clasificación</th><th>Géneros</th><th>Estado</th><th class="text-end">Acciones</th></tr></thead>
          <tbody>${r.items.map(p => {
            const activa = p.estado === "Activa";
            return `
            <tr>
              <td class="fw-semibold">
                ${urlPortada(p) ? `<img src="${esc(urlPortada(p))}" alt="" class="portada-mini me-2" />` : ""}${esc(p.titulo)}
                ${p.urlTrailer ? `<i class="bi bi-youtube text-danger ms-1" title="Tiene tráiler"></i>` : ""}
              </td>
              <td>${fecha(p.fechaLanzamiento)}</td>
              <td>${esc(p.clasificacion || "-")}</td>
              <td>${p.generos.map(g => `<span class="chip">${esc(g.nombre)}</span>`).join("")}</td>
              <td><span class="badge ${activa ? "bg-success" : "bg-secondary"}">${esc(p.estado)}</span></td>
              <td class="text-end text-nowrap">
                ${botonIcono("editar", p.idPelicula, "bi-pencil", "Editar")}
                ${activa
                  ? botonIcono("baja", p.idPelicula, "bi-eye-slash", "Dar de baja (queda inactiva)")
                  : botonIcono("reactivar", p.idPelicula, "bi-arrow-counterclockwise", "Reactivar")}
              </td>
            </tr>`;
          }).join("")}</tbody>
        </table></div>`;
      enlazarAcciones(cont, {
        editar: id => formularioPelicula(r.items.find(p => p.idPelicula === id)),
        baja: id => cambiarEstadoPelicula(id, false, pagina),
        reactivar: id => cambiarEstadoPelicula(id, true, pagina)
      });
    }
    paginador(document.getElementById("paginacionPeliculasAdm"), r, cargarPeliculasAdm);
  } catch (err) {
    errorEn(cont, err);
  }
}

async function cambiarEstadoPelicula(id, reactivar, pagina) {
  if (!reactivar && !confirm("La película dejará de verse en la cartelera. ¿Continuar?")) return;
  try {
    await api(reactivar ? `/api/Pelicula/${id}/reactivar` : `/api/Pelicula/${id}`, { method: reactivar ? "PUT" : "DELETE" });
    toast(reactivar ? "Película reactivada." : "Película dada de baja.");
    cargarPeliculasAdm(pagina);
    refrescarPeliculasCompartidas();
  } catch (err) {
    toast(err.message, "error");
  }
}

function checkList(nombre, lista, seleccionados) {
  const sel = new Set(seleccionados);
  return `<div class="check-list">${lista.map(x => `
    <div class="form-check">
      <input class="form-check-input" type="checkbox" name="${nombre}" value="${x.id}" id="${nombre}-${x.id}" ${sel.has(x.id) ? "checked" : ""}>
      <label class="form-check-label small" for="${nombre}-${x.id}">${esc(x.nombre)}</label>
    </div>`).join("")}</div>`;
}

async function formularioPelicula(p) {
  let cat;
  try {
    cat = await obtenerCatalogosPelicula();
  } catch (err) {
    toast(err.message, "error");
    return;
  }
  const v = p || { titulo: "", descripcion: "", fechaLanzamiento: "", generos: [], idiomas: [], actores: [] };

  abrirFormulario({
    titulo: p ? `Editar "${p.titulo}"` : "Nueva película",
    html: `
      <div class="row g-2">
        <div class="col-md-8"><label class="form-label">Título *</label><input name="titulo" class="form-control form-control-cinema" maxlength="150" required value="${esc(v.titulo)}" /></div>
        <div class="col-md-4"><label class="form-label">Fecha de lanzamiento *</label><input type="date" name="fechaLanzamiento" class="form-control form-control-cinema" required value="${esc(String(v.fechaLanzamiento || "").substring(0, 10))}" /></div>
        <div class="col-12"><label class="form-label">Descripción</label><textarea name="descripcion" rows="2" maxlength="500" class="form-control form-control-cinema">${esc(v.descripcion || "")}</textarea></div>
        <div class="col-md-4"><label class="form-label">País</label><select name="idPais" class="form-select form-select-cinema">${opciones(cat.paises, { vacia: "—", seleccionado: v.idPais })}</select></div>
        <div class="col-md-4"><label class="form-label">Formato</label><select name="idFormatoPelicula" class="form-select form-select-cinema">${opciones(cat.formatos, { vacia: "—", seleccionado: v.idFormatoPelicula })}</select></div>
        <div class="col-md-4"><label class="form-label">Clasificación</label><select name="idClasificacion" class="form-select form-select-cinema">${opciones(cat.clasificaciones, { vacia: "—", seleccionado: v.idClasificacion })}</select></div>
        <div class="col-md-6"><label class="form-label">Distribuidor</label><select name="idDistribuidor" class="form-select form-select-cinema">${opciones(cat.distribuidores, { vacia: "—", seleccionado: v.idDistribuidor })}</select></div>
        <div class="col-md-6"><label class="form-label">Director</label><select name="idDirector" class="form-select form-select-cinema">${opciones(cat.directores, { vacia: "—", seleccionado: v.idDirector })}</select></div>
        <div class="col-md-7">
          <label class="form-label">Tráiler (link de YouTube)</label>
          <input type="url" name="urlTrailer" class="form-control form-control-cinema" maxlength="300"
            placeholder="https://www.youtube.com/watch?v=..." value="${esc(v.urlTrailer || "")}" />
          <div class="small text-muted-silver mt-1">Se muestra en el detalle de la película. Dejalo vacío si no tiene.</div>
        </div>
        <div class="col-md-5">
          <label class="form-label">Foto de portada</label>
          <div class="d-flex gap-2 align-items-start">
            <img id="previewPortada" class="portada-preview ${urlPortada(v) ? "" : "d-none"}" src="${esc(urlPortada(v) || "")}" alt="Vista previa de la portada" />
            <div class="flex-grow-1">
              <input type="file" name="portada" id="inputPortada" accept="image/jpeg,image/png,image/webp" class="form-control form-control-cinema form-control-sm" />
              <div class="small text-muted-silver mt-1">JPG, PNG o WEBP, hasta 2 MB.${urlPortada(v) ? " Elegí otra para reemplazarla." : ""}</div>
            </div>
          </div>
        </div>
        <div class="col-md-4"><label class="form-label">Géneros</label>${checkList("idsGeneros", cat.generos, v.generos.map(g => g.id))}</div>
        <div class="col-md-4"><label class="form-label">Idiomas</label>${checkList("idsIdiomas", cat.idiomas, v.idiomas.map(i => i.id))}</div>
        <div class="col-md-4"><label class="form-label">Actores</label>${checkList("idsActores", cat.actores, v.actores.map(a => a.id))}</div>
      </div>`,
    onSubmit: async form => {
      const fd = new FormData(form);
      const dto = {
        titulo: fd.get("titulo").trim(),
        descripcion: fd.get("descripcion").trim() || null,
        fechaLanzamiento: fd.get("fechaLanzamiento"),
        idPais: numOrNull(fd.get("idPais")),
        idFormatoPelicula: numOrNull(fd.get("idFormatoPelicula")),
        idClasificacion: numOrNull(fd.get("idClasificacion")),
        idDistribuidor: numOrNull(fd.get("idDistribuidor")),
        idDirector: numOrNull(fd.get("idDirector")),
        idsGeneros: fd.getAll("idsGeneros").map(Number),
        idsIdiomas: fd.getAll("idsIdiomas").map(Number),
        idsActores: fd.getAll("idsActores").map(Number),
        urlTrailer: fd.get("urlTrailer").trim() || null
      };

      const archivo = form.querySelector("#inputPortada").files[0];
      if (archivo) validarPortada(archivo);

      let idPelicula = p?.idPelicula;
      if (p) {
        await api(`/api/Pelicula/${idPelicula}`, { method: "PUT", body: dto });
      } else {
        const creada = await api("/api/Pelicula", { method: "POST", body: dto });
        idPelicula = creada.idPelicula;
      }

      // La portada se sube aparte (multipart), una vez que la película ya existe
      if (archivo) {
        const datos = new FormData();
        datos.append("archivo", archivo);
        try {
          await api(`/api/Pelicula/${idPelicula}/portada`, { method: "POST", body: datos });
        } catch (err) {
          toast(`La película se guardó, pero la portada no: ${err.message}`, "error");
        }
      }

      toast(p ? "Película actualizada." : "Película creada.");
      cargarPeliculasAdm(1);
      refrescarPeliculasCompartidas();
    },
    alAbrir: form => {
      // Vista previa de la imagen elegida antes de guardar
      const input = form.querySelector("#inputPortada");
      const preview = form.querySelector("#previewPortada");
      input.addEventListener("change", () => {
        const archivo = input.files[0];
        if (!archivo) return;
        try {
          validarPortada(archivo);
        } catch (err) {
          toast(err.message, "error");
          input.value = "";
          return;
        }
        preview.src = URL.createObjectURL(archivo);
        preview.classList.remove("d-none");
      });
    }
  });
}

// Misma validación que hace la API, para avisar antes de enviar
function validarPortada(archivo) {
  if (!["image/jpeg", "image/png", "image/webp"].includes(archivo.type))
    throw new Error("La portada tiene que ser una imagen JPG, PNG o WEBP.");
  if (archivo.size > 2 * 1024 * 1024)
    throw new Error("La portada no puede superar los 2 MB.");
}

// La cartelera, el buscador de funciones y el alta de funciones usan la lista de películas activas
async function refrescarPeliculasCompartidas() {
  try {
    const activas = await cargarPeliculasActivas();
    llenarSelectPeliculasFuncion(activas);
  } catch { /* se reintenta en la próxima carga */ }
}

// ============================================================================
// FUNCIONES
// ============================================================================
function llenarSelectPeliculasFuncion(activas) {
  document.getElementById("admFuncionPelicula").innerHTML =
    opciones(activas, { valor: "idPelicula", texto: "titulo", vacia: "Elegí una película" });
}

function initFunciones() {
  document.getElementById("admFuncionSala").innerHTML = opciones(salasOpciones, { vacia: "Elegí una sala" });
  document.getElementById("admFuncionHorario").innerHTML = opciones(horariosOpciones, { vacia: "Elegí un horario" });
  const inputFecha = document.getElementById("admFuncionFecha");
  inputFecha.min = hoyIso();

  document.getElementById("formNuevaFuncion").addEventListener("submit", async e => {
    e.preventDefault();
    const form = e.target;
    if (!form.checkValidity()) { form.reportValidity(); return; }
    const fd = new FormData(form);
    try {
      await api("/api/Funcion", {
        method: "POST",
        body: {
          idPelicula: Number(fd.get("idPelicula")),
          idSala: Number(fd.get("idSala")),
          fecha: fd.get("fecha"),
          idHorario: Number(fd.get("idHorario"))
        }
      });
      toast("Función creada.");
      form.reset();
      cargarFuncionesAdm();
    } catch (err) {
      toast(err.message, "error");
    }
  });
}

async function cargarFuncionesAdm() {
  const cont = document.getElementById("tablaFuncionesAdm");
  cargando(cont);
  try {
    const [funciones, activas] = await Promise.all([api("/api/Funcion"), cargarPeliculasActivas()]);
    llenarSelectPeliculasFuncion(activas);

    if (funciones.length === 0) {
      vacio(cont, "No hay funciones programadas.", "bi-calendar-x");
      return;
    }
    const ordenadas = funciones.sort((a, b) =>
      (b.fecha + horaDeHorario(b.idHorario)).localeCompare(a.fecha + horaDeHorario(a.idHorario)));
    cont.innerHTML = `
      <div class="table-responsive" style="max-height: 520px;"><table class="table table-cinema">
        <thead><tr><th>#</th><th>Película</th><th>Fecha</th><th>Hora</th><th>Sala</th><th></th></tr></thead>
        <tbody>${ordenadas.map(f => `
          <tr>
            <td>${f.idFuncion}</td>
            <td class="fw-semibold">${esc(tituloPelicula(f.idPelicula))}</td>
            <td>${fecha(f.fecha)}</td><td>${horaDeHorario(f.idHorario)}</td><td>${esc(nombreSala(f.idSala))}</td>
            <td class="text-end">${botonIcono("butacas", f.idFuncion, "bi-grid-3x3-gap", "Ver ocupación")}</td>
          </tr>`).join("")}</tbody>
      </table></div>`;
    enlazarAcciones(cont, { butacas: id => abrirButacas(ordenadas.find(f => f.idFuncion === id)) });
  } catch (err) {
    errorEn(cont, err);
  }
}

// ============================================================================
// BUTACAS
// ============================================================================
function initButacas() {
  document.getElementById("admButacaSala").innerHTML = opciones(salasOpciones, { vacia: "Elegí una sala" });
  document.getElementById("admButacaTipo").innerHTML = opciones(TIPOS_BUTACA);
  const verSala = document.getElementById("admButacaSalaVer");
  verSala.innerHTML = opciones(salasOpciones);
  verSala.addEventListener("change", cargarButacasAdm);

  document.getElementById("formNuevaButaca").addEventListener("submit", async e => {
    e.preventDefault();
    const form = e.target;
    if (!form.checkValidity()) { form.reportValidity(); return; }
    const fd = new FormData(form);
    const idSala = Number(fd.get("idSala"));
    const idTipoButaca = Number(fd.get("idTipoButaca"));
    const cantidad = Number(fd.get("cantidad"));
    const boton = form.querySelector("button");
    boton.disabled = true;

    let creadas = 0;
    try {
      // La API crea de a una butaca: se llaman en orden para que la numeración sea correlativa
      for (let i = 0; i < cantidad; i++) {
        await api("/api/Butaca", { method: "POST", body: { idSala, idTipoButaca } });
        creadas++;
      }
      toast(`Se agregaron ${creadas} butaca(s) a la ${nombreSala(idSala)}.`);
    } catch (err) {
      toast(`Se agregaron ${creadas} butaca(s). Error: ${err.message}`, "error");
    } finally {
      boton.disabled = false;
      verSala.value = String(idSala);
      cargarButacasAdm();
    }
  });
}

async function cargarButacasAdm() {
  const cont = document.getElementById("mapaButacasAdm");
  const idSala = Number(document.getElementById("admButacaSalaVer").value);
  cargando(cont);
  try {
    const [todas, disponibles] = await Promise.all([
      api(`/api/Butaca/sala/${idSala}`),
      api(`/api/Butaca/sala/${idSala}/disponibles`)
    ]);
    if (todas.length === 0) {
      vacio(cont, "Esta sala todavía no tiene butacas.", "bi-grid-3x3-gap");
      return;
    }
    const idsDisponibles = new Set(disponibles.map(b => b.idButaca));
    const preferencial = TIPOS_BUTACA.find(t => t.nombre === "Preferencial")?.id;
    const butacas = todas.map(b => ({
      idButaca: b.idButaca,
      nroButaca: b.nroButaca,
      disponible: idsDisponibles.has(b.idButaca),
      premium: b.idTipoButaca === preferencial
    }));
    cont.innerHTML = `
      <p class="small text-muted-silver">${todas.length} butacas · ${idsDisponibles.size} en servicio. (El estado por función se ve desde la pestaña Funciones.)</p>
      ${mapaButacasHtml(butacas, new Set(), true)}`;
  } catch (err) {
    errorEn(cont, err);
  }
}

// ============================================================================
// PRODUCTOS DEL SHOP
// ============================================================================
let nombreProductoBuscado = "";

function initProductos() {
  document.getElementById("formBuscarProducto").addEventListener("submit", e => {
    e.preventDefault();
    nombreProductoBuscado = new FormData(e.target).get("nombre").trim();
    cargarProductosAdm();
  });
  document.getElementById("btnNuevoProducto").addEventListener("click", () => formularioProducto(null));
}

async function cargarProductosAdm() {
  const cont = document.getElementById("tablaProductosAdm");
  cargando(cont);
  try {
    const r = await api("/api/ProductoShop", { query: { nombre: nombreProductoBuscado, tamanio: 100 } });
    if (r.items.length === 0) {
      vacio(cont, "No hay productos.", "bi-basket");
      return;
    }
    cont.innerHTML = `
      <div class="table-responsive"><table class="table table-cinema">
        <thead><tr><th>Producto</th><th>Marca</th><th>Tipo</th><th class="text-end">Precio</th><th class="text-end">Stock</th><th class="text-end">Acciones</th></tr></thead>
        <tbody>${r.items.map(p => `
          <tr>
            <td class="fw-semibold">${esc(p.nombre)}<div class="small text-muted-silver">${esc(p.descripcion || "")}</div></td>
            <td>${esc(p.marca || "-")}</td><td>${esc(p.tipoProducto || "-")}</td>
            <td class="text-end">${dinero(p.precio)}</td><td class="text-end">${p.stock}</td>
            <td class="text-end text-nowrap">
              ${botonIcono("editar", p.idProductoShop, "bi-pencil", "Editar")}
              ${botonIcono("eliminar", p.idProductoShop, "bi-trash", "Eliminar")}
            </td>
          </tr>`).join("")}</tbody>
      </table></div>`;
    enlazarAcciones(cont, {
      editar: id => formularioProducto(r.items.find(p => p.idProductoShop === id)),
      eliminar: eliminarProducto
    });
  } catch (err) {
    errorEn(cont, err);
  }
}

function formularioProducto(p) {
  const v = p || { nombre: "", precio: "", descripcion: "", stock: 0 };
  abrirFormulario({
    titulo: p ? `Editar "${p.nombre}"` : "Nuevo producto",
    html: `
      <div class="row g-2">
        <div class="col-md-8"><label class="form-label">Nombre *</label><input name="nombre" class="form-control form-control-cinema" maxlength="100" required value="${esc(v.nombre)}" /></div>
        <div class="col-md-4"><label class="form-label">Precio *</label><input type="number" step="0.01" min="0" name="precio" class="form-control form-control-cinema" required value="${esc(v.precio)}" /></div>
        <div class="col-12"><label class="form-label">Descripción</label><input name="descripcion" class="form-control form-control-cinema" maxlength="500" value="${esc(v.descripcion || "")}" /></div>
        <div class="col-md-4"><label class="form-label">Stock *</label><input type="number" min="0" step="1" name="stock" class="form-control form-control-cinema" required value="${esc(v.stock)}" /></div>
        <div class="col-md-4"><label class="form-label">Marca *</label><select name="idMarca" class="form-select form-select-cinema" required>${opciones(MARCAS, { vacia: "Elegí", seleccionado: v.idMarca })}</select></div>
        <div class="col-md-4"><label class="form-label">Tipo *</label><select name="idTipoProducto" class="form-select form-select-cinema" required>${opciones(TIPOS_PRODUCTO, { vacia: "Elegí", seleccionado: v.idTipoProducto })}</select></div>
      </div>`,
    onSubmit: async form => {
      const fd = new FormData(form);
      const dto = {
        nombre: fd.get("nombre").trim(),
        precio: Number(fd.get("precio")),
        descripcion: fd.get("descripcion").trim() || null,
        stock: Number(fd.get("stock")),
        idMarca: Number(fd.get("idMarca")),
        idTipoProducto: Number(fd.get("idTipoProducto"))
      };
      if (p) await api(`/api/ProductoShop/${p.idProductoShop}`, { method: "PUT", body: dto });
      else await api("/api/ProductoShop", { method: "POST", body: dto });
      toast(p ? "Producto actualizado." : "Producto creado.");
      cargarProductosAdm();
    }
  });
}

async function eliminarProducto(id) {
  if (!confirm("¿Eliminar el producto? Esta acción no se puede deshacer.")) return;
  try {
    await api(`/api/ProductoShop/${id}`, { method: "DELETE" });
    toast("Producto eliminado.");
    cargarProductosAdm();
  } catch (err) {
    toast(err.message, "error");
  }
}

// ============================================================================
// COMBOS
// ============================================================================
function initCombos() {
  document.getElementById("btnNuevoCombo").addEventListener("click", () => formularioCombo(null));
}

async function cargarCombosAdm() {
  const cont = document.getElementById("tablaCombosAdm");
  cargando(cont);
  try {
    const r = await api("/api/Combo", { query: { tamanio: 100 } });
    if (r.items.length === 0) {
      vacio(cont, "No hay combos.", "bi-box2-heart");
      return;
    }
    cont.innerHTML = `
      <div class="table-responsive"><table class="table table-cinema">
        <thead><tr><th>Combo</th><th>Tipo</th><th>Productos</th><th class="text-end">Suma productos</th><th class="text-end">Precio</th><th class="text-end">Acciones</th></tr></thead>
        <tbody>${r.items.map(c => `
          <tr>
            <td class="fw-semibold">${esc(c.descripcion)}</td>
            <td>${esc(c.tipoCombo || "-")}</td>
            <td class="small">${c.productos.map(p => `${p.cantidad} × ${esc(p.producto)}`).join("<br>")}</td>
            <td class="text-end text-muted-silver">${dinero(c.sumaProductos)}</td>
            <td class="text-end fw-bold">${dinero(c.precio)}</td>
            <td class="text-end text-nowrap">
              ${botonIcono("editar", c.idCombo, "bi-pencil", "Editar")}
              ${botonIcono("eliminar", c.idCombo, "bi-trash", "Eliminar")}
            </td>
          </tr>`).join("")}</tbody>
      </table></div>`;
    enlazarAcciones(cont, {
      editar: id => formularioCombo(r.items.find(c => c.idCombo === id)),
      eliminar: eliminarCombo
    });
  } catch (err) {
    errorEn(cont, err);
  }
}

async function formularioCombo(c) {
  let productos;
  try {
    productos = (await api("/api/ProductoShop", { query: { tamanio: 100 } })).items;
  } catch (err) {
    toast(err.message, "error");
    return;
  }
  if (productos.length === 0) {
    toast("Primero cargá productos en la pestaña Productos.", "info");
    return;
  }
  const enCombo = new Map((c?.productos || []).map(p => [p.idProductoShop, p.cantidad]));

  abrirFormulario({
    titulo: c ? `Editar "${c.descripcion}"` : "Nuevo combo",
    html: `
      <div class="row g-2">
        <div class="col-md-6"><label class="form-label">Descripción *</label><input name="descripcion" class="form-control form-control-cinema" maxlength="200" required value="${esc(c?.descripcion || "")}" /></div>
        <div class="col-md-3"><label class="form-label">Tipo *</label><select name="idTipoCombo" class="form-select form-select-cinema" required>${opciones(TIPOS_COMBO, { vacia: "Elegí", seleccionado: c?.idTipoCombo })}</select></div>
        <div class="col-md-3"><label class="form-label">Precio *</label><input type="number" step="0.01" min="0" name="precio" class="form-control form-control-cinema" required value="${esc(c?.precio ?? "")}" /></div>
        <div class="col-12">
          <label class="form-label">Productos del combo (precio actual del shop)</label>
          <div class="check-list" style="max-height: 260px;">
            ${productos.map(p => `
              <div class="d-flex align-items-center gap-2 mb-1">
                <input class="form-check-input mt-0" type="checkbox" name="prod" value="${p.idProductoShop}" data-precio="${p.precio}" id="prod-${p.idProductoShop}" ${enCombo.has(p.idProductoShop) ? "checked" : ""}>
                <label class="form-check-label small flex-grow-1" for="prod-${p.idProductoShop}">${esc(p.nombre)} <span class="text-muted-silver">· ${dinero(p.precio)}</span></label>
                <input type="number" min="1" value="${enCombo.get(p.idProductoShop) || 1}" class="form-control form-control-cinema form-control-sm" style="width: 80px;" data-cantidad="${p.idProductoShop}" aria-label="Cantidad">
              </div>`).join("")}
          </div>
          <div class="small mt-2">Suma de los productos: <strong id="sumaCombo">${dinero(0)}</strong> <span class="text-muted-silver">(el precio del combo no puede superarla)</span></div>
        </div>
      </div>`,
    alAbrir: form => {
      const recalcular = () => {
        let suma = 0;
        form.querySelectorAll("input[name=prod]:checked").forEach(chk => {
          const cant = Number(form.querySelector(`[data-cantidad="${chk.value}"]`).value) || 0;
          suma += Number(chk.dataset.precio) * cant;
        });
        form.querySelector("#sumaCombo").textContent = dinero(suma);
      };
      form.querySelectorAll("input[name=prod], [data-cantidad]").forEach(el => el.addEventListener("input", recalcular));
      recalcular();
    },
    onSubmit: async form => {
      const fd = new FormData(form);
      const items = fd.getAll("prod").map(id => ({
        idProductoShop: Number(id),
        cantidad: Number(form.querySelector(`[data-cantidad="${id}"]`).value) || 1
      }));
      if (items.length === 0) throw new Error("Elegí al menos un producto.");
      const dto = {
        descripcion: fd.get("descripcion").trim(),
        precio: Number(fd.get("precio")),
        idTipoCombo: Number(fd.get("idTipoCombo")),
        productos: items
      };
      if (c) await api(`/api/Combo/${c.idCombo}`, { method: "PUT", body: dto });
      else await api("/api/Combo", { method: "POST", body: dto });
      toast(c ? "Combo actualizado." : "Combo creado.");
      cargarCombosAdm();
    }
  });
}

async function eliminarCombo(id) {
  if (!confirm("¿Eliminar el combo? Esta acción no se puede deshacer.")) return;
  try {
    await api(`/api/Combo/${id}`, { method: "DELETE" });
    toast("Combo eliminado.");
    cargarCombosAdm();
  } catch (err) {
    toast(err.message, "error");
  }
}

// ============================================================================
// ADMINISTRADORES (POST api/Usuario/registro-admin)
// ============================================================================
function initAdministradores() {
  document.getElementById("formNuevoAdmin").addEventListener("submit", async e => {
    e.preventDefault();
    const form = e.target;
    const error = document.getElementById("errorNuevoAdmin");
    error.classList.add("d-none");
    if (!form.checkValidity()) { form.reportValidity(); return; }

    const fd = new FormData(form);
    if (fd.get("password") !== fd.get("password2")) {
      error.textContent = "Las contraseñas no coinciden.";
      error.classList.remove("d-none");
      return;
    }

    const boton = form.querySelector("button[type=submit]");
    boton.disabled = true;
    try {
      const r = await api("/api/Usuario/registro-admin", {
        method: "POST",
        body: {
          nombreUsuario: fd.get("nombreUsuario").trim(),
          email: fd.get("email").trim(),
          password: fd.get("password")
        }
      });
      toast(r.mensaje);
      form.reset();
      cargarAdministradores();
    } catch (err) {
      error.textContent = err.message;
      error.classList.remove("d-none");
    } finally {
      boton.disabled = false;
    }
  });
}

async function cargarAdministradores() {
  const cont = document.getElementById("tablaAdmins");
  cargando(cont);
  try {
    const admins = await api("/api/Usuario/admins");
    if (admins.length === 0) {
      vacio(cont, "No hay administradores.", "bi-shield");
      return;
    }
    cont.innerHTML = `
      <div class="table-responsive"><table class="table table-cinema">
        <thead><tr><th>#</th><th>Usuario</th><th>Correo</th></tr></thead>
        <tbody>${admins.map(a => `
          <tr><td>${a.idUsuario}</td><td class="fw-semibold">${esc(a.nombreUsuario)}</td><td>${esc(a.email)}</td></tr>`).join("")}
        </tbody>
      </table></div>`;
  } catch (err) {
    errorEn(cont, err);
  }
}

// ============================================================================
// RESEÑAS
// ============================================================================
async function cargarReseniasAdm() {
  const cont = document.getElementById("tablaReseniasAdm");
  cargando(cont);
  try {
    const resenias = await api("/api/Resenia");
    if (resenias.length === 0) {
      vacio(cont, "Todavía no hay reseñas.", "bi-chat-quote");
      return;
    }
    resenias.sort((a, b) => new Date(b.fechaResenia) - new Date(a.fechaResenia));
    cont.innerHTML = `
      <div class="table-responsive"><table class="table table-cinema">
        <thead><tr><th>Fecha</th><th>Película</th><th>Cliente</th><th>Puntuación</th><th>Comentario</th></tr></thead>
        <tbody>${resenias.map(r => `
          <tr>
            <td class="text-nowrap">${fechaHora(r.fechaResenia)}</td>
            <td class="fw-semibold">${esc(tituloPelicula(r.idPelicula))}</td>
            <td>#${r.codCliente}</td>
            <td>${estrellas(r.puntuacion)}</td>
            <td class="small">${esc(r.comentario || "")}</td>
          </tr>`).join("")}</tbody>
      </table></div>`;
  } catch (err) {
    errorEn(cont, err);
  }
}
