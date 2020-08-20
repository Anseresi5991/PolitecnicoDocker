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
    public class ConsultaFiltro
    {
        public class AutorFiltro : IRequest<AutorDto> {
            public string AutorGuid { get; set; }
        }

        public class Manejador:IRequestHandler<AutorFiltro, AutorDto> {
            private readonly ContextoPostgre _contexto;
            private readonly IMapper _mapper;
            public Manejador(ContextoPostgre contextoPostgre, IMapper mapper)
            {
                _mapper = mapper;
                _contexto = contextoPostgre;
            }

            public async Task<AutorDto> Handle(AutorFiltro request, CancellationToken cancellationToken)
            {
                AutorLibro autorLibro = await _contexto.AutorLibro.Where(a => a.AutorLibroGuid == request.AutorGuid).FirstOrDefaultAsync();
                if(autorLibro == null)
                {
                    throw new Exception("No se encontro el autor");
                }
                AutorDto autorDto= _mapper.Map<AutorLibro, AutorDto>(autorLibro);
                return autorDto;
            }
        }
    }
}
