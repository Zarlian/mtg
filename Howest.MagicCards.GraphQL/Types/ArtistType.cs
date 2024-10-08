﻿using GraphQL.Types;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;

namespace Howest.MagicCards.GraphQL.Types
{
    public class ArtistType : ObjectGraphType<Artist>
    {
        public ArtistType()
        {
            Name = "Artist";

            Field(a => a.Id, type: typeof(IdGraphType)).Description("The unique identifier of the artist.");
            Field(a => a.FullName).Description("The full name of the artist.");
            Field(a => a.Cards, type: typeof(ListGraphType<CardType>)).Description("The cards of the artist.");
        }
    }
}
