using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalksRepositoryInterface
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public WalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Walk> AddWalkAsync(Walk walk)
        {
            walk.Id = Guid.NewGuid();
            await nZWalksDbContext.AddAsync(walk);
            await nZWalksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteWalkAsync(Guid id)
        {
            var dataToBeDeleted = await nZWalksDbContext.Walks.FirstOrDefaultAsync(
                x => x.Id == id
            );

            if(dataToBeDeleted == null)
            {
                return null;
            }

            nZWalksDbContext.Walks.Remove(dataToBeDeleted);
            await nZWalksDbContext.SaveChangesAsync();
            return dataToBeDeleted;

        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await 
                nZWalksDbContext.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .ToListAsync();
        }

        public async Task<Walk> GetWalkAsync(Guid id)
        {
            return await 
                nZWalksDbContext.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> UpdateWalkAsync(Guid id, Walk walk)
        {
            var dataToBeUpdated = await nZWalksDbContext.Walks.FirstOrDefaultAsync(
                x => x.Id == id
            );

            if(dataToBeUpdated == null)
            {
                return null;
            }

            dataToBeUpdated.Name = walk.Name;
            dataToBeUpdated.Length = walk.Length;
            dataToBeUpdated.RegionId = walk.RegionId;
            dataToBeUpdated.WalkDifficultyId = walk.WalkDifficultyId;

            await nZWalksDbContext.SaveChangesAsync();
            return dataToBeUpdated;
        }
    }
}
