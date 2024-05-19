using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using Howest.MagicCards.Shared.Wrappers;
using Shared.Extensions;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.Filters;
using Shared.Wrappers;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using Amazon.Runtime.Internal.Util;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Howest.MagicCards.WebAPI.Controllers.V1_1
{
    [ApiVersion("1.1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardRepository _cardRepo;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public CardsController(IMapper mapper, ICardRepository cardRepo, IMemoryCache cache)
        {
            _cardRepo = cardRepo;
            _mapper = mapper;
            _cache = cache;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CardReadDTO), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<ActionResult<PagedResponse<IEnumerable<CardReadDTO>>>> GetCards([FromQuery] CardWebFilter filter)
        {

            try
            {
                string cacheKey = GetCacheKey(filter);

                PagedResponse<IEnumerable<CardReadDTO>> cachedResponse = _cache.Get<PagedResponse<IEnumerable<CardReadDTO>>>(cacheKey);

                if (cachedResponse != null)
                {
                    return Ok(cachedResponse);
                }

                IQueryable<Card> allCards = await _cardRepo.GetAllCardsAsync();

                IQueryable<Card> filteredCards = allCards.ToFilteredList(filter);

                if (filteredCards == null || !filteredCards.Any())
                {
                    return StatusCode(StatusCodes.Status404NotFound,
                        new Response<CardReadDTO>()
                        {
                            Succeeded = false,
                            Errors = [$"Status code: {StatusCodes.Status404NotFound}"],
                            Message = "No cards found"
                        });
                }

                int totalCount = filteredCards.Count();
                int totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize);

                PagedResponse<IEnumerable<CardReadDTO>> response = new PagedResponse<IEnumerable<CardReadDTO>>(
                                                                                    filteredCards
                                                                                        .ToPagedList(filter.PageNumber, filter.PageSize)
                                                                                        .ProjectTo<CardReadDTO>(_mapper.ConfigurationProvider)
                                                                                        .ToList(),
                                                                                        filter.PageNumber,
                                                                                        filter.PageSize,
                                                                                        totalCount,
                                                                                        totalPages);


                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                };

                _cache.Set(cacheKey, response, cacheOptions);

                return Ok(response);
            }
            catch (Exception error) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response<CardReadDTO>()
                    {
                        Succeeded = false,
                        Errors = [$"Status code: {StatusCodes.Status500InternalServerError}"],
                        Message = $"({error.Message}) "
                    });
            }

        }
        private string GetCacheKey(CardWebFilter filter)
        {
            return $"cards_{JsonSerializer.Serialize(filter)}";
        }
    }    
}

namespace Howest.MagicCards.WebAPI.Controllers.V1_5
{
    [ApiVersion("1.5")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardRepository _cardRepo;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public CardsController(IMapper mapper, ICardRepository cardRepo, IMemoryCache cache)
        {
            _cardRepo = cardRepo;
            _mapper = mapper;
            _cache = cache;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CardReadDTO), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<ActionResult<PagedResponse<IEnumerable<CardReadDTO>>>> GetCards([FromQuery] CardWebFilter filter)
        {

            try
            {
                string cacheKey = GetCacheKey(filter);

                PagedResponse<IEnumerable<CardReadDTO>> cachedResponse = _cache.Get<PagedResponse<IEnumerable<CardReadDTO>>>(cacheKey);

                if (cachedResponse != null)
                {
                    return Ok(cachedResponse);
                }

                IQueryable<Card> allCards = await _cardRepo.GetAllCardsAsync();

                IQueryable<Card> filteredCards = allCards.ToFilteredList(filter);

                if (filteredCards == null || !filteredCards.Any())
                {
                    return StatusCode(StatusCodes.Status404NotFound,
                        new Response<CardReadDTO>()
                        {
                            Succeeded = false,
                            Errors = [$"Status code: {StatusCodes.Status404NotFound}"],
                            Message = "No cards found"
                        });
                }

                int totalCount = filteredCards.Count();
                int totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize);

                PagedResponse<IEnumerable<CardReadDTO>> response = new PagedResponse<IEnumerable<CardReadDTO>>(
                                                                                    filteredCards
                                                                                        .Sort(filter.SortBy)
                                                                                        .ToPagedList(filter.PageNumber, filter.PageSize)
                                                                                        .ProjectTo<CardReadDTO>(_mapper.ConfigurationProvider)
                                                                                        .ToList(),
                                                                                        filter.PageNumber,
                                                                                        filter.PageSize,
                                                                                        totalCount,
                                                                                        totalPages);


                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                };

                _cache.Set(cacheKey, response, cacheOptions);

                return Ok(response);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response<CardReadDTO>()
                    {
                        Succeeded = false,
                        Errors = [$"Status code: {StatusCodes.Status500InternalServerError}"],
                        Message = $"({error.Message}) "
                    });
            }

        }
        private string GetCacheKey(CardWebFilter filter)
        {
            return $"cards_{JsonSerializer.Serialize(filter)}";
        }

    

        [HttpGet("{id:int}", Name = "GetCardById")]
        [ProducesResponseType(typeof(CardDetailDTO), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<ActionResult<Response<CardDetailDTO>>> GetCard(int id)
        {
            try
            {

                string cacheKey = $"card_{id}";

                Response<CardDetailDTO> cachedCard = _cache.Get<Response<CardDetailDTO>>(cacheKey);
                if (cachedCard != null)
                {
                    return cachedCard;
                }


                Card card = await _cardRepo.GetCardByIdAsync(id);

                if (card == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound,
                        new Response<CardDetailDTO>()
                        {
                            Succeeded = false,
                            Errors = [$"Status code: {StatusCodes.Status404NotFound}"],
                            Message = "No cards found"
                        });
                }

                CardDetailDTO cardDetailDto = _mapper.Map<CardDetailDTO>(card);

                Response<CardDetailDTO> response = new Response<CardDetailDTO>(cardDetailDto);

                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                };

                _cache.Set(cacheKey, response, cacheOptions);

                return Ok(response);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response<CardDetailDTO>()
                    {
                        Succeeded = false,
                        Errors = [$"Status code: {StatusCodes.Status500InternalServerError}"],
                        Message = $"({error.Message}) "
                    });
            }
        }
    }
}