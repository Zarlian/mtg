using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTO;

namespace Howest.MagicCards.Shared.Mappings
{
    public class DecksProfile : Profile
    {
        public DecksProfile()
        {
            CreateMap<CardInDeckDTO, CardInDeck>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Count));

            CreateMap<CardInDeck, CardInDeckDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Count));

            CreateMap<DeckDTO, Deck>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Cards, opt => opt.MapFrom(src =>
                    src.Cards.Select(cardDto => new CardInDeck
                    {
                        Id = cardDto.Id,
                        Count = cardDto.Count
                    }).ToList()));  // Convert List to IQueryable

            CreateMap<Deck, DeckDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Cards, opt => opt.MapFrom(src =>
                    src.Cards.Select(card => new CardInDeckDTO
                    {
                        Id = card.Id,
                        Count = card.Count
                    }).ToList()));  // Convert IQueryable to List
        }

    }
}
