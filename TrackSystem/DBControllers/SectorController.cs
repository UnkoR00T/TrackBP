using BrokeProtocol.Entities;
using BrokeProtocol.Utility;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackSystem.Types;

namespace TrackSystem.DBControllers
{
    public static class SectorController
    {
        public static ILiteCollection<SectorModel> sectorColletion;
        public static void Init()
        {
            sectorColletion = DB.Conn.GetCollection<SectorModel>("sectortime");
        }
        public static void SetSectorTime(ShPlayer player, int sector, double time)
        {
            if (player == null)
            {
                return;
            }
            SectorModel save = new SectorModel()
            {
                PlayerName = player.username,
                Sector = sector,
                Time = time
            }
            ;
            sectorColletion.Insert(save);
        }
        public static void SetLapTime(ShPlayer player, double time)
        {
            if (player == null)
            {
                return;
            }
            SectorModel save = new SectorModel()
            {
                PlayerName = player.username,
                Sector = 0,
                Time = time
            }
            ;
            sectorColletion.Insert(save);
        }
    }
}
