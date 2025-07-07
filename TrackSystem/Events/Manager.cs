using BrokeProtocol.API;
using BrokeProtocol.Collections;
using BrokeProtocol.Entities;
using BrokeProtocol.Managers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackSystem.Types;
using UnityEngine;

namespace TrackSystem.Events
{
    internal class Manager : ManagerEvents
    {

        public Dictionary<string, Vec3> lastPoses = new Dictionary<String, Vec3>();

        [Execution(ExecutionMode.Additive)]
        public override bool FixedUpdate()
        {
            Dictionary<string, Vec3> updatePayload = new Dictionary<string, Vec3>();
            foreach (ShPlayer player in EntityCollections.Humans)
            {
                int speed = (int)(player.Velocity.magnitude * 2.3f);
                player.svPlayer.SetTextElementText("SpeedUIText", speed.ToString() + " km/h");

                Vec3 lastPos;
                if (!lastPoses.TryGetValue(player.username, out lastPos))
                {
                    lastPoses[player.username] = new Vec3(player.Position);
                    continue;
                }

                Vec3 newPos = new Vec3(player.Position);
                if (lastPos != newPos)
                {
                    float distance = (float)Math.Sqrt(
                        Math.Pow(newPos.x - lastPos.x, 2) +
                        Math.Pow(newPos.y - lastPos.y, 2) +
                        Math.Pow(newPos.z - lastPos.z, 2)
                    );

                    if (distance >= 5)
                    {
                        lastPoses[player.username] = newPos;
                        updatePayload[player.username] = newPos;
                    }
                }
            }

            if (updatePayload.Count > 0)
            {
                WebSocketMessage<Dictionary<string, Vec3>> message = new WebSocketMessage<Dictionary<string, Vec3>>()
                {
                    title = "posUpdate",
                    body = updatePayload
                };
                Core.send_message(JsonConvert.SerializeObject(message));
            }

            return true;
        }
    }
}
