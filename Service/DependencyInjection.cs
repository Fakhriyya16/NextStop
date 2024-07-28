
using CloudinaryDotNet;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service.Configurations;
using Service.DTOs.Cities;
using Service.Helpers;
using Service.Interfaces;

namespace Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<ICloudManagement, CloudManagement>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IPlaceService, PlaceService>();
            services.AddScoped<IGeolocationService, GeolocationService>();

            services.AddValidatorsFromAssemblyContaining<CityCreateDtoValidator>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));


            var cloudinarySettings = configuration.GetSection("Cloudinary").Get<CloudinarySettings>();
            var account = new Account(cloudinarySettings.CloudName, cloudinarySettings.ApiKey, cloudinarySettings.ApiSecret);
            var cloudinary = new Cloudinary(account);

            services.AddSingleton(cloudinary);

            return services;
        }
    }
}
