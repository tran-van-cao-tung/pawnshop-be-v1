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

            #region Customer
            // Mapping from Customer to Display Customer
            CreateMap<Customer,DisplayCustomer>().ReverseMap();

            // Mapping from Customer to Contract
            CreateMap<Contract, Customer>().
                ForMember(customer => customer.CustomerId,
                contract => contract.MapFrom(src => src.CustomerId));

            // Mapping from ContractDTO to CustomerDTO
            CreateMap<ContractDTO, CustomerDTO>()
                .ForMember(
                dest => dest.FullName,
                opt => opt.MapFrom(src => src.CustomerName))
                .ForMember(
                dest => dest.CCCD,
                opt => opt.MapFrom(src => src.IdentityCard))
                .ForMember(
                dest => dest.Address,
                opt => opt.MapFrom(src => src.CustomerAddress))
                .ForMember(
                dest => dest.Phone,
                opt => opt.MapFrom(src => src.CustomerPhoneNumber));

            // Mapping from CustomerDTO to Customer
            CreateMap<CustomerDTO, Customer>()
                .ForMember(
                dest => dest.FullName,
                opt => opt.MapFrom(src => src.FullName))
                .ForMember(
                dest => dest.CCCD,
                opt => opt.MapFrom(src => src.CCCD))
                .ForMember(
                dest => dest.Address,
                opt => opt.MapFrom(src => src.Address))
                .ForMember(
                dest => dest.Phone,
                opt => opt.MapFrom(src => src.Phone))
                .ForMember(
                dest => dest.CreatedDate,
                opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(
                dest => dest.Point,
                opt => opt.MapFrom(src => src.Point));
            #endregion Customer

            #region Contract
            // Mapping from Contract to Branch
            CreateMap<Branch, Contract>().ForMember(contract => contract.BranchId,
                branch => branch.MapFrom(src => src.BranchId));

            // Mapping from ContractDTO to ContractAsset
            CreateMap<ContractDTO, ContractAsset>()
                .ForMember(
                dest => dest.ContractAssetName,
                opt => opt.MapFrom(src => src.ContractAssetName))
                .ForMember(
                dest => dest.PawnableProductId,
                opt => opt.MapFrom(src => src.PawnableProductID));

            // Mapping from ContractAssetDTO to ContractAsset
            CreateMap<ContractAssetDTO, ContractAsset>()
                .ForMember(
                dest => dest.ContractAssetName,
                opt => opt.MapFrom(src => src.ContractAssetName))
                .ForMember(
                dest => dest.Description,
                opt => opt.MapFrom(src => src.Description))
                .ForMember(
                dest => dest.PawnableProductId,
                opt => opt.MapFrom(src => src.PawnableProductId));

            // Mapping from ContractDTO to Contract
            CreateMap<ContractDTO, Contract>()
                .ForMember(
                dest => dest.BranchId,
                opt => opt.MapFrom(src => src.BranchId))
                .ForMember(
                dest => dest.CustomerId,
                opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(
                dest => dest.PackageId,
                opt => opt.MapFrom(src => src.PackageId))
                .ForMember(
                dest => dest.ContractAssetId,
                opt => opt.MapFrom(src => src.ContractAssetId))
                .ForMember(
                dest => dest.Loan,
                opt => opt.MapFrom(src => src.Loan))
                .ForMember(
                dest => dest.InsuranceFee,
                opt => opt.MapFrom(src => src.InsuranceFee))
                .ForMember(
                dest => dest.StorageFee,
                opt => opt.MapFrom(src => src.StorageFee))
                .ForMember(
                dest => dest.CustomerRecieved,
                opt => opt.MapFrom(src => src.CustomerRecived))
                .ForMember(
                dest => dest.Description,
                opt => opt.MapFrom(src => src.Description));
            #endregion Contract

            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<Contract, ContractDTO>().ReverseMap();         
            CreateMap<BranchDTO, Branch>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<AttributeDTO, Core.Models.Attribute>();
            CreateMap<PawnableDTO, PawnableProduct>().ReverseMap();
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
