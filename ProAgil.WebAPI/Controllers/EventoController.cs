using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;
using ProAgil.WebAPI.DTO;

namespace ProAgil.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IProAgilRepository repository;
        private readonly bool includePalestrantes = true;
        private readonly IMapper mapper;

        public EventoController(IProAgilRepository repository, IMapper mapper)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await repository.GetAllEventosAssync(includePalestrantes);
                var eventosDTO = mapper.Map<EventoDTO[]>(eventos);
                return Ok(eventosDTO);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload() 
        {
            try 
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                    var fullPath = Path.Combine(pathToSave, fileName.Replace("\"", "").Trim());

                    using (var stream = new FileStream(fullPath, FileMode.Create)) 
                    {
                        file.CopyTo(stream);
                    }
                }
                return Ok();
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
                var eventoDTO = mapper.Map<EventoDTO>(evento);
                return Ok(eventoDTO);
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
                var eventosDTO = mapper.Map<EventoDTO[]>(eventos);
                return Ok(eventosDTO);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventoDTO eventoDTO)
        {
            try
            {
                Evento evento = mapper.Map<Evento>(eventoDTO);
                repository.Add(evento);
                if (await repository.SaveChangesAssync())
                {
                    return Created($"/api/evento/{evento.Id}", mapper.Map<EventoDTO>(evento));
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
            return NotFound();
        }

        [HttpPut("{EventoId}")]
        public async Task<IActionResult> Put(int eventoId, EventoDTO eventoDTO)
        {
            try
            {
                Evento eventoDoBanco = await repository.GetEventoAssync(eventoId, false);
                if (eventoDoBanco == null) return NotFound();

                mapper.Map(eventoDTO, eventoDoBanco);

                repository.Update(eventoDoBanco);
                if (await repository.SaveChangesAssync())
                {
                    return Created($"/api/evento/{eventoId}", mapper.Map<EventoDTO>(eventoDoBanco));
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados Falhou");
            }
            return BadRequest();
        }

        [HttpDelete("{EventoId}")]
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