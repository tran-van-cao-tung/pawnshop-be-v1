using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Models;
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

        private readonly IMapper _mapper;

        public ContractController(IContractService contractService, IMapper mapper)
        {
            _contractService = contractService;
            _mapper = mapper;
        }

        [HttpPost("contract")]
        public async Task<IActionResult> CreateContract(CreateContractRequest request)
        {
            StringBuilder sb = new StringBuilder();
            foreach( Core.Models.Attribute attributes in request.PawnableAttributes){
                sb.Append(attributes.Description + ",");          
            }

            ContractDTO contractDTO = new ContractDTO();
            if (request != null){
                contractDTO.FullName = request.CustomerName;
                contractDTO.Address = request.Address;
                contractDTO.Phone = request.PhoneNumber;
                contractDTO.InsuranceFee = request.InsuranceFee;
                contractDTO.PackageId = request.PackageId;
                contractDTO.Description = sb.ToString();
            }
            var contract = _mapper.Map<Contract>(contractDTO);
            contract.ContractStartDate = DateTime.Now;
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
