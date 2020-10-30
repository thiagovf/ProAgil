using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PalestranteController : ControllerBase
    {
        private readonly IProAgilRepository repository;
        private readonly bool includePalestrantes = true;
        public PalestranteController(IProAgilRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("{PalestranteId}")]
        public async Task<IActionResult> Get(int palestranteId)
        {
            try 
            {
                var palestrante = await repository.GetPalestranteAssync(palestranteId, includePalestrantes);
                return Ok(palestrante);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
        }

        [HttpGet("{Nome}")]
        public async Task<IActionResult> Get(string nome)
        {
            try 
            {
                var palestrantes = await repository.GetAllPalestrantesAssync(nome, includePalestrantes);
                return Ok(palestrantes);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Palestrante palestrante)
        {
            try 
            {
                repository.Add(palestrante);
                if (await repository.SaveChangesAssync())
                {
                    return Created($"/api/palestrante/{palestrante.Id}", palestrante);
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Put(int palestranteId, Palestrante palestrante)
        {
            try 
            {
                Palestrante palestranteDoBanco = await repository.GetPalestranteAssync(palestranteId, false);
                if (palestranteDoBanco == null) return NotFound();

                repository.Update(palestrante);
                if (await repository.SaveChangesAssync())
                {
                    return Created($"/api/palestrante/{palestrante.Id}", palestrante);
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,"Banco de Dados Falhou");
            }
            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int palestranteId)
        {
            try 
            {
                Palestrante palestranteDoBanco = await repository.GetPalestranteAssync(palestranteId, false);
                if (palestranteDoBanco == null) return NotFound();

                repository.Delete(palestranteDoBanco);
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