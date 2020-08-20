using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Dto;
using TiendaServicios.Api.CarritoCompra.Modelo;
using TiendaServicios.Api.CarritoCompra.Persistencia;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;
using TiendaServicios.Api.CarritoCompra.RemoteModel;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
    public class Consulta
    {
        public class Ejecuta : IRequest<CarritoDto>
        {
            public int CarritoSesionId { get; set; }
        }
        public class Manejador : IRequestHandler<Ejecuta, CarritoDto>
        {
            private readonly ContextoMysql _contexto;
            private readonly ILibroService _libroService;
            private readonly IMapper _mapper;

            public Manejador(ContextoMysql contexto,ILibroService libroService, IMapper mapper)
            {
                _contexto = contexto;
                _libroService = libroService;
                _mapper = mapper;
            }
            public async Task<CarritoDto> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                CarritoSesion carritoSesion = await _contexto.CarritoSesion.FirstOrDefaultAsync(x=> x.CarritoSesionId == request.CarritoSesionId);
                List<CarritoSesionDetalle> carritoSesionDetalle = await _contexto.CarritoSesionDetalle.Where(x => x.CarritoSesionId == request.CarritoSesionId).ToListAsync();
                List<CarritoDetalleDto> carritoDetalleDtos = new List<CarritoDetalleDto>();
                foreach(var libro in carritoSesionDetalle)
                {
                   var response = await _libroService.GetLibro(new Guid(libro.ProductoSeleccionado));
                    if (response.resultado)
                    {
                        LibroRemote ObjetoLibro = response.libro;
                        CarritoDetalleDto carritoDetalle = _mapper.Map<LibroRemote, CarritoDetalleDto>(ObjetoLibro);
                        carritoDetalleDtos.Add(carritoDetalle);
                    }
                }
                CarritoDto carritoDto = new CarritoDto()
                {
                    CarritoId = carritoSesion.CarritoSesionId,
                    FechaCreacionSesion = carritoSesion.FechaCreacion,
                    ListaProductos = carritoDetalleDtos
                };
                return carritoDto;
            }
        }
    }
}
