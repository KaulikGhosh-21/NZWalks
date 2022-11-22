using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepositoryInterface
    {
        Task<IEnumerable<Region>> GetAllAsync();
    }
}
