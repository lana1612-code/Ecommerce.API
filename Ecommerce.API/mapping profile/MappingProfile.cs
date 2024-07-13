using AutoMapper;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.DTO;

namespace Ecommerce.API.mapping_profile
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<Products, ProductDTO>()
                    .ForMember(To=>To.Category_Name, From => From
                    .MapFrom(x=> x.Category != null ? x.Category.Name : null));

            CreateMap<Orders, OrderDTO>()
                .ForMember(TO => TO.UserName, From => From
                .MapFrom(x=>x.localUser !=null ? x.localUser.UserName : null));

            CreateMap<PostProductDTO, Products>()
                     .ForMember(To => To.CategoryId, From => From
                     .MapFrom(x =>  x.Category_Id));
        }
    }
}
 