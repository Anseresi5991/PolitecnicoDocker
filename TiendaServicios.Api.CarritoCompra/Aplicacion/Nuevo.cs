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
    public class Nuevo
    {
        public class Ejecuta : IRequest<int>
        {
            public DateTime FechaCreacionSesion { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta,int>
        {
            private readonly ContextoMysql _contexto;
            public Manejador(ContextoMysql contexto)
            {
                _contexto = contexto;
            }
            

            async Task<int> IRequestHandler<Ejecuta, int>.Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                CarritoSesion carritoSesion = new CarritoSesion()
                {
                    FechaCreacion = request.FechaCreacionSesion
                };
                _contexto.CarritoSesion.Add(carritoSesion);
                int value = await _contexto.SaveChangesAsync();
                if (value == 0)
                {
                    throw new Exception("Errores en la insercion del carrito de compra");
                }

                return carritoSesion.CarritoSesionId;
            }
        }
    }
}
