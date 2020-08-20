using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Autor.Modelo;
using TiendaServicios.Api.Autor.Persistencia;

namespace TiendaServicios.Api.Autor.Application
{
    public class Nuevo
    {
        public class Ejecuta:IRequest {

            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public DateTime? FechaNacimiento { get; set; }
        }
        public class EjecutaValidacion: AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellido).NotEmpty();
            }
        }
        public class Manejador : IRequestHandler<Ejecuta>
        {
            public readonly ContextoPostgre _contexto;

            public Manejador(ContextoPostgre contextoPostgre)
            {
                _contexto = contextoPostgre;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                AutorLibro autorLibro = new AutorLibro
                {
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                    FechaNacimiento = request.FechaNacimiento,
                    AutorLibroGuid = Guid.NewGuid().ToString()
                    
                };
                _contexto.AutorLibro.Add(autorLibro);
                int valor =await _contexto.SaveChangesAsync();
                if (valor > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("Error al insetar autor");
            }
        }
    }
}
