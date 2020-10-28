using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IProAgilRepository repository;
        private readonly bool includePalestrantes = true;

        public EventoController(IProAgilRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            ObjectResult resultado;
            try {
                var eventos = await repository.GetAllEventosAssync(includePalestrantes);
                resultado = Ok(eventos);
            } 
            catch (System.Exception)
            {
                resultado = this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
            return resultado;
        }

        [HttpGet("{EventoId}")]
        public async Task<IActionResult> Get(int eventoId)
        {
            ObjectResult resultado;
            try 
            {
                var evento = await repository.GetEventoAssync(eventoId, includePalestrantes);
                resultado = Ok(evento);
            }
            catch (System.Exception)
            {
                resultado = this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
            return resultado;
        }

        [HttpGet("getByTema/{tema}")]
        public async Task<IActionResult> Get(string tema)
        {
            ObjectResult resultado;
            try 
            {
                var eventos = await repository.GetAllEventosAssync(tema, includePalestrantes);
                resultado = Ok(eventos);
            }
            catch (System.Exception)
            {
                resultado = this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
            return resultado;
        }
        
        [HttpPost]
        public async Task<IActionResult> Post(Evento evento)
        {
            ObjectResult resultado;
            try
            {
                repository.Add(evento);
                if (await repository.SaveChangesAssync())
                {
                    resultado = Created($"/api/evento/{evento.Id}", evento);
                } 
                else 
                {
                    resultado = this.StatusCode(StatusCodes.Status400BadRequest, "");
                }
            }
            catch (System.Exception)
            {
                resultado = this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
            return resultado;
        }

        [HttpPut]
        public async Task<IActionResult> Put(int eventoId, Evento evento)
        {
            ObjectResult resultado;
            try 
            {
                Evento eventoDoBanco = await repository.GetEventoAssync(eventoId, false);
                if (eventoDoBanco == null)
                {
                    resultado = this.StatusCode(StatusCodes.Status404NotFound, "");
                }
                else 
                {
                    repository.Update(evento);
                    if (await repository.SaveChangesAssync())
                    {
                        resultado = Created($"/api/evento/{evento.Id}", evento);
                    }
                    else 
                    {
                        resultado = this.StatusCode(StatusCodes.Status400BadRequest, "");
                    }
                }
            }
            catch (System.Exception)
            {
                resultado = this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
            return resultado;
        }
    }
}