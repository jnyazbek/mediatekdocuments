using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
namespace MediaTekDocuments.manager
{
    /// <summary>
    /// Classe indépendante d'accès à une api rest avec éventuellement une "basic authorization"
    /// </summary>
    class ApiRest
    {
        /// <summary>
        /// unique instance de la classe
        /// </summary>
        private static ApiRest instance = null;
        /// <summary>
        /// Objet de connexion à l'api
        /// </summary>
        private readonly HttpClient httpClient;
        /// <summary>
        /// Canal http pour l'envoi du message et la récupération de la réponse
        /// </summary>
        private HttpResponseMessage httpResponse;

        /// <summary>
        /// Constructeur privé pour préparer la connexion (éventuellement sécurisée)
        /// </summary>
        /// <param name="uriApi">adresse de l'api</param>
        /// <param name="authenticationString">chaîne d'authentification</param>
        private ApiRest(String uriApi, String authenticationString = "")
        {
            httpClient = new HttpClient() { BaseAddress = new Uri(uriApi) };
            // prise en compte dans l'url de l'authentificaiton (basic authorization), si elle n'est pas vide
            if (!String.IsNullOrEmpty(authenticationString))
            {
                String base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);
                Console.WriteLine("instance aiîrest initialisée "+ authenticationString );
            }
            else
            {
                Console.WriteLine("connectionstring null or empty " + authenticationString);
            }
        }

        /// <summary>
        /// Crée une instance unique de la classe
        /// </summary>
        /// <param name="uriApi">adresse de l'api</param>
        /// <param name="authenticationString">chaîne d'authentificatio (login:pwd)</param>
        /// <returns></returns>
        public static ApiRest GetInstance(String uriApi, String authenticationString)
        {
            if (instance == null)
            {
                instance = new ApiRest(uriApi, authenticationString);
            }
           
            return instance;
        }

       

        public JObject RecupDistant(string methode, string url, string jsonBody = null)
        {
             HttpResponseMessage httpResponse = null;
            HttpContent content = jsonBody != null ? new StringContent(jsonBody, Encoding.UTF8, "application/json") : null;

            Console.WriteLine($"Méthode: {methode}");
            Console.WriteLine($"URL: {url}");
            if (jsonBody != null)
            {
                Console.WriteLine($"Corps de la requête: {jsonBody}");
            }
            switch (methode)
            {
                case "GET":
                    Console.WriteLine($"Envoi d'une requête get  avec le corps " +jsonBody+" et l'url "+url);
                    httpResponse = httpClient.GetAsync(url).Result;
                    break;
                case "POST":
                    Console.WriteLine($"Envoi d'une requête post  avec le corps " + jsonBody);
                    httpResponse = httpClient.PostAsync(url, content).Result;
                    break;
                case "PUT":
                    httpResponse = httpClient.PutAsync(url, content).Result;
                    break;
                case "DELETE":
                    Console.WriteLine($"Envoi d'une requête delete  avec le corps " + jsonBody);
                    httpResponse = httpClient.DeleteAsync(url).Result;
                    break;
                default:
                    return new JObject();
            }

            if (httpResponse == null || !httpResponse.IsSuccessStatusCode)
            {
                Console.WriteLine("httpresponse is null or failed");
                return new JObject();
            }
            Console.WriteLine("HTT RESPONSE IDENTIFIED " + httpResponse);

            var responseString = httpResponse.Content.ReadAsStringAsync().Result;
            var cleanedResponseString = responseString;
            if (responseString.StartsWith("champs = "))
            {
                cleanedResponseString = responseString.Substring("champs = ".Length);
            }

            Console.WriteLine("Réponse JSON complèteeee: " + cleanedResponseString);

            return JObject.Parse(cleanedResponseString);
        }


      
        // Exemple d'utilisation



    }


}
