using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TareasGant.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListaController : ControllerBase
    {
        [HttpPost("upload-lista")]
        public async Task<IActionResult> ProcesarListasJson(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Archivo no proporcionado o vacío.");

            // Leer el contenido del archivo
            using (var stream = new StreamReader(file.OpenReadStream()))
            {
                var jsonContent = await stream.ReadToEndAsync();
                var listasJson = JsonConvert.DeserializeObject<List<ListaJson>>(jsonContent);

                if (listasJson == null || !listasJson.Any())
                    return BadRequest("El archivo no contiene listas válidas.");

                var listasPorId = listasJson.ToDictionary(e => e.Id);
                var padres = new List<ListaJson>();

                // Paso 1: Identificar padres e hijos(niveles)
                foreach (var lista in listasJson)
                {
                    if (lista.Padre == null)
                    {
                        padres.Add(lista);
                    }
                    else if (listasPorId.TryGetValue((int)lista.Padre, out var padre))
                    {
                        padre.Hijos.Add(lista);
                    }
                }

                // Paso 2: Comparar fechas
                foreach (var padre in padres)
                {
                    CompararFechas(padre);
                }

                return Ok(padres);
            }
        }

        private void CompararFechas(ListaJson padre)
        {
            if (!padre.Hijos.Any())
                return;

            padre.FechaInicio = padre.Hijos.Min(hijo => hijo.FechaInicio);
            padre.FechaFin = padre.Hijos.Max(hijo => hijo.FechaFin);

            foreach (var hijo in padre.Hijos)
            {
                CompararFechas(hijo);
            }
        }
    }

}
