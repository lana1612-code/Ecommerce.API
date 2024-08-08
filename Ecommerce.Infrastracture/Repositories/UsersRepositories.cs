using AutoMapper;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.DTO;
using Ecommerce.Core.IRepositories;
using Ecommerce.Core.IRepositories.IService;
using Ecommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastracture.Repositories
{
    public class UsersRepositories : IUsersRepositories
    {
        private readonly AppDbContext dbContext;
        private readonly UserManager<LocalUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<LocalUser> signInManager;
        private readonly ITokenService tokenServise;
        private readonly IMapper mapper;

        public UsersRepositories(AppDbContext dbContext ,
            UserManager<LocalUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<LocalUser> signInManager,
            ITokenService tokenServise,  
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.tokenServise = tokenServise;
            this.mapper = mapper;
        }
        public bool IsUniqueUser(string Email)
        {
            var result  = dbContext.LocalUser.FirstOrDefault(x=>x.Email == Email);
            return result == null;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDTO.Email);
            var sheckPassword = await signInManager.CheckPasswordSignInAsync(user,
                loginRequestDTO.password,false);

            if(!sheckPassword.Succeeded)
            {
                return new LoginResponseDTO() { User = null, Token = "" };
            }
            var role = await userManager.GetRolesAsync(user);
            return new LoginResponseDTO() {
                User = mapper.Map<LocalUserDTO>(user),
                Token = await tokenServise.CreateTokenAsync(user),
                Role = role.FirstOrDefault()
            };

        }

        public async Task<LocalUserDTO> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            var user = new LocalUser {
             Email = registerationRequestDTO.Email,
            UserName = registerationRequestDTO.Email.Split('@')[0],
            FirstName = registerationRequestDTO.Fname,
            LastName = registerationRequestDTO.Lname,
            Address = registerationRequestDTO.Address,
            };
            using (var transaction = await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                        var result = await userManager.CreateAsync(user, registerationRequestDTO.Password);
                    if (result.Succeeded)
                    {
                       //var roles = await roleManager.Roles.ToListAsync();  
                        //foreach (var role in roles)
                        //{
                        //    if(! await roleManager.RoleExistsAsync(role.Name))
                        //    {
                        //        await roleManager.CreateAsync(new IdentityRole(role.Name));
                        //    }

                        var role = await roleManager.RoleExistsAsync(registerationRequestDTO.Role);
                        if (!role)
                        {
                            throw new Exception($"The Role {registerationRequestDTO.Role} doesnt exist !");
                        }

                        var userRoleREsult = await userManager.AddToRoleAsync(user, registerationRequestDTO.Role);
                        if (userRoleREsult.Succeeded)
                        {
                            await transaction.CommitAsync();
                            var userReturn =  dbContext.LocalUser.FirstOrDefault(u=>u.Email == registerationRequestDTO.Email);
                            return mapper.Map<LocalUserDTO>(userReturn);
                        }
                        else
                        {
                            await transaction.RollbackAsync();//roleback transaction if adding user role fails
                            throw new Exception("exception adding user role Failed");
                        }
                    }
                    else
                    {
                        await transaction.RollbackAsync();//roleback transaction if add user fails
                        throw new Exception("User Registeration Failed");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }

        }
    }
}
