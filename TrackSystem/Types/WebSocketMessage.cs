using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackSystem.Types
{
    public class WebSocketMessage<TBody>
    {
        public string title { get; set; }
        public TBody body { get; set; }
    }
}
