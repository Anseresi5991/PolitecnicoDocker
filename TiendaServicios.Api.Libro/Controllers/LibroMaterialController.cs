using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiendaServicios.Api.Libro.Aplicacion;
using TiendaServicios.Api.Libro.Dto;

namespace TiendaServicios.Api.Libro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroMaterialController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LibroMaterialController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [Route("Crear")]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecutar data)
        {
            return await _mediator.Send(data);
        }
        [HttpGet]
        [Route("TraerLista")]
        public async Task<ActionResult<List<LibreriaMaterialDto>>> TraerLibros()
        {
            return await _mediator.Send(new Consulta.Ejecuta());
        }

        [HttpGet]
        [Route("TraerLibro")]
        public async Task<ActionResult<LibreriaMaterialDto>> TraerLibro(Guid id)
        {
            return await _mediator.Send(new ConsultaFiltro.LibroUnico() { 
            LibroId = id
            });
        }
    }
}
