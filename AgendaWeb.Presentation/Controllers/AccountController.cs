using AgendaWeb.Infra.Data.Entities;
using AgendaWeb.Infra.Data.Interfaces;
using AgendaWeb.Infra.Data.Utils;
using AgendaWeb.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgendaWeb.Presentation.Controllers
{
    public class AccountController : Controller
    {
        //atributo
        private readonly IUsuarioRepository _usuarioRepository;

        //Construtor para inicialização  do atributo
        public AccountController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(AccountLoginViewModel model) 
        {
            if (ModelState.IsValid)
            {
                try
                {
                    

                    //procurar o usuario no banco de dados atraves do email e senha
                    var usuario = _usuarioRepository.GetByEmailESenha(model.Email, CriptografiaUtil.GetMD5(model.Senha));

                    if(usuario != null)
                    {
                        TempData["MensagemSucesso"] = $"Parabéns, {usuario.Nome}! Acesso ao sistema realizado com sucesso.";

                        HttpContext.Session.SetString("nome_usuario", usuario.Nome);

                        //redirecionar para a página inical do projeto
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["MensagemErro"] = "Acesso negado. Usuário inválido";
                    }
                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = ex.Message;
                }
            }

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(AccountRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //verificar se o email informado já está cadastrado no banco de dados
                    if (_usuarioRepository.GetByEmail(model.Email) != null)
                    {
                        TempData["MensagemErro"] = $"O email informado já está cadastrado. Tente outro.";
                    }
                    else
                    {
                        var usuario = new Usuario();

                        usuario.Id = Guid.NewGuid();
                        usuario.Nome = model.Nome;
                        usuario.Email = model.Email;
                        usuario.Senha = CriptografiaUtil.GetMD5(model.Senha);
                        usuario.DataInclusao = DateTime.Now;

                        _usuarioRepository.Create(usuario); //cadastrando o usuário

                        TempData["MensagemSucesso"] = "Usuário cadastrado com sucesso!";
                        ModelState.Clear(); //limpar os campos do formulário
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
