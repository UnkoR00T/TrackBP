using BrokeProtocol.Entities;
using BrokeProtocol.Required;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackSystem.DBControllers;
using TrackSystem.Types;

namespace TrackSystem.Functions
{
    public static class SendRaceControllMenu
    {
        public static void Exec(ShPlayer p)
        {
            if (p.svPlayer.HasPermission("admin"))
            {
                RaceInfoModel raceInfo = RaceInfoController.Get();
                List<LabelID> options = new List<LabelID>
                {
                    new LabelID("Manage Drivers", "md"),
                    new LabelID("DRS Status: " + raceInfo.DRS, "cdrs"),
                    new LabelID("Session: " + raceInfo.Session, "css" ),
                    new LabelID("Session ends at: " + raceInfo.SessionEndsAt, "csea"),
                    new LabelID("Start race", "sr")
                };
                List<LabelID> actions = new List<LabelID>
                {
                    new LabelID("Use", "u")
                };
                p.svPlayer.SendOptionMenu("Race Controll", p.ID, "racec", options.ToArray(), actions.ToArray());
            }
        }
    }
}
