using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Modelo;
using TiendaServicios.Api.CarritoCompra.Persistencia;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
    public class NuevoDetalle
    {
        public class Ejecuta : IRequest
        {
            public int SesionId { get; set; }
            public List<string> ProductoLista { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly ContextoMysql _contexto;
            public Manejador(ContextoMysql contexto)
            {
                _contexto = contexto;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                foreach(string obj in request.ProductoLista)
                {
                    CarritoSesionDetalle detalleSesion = new CarritoSesionDetalle()
                    {
                        FechaCreacion = DateTime.Now,
                        CarritoSesionId = request.SesionId,
                        ProductoSeleccionado = obj
                    };
                    _contexto.CarritoSesionDetalle.Add(detalleSesion);
                }
                int value =await _contexto.SaveChangesAsync();
                if (value > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("Error al insetar el detalle");
            }
        }
    }
}
