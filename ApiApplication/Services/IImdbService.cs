using System.Threading.Tasks;

namespace ApiApplication.Services
{
    public interface IImdbService
    {
        Task<bool> IsHealthyAsync();
    }
}