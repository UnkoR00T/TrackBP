using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackSystem.Types
{
    public class SectorModel
    {
        [BsonId]
        public int Id { get; set; }
        public string PlayerName { get; set; }
        public int Sector {  get; set; }
        public double Time { get; set; }
    }
}
