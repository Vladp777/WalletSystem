using Application.Accounts.Commands;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;


namespace WebApi.IntegrationTests
{
    public class AccountControllerTests: IntegrationTest
    {
        public AccountControllerTests(ApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Create_ShouldReturnUnauthorizedError_WhenNotAuthrized()
        {
            var account = new CreateAccount(Guid.NewGuid(), "ee", 150);

            var response = await _httpClient.PostAsJsonAsync("api/Accounts", account);
            var createdAccount = response.Content.ReadFromJsonAsync<ObjectResult>();

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Create_ShouldCreateAccount_WhenValidData()
        {

            var userId = await AuthenticateAsync();

            var account = new CreateAccount(Guid.Parse(userId), "Test", 100);


            var response = await _httpClient.PostAsJsonAsync("api/Accounts", account);
            var createdAccount = await response.Content.ReadFromJsonAsync<Account>();

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(createdAccount);
        }

        [Fact]
        public async Task Get_ShouldCreateAccount_WhenValidData()
        {

            var userId = await AuthenticateAsync(1);

            var createAccount = new CreateAccount(Guid.Parse(userId), "Test", 100);

            var response = await _httpClient.PostAsJsonAsync("api/Accounts", createAccount);
            var account = await response.Content.ReadFromJsonAsync<Account>();

            //Act
            var result = await _httpClient.GetFromJsonAsync<Account>($"api/Accounts/{account.Id}");


            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(account.Id, result.Id);
        }

        [Fact]
        public async Task Delete_ShouldDeleteAccount_WhenValidData()
        {

            var userId = await AuthenticateAsync(1);

            var createAccount = new CreateAccount(Guid.Parse(userId), "Test", 100);

            var response = await _httpClient.PostAsJsonAsync("api/Accounts", createAccount);
            var account = await response.Content.ReadFromJsonAsync<Account>();

            //Act
            var result = await _httpClient.DeleteAsync($"api/Accounts/{account.Id}");
            var deletedaccount = await result.Content.ReadFromJsonAsync<Account>();


            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(account.Id, deletedaccount.Id);
        }

        [Fact]
        public async Task Update_ShouldUpdateAccount_WhenValidData()
        {
            //Arrange
            var userId = await AuthenticateAsync(1);

            var createAccount = new CreateAccount(Guid.Parse(userId), "Test", 100);

            var response = await _httpClient.PostAsJsonAsync("api/Accounts", createAccount);
            var account = await response.Content.ReadFromJsonAsync<Account>();

            var updatedAccount = new UpdateAccount(account.Id, "New Test", 999);

            //Act
            var result = await _httpClient.PutAsJsonAsync($"api/Accounts", updatedAccount);
            var deletedaccount = await result.Content.ReadFromJsonAsync<Account>();


            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(account.Id, deletedaccount.Id);
        }

        [Fact]
        public async Task GetAll_ShouldCreateAccount_WhenValidData()
        {

            var userId = await AuthenticateAsync(1);

            var createAccount = new CreateAccount(Guid.Parse(userId), "Test", 100);

            var response = await _httpClient.PostAsJsonAsync("api/Accounts", createAccount);
            var account = await response.Content.ReadFromJsonAsync<Account>();

            //Act
            var result = await _httpClient.GetFromJsonAsync<List<Account>>($"api/Accounts/getAll/{account.UserId}");

            Assert.NotNull(result);
            Assert.Equal(account.UserId, result.First().UserId);
        }
    }
}
