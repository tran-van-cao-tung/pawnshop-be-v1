using AutoMapper.Execution;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore.Scaffolding;
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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
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
        DbContextClass _dbContextClass;
        public ContractService(IUnitOfWork unitOfWork, IContractRepository iContractRepository,
            IContractAssetService contractAssetService, IPackageService packageService,
            IInteresDiaryService interesDiaryService, IServiceProvider serviceProvider,
            ILedgerService ledger, IPawnableProductService pawnable, IWareHouseService wareHouse, DbContextClass dbContextClass)
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
            _dbContextClass = dbContextClass;
        }
        private void getParameter()
        {
            _branch = _serviceProvider.GetService(typeof(IBranchService)) as IBranchService;
            _ransom = _serviceProvider.GetService(typeof(IRansomService)) as IRansomService;
            _customer = _serviceProvider.GetService(typeof(ICustomerService)) as ICustomerService;
        }
        public async Task exporteExcel()
        {
            var listContract = await GetAllDisplayContracts(0);
            //set cấu hình excel
            Excel.Application exApp = new Excel.Application();
            Excel.Workbook exBook = exApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            Excel.Worksheet exSheet = (Excel.Worksheet)exBook.Worksheets[1];
            //setting content title
            exSheet.Range["E1"].Font.Size = 20;
            exSheet.Range["E1"].Font.Bold = true;
            exSheet.Range["E1"].Font.Color = Color.Red;
            exSheet.Range["E1"].Value = "HỢP ĐỒNG CẦM ĐỒ";
            //setting thông tin chung
            exSheet.Range["A3:J3"].Font.Size = 16;
            exSheet.Range["A3:J3"].Font.Bold = true;
            exSheet.Range["A3:J3"].Font.Color = Color.DarkBlue;
            //exSheet.Range["A3:J3"].Font.Background = Color.Gray;
            exSheet.Range["A3"].Value = "#";
            exSheet.Range["B3"].Value = "Mã HĐ";
            exSheet.Range["C3"].Value = "Khách Hàng";
            exSheet.Range["D3"].Value = "Mã TS";
            exSheet.Range["E3"].Value = "Tài Sản";
            exSheet.Range["F3"].Value = "Tiền Cầm";
            exSheet.Range["G3"].Value = "Ngày Cầm";
            exSheet.Range["H3"].Value = "Ngày Đến Hạn";
            exSheet.Range["I3"].Value = "Kho";
            exSheet.Range["J3"].Value = "Tình Trạng";
            //Setting column width
            exSheet.Range["B3"].ColumnWidth = 9;
            exSheet.Range["C3"].ColumnWidth = 16;
            exSheet.Range["A3"].ColumnWidth = 4;
            exSheet.Range["D3"].ColumnWidth = 9;
            exSheet.Range["E3"].ColumnWidth = 15;
            exSheet.Range["F3"].ColumnWidth = 12;
            exSheet.Range["G3"].ColumnWidth = 18;
            exSheet.Range["H3"].ColumnWidth = 18;
            exSheet.Range["I3"].ColumnWidth = 9;
            exSheet.Range["J3"].ColumnWidth = 18;
            //--------
            //exSheet.Range["B3"].Columns.AutoFit();
            //exSheet.Range["C3"].Columns.AutoFit();
            //exSheet.Range["A3"].Columns.AutoFit();
            //exSheet.Range["E3"].Columns.AutoFit();
            //exSheet.Range["F3"].Columns.AutoFit();
            //exSheet.Range["G3"].Columns.AutoFit();
            //exSheet.Range["H3"].Columns.AutoFit();
            //exSheet.Range["J3"].Columns.AutoFit();
            //exSheet.Range["I3"].Columns.AutoFit();
            //setting nội dung từng contract
            int number = 1;
            int i = 4;
            foreach (var x in listContract)
            {
                exSheet.Range["A" + i.ToString() + ":J" + i.ToString()].Font.Size = 12;
                exSheet.Range["A" + i.ToString()].Value = number.ToString();
                exSheet.Range["B" + i.ToString()].Value = x.ContractCode;
                exSheet.Range["C" + i.ToString()].Value = x.CustomerName;
                exSheet.Range["D" + i.ToString()].Value = x.CommodityCode;
                exSheet.Range["E" + i.ToString()].Value = x.ContractAssetName;
                exSheet.Range["F" + i.ToString()].Value = x.Loan.ToString();
                exSheet.Range["G" + i.ToString()].Value = x.ContractStartDate.ToString();
                exSheet.Range["H" + i.ToString()].Value = x.ContractEndDate.ToString();
                exSheet.Range["I" + i.ToString()].Value = x.WarehouseName;
                exSheet.Range["J" + i.ToString()].Value = getStatusContract(x.Status);
                i++;
                number++;
            }
            exSheet.Name = "Report";
            exApp.Visible = true;
            exBook.Activate();
            //save Excel
            //exBook.SaveAs("C:\\file.xls", Excel.XlFileFormat.xlWorkbookNormal,
            //    null, null, false, false,
            //    Excel.XlSaveAsAccessMode.xlExclusive,
            //    false, false, false, false, false);
            exApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(exBook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(exApp);
        }

        private string getStatusContract(int status)
        {
            switch (status)
            {
                case 1:
                    return "Đang Tiến Hành";
                case 2:
                    return "Quá Hạn";
                case 3:
                    return "Thanh Lý";
                case 4:
                    return "Đã Đóng";
            }
            return null;
        }

        public async Task<DisplayContractHomePage> getAllContractHomepage()
        {
            getParameter();
            //get all List
            var branchList = await _branch.GetAllBranch(0);
            var ledgerList = await _ledger.GetLedger();
            var contractList = await GetAllContracts(0);
            var ransomList = await _ransom.GetRansom();
            //var customerList = await _customer.GetAllCustomer(0);
            //var assetList = await _iContractAssetService.GetAllContractAssets();
            //var pawnableList = await _pawnable.GetAllPawnableProducts(0);
            //var wareHouseList = await _wareHouse.GetWareHouse(0);
            // khai báo filed hiển thị chung 
            decimal fund = 0;
            decimal loanLedger = 0;
            decimal recveivedInterest = 0;
            decimal totalProfit = 0;
            decimal ransomTotal = 0;
            //get displayHomePage
           // List<DisplayContractHomePage> listDipslay = new List<DisplayContractHomePage>();
            foreach (var contract in contractList)
            {
                var contractId = contract.ContractId;
                //display.contractCode = contract.ContractCode;
                //display.customerName = getCustomerName(customerList, contract.CustomerId);
                //display.assestCode = getAsset(contract.ContractAssetId, assetList, pawnableList, wareHouseList, 1);
                //display.assetName = getAsset(contract.ContractAssetId, assetList, pawnableList, wareHouseList, 2);
                //display.loanContract = contract.Loan;
                //display.startDate = contract.ContractStartDate;
                //display.endDate = contract.ContractEndDate;
                //display.wareName = getAsset(contract.ContractAssetId, assetList, pawnableList, wareHouseList, 3);
                //display.status = contract.Status;
                //get field hiển thị chung
                if (fund == 0)
                {
                    fund = decimal.Parse(getBranchName(contract.BranchId, branchList, false));
                }
                totalProfit += contract.TotalProfit;
                loanLedger += getLedger(ledgerList, contract.BranchId, true);
                recveivedInterest += getLedger(ledgerList, contract.BranchId, false);
                ransomTotal += getRansom(ransomList, contract.ContractId);
                //add list
                //listDipslay.Add(display);
            }
                DisplayContractHomePage x = new DisplayContractHomePage();
                //add field hiển thị chung
                x.totalProfit = totalProfit;
                x.loanLedger = loanLedger;
                x.fund = fund;
                x.recveivedInterest = recveivedInterest;
                x.ransomTotal = ransomTotal;
           
                return x;
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
            var ransomIenumerable = from r in ransomList where r.ContractId == contractId select r;
            var ransom = ransomIenumerable.FirstOrDefault();
            return ransom.TotalPay;
        }

        private decimal getLedger(IEnumerable<Ledger> ledgerList, int branchId, bool v)
        {
            var ledgerIenumerable = from l in ledgerList where l.BranchId == branchId select l;
            var ledger = ledgerIenumerable.FirstOrDefault();
            // true => get loan, false => get receiveInterest (lãi đã nhận)
            if (v)
                return ledger.Loan;
            else
                return ledger.RecveivedInterest;
        }

        private string getBranchName(int branchId, IEnumerable<Branch> branchList, bool v)
        {
            var branchIenumerable = from b in branchList where b.BranchId == branchId select b;
            var branch = branchIenumerable.FirstOrDefault();
            // true => get branch Name, false => get fund
            if (v)
                return branch.BranchName;
            else
                return branch.Fund.ToString();
        }

        private string getAsset(int contractAssetId, IEnumerable<ContractAsset> assetList,
            IEnumerable<PawnableProduct> pawnableList, IEnumerable<Warehouse> warehouseList, int num)
        {
            //get list contract asset
            var assetIenumerable = from a in assetList where a.ContractAssetId == contractAssetId select a;
            var asset = assetIenumerable.FirstOrDefault();
            // 1 => get code Asset, 2 => get name Asset, 3 => get ware name
            switch (num)
            {
                case 1:
                    var pawnableIenumerable = from p in pawnableList
                                              where
                                          p.PawnableProductId == asset.PawnableProductId
                                              select p;
                    var pawnable = pawnableIenumerable.FirstOrDefault();
                    return pawnable.CommodityCode;

                case 2:
                    return asset.ContractAssetName;

                case 3:
                    var wareIenumerable = from w in warehouseList where w.WarehouseId == asset.WarehouseId select w;
                    var wareHouse = wareIenumerable.FirstOrDefault();
                    return wareHouse.WarehouseName;
            }
            return null;
        }

        private string getCustomerName(IEnumerable<Customer> customerList, Guid customerId)
        {
            var customerIenumerable = from c in customerList where c.CustomerId == customerId select c;
            var customer = customerIenumerable.FirstOrDefault();
            return customer.FullName;
        }

        public async Task<bool> CreateContract(Contract contract)
        {
            if (contract != null)
            {
                var contractList = await GetAllContracts(0);
                var count = 0;
                if (contractList != null)
                {
                    count = contractList.Count();
                }
                contract.ContractCode = "CĐ-" + (count + 1).ToString();
                var package = await _iPackageService.GetPackageById(contract.PackageId, contract.InterestRecommend);

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
                if (result > 0)
                {
                    var ransomProvider = _serviceProvider.GetService(typeof(IRansomService)) as IRansomService;
                    var createRansom = await ransomProvider.CreateRansom(contract);
                    var interestProvider = _serviceProvider.GetService(typeof(IInteresDiaryService)) as IInteresDiaryService;
                    var createInterestDiary = await interestProvider.CreateInterestDiary(contract);
                    return true;
                }
                if (await plusPoint(contract))
                    return true;
            }
            return false;
        }
        private async Task<bool> plusPoint(Contract contract)
        {
            var provider = _serviceProvider.GetService(typeof(ICustomerService)) as ICustomerService;
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

        public async Task<ICollection<DisplayContractList>> GetAllDisplayContracts(int num)
        {
            var contractList = await _unitOfWork.Contracts.GetAll();
            //var contractJoinCustomerAndAsset = _dbContextClass.Contract
            //    .Include(c => c.Customer)
            //    .Include(c => c.ContractAsset)
            //    .ToListAsync();

            var contractJoinCustomerJoinAsset = from contract in _dbContextClass.Contract
                                                join customer in _dbContextClass.Customer
                                                on contract.CustomerId equals customer.CustomerId
                                                join contractAsset in _dbContextClass.ContractAsset
                                                on contract.ContractAssetId equals contractAsset.ContractAssetId
                                                join pawnableProduct in _dbContextClass.PawnableProduct
                                                on contractAsset.PawnableProductId equals pawnableProduct.PawnableProductId
                                                join warehouse in _dbContextClass.Warehouse
                                                on contractAsset.WarehouseId equals warehouse.WarehouseId
                                                select new
                                                {
                                                    ContractCode = contract.ContractCode,
                                                    CustomerName = customer.FullName,
                                                    CommodityCode = pawnableProduct.CommodityCode,
                                                    ContractAssetName = contractAsset.ContractAssetName,
                                                    ContractLoan = contract.Loan,
                                                    ContractStartDate = contract.ContractStartDate,
                                                    ContractEndDate = contract.ContractEndDate,
                                                    WarehouseName = warehouse.WarehouseName,
                                                    Status = contract.Status
                                                };

            List<DisplayContractList> displayContractList = new List<DisplayContractList>();
            foreach (var row in contractJoinCustomerJoinAsset)
            {
                DisplayContractList displayContract = new DisplayContractList();
                displayContract.ContractCode = row.ContractCode;
                displayContract.CustomerName = row.CustomerName;
                displayContract.CommodityCode = row.CommodityCode;
                displayContract.ContractAssetName = row.ContractAssetName;
                displayContract.Loan = row.ContractLoan;
                displayContract.ContractStartDate = row.ContractStartDate;
                displayContract.ContractEndDate = row.ContractEndDate;
                displayContract.WarehouseName = row.WarehouseName;
                displayContract.Status = row.Status;
                displayContractList.Add(displayContract);
            }
            List<DisplayContractList> result = await _iContractRepository.displayContractListTakePage(num, displayContractList);
            if (num == 0)
            {
                return displayContractList;
            }
            return result;
        }

        public async Task<IEnumerable<Contract>> GetAllContracts()
        {
            var contractList = await _unitOfWork.Contracts.GetAll();
            if (contractList != null)
            {
                return contractList;

            }
            return null;
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

        public async Task<bool> CreateContractExpiration(int contractId)
        {
            var contract = await _unitOfWork.Contracts.GetById(contractId);
            //using var transaction = await _dbContextClass.Database.BeginTransactionAsync();
            //try
            //{
            if (contract != null)
            {
                contract.ContractId = 0;
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
                var createContractresult = _unitOfWork.Save();
                if (createContractresult > 0)
                {
                    var ransomProvider = _serviceProvider.GetService(typeof(IRansomService)) as IRansomService;
                    await ransomProvider.CreateRansom(contract);
                    var interestProvider = _serviceProvider.GetService(typeof(IInteresDiaryService)) as IInteresDiaryService;
                    await interestProvider.CreateInterestDiary(contract);
                    var oldContract = await _unitOfWork.Contracts.GetById(contractId);
                    oldContract.Status = (int)ContractConst.CLOSE;
                    oldContract.ActualEndDate = DateTime.Now;
                    _unitOfWork.Contracts.Update(oldContract);
                    _unitOfWork.Save();
                    return true;
                }
                //// If everything succeeded, commit the transaction
                //await transaction.CommitAsync();
            }
            //} catch (Exception e)
            //{
            //    // If an error occurred, rollback the transaction
            //    await transaction.RollbackAsync();
            //    throw;
            //}

            return false;
        }

        public async Task<Contract> GetContractByContractCode(string contractCode)
        {
            if (contractCode != null)
            {
                var contract = await _iContractRepository.getContractByContractCode(contractCode);
                return contract;
            }
            return null;
        }

    }
}
