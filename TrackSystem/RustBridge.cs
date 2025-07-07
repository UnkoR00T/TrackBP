using BrokeProtocol.Collections;
using BrokeProtocol.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TrackSystem.DBControllers;
using TrackSystem.Types;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TrackSystem
{
    public class RustBridge
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr GetPlayersDelegate();
        [DllImport("TrackServer.so", CallingConvention = CallingConvention.Cdecl)]
        public static extern void register_get_players(GetPlayersDelegate del);


        public static IntPtr GetPlayersImpl()
        {
            IDCollection<ShPlayer> playersCol = EntityCollections.Humans;
            List<RustPlayer> players = new List<RustPlayer>();
            foreach (var p in playersCol)
            {
                players.Add(new RustPlayer(p));
            }
            string json = JsonConvert.SerializeObject(players);
            byte[] bytes = Encoding.UTF8.GetBytes(json + "\0");
            IntPtr unmanaged = Marshal.AllocHGlobal(bytes.Length);
            Marshal.Copy(bytes, 0, unmanaged, bytes.Length);
            return unmanaged;
        }

        public static void Init()
        {
            register_get_players(GetPlayersImpl);
        }
    }
}
