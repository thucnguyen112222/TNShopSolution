using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNShopSolution.ViewModels.Common;
using TNShopSolution.ViewModels.System.Users;

namespace TNShopSolution.Application.System.Users
{
    public interface IUserService
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);
        Task<ApiResult<bool>> Register(RegisterRequest request);
        Task<ApiResult<bool>> Update(Guid id,UserUpdateRequest request);
        Task<ApiResult<PageResult<UserViewModel>>> GetUsersPaging(GetUserPagingRequest request);
        Task<ApiResult<UserViewModel>> GetById(Guid id);
        Task<ApiResult<bool>> Delete(Guid id);
    }
}
