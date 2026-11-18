
import { apiRequest } from "./api.js";

async function probarConexion() {
    try {
        const respuesta = await apiRequest("/api/Pelicula");

        console.log("¡Conexión exitosa con la API!");
        console.log("Películas recibidas:", respuesta);

    } catch (error) {
        console.error("Error al conectar con la API:", error);
    }
}

probarConexion();
