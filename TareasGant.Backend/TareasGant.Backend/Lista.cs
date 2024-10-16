namespace TareasGant.Backend
{
    public class ListaJson
    {
        public int Id { get; set; }
        public List<ListaJson> Hijo { get; set; } = new List<ListaJson>();
        public int? Padre { get; set; }
        public string Titulo { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        
    }

}
