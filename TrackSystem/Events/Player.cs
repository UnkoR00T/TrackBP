using BrokeProtocol.API;
using BrokeProtocol.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackSystem.DBControllers;
using TrackSystem.Types;

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


    }
}
