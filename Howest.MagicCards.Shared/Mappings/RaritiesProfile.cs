using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTO;

namespace Howest.MagicCards.Shared.Mappings
{
    public class RaritiesProfile : Profile
    {
        public RaritiesProfile() 
        {
            CreateMap<Rarity, RarityReadDTO>();
        }
    }
}
