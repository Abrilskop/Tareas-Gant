namespace TareasGant.Backend
{
    public class ListaJson
    {
        public int Id { get; set; }
        public int? Padre { get; set; }
        public string Titulo { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public List<ListaJson> Hijos { get; set; } = new List<ListaJson>();
    }

}
