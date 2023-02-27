using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
using PawnShopBE.Core.Const;
using PawnShopBE.Core.Requests;
using Services.Services;
using Services.Services.IServices;
using System.Text;

namespace PawnShopBE.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IContractService _contractService;
        private readonly ICustomerService _customerService;
        private readonly IContractAssetService _contractAssetService;
        private readonly IPackageService _packageService;
        private readonly IBranchService _branchService;
        private readonly IKycService _kycService;
        private readonly IMapper _mapper;

        public ContractController(
            IContractService contractService, 
            ICustomerService customer, 
            IContractAssetService contractAssetService, 
            IPackageService packageService,
            IBranchService branchService,
            IKycService kycService,
            IMapper mapper)
        {
            _contractService = contractService;
            _customerService = customer;
            _contractAssetService = contractAssetService;
            _packageService = packageService;
            _branchService = branchService;
            _kycService = kycService;
            _mapper = mapper;
        }

        [HttpPost("contract")]
        public async Task<IActionResult> CreateContract(ContractDTO request)
        {
            StringBuilder sb = new StringBuilder();
            foreach (AttributeDTO attributes in request.PawnableAttributeDTOs)
            {            
                sb.Append(attributes.Description + ",");              
            }
            // Get Interest by recommend
            Package package = await _packageService.GetPackageById(request.PackageId);
            if (request.InterestRecommend != 0)
            {
                package.PackageInterest = request.InterestRecommend;
            }
            //Get Branch
            Branch branch = await _branchService.GetBranchById(request.BranchId);

            // Check if old Customer
            var oldCus = await _customerService.GetCustomerById(request.CustomerId);
            //if (oldCus == null)
            //{
                // Create contract with new Customer
                var customerDTO = _mapper.Map<CustomerDTO>(request);
                    customerDTO.CreatedDate = DateTime.Now;
                    customerDTO.Point = 0;
                var customer = _mapper.Map<Customer>(customerDTO);
                    customer.Status = (int) CustomerConst.ACTIVE;

                // Create new Kyc
                Kyc kyc = new Kyc();
                kyc.IdentityCardBacking = "";
                kyc.IdentityCardFronting = "";
                kyc.FaceImg = "";    
                Kyc createKyc = await _kycService.CreateKyc(kyc);
                customer.KycId = createKyc.KycId;

                var newCus= await _customerService.CreateCustomer(customer);
            //}

          
          
            var contractAsset = _mapper.Map<ContractAsset>(request);
                contractAsset.Description = sb.ToString();
                contractAsset.Status = (int) ContractAssetConst.IN_STOCK;
            await _contractAssetService.CreateContractAsset(contractAsset);


            

            // Create contract
            var contract = _mapper.Map<Contract>(request);
            var listContract = await _contractService.GetAllContracts();
            int countList = 0;
            if (listContract == null)
            {
                countList = listContract.Count();
            }
            contract.ContractAssetId = contractAsset.ContractAssetId;
            contract.CustomerId = customer.CustomerId;
            contract.BranchId = branch.BranchId;
            contract.ContractAsset = contractAsset;
            contract.Customer = customer;
            contract.Branch = branch;
            contract.ContractStartDate = DateTime.Now;           
            contract.ContractEndDate = contract.ContractStartDate.AddDays((double) package.Day);
            
            contract.ContractCode = "CĐ-" + (countList+1).ToString();
            request.CustomerRecived = (request.CustomerRecived - (request.InsuranceFee + request.StorageFee));
            request.Loan = request.CustomerRecived * package.PackageInterest * package.PaymentPeriod;
            
            contract.Status = (int) ContractConst.IN_PROGRESS;
            var response = await _contractService.CreateContract(contract);

            if (response)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
