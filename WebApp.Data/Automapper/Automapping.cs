using AutoMapper;
using WebApp.Data.Entity;
using WebApp.Model.Auth;
using WebApp.Model.Product;

namespace WebApp.Data.Automapper
{
    public class Automapping : Profile
    {
        public Automapping()
        {
            CreateMap<SignUpRequestModel, User>();
            CreateMap<CreateProductRequestModel, Product>();
            CreateMap<UpdateProductModel, Product>();
        }
    }
}
