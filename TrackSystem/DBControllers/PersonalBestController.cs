using BrokeProtocol.Entities;
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
    public static class PersonalBestController
    {
        public static ILiteCollection<PersonalBestModel> personalBestCollection;
        public static void Init()
        {
            personalBestCollection = DB.Conn.GetCollection<PersonalBestModel>("personalBests");
        }

        public static void UpdateLapTime(ShPlayer player, double time)
        {
            PersonalBestModel best = new PersonalBestModel();
            var _best = personalBestCollection.FindOne(x => x.PlayerName == player.username);
            if(_best != null)
                best = _best;
            else
            {
                best.PlayerName = player.username;
                personalBestCollection.Insert(best);
                SendPlayerUpdate.Exec(player);
            }
            if (best.LapTime > time)
            {
                best.LapTime = time;
                personalBestCollection.Update(best);
                SendPlayerUpdate.Exec(player);
            }
        }
        public static void UpdateSectorTime(ShPlayer player, double time, int sector)
        {
            PersonalBestModel best = new PersonalBestModel();
            var _best = personalBestCollection.FindOne(x => x.PlayerName == player.username);
            if (_best != null)
                best = _best;
            else
            {
                best.PlayerName = player.username;
                personalBestCollection.Insert(best);
            }
            if (sector == 1)
            {
                if(best.Sectors.One > time) best.Sectors.One = time;
            }
            if (sector == 2)
            {
                if (best.Sectors.Two > time) best.Sectors.Two = time;
            }
            if (sector == 3)
            {
                if (best.Sectors.Three > time) best.Sectors.Three = time;
            }
            personalBestCollection.Update(best);
            SendPlayerUpdate.Exec(player);
        }
        public static void UpdateTimeTrap(ShPlayer player, float time)
        {
            PersonalBestModel best = new PersonalBestModel();
            var _best = personalBestCollection.FindOne(x => x.PlayerName == player.username);
            if (_best != null)
                best = _best;
            else
            {
                best.PlayerName = player.username;
                personalBestCollection.Insert(best);
                SendPlayerUpdate.Exec(player);

            }
            if (best.TimeTrap < time)
            {
                best.TimeTrap = time;
                personalBestCollection.Update(best);
                SendPlayerUpdate.Exec(player);
            }
        }

        public static PersonalBestModel GetPersonalBests(ShPlayer player)
        {
            PersonalBestModel best = new PersonalBestModel();
            var _best = personalBestCollection.FindOne(x => x.PlayerName == player.username);
            if (_best != null)
                best = _best;
            return best;
        }
    }
}
