using System.Net.Http;
using System.Threading.Tasks;
using Middleware_REST_API.Model;
using Newtonsoft.Json;

namespace Middleware_REST_API.Services
{
    public class ExternalUserService
    {
        private readonly HttpClient _httpClient;

        public ExternalUserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<User> GetUsersFromExternalApi(string username)
        {
            var response = await _httpClient.GetAsync("https://dummyjson.com/users");
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<UserResponse>(responseContent);

            var user = users.Users.FirstOrDefault(u => u.Username == username);


            return user;
        }
    }
}