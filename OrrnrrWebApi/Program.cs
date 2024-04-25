
using DataAccessLib.Models;
using OrrnrrWebApi.Authorization;
using OrrnrrWebApi.Filters;
using OrrnrrWebApi.Services;

namespace OrrnrrWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            AuthProviderManager.SecretKey = builder.Configuration.GetValue<string>("SecretKey") ?? throw new ArgumentNullException("SecretKey");
            ContextManager.Instance.UseOrrnrrContext(builder.Configuration.GetConnectionString("orrnrr")?? throw new ArgumentNullException("orrnrr connectionString", "구성 파일에서 orrnrr 연결문자열을 찾을 수 없습니다."));

            //Console.WriteLine(AuthProviderManager.CreateJwtProvider().CreateSecondSuperUserAccessToken());

            // Add services to the container.
            builder.Services.AddControllers(options =>
            {
                // Action Filters
                options.Filters.Add<CommonLogingFilter>(1);
                options.Filters.Add<ExceptionFilter>(11);
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<ITokenSourceService, TokenSourceService>();
            builder.Services.AddScoped<IOrdersService, OrdersService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ITransactionService, TransactionService>();
            builder.Services.AddScoped(x => ContextManager.Instance.OrrnrrContext);

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
