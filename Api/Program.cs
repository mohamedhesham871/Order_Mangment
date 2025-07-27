
using Api.Middleware;
using Domin.Contract;
using Domin.Models.identitys;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.Contexts;
using Persistence.DataSeeding;
using Persistence.Repository;
using service;
using ServiceAbstraction;
using Shared;
using System.Text;
using System.Text.Json.Serialization;


namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
           
            #region Contexts
            builder.Services.AddDbContext<ApplicationDbContexts>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                          .AddEntityFrameworkStores<ApplicationDbContexts>();

            #endregion
            #region Services
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IProductService,ProductServices>();
            builder.Services.AddScoped<IAuthServices, AuthServices>();
            builder.Services.AddScoped<ICustomerService, CustomerServices>();
            builder.Services.AddScoped<IOrderServices, OrderService>();
            builder.Services.AddScoped<IInvoiceService, InvoicesService>();
            builder.Services.AddScoped<IPaymentservice, PaymentService>();


            #endregion
            #region Token information
            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(key: "JWToption"));

            var JwtOptions = builder.Configuration.GetSection("JWToption").Get<JwtOptions>();

            builder.Services.AddAuthentication(Options =>
            {
                Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })//Check validation of token
                .AddJwtBearer(Options =>
                 Options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,


                     ValidIssuer = JwtOptions.Issuer,
                     ValidAudience = JwtOptions.Audience,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SecretKey))
                 });
            #endregion

            var app = builder.Build();
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();
            SeedingRole.SeedingUser(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
