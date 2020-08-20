using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;
using TiendaServicios.Api.CarritoCompra.RemoteModel;

namespace TiendaServicios.Api.CarritoCompra.RemoteService
{
    public class LibrosService : ILibroService
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<LibrosService> _logger;

        public LibrosService(IHttpClientFactory httpClient,ILogger<LibrosService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<(bool resultado, LibroRemote libro, string ErrorMessage)> GetLibro(Guid LibroId)
        {
            try
            {
                var cliente = _httpClient.CreateClient("Libros");
                var response =await cliente.GetAsync($"api/LibroMaterial/TraerLibro?id={LibroId}");
                if (response.IsSuccessStatusCode)
                {
                    string contenido = await response.Content.ReadAsStringAsync();
                    JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    LibroRemote libroRemote = JsonSerializer.Deserialize<LibroRemote>(contenido, options);
                    return (true, libroRemote, null);
                }
                return (false, null, response.ReasonPhrase);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return (false, null, e.Message);
            }
            throw new NotImplementedException();
        }
    }
}
