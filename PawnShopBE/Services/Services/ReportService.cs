using PawnShopBE.Core.Const;
using PawnShopBE.Core.Display;
using PawnShopBE.Core.Models;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ReportService : IReportService
    {
        private IContractService _contract;
        private ICustomerService _customer;
        private IContractAssetService _asset;
        private IPawnableProductService _pawnable;

        public ReportService(IContractService contract, ICustomerService customer, 
            IContractAssetService asset
            , IPawnableProductService pawnable)
        {
            _contract = contract;
            _customer = customer;
            _asset = asset;
            _pawnable = pawnable;
        }

        public async Task<IEnumerable<DisplayReportTransaction>> getReportTransaction(int number)
        {
            var reportList= new List<DisplayReportTransaction>();
            //get contract
            var contractList =await _contract.GetAllContracts(0);
            //get customer
            var customerList = await _customer.GetAllCustomer(0);
            //get Asset
            var assetList = await _asset.GetAllContractAssets();
            //get pawnable
            var pawnableList = await _pawnable.GetAllPawnableProducts(0);

            foreach (var contract in contractList)
            {
                var customer = getCustomer(contract.CustomerId, customerList);
                var asset= getAsset(contract.ContractAssetId,assetList);
                var pawnable = getPawnable(asset.PawnableProductId, pawnableList);
                //đưa value vào report
                var report = new DisplayReportTransaction();
                report.contractCode= contract.ContractCode;
                report.customerName = customer.FullName;
                report.assetCode = pawnable.CommodityCode;
                report.assetName = asset.ContractAssetName;
                report.loan=contract.Loan;
                report.startDate = contract.ContractStartDate; 
                report.endDate=contract.ContractEndDate;
                reportList.Add(report);
            }

            if (number == 0)
            {
                return reportList;
            }
            var result =await TakePage(number, reportList);
            return result;

        }
        private async Task<IEnumerable<DisplayReportTransaction>> TakePage(int number, IEnumerable<DisplayReportTransaction> list)
        {
            var numPage = (int)NumberPage.numPage;
            var skip = (numPage * number) - numPage;
            return list.Skip(skip).Take(numPage);
        }

        private PawnableProduct getPawnable(int pawnableProductId, IEnumerable<PawnableProduct> pawnableList)
        {
            var pawnableIenumerable= from p in pawnableList where p.PawnableProductId == pawnableProductId select p;
            var pawnable=pawnableIenumerable.FirstOrDefault();
            return pawnable;
        }

        private ContractAsset getAsset(int contractAssetId, IEnumerable<ContractAsset> assetList)
        {
            var assetIenumerable= from a in assetList where a.ContractAssetId== contractAssetId select a;
            var asset= assetIenumerable.FirstOrDefault();
            return asset;
        }

        private Customer getCustomer(Guid customerId, IEnumerable<Customer> customerList)
        {
            var customerIenumerable = from c in customerList where c.CustomerId == customerId select c;
            var customer = customerIenumerable.FirstOrDefault();
            return customer;
        }
    }
}
