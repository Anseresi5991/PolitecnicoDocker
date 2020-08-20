using AutoMapper;
using GenFu;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiendaServicios.Api.Libro.Aplicacion;
using TiendaServicios.Api.Libro.Dto;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;
using Xunit;

namespace TiendaServicios.Api.Libro.Tests
{
    public class LibrosServiceTest
    {
        private IEnumerable<LibreriaMaterial> ObtenerDataPrueba()
        {
            // este metodo es para llenar con data de genfu
            A.Configure<LibreriaMaterial>()
                .Fill(x => x.Titulo).AsArticleTitle()
                .Fill(x => x.LibreriaMaterialId, () => { return Guid.NewGuid(); });

            List<LibreriaMaterial> lista = A.ListOf<LibreriaMaterial>(30);
            lista[0].LibreriaMaterialId = Guid.Empty;
            return lista;
        }

        private Mock<ContextoSqlserver> CrearContexto()
        {
            IQueryable<LibreriaMaterial> dataPrueba = ObtenerDataPrueba().AsQueryable();
            Mock<DbSet<LibreriaMaterial>> dbSet = new Mock<DbSet<LibreriaMaterial>>();
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider).Returns(dataPrueba.Provider);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Expression).Returns(dataPrueba.Expression);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.ElementType).Returns(dataPrueba.ElementType);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.GetEnumerator()).Returns(dataPrueba.GetEnumerator());

            dbSet.As<IAsyncEnumerable<LibreriaMaterial>>()
            .Setup(x => x.GetAsyncEnumerator(new System.Threading.CancellationToken()))
            .Returns(new AsyncEnumerator<LibreriaMaterial>(dataPrueba.GetEnumerator()));

            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x=>x.Provider).Returns(new AsyncQueryProvider<LibreriaMaterial>(dataPrueba.Provider));

            Mock<ContextoSqlserver> contexto = new Mock<ContextoSqlserver>();
            contexto.Setup(x => x.LibreriaMaterial).Returns(dbSet.Object);
            return contexto;
        }

        [Fact]
        public async void GetLibroPorId()
        {
            Mock<ContextoSqlserver> mockContexto = CrearContexto();

            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingTest());
            });
            IMapper mapper = mapConfig.CreateMapper();

            ConsultaFiltro.LibroUnico request = new ConsultaFiltro.LibroUnico();
            request.LibroId = Guid.Empty;

            ConsultaFiltro.Manejador manejador = new ConsultaFiltro.Manejador(mockContexto.Object,mapper);
            LibreriaMaterialDto libro = await manejador.Handle(request,new System.Threading.CancellationToken());
            Assert.NotNull(libro);
            Assert.True(libro.LibreriaMaterialId == Guid.Empty);

        }

        [Fact]
        public async void GetLibros()
        {
            //System.Diagnostics.Debugger.Launch();
            Mock<ContextoSqlserver> mockContexto = CrearContexto();

            MapperConfiguration mapConfig = new MapperConfiguration(cfg=> {
                cfg.AddProfile(new MappingTest());
            });
            IMapper mapper = mapConfig.CreateMapper();

            Consulta.Manejador manejador = new Consulta.Manejador(mockContexto.Object, mapper);

            Consulta.Ejecuta request = new Consulta.Ejecuta();
            List<LibreriaMaterialDto> lista = await manejador.Handle(request, new System.Threading.CancellationToken());
            Assert.True(lista.Any());    
        }

        [Fact]
        public async void GuardarLibro()
        {
            DbContextOptions<ContextoSqlserver> options = new DbContextOptionsBuilder<ContextoSqlserver>()
                .UseInMemoryDatabase(databaseName: "BaseDatosLibro")
                .Options;

            ContextoSqlserver contexto = new ContextoSqlserver(options);

            Nuevo.Ejecutar request = new Nuevo.Ejecutar();
            request.Titulo = "Libro Microservice";
            request.AutorLibro = Guid.Empty;
            request.FechaPublicacion = DateTime.Now;

            Nuevo.Manejador manejador = new Nuevo.Manejador(contexto);
            var libro = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.True(libro != null);
        }
    }
}
