using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackSystem.Types
{
    public class RaceInfoModel
    {
        [BsonId]
        public int Id { get; set; }
        public bool DRS {  get; set; }
        public string Session {  get; set; }
        public double SessionEndsAt { get; set; }
        public RaceInfoModel()
        {
            DRS = false;
            Session = "P 0";
            SessionEndsAt = Time.DateTimeToDouble(new DateTime().AddHours(24));
        }
    }
}
