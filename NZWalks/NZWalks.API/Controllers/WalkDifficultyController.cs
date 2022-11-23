using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepositoryInterface walkDifficultyRepository;
        private readonly IMapper mapper;
        public WalkDifficultyController(
            IWalkDifficultyRepositoryInterface walkDifficultyRepository,
            IMapper mapper
        )
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficulty()
        {
            var walkDifficultyDomain = await walkDifficultyRepository.GetAllAsync();

            var walkDifficultyDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficultyDomain);

            return Ok(walkDifficultyDTO);
        }

        [HttpGet]
        [Route("{Id}")]
        [ActionName("GetWalkDifficulty")]
        public async Task<IActionResult> GetWalkDifficulty(Guid Id)
        {
            var walkDifficultyDomain = await walkDifficultyRepository.GetWalkDifficultyAsync(Id);

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            return Ok(walkDifficultyDTO);
        }


        [HttpPost]
        public async Task<IActionResult> AddWalkDifficulty(Models.DTO.AddWalkDifficulty addWalkDifficulty)
        {
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty()
            {
                Code = addWalkDifficulty.Code
            };

            var addedData = await walkDifficultyRepository.AddWalkDifficultyAsync(walkDifficultyDomain);

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(addedData);

            return CreatedAtAction(nameof(GetWalkDifficulty), new { Id = walkDifficultyDTO.Id }, walkDifficultyDTO);
        }


        [HttpPut]
        [Route("{Id}")]
        public async Task<IActionResult> UpdateWalkDifficulty(
            [FromRoute] Guid Id,
            [FromBody] Models.DTO.UpdateWalkDifficulty updateWalkDifficulty
        )
        {
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty()
            {
                Code = updateWalkDifficulty.Code
            };

            var updatedData = 
                await walkDifficultyRepository.UpdateWalkDifficultyAsync(Id, walkDifficultyDomain);

            if(updatedData == null)
            {
                return NotFound();
            }

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(updatedData);

            return Ok(walkDifficultyDTO);
        }

        [HttpDelete]
        [Route("{Id}")]
        public async Task<IActionResult> DeleteWalkDifficulty(Guid Id)
        {
            var dataDomain = await walkDifficultyRepository.DeleteWalkDifficultyAsync(Id);

            if(dataDomain == null)
            {
                return NotFound();
            }

            var dataDTO = mapper.Map<Models.DTO.WalkDifficulty>(dataDomain);

            return Ok(dataDTO);
        }
    }
}
