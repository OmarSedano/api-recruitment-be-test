using IMDbApiLib;
using System.Threading.Tasks;
using IMDbApiLib.Models;

namespace ApiApplication.Services
{
    public class ImdbService : IImdbService
    {
        private const string ApiKey = "k_ecm9jl9u";
        private ApiLib _apiLib;

        public ImdbService()
        {
            _apiLib = new ApiLib(ApiKey);
        }

        //TODO: change call to base url of imdb api and check for 200 status
        public async Task<bool> IsHealthyAsync()
        {
            var boxOfficeData = await _apiLib.BoxOfficeAsync();
            return string.IsNullOrEmpty(boxOfficeData.ErrorMessage);
        }

        public async Task<TitleData> GetTitleDataAsync(string imdbId)
        {
            return await _apiLib.TitleAsync(imdbId);
        }
    }
}
