using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {

        private readonly IWalksRepositoryInterface walksRepository;
        private readonly IMapper mapper;
        private readonly IRegionRepositoryInterface regionRepository;
        private readonly IWalkDifficultyRepositoryInterface walkDifficultyRepository;

        public WalksController(
            IWalksRepositoryInterface walksRepository, 
            IMapper mapper,
            IRegionRepositoryInterface regionRepository,
            IWalkDifficultyRepositoryInterface walkDifficultyRepository
        )
        {
            this.walksRepository = walksRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalks()
        {
            var walksDomain = await walksRepository.GetAllAsync();

            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksDomain);

            return Ok(walksDTO);
        }


        [HttpGet]
        [Route("{Id}")]
        [ActionName("GetWalkById")]
        public async Task<IActionResult> GetWalkById(Guid Id)
        {
            var walkDomain = await walksRepository.GetWalkAsync(Id);

            if(walkDomain == null)
            {
                return NotFound();
            }

            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            return Ok(walkDTO);
        }


        [HttpPost]
        public async Task<IActionResult> AddWalk(Models.DTO.AddWalk addWalk)
        {

            if (!await(ValidateAddWalkAsync(addWalk)))
            {
                return BadRequest(ModelState);
            }

            // convert data from addwalk DTO model to domain model
            var domainData = new Models.Domain.Walk()
            {
                Name = addWalk.Name,
                Length = addWalk.Length,
                RegionId = addWalk.RegionId,
                WalkDifficultyId = addWalk.WalkDifficultyId,
            };

            // send data to repository
            var addedData = await walksRepository.AddWalkAsync(domainData);

            // convert data back to DTO model
            var dataDTO = mapper.Map<Models.DTO.Walk>(addedData);

            return CreatedAtAction(nameof(GetWalkById), new {Id = addedData.Id}, dataDTO);

        }

        [HttpPut]
        [Route("{Id}")]
        public async Task<IActionResult> UpdateWalk([FromRoute] Guid Id, [FromBody] Models.DTO.UpdateWalk updateWalk)
        {

            if (!await (ValidateUpdateWalkAsync(updateWalk)))
            {
                return BadRequest(ModelState);
            }

            // convert the updatewalk model to domain model
            var domainModelData = new Models.Domain.Walk()
            {
                Name=updateWalk.Name,
                Length=updateWalk.Length,
                RegionId=updateWalk.RegionId,
                WalkDifficultyId=updateWalk.WalkDifficultyId
            };

            // send data to walk repository
            var updatedWalk = await walksRepository.UpdateWalkAsync(Id, domainModelData);

            // check if null
            if (updatedWalk == null)
            {
                return NotFound();
            }

            // if not chnage back to region DTO model
            var updatedWalkDTO = mapper.Map<Models.DTO.Walk>(updatedWalk);
            return Ok(updatedWalkDTO);
        }


        [HttpDelete]
        [Route("{Id}")]
        public async Task<IActionResult> DeleteWalk(Guid Id)
        {
            var walk = await walksRepository.DeleteWalkAsync(Id);
            
            if(walk == null)
            {
                return NotFound();
            }

            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);
            return Ok(walkDTO);
        }


        #region Private methods

        private async Task<bool> ValidateAddWalkAsync(Models.DTO.AddWalk addWalk)

        {
            if (addWalk == null)
            {
                ModelState.AddModelError(nameof(addWalk),
                    $"The data provided cannot be null.");
                return false;
            };

            if (string.IsNullOrWhiteSpace(addWalk.Name))
            {
                ModelState.AddModelError(nameof(addWalk.Name),
                    $"{nameof(addWalk.Name)} cannot be null, empty or include white spaces.");
            };

            if (addWalk.Length <= 0)
            {
                ModelState.AddModelError(nameof(addWalk.Length),
                    $"{nameof(addWalk.Length)} cannot be less than or equal to zero.");
            };

            var region = await regionRepository.GetAsync(addWalk.RegionId);
            if(region == null)
            {
                ModelState.AddModelError(nameof(addWalk.RegionId),
                    $"{nameof(addWalk.RegionId)} is invalid.");
            }

            var walkDifficulty = await 
                walkDifficultyRepository.GetWalkDifficultyAsync(addWalk.WalkDifficultyId);

            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(addWalk.WalkDifficultyId),
                    $"{nameof(addWalk.WalkDifficultyId)} is invalid.");
            }

            if(ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalk updateWalk)

        {
            if (updateWalk == null)
            {
                ModelState.AddModelError(nameof(updateWalk),
                    $"The data provided cannot be null.");
                return false;
            };

            if (string.IsNullOrWhiteSpace(updateWalk.Name))
            {
                ModelState.AddModelError(nameof(updateWalk.Name),
                    $"{nameof(updateWalk.Name)} cannot be null, empty or include white spaces.");
            };

            if (updateWalk.Length <= 0)
            {
                ModelState.AddModelError(nameof(updateWalk.Length),
                    $"{nameof(updateWalk.Length)} cannot be less than or equal to zero.");
            };

            var region = await regionRepository.GetAsync(updateWalk.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalk.RegionId),
                    $"{nameof(updateWalk.RegionId)} is invalid.");
            }

            var walkDifficulty = await
                walkDifficultyRepository.GetWalkDifficultyAsync(updateWalk.WalkDifficultyId);

            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalk.WalkDifficultyId),
                    $"{nameof(updateWalk.WalkDifficultyId)} is invalid.");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
