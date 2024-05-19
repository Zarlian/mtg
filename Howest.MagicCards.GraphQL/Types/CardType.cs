using GraphQL.Types;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;

namespace Howest.MagicCards.GraphQL.Types
{
    public class CardType : ObjectGraphType<Card>
    {
        public CardType()
        {
            Name = "Card";

            Field(c => c.Id, type: typeof(IdGraphType)).Description("The unique identifier of the card.");
            Field(c => c.Name, nullable: true).Description("The name of the card.");
            Field(c => c.ManaCost, nullable: true).Description("The mana cost of the card.");
            Field(c => c.ConvertedManaCost, nullable: true).Description("The converted mana cost of the card.");
            Field(c => c.Type, nullable: true).Description("The type of the card.");
            Field(c => c.RarityCode, nullable: true).Description("The rarity code of the card.");
            Field(c => c.SetCode, nullable: true).Description("The set code of the card.");
            Field(c => c.Text, nullable: true).Description("The text description of the card.");
            Field(c => c.Flavor, nullable: true).Description("The flavor text of the card.");
            Field(c => c.Number, nullable: true).Description("The card number within its set.");
            Field(c => c.Power, nullable: true).Description("The power value (if applicable) of the card.");
            Field(c => c.Toughness, nullable: true).Description("The toughness value (if applicable) of the card.");
            Field(c => c.Layout, nullable: true).Description("The layout of the card.");
            Field(c => c.MultiverseId, nullable: true).Description("The Multiverse ID of the card.");
            Field(c => c.OriginalImageUrl, nullable: true).Description("The original image URL of the card.");
            Field(c => c.Image, nullable: true).Description("The image URL of the card.");
            Field(c => c.OriginalText, nullable: true).Description("The original text of the card.");
            Field(c => c.OriginalType, nullable: true).Description("The original type of the card.");
            Field(c => c.MtgId, nullable: true).Description("The Magic: The Gathering ID of the card.");
            Field(c => c.Variations, nullable: true).Description("The variations of the card.");
            Field(c => c.Artist, type: typeof(ArtistType)).Description("The artist of the card.");
        }
    }
}
