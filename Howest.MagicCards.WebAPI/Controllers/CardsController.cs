using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using Shared.Wrappers;
using Shared.Filters;
using Shared.Extensions;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.Filters;
using Howest.MagicCards.Shared.Wrappers;

namespace Howest.MagicCards.WebAPI.Controllers.V1_1
{
    [ApiVersion("1.1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardRepository _cardRepo;
        private readonly IMapper _mapper;

        public CardsController(IMapper mapper, ICardRepository cardRepo)
        {
            _cardRepo = cardRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CardReadDTO), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<ActionResult<PagedResponse<IEnumerable<CardReadDTO>>>> GetCards([FromQuery] CardFilter filter)
        {

            try
            {
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

                return Ok(new PagedResponse<IEnumerable<CardReadDTO>>(
                    filteredCards
                    .ToPagedList(filter.PageNumber, filter.PageSize)
                    .ProjectTo<CardReadDTO>(_mapper.ConfigurationProvider)
                    .ToList(),
                    filter.PageNumber,
                    filter.PageSize,
                    totalCount,
                    totalPages)
                    );
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

        public CardsController(IMapper mapper, ICardRepository cardRepo)
        {
            _cardRepo = cardRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CardReadDTO), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<ActionResult<PagedResponse<IEnumerable<CardReadDTO>>>> GetCards([FromQuery] CardFilter filter)
        {

            try
            {
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

                return Ok(new PagedResponse<IEnumerable<CardReadDTO>>(
                    filteredCards
                    .Sort(filter.SortBy)
                    .ToPagedList(filter.PageNumber, filter.PageSize)
                    .ProjectTo<CardReadDTO>(_mapper.ConfigurationProvider)
                    .ToList(),
                    filter.PageNumber,
                    filter.PageSize,
                    totalCount,
                    totalPages)
                    );
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

        [HttpGet("{id:int}", Name = "GetCardById")]
        [ProducesResponseType(typeof(CardDetailDTO), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<ActionResult<Response<CardDetailDTO>>> GetCard(int id)
        {
            try
            {
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

                return Ok(new Response<CardDetailDTO>(cardDetailDto));
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