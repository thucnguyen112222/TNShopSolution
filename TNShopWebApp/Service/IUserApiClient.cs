using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TNShopSolution.ViewModels.Common;
using TNShopSolution.ViewModels.System.Users;

namespace TNShopWebApp.Service
{
    public interface IUserApiClient
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);
        Task<ApiResult<PageResult<UserViewModel>>> GetUserPaging(GetUserPagingRequest request);
         Task<ApiResult<bool>> RegisterUser(RegisterRequest register);
        Task<ApiResult<bool>> UpdateUser(Guid id, UserUpdateRequest register);
        Task<ApiResult<UserViewModel>> GetUserById(Guid id);
        Task<ApiResult<bool>> Delete(Guid id);

    }
}
