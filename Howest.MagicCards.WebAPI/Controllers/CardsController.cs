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
        public ActionResult<PagedResponse<IEnumerable<CardReadDTO>>> GetCards([FromQuery] CardFilter filter)
        {

            IQueryable<Card> allCards = _cardRepo.GetAllCards();

            if (allCards == null)
            {
                return NotFound("No cards found");
            }


            IQueryable<Card> filteredCards = allCards.ToFilteredList(filter);

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
    }
}
