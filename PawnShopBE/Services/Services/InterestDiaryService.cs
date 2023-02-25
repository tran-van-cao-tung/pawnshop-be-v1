using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class InterestDiaryService : IInteresDiaryService
    {
        private readonly IUnitOfWork _unit;
        private readonly InterestDiary diary;

        public InterestDiaryService(IUnitOfWork unitOfWork)
        {
            _unit = unitOfWork;
        }
        public async Task<bool> CreateInteresDiary(InterestDiary interestDiary)
        {
            if (interestDiary != null)
            {
                await _unit.InterestDiaries.Add(interestDiary);
                var result = _unit.Save();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> DeleteInteresDiary(int interestDiaryId)
        {

            var diaryDelete = _unit.InterestDiaries.SingleOrDefault(diary, j => j.InterestDiaryId == interestDiaryId);
            if (diaryDelete != null)
            {
                _unit.InterestDiaries.Delete(diaryDelete);
                var result = _unit.Save();
                if (result > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<IEnumerable<InterestDiary>> GetInteresDiary()
        {
            var result = await _unit.InterestDiaries.GetAll();
            return result;
        }

        public Task<InterestDiary> GetInteresDiaryById(int interestDiaryId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateInteresDiary(InterestDiary interestDiary)
        {
            var diaryUpdate = _unit.InterestDiaries.SingleOrDefault
                (interestDiary, j => j.InterestDiaryId == interestDiary.InterestDiaryId);
            if (diaryUpdate != null)
            {
                diaryUpdate.ContractId = interestDiary.ContractId;
                diaryUpdate.Payment = interestDiary.Payment;
                diaryUpdate.Penalty = interestDiary.Penalty;
                diaryUpdate.TotalPay = interestDiary.TotalPay;
                diaryUpdate.PaidMoney = interestDiary.PaidMoney;
                diaryUpdate.DueDate = interestDiary.DueDate;
                diaryUpdate.NextDueDate = interestDiary.NextDueDate;
                diaryUpdate.PaidDate = interestDiary.PaidDate;
                diaryUpdate.Status = interestDiary.Status;
                diaryUpdate.Description = interestDiary.Description;
                diaryUpdate.ProofImg = interestDiary.Description;
                _unit.InterestDiaries.Update(diaryUpdate);
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
