using Amazon.Runtime.Internal.Util;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.Shared.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

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
        private readonly IMemoryCache _cache;

        public RarityController(IMapper mapper, IRarityRepository rarityRepo, IMemoryCache cache)
        {
            _rarityRepo = rarityRepo;
            _mapper = mapper;
            _cache = cache;
            _cache = cache;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RarityReadDTO>>> GetRaritiesAsync()
        {

            string cacheKey = "Rarities";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<RarityReadDTO> cachedRarities))
            {
                return Ok(cachedRarities);
            }

            IQueryable<Rarity> allRarities = await _rarityRepo.GetAllRaritiesAsync();

            IEnumerable<RarityReadDTO> rarities = allRarities
                .ProjectTo<RarityReadDTO>(_mapper.ConfigurationProvider)
                .ToList();

            MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            };

            _cache.Set(cacheKey, rarities, cacheOptions);

            return Ok(rarities);

        }

    }
}
