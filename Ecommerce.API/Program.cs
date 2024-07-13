
using Ecommerce.API.mapping_profile;
using Ecommerce.Core.IRepositories;
using Ecommerce.Infrastracture.Repositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;


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
            builder.Services.AddScoped(typeof(IGenericRepositories<>), typeof(GenericRepositories<>));
            builder.Services.AddScoped(typeof(IProductRepository), typeof(ProductRepository));
            builder.Services.AddScoped(typeof(IUniteOfWork<>), typeof(UniteOfWork<>));
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            

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
