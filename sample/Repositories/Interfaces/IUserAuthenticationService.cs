using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sample.Models.DTO;

namespace sample.Repositories.Interfaces
{
    public interface IUserAuthenticationService
    {
        Task<Status> LoginAsync(LoginModel login);

        Task<Status> RegistrationAsync(RegistrationModel registration);

        Task logOutAsync();
    }
}
