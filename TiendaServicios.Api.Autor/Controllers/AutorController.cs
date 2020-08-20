using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiendaServicios.Api.Autor.Application;
using TiendaServicios.Api.Autor.Dto;
using TiendaServicios.Api.Autor.Modelo;

namespace TiendaServicios.Api.Autor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AutorController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta request)
        {
            return await _mediator.Send(request);
        }

        [HttpGet]
        [Route("Autores")]
        public async Task<ActionResult<List<AutorDto>>> GetAutores()
        {
            return await _mediator.Send(new Consulta.ListaAutor());
        }

        [HttpGet]
        [Route("Autor")]
        public async Task<ActionResult<AutorDto>> GetAutor(string id)
        {
            return await _mediator.Send(new ConsultaFiltro.AutorFiltro{ 
            AutorGuid = id
            });
        }


    }
}
