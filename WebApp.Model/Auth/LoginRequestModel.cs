using Google.Apis.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Model.Auth
{
    public class LoginRequestModel
    {
        [Required(ErrorMessage ="Email is required.")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage ="Password is required.")]
        public string Password { get; set; } = string.Empty;
    }

    public class IdTokenRequest
    {
        public string IdToken { get; set; } = string.Empty;
    }


    public class Clock : IClock
    {
        public DateTime Now => DateTime.Now.AddMinutes(10);

        public DateTime UtcNow => DateTime.UtcNow.AddMinutes(10);
    }
}
