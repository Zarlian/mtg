using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;

namespace Howest.MagicCards.GraphQL.Queries
{
    public static class CardQuery
    {
        public static async Task<List<Card>> GetCardsAsync(ICardRepository cardRepo, string power, string toughness)
        {
            IQueryable<Card> cards = await cardRepo.GetAllCardsAsync();

            return cards.FilterByPower(power)
                        .FilterByToughness(toughness)
                        .ToList();
        }

        private static IQueryable<Card> FilterByPower(this IQueryable<Card> cards, string power)
        {
            if (!string.IsNullOrEmpty(power))
            {
                cards = cards.Where(c => c.Power == power);
            }

            return cards;
        }

        private static IQueryable<Card> FilterByToughness(this IQueryable<Card> cards, string toughness)
        {
            if (!string.IsNullOrEmpty(toughness))
            {
                cards = cards.Where(c => c.Toughness == toughness);
            }

            return cards;
        }
    }
}
