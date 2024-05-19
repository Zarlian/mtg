﻿using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface IArtistRepository
    {
        Task<IQueryable<Artist>> GetAllArtistsAsync();

        Task<Artist> GetArtistByIdAsync(long id);
    }
}
