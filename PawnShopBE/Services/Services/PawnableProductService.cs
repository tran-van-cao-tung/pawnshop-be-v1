using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class PawnableProductService : IPawnableProductService
    {
        public IUnitOfWork _unitOfWork;

        public PawnableProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreatePawnableProduct(PawnableProduct pawnableProduct)
        {
            if (pawnableProduct != null)
            {               
                await _unitOfWork.PawnableProduct.Add(pawnableProduct);
                var result = _unitOfWork.Save();

                if (result > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public Task<IEnumerable<PawnableProduct>> GetAllPawnableProducts()
        {
            throw new NotImplementedException();
        }

        public Task<PawnableProduct> GetPawnableProductById(int pawnableProductId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdatePawnableProduct(PawnableProduct pawnableProduct)
        {
            if (pawnableProduct != null)
            {
                var pawnableProductUpdate = await _unitOfWork.PawnableProduct.GetById(pawnableProduct.PawnableProductId);
                if (pawnableProductUpdate != null)
                {
                    pawnableProductUpdate.Status = pawnableProduct.Status;
                    _unitOfWork.PawnableProduct.Update(pawnableProductUpdate);

                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
    }
}
