using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackSystem.Types
{
    public class PersonalBestModel
    {
        [BsonId]
        public int Id { get; set; }
        public string PlayerName {  get; set; }
        public double LapTime { get; set; } = 9999;
        public Sectors Sectors { get; set; } = new Sectors()
        {
            One = 999999,
            Two = 999999,
            Three = 999999
        };
        public float TimeTrap { get; set; } = 0;
    }

    public class Sectors
    {
        public double One { get; set; }
        public double Two { get; set; }
        public double Three { get; set; }
    }
}
