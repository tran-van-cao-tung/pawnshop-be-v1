using PawnShopBE.Core.Display;
using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface IContractService
    {
        Task<Contract> CreateContract(Contract contract);

        Task<IEnumerable<Contract>> GetAllContracts(int num);
        Task<Contract> GetContractById(int contractId);
        Task<DisplayContractDetail> GetContractDetail(int contractId);
        Task<bool> UpdateContract(string contractCode, Contract contract);
        Task<bool> DeleteContract(int contractId);
        Task<bool> UploadContractImg(int contractId, string customerImg, string contractImg);
        Task<bool> exporteExcel();
        Task<DisplayContractHomePage> getAllContractHomepage(int numpage);

    }
}
