using AutoMapper;
using AutoMapper.QueryableExtensions;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.Shared.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Howest.MagicCards.WebAPI.Controllers
{
    [Route("api/[controller]")]
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
            return Ok(_rarityRepo.GetAllRarities().
                ProjectTo<RarityReadDTO>(_mapper.ConfigurationProvider));
        }

    }
}
