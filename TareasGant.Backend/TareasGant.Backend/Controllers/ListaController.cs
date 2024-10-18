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
            using (var LectorFile = new StreamReader(file.OpenReadStream()))
            {
                var ContenidoJson = await LectorFile.ReadToEndAsync();
                var listasJson = JsonConvert.DeserializeObject<List<ListaJson>>(ContenidoJson); // leer archivo y crear lista

                if (listasJson == null || !listasJson.Any())
                    return BadRequest("El archivo no contiene listas válidas.");

                var listasPorId = listasJson.ToDictionary(item => item.Id);
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
                    BubbleSortHijosPorFechaInicio(padre.Hijo);
                    CompararFechasDelPadre(padre);
                }

                return Ok(padres);
            }
        }

        public void BubbleSortHijosPorFechaInicio(List<ListaJson> hijos)
        {
            int totalHijos = hijos.Count;
            for (int i = 0; i < totalHijos - 1; i++)
            {
                for (int j = 0; j < totalHijos - i - 1; j++)
                {
                    if (DateTime.Parse(hijos[j].FechaInicio) > DateTime.Parse(hijos[j + 1].FechaInicio))
                    {
                        // Intercambiar hijos[j] y hijos[j + 1]
                        var auxiliar = hijos[j];
                        hijos[j] = hijos[j + 1];
                        hijos[j + 1] = auxiliar;
                    }
                }
            }
        }

        public void CompararFechasDelPadre(ListaJson padre)
        {
            if (padre.Hijo.Any())
            {
                padre.FechaInicio = padre.Hijo.Min(hijo => hijo.FechaInicio);
                padre.FechaFin = padre.Hijo.Max(hijo => hijo.FechaFin);
            }
        }
    }

}

