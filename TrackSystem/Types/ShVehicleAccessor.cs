using BrokeProtocol.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TrackSystem.Types
{
    public static class ShVehicleAccessor
    {
        private static readonly FieldInfo engineFactorField = typeof(ShVehicle).GetField("engineFactor", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly FieldInfo engineStrengthField = typeof(ShVehicle).GetField("engineStrength", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly FieldInfo brakeFactorField = typeof(ShVehicle).GetField("brakeFactor", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly FieldInfo brakeStrengthField = typeof(ShVehicle).GetField("brakeStrength", BindingFlags.NonPublic | BindingFlags.Instance);

        public static float GetEngineFactor(ShVehicle vehicle) => (float)engineFactorField.GetValue(vehicle);
        public static void SetEngineFactor(ShVehicle vehicle, float value) => engineFactorField.SetValue(vehicle, value);

        public static float GetEngineStrength(ShVehicle vehicle) => (float)engineStrengthField.GetValue(vehicle);
        public static void AddEngineStrength(ShVehicle vehicle, float value) => engineStrengthField.SetValue(vehicle, GetEngineStrength(vehicle) + value);
        public static void RemoveEngineStrength(ShVehicle vehicle, float value) => engineStrengthField.SetValue(vehicle, GetEngineStrength(vehicle) - value);

        public static float GetBrakeFactor(ShVehicle vehicle) => (float)brakeFactorField.GetValue(vehicle);
        public static void SetBrakeFactor(ShVehicle vehicle, float value) => brakeFactorField.SetValue(vehicle, value);

        public static float GetBrakeStrength(ShVehicle vehicle) => (float)brakeStrengthField.GetValue(vehicle);
        public static void SetBrakeStrength(ShVehicle vehicle, float value) => brakeStrengthField.SetValue(vehicle, value);
    }
}
