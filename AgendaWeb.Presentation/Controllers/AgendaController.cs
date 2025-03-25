using AgendaWeb.Infra.Data.Entities;
using AgendaWeb.Infra.Data.Interfaces;
using AgendaWeb.Presentation.Models;
using AgendaWeb.Reports.Interfaces;
using AgendaWeb.Reports.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AgendaWeb.Presentation.Controllers
{
    public class AgendaController : Controller
    {
        //atributo
        private readonly IEventoRepository _eventoRepository;

        //construtor para inicializar o atributo
        public AgendaController(IEventoRepository eventoRepository)
        {
            _eventoRepository = eventoRepository;
        }

        public IActionResult Cadastro()
        {
            return View();
        }


        [HttpPost] //Annotation indica que o método será executado no SUBMIT
        public IActionResult Cadastro(EventoCadastroViewModel model)
        {
            //verificar se todos os campos passaram nas regras de validação
            if (ModelState.IsValid)
            {
                try
                {
                    //ler o usuário autenticado na sessão
                    var json = HttpContext.Session.GetString("usuario");
                    var usuario = JsonConvert.DeserializeObject<UserIdentityModel>(json);

                    var evento = new Evento
                    {
                        Id = Guid.NewGuid(),
                        Nome = model.Nome,
                        Data = Convert.ToDateTime(model.Data),
                        Hora = TimeSpan.Parse(model.Hora),
                        Descricao = model.Descricao,
                        Prioridade = Convert.ToInt32(model.Prioridade),
                        DataInclusao = DateTime.Now,
                        DataAlteracao = DateTime.Now,
                        IdUsuario = usuario.Id //foreign key
                    };

                    //gravando no banco de dados
                    _eventoRepository.Create(evento);

                    TempData["MensagemSucesso"] = $"Evento {evento.Nome}, cadastrado com sucesso.";
                    ModelState.Clear(); //limpando os campos do formulário (model)
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }
            else
            {
                TempData["MensagemAlerta"] = "Ocorreram erros de validação no preenchimento do formulário.";
            }

            return View();
        }

        public IActionResult Consulta()
        {
            return View();
        }
        [HttpPost] //Annotation indica que o método será executado no SUBMIT
        public IActionResult Consulta(EventoConsultaViewModel model)
        {
            //verificar se todos os campos da model passaram nas validações
            if (ModelState.IsValid)
            {
                try
                {
                    //converter as datas
                    var dataMin = Convert.ToDateTime(model.DataMin);
                    var dataMax = Convert.ToDateTime(model.DataMax);

                    //verificando se a data de inicio é menor ou igual a data de fim
                    if (dataMin <= dataMax)
                    {
                        //ler o usuário autenticado na sessão
                        var json = HttpContext.Session.GetString("usuario");
                        var usuario = JsonConvert.DeserializeObject<UserIdentityModel>(json);

                        //realizando a consulta de eventos
                        model.Eventos = _eventoRepository.GetByDatas(dataMin, dataMax, model.Ativo, usuario.Id);

                        //verificando se algum evento foi obtido
                        if (model.Eventos.Count > 0)
                        {
                            TempData["MensagemSucesso"] = $"{model.Eventos.Count} evento(s) obtido(s) para a pesquisa";
                        }
                        else
                        {
                            TempData["MensagemAlerta"] = "Nenhum evento foi encontrado para a pesquisa realizada.";
                        }
                    }
                    else
                    {
                        TempData["MensagemErro"] = "A data de início deve ser menor ou igual a data de término.";
                    }
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }
            else
            {
                TempData["MensagemAlerta"] = "Ocorreram erros de validação no preenchimento do formulário.";
            }

            //voltando para a página
            return View(model);
        }

        public IActionResult Edicao(Guid id)
        {
            var model = new EventoEdicaoViewModel();

            try
            {
                //consultar o evento no banco de dados atraves do ID
                var evento = _eventoRepository.GetById(id);

                //preencher os dados da classe model com as informações do evento
                model.Id = evento.Id;
                model.Nome = evento.Nome;
                model.Data = Convert.ToDateTime(evento.Data).ToString("yyyy-MM-dd");
                model.Hora = evento.Hora.ToString();
                model.Descricao = evento.Descricao;
                model.Prioridade = evento.Prioridade.ToString();
                model.Ativo = evento.Ativo;
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = e.Message;
            }

            //enviando o model para a página
            return View(model);
        }

        /*
         * Método para receber o SUBMIT da página de edição (POST)
         */
        [HttpPost]
        public IActionResult Edicao(EventoEdicaoViewModel model)
        {
            //verificar se todos os campos passaram nas regras de validação
            if (ModelState.IsValid)
            {
                try
                {
                    //obtendo os dados do evento no banco de dados..
                    var evento = _eventoRepository.GetById(model.Id);

                    //ler o usuário autenticado na sessão
                    var json = HttpContext.Session.GetString("usuario");
                    var usuario = JsonConvert.DeserializeObject<UserIdentityModel>(json);

                    //modificar os dados do evento
                    evento.Nome = model.Nome;
                    evento.Data = Convert.ToDateTime(model.Data);
                    evento.Hora = TimeSpan.Parse(model.Hora);
                    evento.Descricao = model.Descricao;
                    evento.Prioridade = Convert.ToInt32(model.Prioridade);
                    evento.Ativo = model.Ativo;
                    evento.DataAlteracao = DateTime.Now;
                    evento.IdUsuario = usuario.Id;

                    //atualizando no banco de dados
                    _eventoRepository.Update(evento);

                    TempData["MensagemSucesso"] = $"Evento '{evento.Nome}', atualizado com sucesso.";

                    //redirecionamento para a página de consulta
                    return RedirectToAction("Consulta");
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }
            else
            {
                TempData["MensagemAlerta"] = "Ocorreram erros de validação no preenchimento do formulário.";
            }

            return View();
        }

        public IActionResult Exclusao(Guid id)
        {
            try
            {
                //buscar o evento no banco de dados
                var evento = _eventoRepository.GetById(id);

                //excluindo o evento
                _eventoRepository.Delete(evento);

                TempData["MensagemSucesso"] = $"Evento '{evento.Nome}', excluído com sucesso.";
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = e.Message;
            }

            //redirecionar de volta para a página de consulta
            return RedirectToAction("Consulta");
        }

        public IActionResult Relatorio()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Relatorio(EventoRelatorioViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //capturar as datas enviadas
                    DateTime dataMin = Convert.ToDateTime(model.DataMin);
                    DateTime dataMax = Convert.ToDateTime(model.DataMax);

                    //ler o usuário autenticado na sessão
                    var json = HttpContext.Session.GetString("usuario");
                    var usuario = JsonConvert.DeserializeObject<UserIdentityModel>(json);

                    //consultar os eventos no banco atraves das datas
                    var eventos = _eventoRepository.GetByDatas(dataMin, dataMax, model.Ativo, usuario.Id);

                    //verificar se algum evento foi obtido
                    if (eventos.Count > 0)
                    {
                        //criando um objeto para a interface..
                        IEventoReportService eventoReportService = null; //vazio

                        //variaveis para definir os parametros de download
                        var contentType = string.Empty; //MIME TYPE
                        var fileName = string.Empty;

                        switch (model.Formato)
                        {
                            case 1: //Polimorfismo
                                eventoReportService = new EventoReportServicePdf();
                                contentType = "application/pdf";
                                fileName = $"eventos_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.pdf";

                                break;


                            case 2: //Polimorfismo
                                eventoReportService = new EventoReportServiceExcel();
                                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                fileName = $"eventos_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.xlsx";
                                break;
                        }

                        //gerando o arquivo do relatorio
                        var arquivo = eventoReportService.Create(dataMin, dataMax, eventos);

                        //DOWNLOAD do Arquivo
                        return File(arquivo, contentType, fileName);
                    }
                    else
                    {
                        TempData["MensagemAlerta"] = "Nenhum evento foi obtido para a pesquisa informada.";
                    }
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }

            return View();
        }
    }
}
