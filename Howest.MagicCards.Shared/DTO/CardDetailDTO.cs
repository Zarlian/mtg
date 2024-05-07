using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.DTO
{
    public record CardDetailDTO : CardReadDTO
    {
        public string Flavor { get; set; }
        public string Toughness { get; set; }
        public string Layout { get; set; }
        public string ConvertedManaCost { get; set; }
        public IEnumerable<string> CardColors { get; set; }
    }
}
