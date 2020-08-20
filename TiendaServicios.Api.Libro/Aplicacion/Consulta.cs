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
    public class Consulta
    {
        public class Ejecuta : IRequest<List<LibreriaMaterialDto>> { }
        public class Manejador : IRequestHandler<Ejecuta, List<LibreriaMaterialDto>>
        {
            private readonly ContextoSqlserver _contexto;
            private readonly IMapper _mapper;
            public Manejador(ContextoSqlserver contexto,IMapper mapper)
            {
                _contexto = contexto;
                _mapper = mapper;
            }
            public async Task<List<LibreriaMaterialDto>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                List<LibreriaMaterial> libreriaMaterial = await _contexto.LibreriaMaterial.ToListAsync();
                List<LibreriaMaterialDto> libreriaMaterialDto=_mapper.Map<List<LibreriaMaterial>, List<LibreriaMaterialDto>>(libreriaMaterial);
                return libreriaMaterialDto;
            }
        }
    }
}
