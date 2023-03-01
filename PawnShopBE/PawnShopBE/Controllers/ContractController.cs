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
                sb.Append(attributes.Description + "/");              
            }
            // Get Interest by recommend
            Package package = await _packageService.GetPackageById(request.PackageId);
            if (request.InterestRecommend != 0)
            {
                package.PackageInterest = request.InterestRecommend;
            }
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

            contract.ContractAssetId = contractAsset.ContractAssetId;
            contract.CustomerId = customer.CustomerId;
            contract.BranchId = request.BranchId;
            contract.ContractAsset = contractAsset;
            contract.Customer = customer;
      
  
            var response = await _contractService.CreateContract(contract);
            if (response)
            {
                var interestDiary = new InterestDiary();

            }
            if (response)
            {
                return Ok(response); 
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("contracts")]
        public async Task<IActionResult> GetAllContracts()
        {
            var listContracts = await _contractService.GetAllContracts();
            if (listContracts == null)
            {
                return NotFound();
            }
            return Ok(listContracts);
        }

        [HttpPut("contract")]
        public async Task<IActionResult> UpdateContract(ContractDTO request)
        {       
                var contract = _mapper.Map<Contract>(request);
                var response = await _contractService.UpdateContract(contract);
                if (response)
                {
                    return Ok(response);
                }         
            return BadRequest();
        }
    }
}
