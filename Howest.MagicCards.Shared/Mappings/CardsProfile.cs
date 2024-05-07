using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTO;

namespace Howest.MagicCards.Shared.Mappings
{
    public class CardsProfile : Profile
    {
        public CardsProfile() 
        {
            CreateMap<Card, CardReadDTO>()
                .ForMember(dto => dto.ImageUrl,
                            opt => opt.MapFrom(c => c.OriginalImageUrl))
                .ForMember(dto => dto.Rarity,
                            opt => opt.MapFrom(c => c.Rarity.Name))
                .ForMember(dto => dto.Artist,
                            opt => opt.MapFrom(c => c.Artist.FullName))
                .ForMember(dto => dto.Set,
                            opt => opt.MapFrom(c => c.Set.Name))
                .ReverseMap();

            CreateMap<Card, CardDetailDTO>()
                 .IncludeBase<Card, CardReadDTO>()
                 .ForMember(dto => dto.CardColors,
                            opt => opt.MapFrom(c => c.CardColors.Select(cc => cc.Color.Name)))
                 .ReverseMap();

            CreateMap<CardInDeckDTO, CardInDeck>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Count));

            CreateMap<CardInDeck, CardInDeckDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Count));

            
        }

    }
}
