using BrokeProtocol.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackSystem.DBControllers;
using TrackSystem.Types;

namespace TrackSystem.Functions
{
    public class SendPlayerUpdate
    {
        public static void Exec(ShPlayer p)
        {
            var data = new RustPlayer(p);
            var payload = new WebSocketMessage<RustPlayer>()
            {
                title = "playerUpdate",
                body = data,
            };
            var json = JsonConvert.SerializeObject(payload);
            Core.send_message(json);
            return;
        }
    }
}
