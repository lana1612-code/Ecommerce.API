
using Ecommerce.API.mapping_profile;
using Ecommerce.Core.IRepositories;
using Ecommerce.Infrastracture.Repositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Core.Entities.DTO;
using Microsoft.AspNetCore.Identity;
using Ecommerce.Core.Entities;
using Ecommerce.Core.IRepositories.IService;
using System.Threading.RateLimiting;
using Ecommerce.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace Ecommerce.API
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
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).UseLazyLoadingProxies();
            }
             );
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (ActionContext) =>
                {
                    var errors = ActionContext.ModelState.Where(x=>x.Value.Errors.Count()>0)
                    .SelectMany(x=>x.Value.Errors)
                    .Select(e=>e.ErrorMessage).ToList();

                    var validation = new ApiValidationResponse(statusCode : 400) { Errors = errors };
                    return new BadRequestObjectResult(validation);
                    
                };
            });
            builder.Services.AddControllers(options =>
            {
                options.CacheProfiles.Add(
                    "defaultCache", new CacheProfile()
                    {
                        Duration = 30,
                        Location =ResponseCacheLocation.Any
                    });


            });
            //builder.Services.AddIdentity<LocalUser, IdentityRole<int>>()
            //   .AddEntityFrameworkStores<AppDbContext>()
            //   .AddDefaultTokenProviders();

            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped(typeof(IGenericRepositories<>), typeof(GenericRepositories<>));
            builder.Services.AddScoped(typeof(IProductRepository), typeof(ProductRepository));
            builder.Services.AddScoped(typeof(IUniteOfWork<>), typeof(UniteOfWork<>));
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddScoped(typeof(IUsersRepositories),typeof(UsersRepositories));
            builder.Services.AddScoped(typeof(ITokenService), typeof(TokenService));
            builder.Services.AddTransient<IEmailService, EmailService>();

            builder.Services.Configure<DataProtectionTokenProviderOptions>(option =>
            {
                option.TokenLifespan= TimeSpan.FromHours(1);
            });

            //builder.Services.AddAutoMapper(typeof(UserManager<LocalUser>));
            //builder.Services.AddAutoMapper(typeof(RoleManager<IdentityRole>));
            //builder.Services.AddAutoMapper(typeof(SignInManager<IdentityRole>));

            var key = builder.Configuration.GetValue<string>("apiSetting:SecretKey");

            builder.Services.AddAuthentication(
                option =>
                {
                    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(option =>
                {
                    option.RequireHttpsMetadata = false;
                    option.SaveToken = true;
                    option.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                        ValidateAudience=false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key))
                    };

                });

            builder.Services.AddAuthentication();
            builder.Services.AddIdentity<LocalUser, IdentityRole>(option =>
            {
                option.Password.RequiredLength = 1;
                option.Password.RequireUppercase = false;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireLowercase = false;
                option.Password.RequireDigit = false;
                option.Password.RequiredUniqueChars = 0;


            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            /*
            
            */
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
