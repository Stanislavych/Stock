using Stock.Common.Dto;
using Stock.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.BusinessLogic.Interfaces
{
    public interface IAuthService
    {
        Task<User> Register(UserDto request);
        Task<string> Login(UserDto request);
    }
}
