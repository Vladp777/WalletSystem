using Application.Authentication.Commands;
using Application.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Application.Authentication.Queries;

namespace WebApi.IntegrationTests
{
    public class IntegrationTest: IClassFixture<ApiFactory>
    {
        protected readonly HttpClient _httpClient;

        protected IntegrationTest(ApiFactory factory)
        {
            _httpClient = factory.HttpClient;
        }

        protected async Task<string> AuthenticateAsync(int i = default)
        {
            var response = await GetJwtAsync(i);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", response.Token);

            return response.UserId;
        }

        private async Task<AuthenticationResult> GetJwtAsync(int i)
        {
            HttpResponseMessage response;
            if (i == 0)
            {
                response = await _httpClient.PostAsJsonAsync("api/Auth/register", new CreateUserCommand
                (
                    "test@integration.com",
                    "Name",
                    "Password1@!"
                ));
            }
            else
            {
                response = await _httpClient.PostAsJsonAsync("api/Auth/login", new LoginUserQuery
                (
                    "test@integration.com",
                    "Password1@!"
                ));
            }


            var registrationResponse = await response.Content.ReadFromJsonAsync<AuthenticationResult>();

            return registrationResponse;
        }
    }
}
