using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackSystem.Functions;
using TrackSystem.Types;

namespace TrackSystem.DBControllers
{
    public class RaceInfoController
    {
        public static ILiteCollection<RaceInfoModel> raceInfoCollection;
        public static void Init()
        {
            raceInfoCollection = DB.Conn.GetCollection<RaceInfoModel>("info");
            if(raceInfoCollection.Count() == 0)
            {
                RaceInfoModel raceInfo = new RaceInfoModel();
                raceInfoCollection.Insert(raceInfo);
            } 
        }
        public static RaceInfoModel Get()
        {   
            return raceInfoCollection.FindOne(x => true);
        }
        public static void Update(RaceInfoModel newRaceInfo)
        {
            raceInfoCollection.Update(newRaceInfo);
            WSUtils.SendRaceInfoUpdate(newRaceInfo);
        }
    }
}
