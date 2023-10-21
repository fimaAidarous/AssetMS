using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using sample.Data;
using sample.Models.DTO;
using sample.Repositories.Interfaces;

namespace sample.Repositories.Implementation
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserAuthenticationService(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Status> ChangePasswordAsync(ChangePasswordModel model, string username)
        {
            var status = new Status();

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                status.Message = "User does not exist";
                status.StatusCode = 0;
                return status;
            }
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                status.Message = "Password has updated successfully";
                status.StatusCode = 1;
            }
            else
            {
                status.Message = "Some error occcured";
                status.StatusCode = 0;
            }
            return status;
        }

        public async Task<Status> LoginAsync(LoginModel login)
        {
            var status = new Status();
            // check the username
            var user = await _userManager.FindByNameAsync(login.Username);
            if (user is null)
            {
                status.StatusCode = 0;
                status.Message = "Invalid username";
                return status;
            }
            // check the password
            if (!await _userManager.CheckPasswordAsync(user, login.Password))
            {
                status.StatusCode = 0;
                status.Message = "Incorrect Password";
                return status;
            }
            var signInResult = await _signInManager.PasswordSignInAsync(
                user,
                login.Password,
                false,
                true
            );
            if (!signInResult.Succeeded)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>() { new Claim(ClaimTypes.Name, login.Username) };
                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }
                status.StatusCode = 1;
                status.Message = "User successfully logged in";
                return status;
            }
            else if (signInResult.IsLockedOut)
            {
                status.StatusCode = 0;
                status.Message = "User locked out";
                return status;
            }
            else
            {
                status.StatusCode = 1;
                status.Message = "Error on loggin in";
                return status;
            }
        }

        public async Task logOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<Status> RegistrationAsync(RegistrationModel registration)
        {
            var status = new Status();
            // check if the user exists
            var existUser = await _userManager.FindByEmailAsync(registration.Email);
            if (existUser is not null)
            {
                status.StatusCode = 0;
                status.Message = "this user already exists";
                return status;
            }
            var applicationUser = new ApplicationUser
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                Name = registration.Name,
                Email = registration.Email,
                UserName = registration.Username,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(applicationUser, registration.Password);

            // check the registration
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                var errors = string.Join(", ", result.Errors);
                status.Message = errors;
                return status;
            }

            // role management
            if (!await _roleManager.RoleExistsAsync(registration.Role!))
            {
                await _roleManager.CreateAsync(new IdentityRole(registration.Role!));
            }
            await _userManager.AddToRoleAsync(applicationUser, registration.Role!);
            status.StatusCode = 1;
            status.Message = "User has registered successfully";
            return status;
        }

        public async Task<Status> UpdateAsync(string id, UserEditModel UserEdit)
        {

            // check if the user exist
            // Update the properties
            // Remove previous roles
            // check if the new roles exist
            // create if the roles doesn't exist
            // update new role

            var status = new Status();

            var targetUser = await _userManager.FindByIdAsync(id);
            if (targetUser is null) return new Status() { Message = "This User doesnot exist", StatusCode = 0 };
            targetUser.Email = UserEdit.Email;
            targetUser.UserName = UserEdit.Username;
            targetUser.Name = UserEdit.Name;

            var prevRoles = await _userManager.GetRolesAsync(targetUser);

            var result = await _userManager.UpdateAsync(targetUser);
            if (!result.Succeeded) return new Status() { StatusCode = 0, Message = string.Join(",", result.Errors) };

            var result2 = await _userManager.ChangePasswordAsync(targetUser, UserEdit.CurrentPassword!, UserEdit.Password!);
            if (!result2.Succeeded) return new Status() { StatusCode = 0, Message = string.Join(",", result2.Errors) };

            await _userManager.RemoveFromRolesAsync(targetUser, prevRoles);

            if (!await _roleManager.RoleExistsAsync(UserEdit.Role!))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserEdit.Role!));
            }
            await _userManager.AddToRoleAsync(targetUser, UserEdit.Role!);

            status.Message = "User successfully updated";
            status.StatusCode = 1;
            return status;
        }
    }
}
