using BrokeProtocol.API;
using BrokeProtocol.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackSystem.DBControllers;
using UnityEngine;

namespace TrackSystem.Types
{
    internal class RustPlayer
    {
        public string PlayerName { get; set; }
        public int PlayerId { get; set; }
        public Vec3 Pos { get; set; }
        public CustomData CustomData { get; set; }
        public PersonalBestModel PersonalBests { get; set; }

        public RustPlayer(ShPlayer p)
        {
            PlayerName = p.username;
            PlayerId = p.ID;
            Pos = new Vec3(p.Position);
            CustomData = p.svPlayer.CustomData;
            PersonalBests = PersonalBestController.GetPersonalBests(p);
        }
    }
}
