using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Autor.Dto;
using TiendaServicios.Api.Autor.Modelo;
using TiendaServicios.Api.Autor.Persistencia;

namespace TiendaServicios.Api.Autor.Application
{
    public class Consulta
    {
        public class ListaAutor : IRequest<List<AutorDto>>{}
        public class Manejador : IRequestHandler<ListaAutor, List<AutorDto>>
        {
            private readonly ContextoPostgre _contexto;
            private readonly IMapper _mapper;
            public Manejador(ContextoPostgre contextoPostgre,IMapper mapper)
            {
                _mapper = mapper;
                _contexto = contextoPostgre;
            }
            public async Task<List<AutorDto>> Handle(ListaAutor request, CancellationToken cancellationToken)
            {
                List<AutorLibro> autorLibro = await _contexto.AutorLibro.ToListAsync();
                List<AutorDto> listAutorDto = _mapper.Map<List<AutorLibro>, List<AutorDto>>(autorLibro);
                return listAutorDto;
            }
        }
    }
}
