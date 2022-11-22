using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
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
    }
}
