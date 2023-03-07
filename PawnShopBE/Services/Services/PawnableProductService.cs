﻿using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attribute = PawnShopBE.Core.Models.Attribute;

namespace Services.Services
{
    public class PawnableProductService : IPawnableProductService
    {
        public IUnitOfWork _unitOfWork;
        private IAttributeService _attribute;
       

        public PawnableProductService(IUnitOfWork unitOfWork,IAttributeService attribute)
        {
            _unitOfWork = unitOfWork;
            _attribute = attribute;
        }

        public async Task<bool> CreatePawnableProduct(PawnableProduct pawnableProduct)
        {
            if (pawnableProduct != null)
            {
                _unitOfWork.PawnableProduct.Add(pawnableProduct);
                var result = _unitOfWork.Save();
                ////get pawnable
                //var pawnableList = await GetAllPawnableProducts();
                //var pawnableId = pawnableList.LastOrDefault().PawnableProductId;
                ////create Attribute
                //var attribute = new Attribute();
                //foreach (var x in pawnableProduct.Attributes)
                //{
                //    attribute.Description = x.Description;
                //    attribute.PawnableProductId = pawnableId;
                //}
                //// var attribute = _mapper.Map<Attribute>(attributeDTO);
                //try
                //{
                //    var respone = await _attribute.CreateAttribute(attribute);
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex.Message);
                //}
                if (result > 0)
                    return true;
            }
            return false;
        }

        public async Task<IEnumerable<PawnableProduct>> GetAllPawnableProducts()
        {
            var result = await _unitOfWork.PawnableProduct.GetAll();
            return result;
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
                    pawnableProductUpdate.TypeOfProduct=pawnableProduct.TypeOfProduct;
                    pawnableProductUpdate.CommodityCode=pawnableProduct.CommodityCode;
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
