using ApiMarcadoresDaniel.Models;
using ApiMarcadoresDaniel.Resultados.Marcadores;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApiMarcadoresDaniel.Service.Marcadores
{
    public class GetMarcadores_Business
    {
        public class GetMarcadoresComando : IRequest<List<ItemMarcador>>
        {
            
        }

        public class EjecutaValidacion : AbstractValidator<GetMarcadoresComando>
        {
            public EjecutaValidacion()
            {
                
            }
        }

        public class Manejador : IRequestHandler<GetMarcadoresComando, List<ItemMarcador>>
        {
            private readonly IValidator<GetMarcadoresComando> _validator;

            public Manejador(IValidator<GetMarcadoresComando> validator)
            {
                _validator = validator;
            }

            public async Task<List<ItemMarcador>> Handle(GetMarcadoresComando request, CancellationToken cancellationToken)
            {
                var validation = await _validator.ValidateAsync(request);

                if (!validation.IsValid)
                {
                    // como no hay validaciones, no voy a tener errores de validacion
                }

                using (HttpClient client = new HttpClient())
                {
                    string user = "daniel";
                    string password = "secreta";

                    var data = new
                    {
                        nombreUsuario = user,
                        password = password
                    };

                    string jsonContent = JsonConvert.SerializeObject(data);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync("https://prog3.nhorenstein.com/api/usuario/LoginUsuarioWeb", content);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var datos = JsonConvert.DeserializeObject<UsuarioTokenResponse>(jsonResponse);
                        string idUsuario = datos.idUsuario;
                        string token = datos.token;

                        return await ObtenerMarcadores(idUsuario, token);
                    }
                }

              
                return null;
            }

            private async Task<List<ItemMarcador>> ObtenerMarcadores(string usuario, string token)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage response = await client.GetAsync("https://prog3.nhorenstein.com/api/marcador/GetMarcadores");

                    var nuevaListaMarcadores = new List<ItemMarcador>();

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var respuesta = JsonConvert.DeserializeObject<RespuestaMarcadores>(jsonResponse);

                        if (respuesta != null && respuesta.Ok)
                        {
                            nuevaListaMarcadores.AddRange(respuesta.LitadoMarcadores);
                            return nuevaListaMarcadores;
                        }
                    }
                }               

                return null;
            }
        }


    }
}

