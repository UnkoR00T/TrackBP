using BrokeProtocol.Entities;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackSystem.Types;

namespace TrackSystem.DBControllers
{
    public static class TimeTrapController
    {
        public static ILiteCollection<TimeTrapModel> timetrapCollection;
        public static void Init()
        {
            timetrapCollection = DB.Conn.GetCollection<TimeTrapModel>("timetrap");
        }

        public static void AddPlayerTime(ShPlayer player, float speed)
        {
            TimeTrapModel save = new TimeTrapModel()
            {
                PlayerName = player.username,
                Speed = speed
            };
            timetrapCollection.Insert(save);
        }
    }
}
