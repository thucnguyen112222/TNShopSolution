using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TNShopSolution.ViewModels.System.Users;

namespace TNShopWebApp.Service
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory httpClientFactory;
        public UserApiClient(IHttpClientFactory httpClient)
        {
            httpClientFactory = httpClient;
        }

        public async Task<string> Authenticate(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpcontent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = httpClientFactory.CreateClient();
            var response = await client.PostAsync("https://localhost:5001/api/users/Authenticate", httpcontent);
            var token = await response.Content.ReadAsStringAsync();
            return token;
        }
    }
}
