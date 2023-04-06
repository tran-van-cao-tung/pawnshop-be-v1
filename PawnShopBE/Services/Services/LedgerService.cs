﻿using Microsoft.EntityFrameworkCore;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Infrastructure.Helpers;
using Services.Services.IServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class LedgerService : ILedgerService
    {
        private readonly IUnitOfWork _unit;
        private readonly DbContextClass _dbContext;

        public LedgerService(IUnitOfWork unitOfWork, DbContextClass dbContextClass)
        {
            _unit = unitOfWork;
            _dbContext = dbContextClass;
        }
        public async Task<bool> CreateLedger(Ledger ledger)
        {
            if (ledger != null)
            {
                await _unit.Ledgers.Add(ledger);
                var result = _unit.Save();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }



        public async Task<IEnumerable<Ledger>> GetLedger()
        {
            var result = await _unit.Ledgers.GetAll();
            return result;
        }

        public async Task<IEnumerable<Ledger>> GetLedgersByBranchId(int branchId)
        {
            try
            {
                var ledger = await _dbContext.Set<Ledger>()
                            .Where(l => l.BranchId == branchId)
                            .ToListAsync();
                if (ledger != null)
                {
                    return ledger;
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public async Task<bool> UpdateLedger(Ledger ledger)
        {
            var ledgerUpdate = _unit.Ledgers.SingleOrDefault(ledger, j => j.LedgerId == ledger.LedgerId);
            if (ledgerUpdate != null)
            {
                ledgerUpdate.ReceivedPrincipal = ledger.ReceivedPrincipal;
                ledgerUpdate.RecveivedInterest = ledger.RecveivedInterest;
                ledgerUpdate.Loan = ledger.Loan;
                ledgerUpdate.Balance = ledger.Balance;
                //ledgerUpdate.FromDate = ledger.FromDate;
                //ledgerUpdate.ToDate = ledger.ToDate;
                ledgerUpdate.Fund = ledger.Fund;
                ledgerUpdate.Status = ledger.Status;
                ledgerUpdate.LiquidationMoney = ledger.LiquidationMoney;

                _unit.Ledgers.Update(ledgerUpdate);
                var result = _unit.Save();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
