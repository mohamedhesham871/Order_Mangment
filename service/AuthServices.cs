using Domin.Exceptions;
using Domin.Models.identitys;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServiceAbstraction;
using Shared;
using Shared.Dtos.User;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace service
{
    public class AuthServices(UserManager<AppUser> manager , IOptions<JwtOptions> options) : IAuthServices
    {
        public async Task<UserResult> LogIn(UserLoginDto userLoginDto)
        {
            var user =await manager.FindByEmailAsync(userLoginDto.Email);
            if (user is null)
            {
                throw new NotFoundException("User not found with this email address.");
            }
            var isPasswordValid = await manager.CheckPasswordAsync(user, userLoginDto.Password);
            if (!isPasswordValid)
            {
                throw new NotFoundException("Invalid password !!");
            }
            return new UserResult
            {
                UserId = user.Id,
                UserEmail = user.Email!,
                UserName = user.UserName!,
                Token = await GenerateToken(user)
            };
        }
        public async Task<UserResult> Register(RegisterDto userRegsiterDto)
        {
            var user = new AppUser
            {
                UserName = userRegsiterDto.UserName,
                Email = userRegsiterDto.Email,
                City = userRegsiterDto.City,
                Country = userRegsiterDto.Country,
                Street = userRegsiterDto.Street

            };
            var result = await manager.CreateAsync(user, userRegsiterDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new ValidationErrorsExecption(errors);
            }
            await manager.AddToRoleAsync(user, "Customer");


            return new UserResult()
            {
                UserId = user.Id,
                UserEmail = user.Email,
                UserName = user.UserName!,
                Token = await GenerateToken(user)
            };

        }
        public async Task<string> GenerateToken(AppUser user)
        {
            var jwtoptions = options.Value;
            //1- Add Claims 
            var userClaims = new List<Claim>
           {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!)
           };
            //2- Add all roles to Claims 
            var userRoles = await manager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            // 3- Generate Security Key
            var SecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtoptions.SecretKey));
            // 4- Generate Token 
            var JwtToken = new JwtSecurityToken(
                issuer: jwtoptions.Issuer,
                audience: jwtoptions.Audience,
                claims: userClaims,
                expires: DateTime.UtcNow.AddDays(jwtoptions.DurationDays),
                signingCredentials: new SigningCredentials(SecretKey, SecurityAlgorithms.HmacSha256Signature)
                );
            var TokenHandler = new JwtSecurityTokenHandler().WriteToken(JwtToken);
            return TokenHandler;


        }
    }
    
}
