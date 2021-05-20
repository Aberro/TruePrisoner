using HarmonyLib;
using TaleWorlds.CampaignSystem;

namespace TruePrisoners
{
    [HarmonyPatch(typeof(Campaign), "HourlyTick")]
    class PrisonerEscapeTick
    {
        public static void Postfix()
        {
            if (Support.escapeCounter > 0)
                Support.escapeCounter--;
        }
    }
}
