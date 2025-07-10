using BrokeProtocol.API;
using BrokeProtocol.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TrackSystem.DBControllers;
using TrackSystem.Functions;

namespace TrackSystem
{
    public class Core : Plugin
    {
        [DllImport("TrackServer.so", CallingConvention = CallingConvention.Cdecl)]
        public static extern void init_server();

        [DllImport("TrackServer.so", CallingConvention = CallingConvention.Cdecl)]
        public static extern void send_message([MarshalAs(UnmanagedType.LPStr)] string s);
        public Core() {
            Info = new PluginInfo("TrackSystem", "TS", "Track");
            DB.Init();
            init_server();
            RustBridge.Init();
            RaceInfoController.Init();
            CommandHandler.RegisterCommand("setnumber", new Action<ShPlayer, ShPlayer, int>(SetPlayerNumber.Exec), null, "admin");
            CommandHandler.RegisterCommand("setcolor", new Action<ShPlayer, ShPlayer, string>(SetPlayerColor.Exec), null, "admin");
            CommandHandler.RegisterCommand("race", new Action<ShPlayer>(SendRaceControllMenu.Exec), null, "admin");
        }
    }
}
