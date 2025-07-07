using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackSystem.Types
{
    public class TimeTrapModel
    {
        [BsonId]
        public int Id { get; set; }
        public string PlayerName { get; set; }
        public float Speed { get; set; }
    }
}
