using Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.SqlEdge;

namespace WebApi.IntegrationTests
{
    public class ApiFactory: WebApplicationFactory<Program>, IAsyncLifetime
    {
        private SqlEdgeContainer _db = new SqlEdgeBuilder()
           
            .WithPassword("password1!")
            .Build();

        public HttpClient HttpClient;

        public async Task InitializeAsync()
        {
            await _db.StartAsync();
            HttpClient = CreateClient();
            //_dbConnection = new SqlConnection(_db.GetConnectionString());
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<ApplicationDbContext>(opts => opts.UseSqlServer(_db.GetConnectionString()));
            });
        }

        public async Task DisposeAsync()
        {
            await _db.StopAsync();
        }
    }
}
