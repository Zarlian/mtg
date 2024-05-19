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
    public class SetController : ControllerBase
    {

        private readonly ISetRepository _setRepo;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public SetController(IMapper mapper, ISetRepository setRepo, IMemoryCache cache)
        {   
            _setRepo = setRepo;
            _mapper = mapper;
            _cache = cache;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SetReadDTO>>> GetSets()
        {
            string cacheKey = "Sets";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<RarityReadDTO> cachedSets))
            {
                return Ok(cachedSets);
            }

            IQueryable<Set> allSets = await _setRepo.GetAllSetsAsync();

            IEnumerable<SetReadDTO> sets = allSets
                .ProjectTo<SetReadDTO>(_mapper.ConfigurationProvider)
                .ToList();

            MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            };

            _cache.Set(cacheKey, sets, cacheOptions);

            return Ok(sets);

        }
    }
}
