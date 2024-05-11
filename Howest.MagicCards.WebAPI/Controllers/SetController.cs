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
    public class SetController : ControllerBase
    {

        private readonly ISetRepository _setRepo;
        private readonly IMapper _mapper;

        public SetController(IMapper mapper, ISetRepository setRepo)
        {
            _setRepo = setRepo;
            _mapper = mapper;
        }

        
        [HttpGet]
        public ActionResult<IEnumerable<SetReadDTO>> GetSets()
        {
            IQueryable<Set> allSets = _setRepo.GetAllSetsAsync().Result;

            return Ok(allSets.
                ProjectTo<SetReadDTO>(_mapper.ConfigurationProvider));
        }
    }
}
