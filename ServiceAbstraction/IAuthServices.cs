using Shared.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IAuthServices
    {
        Task<UserResult> LogIn(UserLoginDto userLoginDto);
        Task<UserResult> Register(RegisterDto userRegsiterDto);
       
    }
}
