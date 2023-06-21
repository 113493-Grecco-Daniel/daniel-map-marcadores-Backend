using ApiMarcadoresDaniel.Resultados.Marcadores;
using ApiMarcadoresDaniel.Service.Marcadores;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

    namespace ApiMarcadoresDaniel.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class MarcadoresController : ControllerBase
        {
            private readonly IMediator _mediator;

            public MarcadoresController(IMediator mediator)
            {
                _mediator = mediator;
            }

            [HttpGet]
            public async Task<ListaMarcadores> ObtenerMarcadores()
            {
                var resultado= new ListaMarcadores();
                var marcadores = await _mediator.Send(new GetMarcadores_Business.GetMarcadoresComando());

                if (marcadores != null)
                {
                    foreach (var item in marcadores)
                    {
                        var itemMarcador = new ItemMarcador();
                        itemMarcador.Latitud = item.Latitud;
                        itemMarcador.Longitud = item.Longitud;
                        itemMarcador.Info = item.Info;

                        resultado.ListMarcadores.Add(itemMarcador);
                    }
                    resultado.StatusCode= System.Net.HttpStatusCode.OK;
                    resultado.Ok = true;
                
                    return resultado;
                }
                else
                {
                    resultado.StatusCode = System.Net.HttpStatusCode.NotFound;
                    resultado.MensajeError = "No se encontraron marcadores";
                    resultado.Ok = false;

                    return resultado;
                }
            }        
        }
    }
