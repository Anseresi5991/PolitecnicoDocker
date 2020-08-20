using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;

namespace TiendaServicios.Api.Libro.Aplicacion
{
    public class Nuevo
    {
        public class Ejecutar : IRequest
        {
            public string Titulo { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public Guid? AutorLibro { get; set; }
        }
        public class EjecutaValidacion : AbstractValidator<Ejecutar>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
                RuleFor(x => x.AutorLibro).NotEmpty();
            }
        }
        public class Manejador : IRequestHandler<Ejecutar>
        {
            private readonly ContextoSqlserver _contexto;
            public Manejador(ContextoSqlserver contexto)
            {
                _contexto = contexto;
            }

            public async Task<Unit> Handle(Ejecutar request, CancellationToken cancellationToken)
            {
                LibreriaMaterial libreriaMaterial = new LibreriaMaterial()
                {
                    Titulo = request.Titulo,
                    FechaPublicacion = request.FechaPublicacion,
                    AutorLibro = request.AutorLibro
                };
                _contexto.LibreriaMaterial.Add(libreriaMaterial);
                int value =await _contexto.SaveChangesAsync();
                if (value > 0)
                    return Unit.Value;
                throw new Exception("No se pudo guardar el libro");
            }
        }
    }
}
