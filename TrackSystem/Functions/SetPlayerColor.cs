using BrokeProtocol.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackSystem.Functions
{
    public class SetPlayerColor
    {
        public static void Exec(ShPlayer sender, ShPlayer target, string color)
        {
            if (sender.svPlayer.HasPermission("admin"))
            {
                target.svPlayer.CustomData.Add("driverColor", color);
                WSUtils.SendPlayerUpdate(target);
                sender.svPlayer.SendGameMessage($"&c[RACE] &fPlayer's color has been set to #{color}");
                target.svPlayer.SendGameMessage($"&c[RACE] &fYours color has been set to #{color} by {sender.username}");
            }
            else
            {
                sender.svPlayer.SendGameMessage($"&c[RACE] &fNo access");
            }
        }
    }
}
