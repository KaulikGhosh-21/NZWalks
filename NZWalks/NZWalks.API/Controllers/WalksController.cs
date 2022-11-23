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
        
        public WalksController(IWalksRepositoryInterface walksRepository, IMapper mapper)
        {
            this.walksRepository = walksRepository;
            this.mapper = mapper;
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
    }
}
