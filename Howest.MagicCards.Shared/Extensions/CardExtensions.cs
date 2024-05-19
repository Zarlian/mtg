using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.Filters;
using Microsoft.OpenApi.Extensions;
using System.Linq;
using System.Net.NetworkInformation;

namespace Shared.Extensions;

public static class CardExtensions
{
    public static IQueryable<Card> ToFilteredList(this IQueryable<Card> cards, CardWebFilter filter)
    {
        return cards.Where(c =>    c.Name.Contains(filter.CardName)
                                & c.Text.Contains(filter.CardText)
                                & c.Set.Name.Contains(filter.Set)
                                & c.Rarity.Name.Contains(filter.Rarity)
                                & c.Artist.FullName.Contains(filter.Artist)
                                & c.Type.Contains(filter.CardType)
                                );
    }


    public static IQueryable<Card> Sort(this IQueryable<Card> cards, string orderBy)
    {
        bool asc = true;

        if(!string.IsNullOrEmpty(orderBy))
        {
            asc = orderBy.StartsWith("asc");
        }

        return asc ? cards.OrderBy(c => c.Name) : cards.OrderByDescending(c => c.Name);
    }
}


