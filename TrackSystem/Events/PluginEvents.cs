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
                if (player.svPlayer.CustomData.TryGetValue<int>("lastSector", out int lastSector))
                {
                    if (player.svPlayer.CustomData.TryGetValue<double>($"enteredSector{lastSector}", out double time))
                    {
                        double realTime = nowTime - time;
                        PersonalBestModel best = PersonalBestController.GetPersonalBests(player);
                        if (sector == 1 && lastSector == 3)
                        {
                            var sectorDiff = realTime - best.Sectors.One;
                            showDiff(player, sectorDiff);
                        }
                        if (sector == 2 && lastSector == 1)
                        {
                            var sectorDiff = realTime - best.Sectors.Two;
                            showDiff(player, sectorDiff);
                        }
                        if (sector == 3 && lastSector == 2)
                        {
                            var sectorDiff = realTime - best.Sectors.Three;
                            showDiff(player, sectorDiff);
                        }
                        PersonalBestController.UpdateSectorTime(player, realTime, sector);
                        SectorController.SetSectorTime(player, sector, realTime);
                    }

                    if (sector == 1 && lastSector == 3)
                    {
                        Lap(player);
                    }
                }
                player.svPlayer.VisualElementDisplay("TimeDiff", true);

                player.svPlayer.CustomData.Add<double>($"enteredSector{sector}", nowTime);
                player.svPlayer.CustomData.Add<int>($"lastSector", sector);

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
            if (player.svPlayer.CustomData.TryGetValue<double>($"enteredSector1", out double lapTime))
            {
                DateTime now = DateTime.Now;
                double nowTime = Time.DateTimeToDouble(now);
                double realTime = nowTime - lapTime;
                PersonalBestModel best = PersonalBestController.GetPersonalBests(player);
                var sectorDiff = realTime - best.LapTime;

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
            PersonalBestController.AddLap(player);
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
