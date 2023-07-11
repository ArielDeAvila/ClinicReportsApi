using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace ClinicReportsAPI.Extensions;

//documentar swagger de forma personalizada 
public static class SwaggerExtension
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        var openApi = new OpenApiInfo
        {
            Title = "System Report API",
            Version = "v1",
            Description = "System of centralized medical record management API 2023",
            TermsOfService = new Uri("http://opensource.org/licenses/NIT"), //Url donde deberían ir nuestros terminos de uso
            Contact = new OpenApiContact
            {
                Name = "Ariel De avila",
                Email = "arieldeavila-1996@hotmail.com",
                Url = new Uri("http://sirtech.com.pe")
            },
            License = new OpenApiLicense
            {
                Name = "License",
                Url = new Uri("http://opensource.org/licenses")
            }
        };

        services.AddSwaggerGen(s =>
        {
            openApi.Version = "v1";

            s.SwaggerDoc("v1", openApi);

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Jwt Authentication",
                Description = "Jwt Bearer Token",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            s.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);

            s.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {securityScheme, new string[]{ } }
            });


        });

        return services;
    }
}


