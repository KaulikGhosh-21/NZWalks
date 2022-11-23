using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepositoryInterface
    {
        private readonly NZWalksDbContext nZWalksDbContext;
        public WalkDifficultyRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }
        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await nZWalksDbContext.WalkDifficulty.ToListAsync();
        }

        public async Task<WalkDifficulty> GetWalkDifficultyAsync(Guid id)
        {
            return await nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<WalkDifficulty> AddWalkDifficultyAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await nZWalksDbContext.WalkDifficulty.AddAsync(walkDifficulty);
            await nZWalksDbContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> UpdateWalkDifficultyAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var dataToBeUpdated = await nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
            
            if(dataToBeUpdated == null)
            {
                return null;
            }

            dataToBeUpdated.Code = walkDifficulty.Code;
            await nZWalksDbContext.SaveChangesAsync();
            return dataToBeUpdated;
        }

        public async Task<WalkDifficulty> DeleteWalkDifficultyAsync(Guid id)
        {
            var dataToBeDeleted = await nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);

            if (dataToBeDeleted == null)
            {
                return null;
            }

            nZWalksDbContext.WalkDifficulty.Remove(dataToBeDeleted);
            await nZWalksDbContext.SaveChangesAsync();
            return dataToBeDeleted;

        }
    }
}
