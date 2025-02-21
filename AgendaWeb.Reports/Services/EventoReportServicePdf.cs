 using AgendaWeb.Infra.Data.Entities;
using AgendaWeb.Reports.Interfaces;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaWeb.Reports.Services
{
    /// <summary>
    /// Classe para geração do relatório em formato PDF
    /// </summary>
    public class EventoReportServicePdf : IEventoReportService
    {
        /// <summary>
        /// Método para geração do relatório
        /// </summary>
        /// <param name="dataMin">Data de inicio da pesquisa</param>
        /// <param name="dataMax">Data de fim da pesquisa</param>
        /// <param name="eventos">Lista de eventos</param>
        /// <returns>Arquivo em formato byte</returns>
        public byte[] Create(DateTime dataMin, DateTime dataMax, List<Evento> eventos)
        {
            //criando o arquivo PDF
            var memoryStream = new MemoryStream();
            var pdf = new PdfDocument(new PdfWriter(memoryStream));

            //escrevendo o conteudo do arquivo
            using (var document = new Document(pdf))
            {
                //espaçamento entre linhas
                document.SetProperty(Property.LEADING, new Leading(Leading.MULTIPLIED, 0.7f));

                //título do documento
                document.Add(new Paragraph("Relatório de Eventos").AddStyle(new Style().SetFontSize(24)));
                document.Add(new Paragraph("COTI Informática - Agenda Web").AddStyle(new Style().SetFontSize(14)));

                document.Add(new Paragraph("\n"));

                //informações do relatório
                var table = new Table(2); //2 -> numero de colunas
                
                table.AddCell("Data de início:");
                table.AddCell(dataMin.ToString("dd/MM/yyyy"));

                table.AddCell("Data de término:");
                table.AddCell(dataMax.ToString("dd/MM/yyyy"));

                table.AddCell("Quantidade:");
                table.AddCell($"{eventos.Count} evento(s) obtidos.");

                document.Add(table);

                document.Add(new Paragraph("\n"));
                document.Add(new LineSeparator(new SolidLine(1f)));
                document.Add(new Paragraph("\n"));

                //imprimindo os eventos..
                foreach (var item in eventos)
                {
                    var prioridade = item.Prioridade == 1 ? "BAIXA" : item.Prioridade == 2 ? "MÉDIA" : "ALTA";
                    var status = item.Ativo == 1 ? "Sim" : "Não";

                    document.Add(new Paragraph($"ID: {item.Id}"));
                    document.Add(new Paragraph($"{item.Nome}"));
                    document.Add(new Paragraph($"Data: {((DateTime)item.Data).ToString("dd/MM/yyyy")}"));
                    document.Add(new Paragraph($"Hora: {item.Hora.ToString()}"));
                    document.Add(new Paragraph($"{item.Descricao}"));
                    document.Add(new Paragraph($"Prioridade: {prioridade}"));
                    document.Add(new Paragraph($"Cadastrado em: {((DateTime)item.DataInclusao).ToString("dd/MM/yyyy HH:mm")}"));
                    document.Add(new Paragraph($"Última alteração em: {((DateTime)item.DataAlteracao).ToString("dd/MM/yyyy HH:mm")}"));
                    document.Add(new Paragraph($"Ativo: {status}"));

                    document.Add(new Paragraph("\n"));
                    document.Add(new LineSeparator(new SolidLine(1f)));
                    document.Add(new Paragraph("\n"));
                }
            }

            //retornando o conteúdo do arquivo..
            return memoryStream.ToArray();
        }
    }
}
