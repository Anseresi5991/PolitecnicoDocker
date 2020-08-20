using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServicios.Api.Autor.Dto;
using TiendaServicios.Api.Autor.Modelo;

namespace TiendaServicios.Api.Autor.Mapper
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<AutorLibro, AutorDto>();
        }
    }
}
