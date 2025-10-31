using Microsoft.AspNetCore.Mvc;

namespace BancoDigital.ContaCorrente.API.V1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ContaCorrenteController : ControllerBase
    {
        [HttpPost]
        public Task<string> CadastrarContaCorrente(string cpf, string senha)
        {
            
        }
    }
}