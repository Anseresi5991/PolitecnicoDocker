using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Modelo;

namespace TiendaServicios.Api.Libro.Persistencia
{
    public class ContextoSqlserver:DbContext
    {
        public ContextoSqlserver() { }
        public ContextoSqlserver(DbContextOptions<ContextoSqlserver> options) : base(options)
        {
        }
        public virtual DbSet<LibreriaMaterial> LibreriaMaterial { get; set; }
    }
}
