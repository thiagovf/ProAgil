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
            try {
                var eventos = await repository.GetAllEventosAssync(includePalestrantes);
                return Ok(eventos);
            } 
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
        }

        [HttpGet("{EventoId}")]
        public async Task<IActionResult> Get(int eventoId)
        {
            try 
            {
                var evento = await repository.GetEventoAssync(eventoId, includePalestrantes);
                return Ok(evento);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
        }

        [HttpGet("getByTema/{tema}")]
        public async Task<IActionResult> Get(string tema)
        {
            try 
            {
                var eventos = await repository.GetAllEventosAssync(tema, includePalestrantes);
                return Ok(eventos);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Post(Evento evento)
        {
            try
            {
                repository.Add(evento);
                if (await repository.SaveChangesAssync())
                {
                    return Created($"/api/evento/{evento.Id}", evento);
                } 
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
            return NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> Put(Evento evento)
        {
            try 
            {
                Evento eventoDoBanco = await repository.GetEventoAssync(evento.Id, false);
                if (eventoDoBanco == null) return NotFound();
                
                repository.Update(evento);
                if (await repository.SaveChangesAssync())
                {
                    return Created($"/api/evento/{evento.Id}", evento);
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
            return BadRequest();
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete(int eventoId)
        {
            try 
            {
                Evento eventoDoBanco = await repository.GetEventoAssync(eventoId, false);
                if (eventoDoBanco == null) return NotFound();
                
                repository.Delete(eventoDoBanco);
                if (await repository.SaveChangesAssync())
                {
                    return Ok();
                }
            } 
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
            return BadRequest();
        }
    }
}