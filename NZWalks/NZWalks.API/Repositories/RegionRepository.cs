using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class RegionRepository : IRegionRepositoryInterface
    {

        private readonly NZWalksDbContext nzWalksDbContext;

        public RegionRepository(NZWalksDbContext nzWalksDbContext)
        {
            this.nzWalksDbContext = nzWalksDbContext;
        }

        public async Task<Region> AddAsync(Region region)
        {
            region.Id = Guid.NewGuid();
            await nzWalksDbContext.Regions.AddAsync(region);
            await nzWalksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var regionToBeDeleted =
                await nzWalksDbContext.Regions.FirstOrDefaultAsync(ele => ele.Id == id);

            if(regionToBeDeleted == null)
            {
                return null;
            }

            nzWalksDbContext.Regions.Remove(regionToBeDeleted);
            await nzWalksDbContext.SaveChangesAsync();
            return regionToBeDeleted;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await nzWalksDbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetAsync(Guid id)
        {
            return await nzWalksDbContext.Regions.FirstOrDefaultAsync(ele => ele.Id == id);
        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
            var regionToUpdate =
                await nzWalksDbContext.Regions.FirstOrDefaultAsync(ele => ele.Id == id);

            if(region == null)
            {
                return null;
            }

            regionToUpdate.Code = region.Code;
            regionToUpdate.Name = region.Name;
            regionToUpdate.Area = region.Area;
            regionToUpdate.Lat = region.Lat;
            regionToUpdate.Long = region.Long;
            regionToUpdate.Population = region.Population;

            await nzWalksDbContext.SaveChangesAsync();
            return regionToUpdate;

        }
    }
}
