using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.IServices
{
    public interface IInteresDiaryService
    {
        Task<bool> CreateInteresDiary(InterestDiary interestDiary);
        Task<IEnumerable<InterestDiary>> GetInteresDiary();
        Task<InterestDiary> GetInteresDiaryById(int interestDiaryId);
        Task<bool> UpdateInteresDiary(InterestDiary interestDiary);
        Task<bool> DeleteInteresDiary(int interestDiaryId);
    }
}
