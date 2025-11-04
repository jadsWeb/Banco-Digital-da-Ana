using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BancoDigital.ContaCorrente.API.Domain.Interfaces;
using BancoDigital.ContaCorrente.API.Infra.Seguranca;
using Microsoft.IdentityModel.Tokens;

namespace BancoDigital.ContaCorrente.API.Infra.Repositorys
{
    public class JwtService() : IJwtService
    {
        public string GerarToken(string idContaCorrente, string NumeroConta, JwtConfiguracoes _jwtConfiguracoes)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguracoes.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, idContaCorrente),
                new Claim("numeroConta", NumeroConta)
            };

            var token = new JwtSecurityToken(
                issuer: _jwtConfiguracoes.Issuer,
                audience: _jwtConfiguracoes.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtConfiguracoes.ExpirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}