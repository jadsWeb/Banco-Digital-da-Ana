using BancoDigital.ContaCorrente.API.Domain.Config;
using BancoDigital.ContaCorrente.API.Infra.Config;
using BancoDigital.ContaCorrente.API.Infra.Seguranca;
using FluentValidation;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<JwtConfiguracoes>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtConfiguracoes>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings!.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
    };

    // 🔹 Personaliza o comportamento em erros de token
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            context.NoResult(); // evita resposta padrão
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";

            var message = context.Exception switch
            {
                SecurityTokenExpiredException => "Token expirado.",
                SecurityTokenInvalidSignatureException => "Assinatura do token inválida.",
                _ => "Token inválido ou malformado."
            };

            var response = new
            {
                error = "USER_FORBIDDEN",
                message
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    };
});

builder.Services.AddApplicationServicesInfra(builder.Configuration);
builder.Services.AddApplicationServicesDomain();
builder.Services.AddControllers();builder.Services.AddEndpointsApiExplorer();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddSwaggerGen(c =>
{
    //c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Banco Digital - Conta Corrente API",
        Version = "v1",
        Description = "API para operações de conta corrente: cadastro, login, movimentações e saldo."
    });

    // Configura autenticação via Bearer Token (JWT)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT desta forma: **Bearer {seu token}**"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BankMore - Conta Corrente API v1");
        c.RoutePrefix = string.Empty; // Deixa o Swagger na raiz (http://localhost:5001)
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();