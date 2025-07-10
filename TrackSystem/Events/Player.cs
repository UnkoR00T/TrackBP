using BrokeProtocol.API;
using BrokeProtocol.Collections;
using BrokeProtocol.Entities;
using BrokeProtocol.Managers;
using BrokeProtocol.Required;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackSystem.DBControllers;
using TrackSystem.Functions;
using TrackSystem.Types;
using UnityEngine;

namespace TrackSystem.Events
{
    public class Player : PlayerEvents
    {
        [Execution(ExecutionMode.Additive)]
        public override bool Mount(ShPlayer player, ShMountable mount, byte seat)
        {

            if (player == null) return false;
            player.svPlayer.VisualTreeAssetClone("SpeedMeter");
            player.svPlayer.VisualElementDisplay("DRSText", false);
            player.svPlayer.VisualElementDisplay("TimeDiff", false);

            return true;
        }

        [Execution(ExecutionMode.Additive)]
        public override bool Dismount(ShPlayer player)
        {
            player.svPlayer.VisualElementRemove("SpeedMeter");
            return true;
        }

        [Execution(ExecutionMode.Additive)]
        public override bool Ready(ShPlayer p)
        {
            var data = new RustPlayer(p);
            var payload = new WebSocketMessage<RustPlayer>()
            {
                title = "playerJoin",
                body = data,
            };
            var json = JsonConvert.SerializeObject(payload);
            Core.send_message(json);
            return true;
        }

        [Execution(ExecutionMode.Additive)]
        public override bool Destroy(ShEntity entity)
        {
            if(entity is ShPlayer p)
            {
                var payload = new WebSocketMessage<string>()
                {
                    title = "playerLeave",
                    body = p.username,
                };
                var json = JsonConvert.SerializeObject(payload);
                Core.send_message(json);
            }
            return true;
        }

        [Execution(ExecutionMode.Additive)]
        public override bool OptionAction(ShPlayer player, int targetID, string id, string optionID, string actionID)
        {
            if(id == "racec")
            {
                switch (optionID)
                {
                    case "md":
                        player.svPlayer.DestroyMenu(id);
                        List<LabelID> options = new List<LabelID>();
                        foreach(ShPlayer p in EntityCollections.Humans)
                        {
                            LabelID labelID = new LabelID(p.username, p.username);
                            if(p.svPlayer.CustomData.TryGetValue<int>("driverNumber", out int number))
                            {
                                labelID.label += $" ({number})";
                            }
                            if(p.svPlayer.CustomData.TryGetValue<string>("driverColor", out string color))
                            {
                                labelID.label += $" #{color}";
                            }
                            options.Add(labelID);
                        }
                        options.Add(new LabelID("Back", "b"));
                        List<LabelID> actions = new List<LabelID>
                        { 
                            new LabelID("Use", "u")
                        };
                        player.svPlayer.SendOptionMenu("Drivers", player.ID, "md", options.ToArray(), actions.ToArray());
                        break;
                    case "cdrs":
                        player.svPlayer.DestroyMenu(id);
                        RaceInfoModel raceInfo = RaceInfoController.Get();
                        raceInfo.DRS = !raceInfo.DRS;
                        RaceInfoController.Update(raceInfo);
                        SendRaceControllMenu.Exec(player);
                        break;
                    case "css":
                        player.svPlayer.DestroyMenu(id);
                        player.svPlayer.SendInputMenu("Set Session:", player.ID, "css", 16);
                        break;
                    case "csea":
                        player.svPlayer.DestroyMenu(id);
                        player.svPlayer.SendInputMenu("Set Session duration (in mins):", player.ID, "csea", 16);
                        break;
                    case "sr":
                        player.svPlayer.DestroyMenu(id);
                        SvManager.Instance.StartCoroutine(startRace());
                        break;
                    case "cpb":
                        PersonalBestController.Clear();
                        player.svPlayer.DestroyMenu(id);
                        player.svPlayer.SendGameMessage("&c[RACE] &fCleared");
                        break;

                }
            }
            if(id=="md" && optionID == "b")
            {
                player.svPlayer.DestroyMenu(id);
                SendRaceControllMenu.Exec(player);
            }
            return true;
        }
        [Execution(ExecutionMode.Additive)]
        public override bool SubmitInput(ShPlayer player, int targetID, string id, string input)
        {
            if(id == "css")
            {
                player.svPlayer.DestroyMenu(id);
                RaceInfoModel raceInfo = RaceInfoController.Get();
                raceInfo.Session = input;
                RaceInfoController.Update(raceInfo);
                SendRaceControllMenu.Exec(player);
            }else if(id == "csea")
            {
                player.svPlayer.DestroyMenu(id);
                try
                {
                    int time = Int32.Parse(input);
                    Debug.Log(time);
                    DateTime now = DateTime.Now.AddMinutes(time);
                    Debug.Log(now);
                    double ends = Time.DateTimeToDouble(now);
                    Debug.Log(ends);
                    RaceInfoModel raceInfo = RaceInfoController.Get();
                    raceInfo.SessionEndsAt = ends;
                    RaceInfoController.Update(raceInfo);
                }
                catch (Exception e)
                {}
                SendRaceControllMenu.Exec(player);
            }
            return true;
        }
        public IEnumerator startRace()
        {
            InterfaceHandler.SendGameMessageToAll("&c[RACE] RACE IS STARTING");
            float delay = 2f;
            yield return new WaitForSeconds(delay);
            InterfaceHandler.SendGameMessageToAll("&c▮▮▮▮▮▮▮▮▮▮");
            yield return new WaitForSeconds(delay);
            InterfaceHandler.SendGameMessageToAll("&c▮▮▮▮▮▮▮▮▮▮");
            yield return new WaitForSeconds(delay);
            InterfaceHandler.SendGameMessageToAll("&c▮▮▮▮▮▮▮▮▮▮");
            yield return new WaitForSeconds(delay + 1f);
            InterfaceHandler.SendGameMessageToAll("&e▮▮▮▮▮▮▮▮▮▮");
            yield return new WaitForSeconds(delay + 1f);
            InterfaceHandler.SendGameMessageToAll("&2▮▮▮▮▮▮▮▮▮▮");
            InterfaceHandler.SendGameMessageToAll("&2▮▮▮▮▮▮▮▮▮▮");
            InterfaceHandler.SendGameMessageToAll("&2▮▮▮▮▮▮▮▮▮▮");
        }
    }
}
