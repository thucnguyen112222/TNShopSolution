using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TNShopSolution.ViewModels.System.Users;

namespace TNShopWebApp.Service
{
    public interface IUserApiClient
    {
        Task<string> Authenticate(LoginRequest request);

    }
}
