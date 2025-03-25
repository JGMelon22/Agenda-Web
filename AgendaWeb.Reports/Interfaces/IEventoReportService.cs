using AgendaWeb.Infra.Data.Entities;

namespace AgendaWeb.Reports.Interfaces
{
    public interface IEventoReportService
    {
        //método para fazer a geração de um relatório
        byte[] Create(DateTime dataMin, DateTime dataMax, List<Evento> eventos);
    }
}
