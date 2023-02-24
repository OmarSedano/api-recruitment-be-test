using IMDbApiLib;
using System.Threading.Tasks;

namespace ApiApplication.Services
{
    public class ImdbService : IImdbService
    {
        //TODO: change call to base url of imdb api and check for 200 status
        public async Task<bool> IsHealthyAsync()
        {
            var apiLib = new ApiLib("k_ecm9jl9u");
            var boxOfficeData = await apiLib.BoxOfficeAsync();
            return string.IsNullOrEmpty(boxOfficeData.ErrorMessage);
        }
    }
}
