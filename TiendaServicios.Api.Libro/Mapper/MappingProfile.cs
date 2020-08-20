using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Dto;
using TiendaServicios.Api.Libro.Modelo;

namespace TiendaServicios.Api.Libro.Mapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<LibreriaMaterial, LibreriaMaterialDto>();
        }
    }
}
