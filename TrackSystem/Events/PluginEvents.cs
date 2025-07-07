using BrokeProtocol.API;
using BrokeProtocol.Entities;
using BrokeProtocol.Properties;
using BrokeProtocol.Utility;
using RSG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackSystem.DBControllers;
using TrackSystem.Types;
using UnityEngine;

namespace TrackSystem.Events
{

    internal class PluginEvents : IScript
    {
        [CustomTarget]
        public void EnterSector(Serialized trigger, ShPhysical physical)
        {
            if (physical is ShPlayer player)
            {
                DateTime now = DateTime.Now;
                double nowTime = Time.DateTimeToDouble(now);
                string data = trigger.data;
                if (String.IsNullOrWhiteSpace(data))
                {
                    return;
                }
                int sector = Int32.Parse(data);
                player.svPlayer.SendGameMessage($"===== SECTOR {sector} =====");
                player.svPlayer.SendGameMessage($"===== ENTERD: {nowTime} =====");
                int lastsector = sector == 1 ? 3 : sector - 1;
                if (player.svPlayer.CustomData.TryGetValue<double>($"enteredSector{lastsector}", out double time))
                {
                    double realTime = nowTime - time;
                    player.svPlayer.SendGameMessage($"===== LAST: {time} =====");
                    player.svPlayer.SendGameMessage($"===== DIFF: {realTime} =====");
                    PersonalBestModel best = PersonalBestController.GetPersonalBests(player);
                    if(sector == 1)
                    {
                        var sectorDiff = realTime - best.Sectors.One;
                        showDiff(player, sectorDiff);
                    }
                    if (sector == 2)
                    {
                        var sectorDiff = realTime - best.Sectors.Two;
                        showDiff(player, sectorDiff);
                    }
                    if (sector == 3)
                    {
                        var sectorDiff = realTime - best.Sectors.Three;
                        showDiff(player, sectorDiff);
                    }
                    PersonalBestController.UpdateSectorTime(player, realTime, sector);
                    SectorController.SetSectorTime(player, sector, realTime);
                }

                if (sector == 1)
                {
                    Lap(player);
                }
                player.svPlayer.VisualElementDisplay("TimeDiff", true);

                player.svPlayer.CustomData.Add<double>($"enteredSector{sector}", nowTime);
                player.svPlayer.CustomData.Add<double>($"lastSector", sector);
                player.svPlayer.SendGameMessage($"===== SAVED =====");

            }
        }
        public void showDiff(ShPlayer player, double sectorDiff)
        {
            if (sectorDiff >= -100)
            {
                string sign = sectorDiff > 0 ? "+" : (sectorDiff < 0 ? "-" : "");
                string text = $"{sign}{Math.Abs(sectorDiff):F3}";
                player.svPlayer.SetTextElementText("TimeDiff", text + "s");
                player.svPlayer.StartCoroutine(hideDiff(player));
            }
            else
            {
                return;
            }
        }

        public void Lap(ShPlayer player)
        {
            int lastSector;
            if (!player.svPlayer.CustomData.TryGetValue<int>("lastSector", out lastSector))
            {
                return;
            }
            if (lastSector != 3)
            {
                player.svPlayer.SendGameMessage("&c[RACE] &fCheating isnt ok :/");
                return;
            }
            if (player.svPlayer.CustomData.TryGetValue<double>($"enteredSector1", out double lapTime))
            {
                DateTime now = DateTime.Now;
                double nowTime = Time.DateTimeToDouble(now);
                double realTime = nowTime - lapTime;
                player.svPlayer.SendGameMessage($"===== LAP-TIME: {realTime} =====");
                PersonalBestModel best = PersonalBestController.GetPersonalBests(player);
                var sectorDiff = realTime - best.LapTime;


                Console.WriteLine($"[DEBUG] realTime: {realTime}");
                Console.WriteLine($"[DEBUG] best lapTime: {best.LapTime}");
                Console.WriteLine($"[DEBUG] realTime - best.LapTime = {realTime - best.LapTime}");

                if (sectorDiff >= -100)
                {
                    string sign = sectorDiff > 0 ? "+" : (sectorDiff < 0 ? "-" : "");
                    string text = $"{sign}{Math.Abs(sectorDiff):F3}";
                    player.svPlayer.SetTextElementText("TimeDiff", text + "s");
                    player.svPlayer.StartCoroutine(hideDiff(player));

                }
                SectorController.SetLapTime(player, realTime);
                PersonalBestController.UpdateLapTime(player, realTime);
            }
        }

        public IEnumerator hideDiff(ShPlayer p)
        {
            yield return new WaitForSeconds(3f);
            p.svPlayer.VisualElementDisplay("TimeDiff", false);
        }

        [CustomTarget]
        public void SpeedTrap(Serialized trigger, ShPhysical physical)
        {
            if (physical is ShPlayer player)
            {

                TimeTrapController.AddPlayerTime(player, player.Velocity.magnitude * 2.3f);
                PersonalBestController.UpdateTimeTrap(player, player.Velocity.magnitude * 2.3f);

            }
        }

        [CustomTarget]
        public void DRSZoneEnter(Serialized trigger, ShPhysical physical)
        {
            if(physical is ShPlayer player && RaceInfoController.Get().DRS)
            {
                player.svPlayer.VisualElementDisplay("DRSText", true);
            }
        }
        [CustomTarget]
        public void DRSZoneExit(Serialized trigger, ShPhysical physical)
        {
            if (physical is ShPlayer player)
            {
                player.svPlayer.VisualElementDisplay("DRSText", false);
            }
        }
    }
}
