using BrokeProtocol.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackSystem.Functions
{
    public class SetPlayerNumber
    {
        public static void Exec(ShPlayer sender, ShPlayer target, int number)
        {
            if (sender.svPlayer.HasPermission("admin"))
            {
                target.svPlayer.CustomData.Add("driverNumber", number);
                SendPlayerUpdate.Exec(target);
                sender.svPlayer.SendGameMessage($"&c[RACE] &fPlayer's number has been set to {number}");
                target.svPlayer.SendGameMessage($"&c[RACE] &fYours number has been set to {number} by {sender.username}");
            }
            else
            {
                sender.svPlayer.SendGameMessage($"&c[RACE] &fNo access");
            }
        }
    }
}
