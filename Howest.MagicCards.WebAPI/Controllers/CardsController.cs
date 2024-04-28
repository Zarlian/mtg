using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Howest.MagicCards.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardRepository _cardRepo;

        public CardsController()
        {
            _cardRepo = new CardRepository();
        }

        [HttpGet]
        public ActionResult<IEnumerable<CardReadDTO>> GetCards()
        {
            return Ok(_cardRepo.GetAllCards());
        }
    }
}
