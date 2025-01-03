using AgendaWeb.Infra.Data.Entities;
using AgendaWeb.Infra.Data.Interfaces;
using AgendaWeb.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost] //Annotation indica que o método será executado no SUBMIT]
        public IActionResult Cadastro(EventoCadastroViewModel model)
        {
            //verificar se todos os campos passaram nas regras de validação
            if(ModelState.IsValid)
            {
                try
                {
                    var evento = new Evento
                    {
                        Id = Guid.NewGuid(),
                        Nome = model.Nome,
                        Data = Convert.ToDateTime(model.Data),
                        Hora = TimeSpan.Parse(model.Hora),
                        Descricao = model.Descricao,
                        Prioridade = Convert.ToInt32(model.Prioridade),
                        DataInclusao = DateTime.Now,
                        DataAlteracao = DateTime.Now
                    };

                    //gravando no banco de dados
                    _eventoRepository.Create(evento);

                    TempData["MensagemSucesso"] = $"Evento {evento.Nome}, cadastrado com sucesso.";
                    ModelState.Clear(); //limpando os campos do formulário (model)
                }
                catch(Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }
            else
            {
                TempData["MensagemAlerta"] = "Ocorreram erros de validação no preenchimento do formulár io";
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
            if(ModelState.IsValid)
            {
                try
                {
                    //converter as datas
                    var dataMin = Convert.ToDateTime(model.DataMin);
                    var dataMax = Convert.ToDateTime(model.DataMax);

                    //realizando a consulta de eventos
                    model.Eventos = _eventoRepository.GetByDatas(dataMin, dataMax, model.Ativo);

                    //verificando se algum evento foi obtido
                    if(model.Eventos.Count > 0)
                    {
                        TempData["MensagemSucesso"] = $"{model.Eventos.Count} Evento(s) obtido(s) para a pesquisa";
                    }
                    else
                    {
                        TempData["MensagemAlerta"] = "Nenhum evento foi encontrado para a pesquisa realizada";
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

            return View(model);
        }

        public IActionResult Edicao()
        {
            return View();
        }

        public IActionResult Relatorio()
        {
            return View();
        }
    }
}
