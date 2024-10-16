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

                // Paso 1: Identificar padres e hijos (niveles)
                foreach (var lista in listasJson)
                {
                    if (lista.Padre == null)
                    {
                        padres.Add(lista);
                    }
                    else if (listasPorId.TryGetValue((int)lista.Padre, out var padre))
                    {
                        padre.Hijo.Add(lista);
                    }
                }

                // Paso 2: Ordenar hijos y ajustar fechas
                foreach (var padre in padres)
                {
                    ComparacionHijosPorFechaInicio(padre.Hijo);

                }

                return Ok(padres);
            }
        }

        public void ComparacionHijosPorFechaInicio(List<ListaJson> hijos)
        {
            int totalHijos = hijos.Count;
            for (int HijoActual = 0; HijoActual < totalHijos - 1; HijoActual++)
            {
                for (int SiguienteHijo = 0; SiguienteHijo < totalHijos - HijoActual - 1; SiguienteHijo++)
                {
                    if (DateTime.Parse(hijos[SiguienteHijo].FechaInicio) > DateTime.Parse(hijos[SiguienteHijo + 1].FechaInicio))
                    {
                        // Intercambiar hijos[SiguienteHijo] y hijos[SiguienteHijo + 1]
                        var temp = hijos[SiguienteHijo];
                        hijos[SiguienteHijo] = hijos[SiguienteHijo + 1];
                        hijos[SiguienteHijo + 1] = temp;
                    }
                }
            }
        }

    }

}

