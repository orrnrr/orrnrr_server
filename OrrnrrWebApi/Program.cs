
using DataAccessLib.Models;
using OrrnrrWebApi.Exceptions;

namespace OrrnrrWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            ContextManager.Instance.UseOrrnrrContext(config.GetConnectionString("orrnrr") ?? throw new ArgumentNullException(nameof(config), "���� ���Ͽ��� orrnrr ���Ṯ�ڿ��� ã�� �� �����ϴ�."));

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(options => options.Filters.Add<ExceptionFilter>());

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
