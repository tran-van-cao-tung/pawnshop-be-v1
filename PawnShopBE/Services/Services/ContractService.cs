using AutoMapper.Execution;
using Google.Protobuf.WellKnownTypes;
using PawnShopBE.Core.Const;
using PawnShopBE.Core.Display;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Infrastructure.Helpers;
using PawnShopBE.Infrastructure.Repositories;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ContractService : IContractService
    {
        public IUnitOfWork _unitOfWork;
        public IPackageService _iPackageService;
        public IInteresDiaryService _iInterestDiaryService;
        public IContractRepository _iContractRepository;
        private IServiceProvider _serviceProvider;
        //interface xài cho api homepage
        private ILedgerService _ledger;
        private IContractAssetService _iContractAssetService;
        private IBranchService _branch;
        private IRansomService _ransom;
        private ICustomerService _customer;
        private IPawnableProductService _pawnable;
        private IWareHouseService _wareHouse;
        public ContractService(IUnitOfWork unitOfWork, IContractRepository iContractRepository,
            IContractAssetService contractAssetService, IPackageService packageService,
            IInteresDiaryService interesDiaryService, IServiceProvider serviceProvider,
            ILedgerService ledger, IPawnableProductService pawnable, IWareHouseService wareHouse)
        {
            _unitOfWork = unitOfWork;
            _iContractRepository = iContractRepository;
            _iContractAssetService = contractAssetService;
            _iPackageService = packageService;
            _iInterestDiaryService = interesDiaryService;
            _serviceProvider = serviceProvider;
            _ledger = ledger;
            _pawnable = pawnable;
            _wareHouse = wareHouse;
            getParameter();
        }
        private void getParameter()
        {
         _branch = _serviceProvider.GetService(typeof(IBranchService)) as IBranchService;
         _ransom = _serviceProvider.GetService(typeof(IRansomService)) as IRansomService;
         _customer = _serviceProvider.GetService(typeof(ICustomerService)) as ICustomerService;
        }
        public async Task<IEnumerable<DisplayContractHomePage>> getAllContractHomepage(int numpage)
        {
            //get all List
            var branchList = await _branch.GetAllBranch(0);
            var customerList = await _customer.GetAllCustomer(0);
            var ledgerList = await _ledger.GetLedger();
            var assetList = await _iContractAssetService.GetAllContractAssets();
            var contractList = await GetAllContracts(0);
            var pawnableList = await _pawnable.GetAllPawnableProducts(0);
            var wareHouseList = await _wareHouse.GetWareHouse(0);
            //var ransomList=await _ransom.GetRansom();
            // khai báo filed hiển thị chung 
            decimal fund = 0;
            decimal loanLedger  = 0;
            decimal recveivedInterest  = 0;
            decimal totalProfit  = 0;
            decimal ransomTotal  = 0;
            //get displayHomePage
            List<DisplayContractHomePage> listDipslay =new List<DisplayContractHomePage>();
            DisplayContractHomePage display = new DisplayContractHomePage();
            foreach (var contract in contractList)
            {
                var contractId = contract.ContractId;
                display.contractCode = contract.ContractCode;
                display.customerName = getCustomerName(customerList, contract.CustomerId);
                display.assestCode = getAsset(contract.ContractAssetId, assetList, pawnableList,wareHouseList, 1);
                display.assetName = getAsset(contract.ContractAssetId, assetList, pawnableList,wareHouseList,2);
                display.loanContract = contract.Loan;
                display.startDate = contract.ContractStartDate;
                display.endDate=contract.ContractEndDate;
                display.wareName = getAsset(contract.ContractAssetId, assetList, pawnableList, wareHouseList, 3);
                display.status= contract.Status;
                //get field hiển thị chung
                if (fund == 0) {
                    fund = decimal.Parse(getBranchName(contract.BranchId, branchList, false));
                }
                totalProfit += contract.TotalProfit;
                loanLedger += getLedger(ledgerList, contract.BranchId, true);
                recveivedInterest += getLedger(ledgerList, contract.BranchId, false);
               // ransomTotal += getRansom(ransomList, contract.ContractId);
               //add list
               listDipslay.Add(display);
            }
            //để field hiển thị chung gắn vào phần từ đầu trong list
            foreach(var x in listDipslay)
            {
                //add field hiển thị chung
                x.totalProfit = totalProfit;
                x.loanLedger= loanLedger;
                x.fund= fund;
                x.recveivedInterest= recveivedInterest;
                x.ransomTotal= ransomTotal;
                break;
            }
            //phân trang
            if (numpage == 0)
            {
                return listDipslay;
            }
            var result = await TakePage(numpage, listDipslay);
            return result;
           
        }
         private async Task<IEnumerable<DisplayContractHomePage>> TakePage
            (int number, IEnumerable<DisplayContractHomePage> list)
        {
            var numPage = (int)NumberPage.numPage;
            var skip = (numPage * number) - numPage;
            return list.Skip(skip).Take(numPage);
        }
        private decimal getRansom(IEnumerable<Ransom> ransomList, int contractId)
        {
            var ransomIenumerable= from r in ransomList where r.ContractId== contractId select r;   
            var ransom= ransomIenumerable.FirstOrDefault();
            return ransom.TotalPay;
        }

        private decimal getLedger(IEnumerable<Ledger> ledgerList, int branchId, bool v)
        {
            var ledgerIenumerable= from l in ledgerList where l.BranchId== branchId select l;
            var ledger=ledgerIenumerable.FirstOrDefault();
            // true => get loan, false => get receiveInterest (lãi đã nhận)
            if (v)
                return ledger.Loan;
            else
                return ledger.RecveivedInterest;
        }

        private string getBranchName(int branchId, IEnumerable<Branch> branchList,bool v)
        {
            var branchIenumerable= from b in branchList where b.BranchId == branchId select b;
            var branch = branchIenumerable.FirstOrDefault();
            // true => get branch Name, false => get fund
            if (v)
                return branch.BranchName;
            else
                return branch.Fund.ToString();
        }

        private string getAsset(int contractAssetId, IEnumerable<ContractAsset> assetList, 
            IEnumerable<PawnableProduct> pawnableList,IEnumerable<Warehouse> warehouseList, int num)
        {
            //get list contract asset
            var assetIenumerable= from a in assetList where a.ContractAssetId== contractAssetId select a;
            var asset = assetIenumerable.FirstOrDefault();
            // 1 => get code Asset, 2 => get name Asset, 3 => get ware name
            switch (num) {
                case 1:
                    var pawnableIenumerable = from p in pawnableList where
                                          p.PawnableProductId == asset.PawnableProductId select p;
                    var pawnable= pawnableIenumerable.FirstOrDefault();
                    return pawnable.CommodityCode;

                case 2:
                    return asset.ContractAssetName;

                case 3:
                    var wareIenumerable = from w in warehouseList where w.WarehouseId == asset.WarehouseId select w;
                    var wareHouse=wareIenumerable.FirstOrDefault();
                    return wareHouse.WarehouseName;
            }
            return null;
        }

        private string getCustomerName(IEnumerable<Customer> customerList, Guid customerId)
        {
           var customerIenumerable= from c in customerList where c.CustomerId== customerId select c;
            var customer = customerIenumerable.FirstOrDefault();
            return customer.FullName;
        }

        public async Task<Contract> CreateContract(Contract contract)
        {
            if (contract != null)
            {
                contract.Branch = null;
                contract.Package = null;
                contract.Customer = null;
                contract.ContractAsset = null;
                var contractList = await GetAllContracts(0);
                var count = 0;
                if (contractList != null)
                {
                    count = contractList.Count();
                }
                contract.ContractCode = "CĐ-" + (count + 1).ToString();
                var package = await _unitOfWork.Packages.GetById(contract.PackageId);

                if (package != null)
                {

                    var fee = contract.InsuranceFee + contract.StorageFee;
                    var period = package.Day / package.PaymentPeriod;
                    double interest = 0;

                    // Use recommend interest if input
                    if (contract.InterestRecommend != 0)
                    {
                        interest = contract.InterestRecommend * 0.01;
                    }
                    interest = package.PackageInterest * 0.01;
                    contract.TotalProfit = (contract.Loan * (decimal)interest) + (fee * period);
                }
                contract.ContractStartDate = DateTime.Now;
                contract.ContractEndDate = contract.ContractStartDate.AddDays((double)package.Day - 1);
                contract.Status = (int)ContractConst.IN_PROGRESS;
                await _unitOfWork.Contracts.Add(contract);

                var result = _unitOfWork.Save();

                if (await plusPoint(contract))
                    return contract;
            }
            return null;
        }
        private async Task<bool> plusPoint(Contract contract)
        {
            var provider= _serviceProvider.GetService(typeof(ICustomerService)) as ICustomerService;
            var customerList = await provider.GetAllCustomer(0);
            var customerIenumerable = from c in customerList where c.CustomerId == contract.CustomerId select c;
            var customer = new Customer();
            customer = customerIenumerable.FirstOrDefault();
            //plus point
            customer.Point += 100;
            if (await provider.UpdateCustomer(customer))
                return true;
            else
                return false;
        }

        public async Task<bool> DeleteContract(int contractId)
        {
            if (contractId != null)
            {
                var contract = await _unitOfWork.Contracts.GetById(contractId);
                if (contract != null)
                {
                    _unitOfWork.Contracts.Delete(contract);
                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }

        public async Task<IEnumerable<Contract>> GetAllContracts(int num)
        {
            var contractList = await _unitOfWork.Contracts.GetAll();
            if (num == 0)
            {
                return contractList;
            }
            var result = await _unitOfWork.Contracts.TakePage(num, contractList);
            return result;
        }


        public async Task<Contract> GetContractById(int contractId)
        {
            if (contractId != null)
            {
                var contract = await _unitOfWork.Contracts.GetById(contractId);
                if (contract != null)
                {
                    return contract;
                }
            }
            return null;
        }

        public async Task<bool> UpdateContract(string contractCode, Contract contract)
        {
            if (contract != null)
            {
                var contractUpdate = await _iContractRepository.getContractByContractCode(contractCode);
                if (contractUpdate.ContractId == contract.ContractId)
                {
                    // Update Asset
                    if (contract.ContractAsset != null)
                    {
                        var assetUpdate = await _iContractAssetService.UpdateContractAsset(contract.ContractAsset);
                        if (assetUpdate == false)
                        {
                            return false;
                        }
                    }

                    contractUpdate.StorageFee = contract.StorageFee;
                    contractUpdate.InsuranceFee = contract.InsuranceFee;
                    contractUpdate.Loan = contract.Loan;
                    contractUpdate.ContractVerifyImg = contract.ContractVerifyImg;
                    contractUpdate.ActualEndDate = contract.ActualEndDate;
                    contractUpdate.UpdateDate = DateTime.Now;

                    var package = await _unitOfWork.Packages.GetById(contract.PackageId);

                    if (package != null)
                    {

                        var fee = contractUpdate.InsuranceFee + contractUpdate.StorageFee;
                        var period = package.Day / package.PaymentPeriod;
                        double interest = 0;

                        // Use recommend interest if input
                        if (contractUpdate.InterestRecommend != 0)
                        {
                            interest = contractUpdate.InterestRecommend * 0.01;
                        }
                        interest = package.PackageInterest * 0.01;
                        contractUpdate.TotalProfit = (contractUpdate.Loan * (decimal)interest + fee) * period;
                    }

                    _unitOfWork.Contracts.Update(contractUpdate);

                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
        public async Task<DisplayContractDetail> GetContractDetail(int contractId)
        {
            if (contractId != null)
            {
                var contract = await _unitOfWork.Contracts.GetById(contractId);
                var customer = await _unitOfWork.Customers.GetById(contract.CustomerId);
                var package = await _iPackageService.GetPackageById(contract.PackageId, contract.InterestRecommend);
                List<InterestDiary> interestDiaries = (List<InterestDiary>)await _iInterestDiaryService.GetInteresDiariesByContractId(contractId);

                decimal interestPaid = 0;
                decimal interestDebt = 0;
                foreach (InterestDiary interestDiary in interestDiaries)
                {
                    interestPaid = interestPaid + interestDiary.PaidMoney;
                    interestDebt = interestDebt + interestDiary.InterestDebt;
                }
                var contractDetail = new DisplayContractDetail();
                contractDetail.CustomerName = customer.FullName;
                contractDetail.Phone = contract.Customer.Phone;
                contractDetail.Loan = contract.Loan;
                contractDetail.ContractStartDate = contract.ContractStartDate;
                contractDetail.ContractEndDate = contract.ContractEndDate;
                contractDetail.PackageInterest = package.PackageInterest;
                contractDetail.InterestPaid = interestPaid;
                contractDetail.InterestDebt = interestDebt;
                contractDetail.Status = contract.Status;

                if (contract != null)
                {
                    return contractDetail;
                }
            }
            return null;
        }

        public async Task<bool> UploadContractImg(int contractId, string customerImg, string contractImg)
        {
            var contract = await _unitOfWork.Contracts.GetById(contractId);
            if (contract != null && (customerImg != null || contractImg != null))
            {
                contract.CustomerVerifyImg = customerImg;
                contract.ContractVerifyImg = contractImg;
            }
            _unitOfWork.Contracts.Update(contract);
            var result = _unitOfWork.Save();

            if (result > 0)
                return true;
            else
                return false;
        }
        }
    }

