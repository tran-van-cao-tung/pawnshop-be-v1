using AutoMapper;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;

namespace PawnShopBE.Helpers
{
    public class MappingProfile : Profile 
    {
        public MappingProfile()
        {
            // Mapping tu DTO sang entity
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<Contract, ContractDTO>().ReverseMap();         
            CreateMap<BranchDTO, Branch>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<AttributeDTO, Core.Models.Attribute>();
            CreateMap<PawnableProductDTO, PawnableProduct>()
                .ForMember(
                    dest => dest.Attributes,
                    opt => opt.MapFrom(src => src.AttributeDTOs));
            
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<Kyc,KycDTO>().ReverseMap();
            CreateMap<Job, JobDTO>().ReverseMap();
            CreateMap<Warehouse, WareHouseDTO>().ReverseMap();
            CreateMap<LiquidationDTO,Liquidtation>().ReverseMap();
            CreateMap<InterestDiary, InterestDiaryDTO>().ReverseMap();
            CreateMap<ContractAsset, ContractAssetDTO>().ReverseMap();
            CreateMap<DependentPeople, DependentPeopleDTO>().ReverseMap();
            CreateMap<CustomerRelativeDTO, CustomerRelativeRelationship>().ReverseMap();
        }
    }
}
