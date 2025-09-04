using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Million.RealState.Application.Features.Authentication.Commands;
using Million.RealState.Domain.DTOs;
using Million.RealState.Domain.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace Million.RealState.Application.Features.Authentication.Handlers;

public class AuthenticationCommandHandler(IConfiguration _configuration) : IRequestHandler<AuthenticationCommand, ApiResponse<string>>
{

    // Main handler for authentication requests
    public Task<ApiResponse<string>> Handle(AuthenticationCommand request, CancellationToken cancellationToken)
    {
        // Validate user credentials
        (bool flowControl, Task<ApiResponse<string>> value) = ValidateAuthentication(request);
        if (!flowControl)
        {
            return value;
        }

        // Generate JWT token for valid user
        var token = GenerateToken(request.login.UserName);
        if (string.IsNullOrEmpty(token))
        {
            return Task.FromResult(ApiResponse<string>.ErrorResult(Constants.TokenError, Constants.AuthFailure));
        }

        return Task.FromResult(ApiResponse<string>.SuccessResult(token, Constants.Success));
    }

    // Validates the authentication request (username and password)
    private static (bool flowControl, Task<ApiResponse<string>> value) ValidateAuthentication(AuthenticationCommand request)
    {
        // Check if username or password is missing
        if (string.IsNullOrEmpty(request.login.UserName) || string.IsNullOrEmpty(request.login.Password))
        {
            return (flowControl: false, value: Task.FromResult(ApiResponse<string>.ErrorResult(Constants.UserPasswordRequired, Constants.ValidError)));
        }

        // Validate username with regex pattern
        if (!Regex.IsMatch(request.login.UserName, Constants.UserPattern))
        {
            return (flowControl: false, value: Task.FromResult(ApiResponse<string>.ErrorResult(Constants.TokenError, Constants.AuthFailure)));
        }

        // Validate password with regex pattern
        if (!Regex.IsMatch(request.login.Password, Constants.PasswordPattern))
        {
            return (flowControl: false, value: Task.FromResult(ApiResponse<string>.ErrorResult(Constants.TokenError, Constants.AuthFailure)));
        }

        return (flowControl: true, value: null);
    }

    // Generates a JWT token for the authenticated user
    private string GenerateToken(string username)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[Constants.JWTKey]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Build JWT token with claims
        var token = new JwtSecurityToken(
            _configuration[Constants.JWT],
            _configuration[Constants.JWTAudience],
            claims: new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            },

            expires: DateTime.Now.AddMinutes(double.Parse(_configuration[Constants.JwtAccessTokenExpiration])),
            signingCredentials: creds);

        // Return token string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
