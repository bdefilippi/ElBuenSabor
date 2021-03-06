using ElBuenSabor.Models.Request;
using ElBuenSabor.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> Authorize(AuthRequest model);
    }
}
