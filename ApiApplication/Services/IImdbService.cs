using IMDbApiLib.Models;
using System.Threading.Tasks;

namespace ApiApplication.Services
{
    public interface IImdbService
    {
        Task<bool> IsHealthyAsync();
        Task<TitleData> GetTitleDataAsync(string imdbId);
    }
}