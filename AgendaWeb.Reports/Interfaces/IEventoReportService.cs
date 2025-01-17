using AgendaWeb.Infra.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaWeb.Reports.Interfaces
{
    public interface IEventoReportService
    {
        //método para fazer a geração de um relatório
        byte[] Create(DateTime dataMin, DateTime dataMax, List<Evento> eventos);
    }
}
