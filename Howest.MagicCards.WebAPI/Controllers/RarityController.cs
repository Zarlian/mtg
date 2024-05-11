using AutoMapper;
using AutoMapper.QueryableExtensions;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.Shared.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Howest.MagicCards.WebAPI.Controllers
{
    [ApiVersion("1.1")]
    [ApiVersion("1.5")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class RarityController : ControllerBase
    {
        private readonly IRarityRepository _rarityRepo;
        private readonly IMapper _mapper;

        public RarityController(IMapper mapper, IRarityRepository rarityRepo)
        {
            _rarityRepo = rarityRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RarityReadDTO>> GetRarities()
        {
            IQueryable<Rarity> allRarities = _rarityRepo.GetAllRaritiesAsync().Result;

            return Ok(allRarities.
                ProjectTo<RarityReadDTO>(_mapper.ConfigurationProvider));
        }

    }
}
