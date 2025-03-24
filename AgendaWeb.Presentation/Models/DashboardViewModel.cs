namespace AgendaWeb.Presentation.Models
{
    public class DashboardViewModel
    {
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }

        public int TotalPrioridadeAlta { get; set; }
        public int TotalPrioridadeMedia { get; set; }
        public int TotalPrioridadeBaixa { get; set; }

        public int TotalAtivos { get; set; }
        public int TotalInativos { get; set; }
    }
}
