using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Runtime.InteropServices;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        //private readonly NZWalksDbContext nzWalksDbContext;

        //public RegionsController(NZWalksDbContext nzWalksDbContext)
        //{
        //    this.nzWalksDbContext = nzWalksDbContext;
        //}


        private readonly IRegionRepositoryInterface regionRepository;
        private readonly IMapper mapper;
        public RegionsController(
            IRegionRepositoryInterface regionRepository,
            IMapper mapper   
        )
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }



        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {

            var regions = await regionRepository.GetAllAsync();

            //var regionsDTO = new List<Models.DTO.Region>();

            //regions.ToList().ForEach(domainRegion =>
            //{
            //    var regionDTO = new Models.DTO.Region
            //    {
            //        Id = domainRegion.Id,
            //        Name = domainRegion.Name,
            //        Code = domainRegion.Code,
            //        Area = domainRegion.Area,
            //        Lat = domainRegion.Lat,
            //        Long = domainRegion.Long,
            //        Population = domainRegion.Population,
            //    };

            //    regionsDTO.Add(regionDTO);
            //});

            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);


            //var regions = new List<Region>()
            //{
            //    new Region
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Wellignton",
            //        Code = "WLG",
            //        Area = 227755,
            //        Lat = -1.8122,
            //        Long = 78.990,
            //        Population = 10000
            //    },
            //    new Region
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Auckland",
            //        Code = "AUK",
            //        Area = 227755,
            //        Lat = -1.8122,
            //        Long = 78.990,
            //        Population = 10000
            //    }
            //};

            return Ok(regionsDTO);
        }




        [HttpGet]
        [Route("{Id}")]
        [ActionName("GetRegionById")]
        public async Task<IActionResult> GetRegionById(Guid Id)
        {
            var region = await regionRepository.GetAsync(Id);

            if (region == null)
            {
                return NotFound();
            }

            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            return Ok(regionDTO);
        }




        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {

            if (!ValidateAddRegionAsync(addRegionRequest))
            {
                return BadRequest(ModelState);
            }

            // convert the addregionrequest contract model to domain model
            var regionDTO = new Models.Domain.Region()
            {
                Name = addRegionRequest.Name,
                Code = addRegionRequest.Code,
                Population = addRegionRequest.Population,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Area = addRegionRequest.Area,
            };

            // send region data to repository
            var region = await regionRepository.AddAsync(regionDTO);

            // convert domain model data again to DTO model
            var regionDTOData = new Models.DTO.Region()
            {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code,
                Population = region.Population,
                Lat = region.Lat,
                Long = region.Long,
                Area = region.Area,
            };

            return CreatedAtAction(nameof(GetRegionById), new { id = regionDTOData.Id },
                regionDTOData);
        }




        [HttpDelete]
        [Route("{Id}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid Id)
        {
            var region = await regionRepository.DeleteAsync(Id);

            if(region == null)
            {
                return NotFound();
            }

            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            return Ok(regionDTO);
        }




        [HttpPut]
        [Route("{Id}")]
        public async Task<IActionResult> UpdateRegionAsync(
            [FromRoute] Guid Id, 
            [FromBody] Models.DTO.UpdateRegionRequest updateRegionRequest
        )
        {
            if (!ValidateUpdateRegionAsync(updateRegionRequest))
            {
                return BadRequest(ModelState);
            }

            // convert the updateregionrequest model to domain model
            var domainModelData = new Models.Domain.Region()
            {
                Name = updateRegionRequest.Name,
                Code = updateRegionRequest.Code,
                Population = updateRegionRequest.Population,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Area = updateRegionRequest.Area,
            };

            // send data to region repository
            var updatedRegion = await regionRepository.UpdateAsync(Id, domainModelData);

            // check if null
            if(updatedRegion == null)
            {
                return NotFound();
            }

            // if not chnage back to region DTO model
            var updatedRegionDTO = mapper.Map<Models.DTO.Region>(updatedRegion);
            return Ok(updatedRegionDTO);
        }

        #region private methods

        private bool ValidateAddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)

        {
            if(addRegionRequest == null)
            {
                ModelState.AddModelError(nameof(addRegionRequest),
                    $"The data provided cannot be null.");
                return false;
            };

            if (string.IsNullOrWhiteSpace(addRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Code),
                    $"{nameof(addRegionRequest.Code)} cannot be null, empty or include white spaces.");
            };

            if (addRegionRequest.Code.Length > 5)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Code),
                    $"{nameof(addRegionRequest.Code)} cannot be more than 5 charecters.");
            };


            if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Name),
                    $"{nameof(addRegionRequest.Name)} cannot be null, empty or include white spaces.");
            };

            if (addRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Area),
                    $"{nameof(addRegionRequest.Area)} cannot be less than or equal to zero.");
            };

            if (addRegionRequest.Lat == 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Lat),
                    $"{nameof(addRegionRequest.Lat)} cannot be equal to zero.");
            };

            if (addRegionRequest.Long == 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Long),
                    $"{nameof(addRegionRequest.Long)} cannot be equal to zero.");
            }

            if(ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;


        }

        private bool ValidateUpdateRegionAsync(Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            if (updateRegionRequest == null)
            {
                ModelState.AddModelError(nameof(updateRegionRequest),
                    $"The data provided cannot be null.");
                return false;
            };

            if (string.IsNullOrWhiteSpace(updateRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code),
                    $"{nameof(updateRegionRequest.Code)} cannot be null, empty or include white spaces.");
            };

            if (updateRegionRequest.Code.Length > 5)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code),
                    $"{nameof(updateRegionRequest.Code)} cannot be more than 5 charecters.");
            };


            if (string.IsNullOrWhiteSpace(updateRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Name),
                    $"{nameof(updateRegionRequest.Name)} cannot be null, empty or include white spaces.");
            };

            if (updateRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Area),
                    $"{nameof(updateRegionRequest.Area)} cannot be less than or equal to zero.");
            };

            if (updateRegionRequest.Lat == 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Lat),
                    $"{nameof(updateRegionRequest.Lat)} cannot be equal to zero.");
            };

            if (updateRegionRequest.Long == 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Long),
                    $"{nameof(updateRegionRequest.Long)} cannot be equal to zero.");
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
