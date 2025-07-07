using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackSystem.DBControllers;

namespace TrackSystem
{
    public static class DB
    {
        public static LiteDatabase Conn;
        private static readonly string DbFilePath = Path.Combine(Directory.GetCurrentDirectory(), "trackDB.db");

        public static void Init()
        {
            try
            {
                Conn = new LiteDatabase(DbFilePath);
                Console.WriteLine("[TS] LiteDB initialized at: " + DbFilePath);
                TimeTrapController.Init();
                SectorController.Init();
                PersonalBestController.Init();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[TS] Failed to initialize LiteDB: " + ex.Message);
                throw;
            }
        }
    }
}
