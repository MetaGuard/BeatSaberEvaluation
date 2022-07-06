using HarmonyLib;
using SaberTailor.Settings;
using System.Reflection;

namespace SaberTailor.HarmonyPatches
{
    /// <summary>
    /// Apply and remove all of our Harmony patches through this class
    /// </summary>
    public class SaberTailorPatches
    {
        private static Harmony instance;

        public static bool IsPatched { get; private set; }
        public const string InstanceId = "com.shadnix.beatsaber.sabertailor";

        internal static void ApplyHarmonyPatches()
        {
            if (instance == null)
            {
                instance = new Harmony(InstanceId);
            }

            if (!IsPatched)
            {
                instance.PatchAll(Assembly.GetExecutingAssembly());
                IsPatched = true;
            }
        }

        internal static void RemoveHarmonyPatches()
        {
            if (instance != null && IsPatched)
            {
                instance.UnpatchSelf();
                IsPatched = false;
            }
        }

        internal static void CheckHarmonyPatchStatus()
        {
            if (Configuration.Grip.IsGripModEnabled || (Configuration.Trail.TweakEnabled && Configuration.Trail.TrailEnabled))
            {
                ApplyHarmonyPatches();
            }
            else
            {
                RemoveHarmonyPatches();
            }
        }
    }
}
