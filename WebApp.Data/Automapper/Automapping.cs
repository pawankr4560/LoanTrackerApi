using AutoMapper;
using WebApp.Data.Entity;
using WebApp.Model.Auth;

namespace WebApp.Data.Automapper
{
    public class Automapping : Profile
    {
        public Automapping()
        {
            CreateMap<SignUpRequestModel, User>();
        }
    }
}
