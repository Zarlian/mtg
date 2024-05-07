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
            return Ok(_setRepo.GetAllSets().
                ProjectTo<SetReadDTO>(_mapper.ConfigurationProvider));
        }
    }
}
