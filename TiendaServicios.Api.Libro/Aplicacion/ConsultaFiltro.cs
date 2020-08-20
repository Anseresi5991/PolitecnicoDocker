using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Dto;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;

namespace TiendaServicios.Api.Libro.Aplicacion
{
    public class ConsultaFiltro
    {
        public class LibroUnico : IRequest<LibreriaMaterialDto>
        {
            public Guid? LibroId { get; set; }
        }
        public class Manejador : IRequestHandler<LibroUnico, LibreriaMaterialDto>
        {
            private readonly ContextoSqlserver _contexto;
            private readonly IMapper _mapper;

            public Manejador(ContextoSqlserver contexto,IMapper mapper)
            {
                _contexto = contexto;
                _mapper = mapper;
            }
            public async Task<LibreriaMaterialDto> Handle(LibroUnico request, CancellationToken cancellationToken)
            {
                LibreriaMaterial libreriaMaterial =await _contexto.LibreriaMaterial.Where(lm => lm.LibreriaMaterialId == request.LibroId).FirstOrDefaultAsync();
                if (libreriaMaterial == null)
                {
                    throw new Exception("No se encontro el libro");
                }
                LibreriaMaterialDto libreriaMaterialDto = _mapper.Map<LibreriaMaterial, LibreriaMaterialDto>(libreriaMaterial);
                return libreriaMaterialDto;
            }
        }
    }
}
