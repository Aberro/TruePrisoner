using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;

namespace TruePrisoners
{
    [HarmonyPatch(typeof(DefaultRansomValueCalculationModel), "PrisonerRansomValue")]
    class RansomValueOverride
    {
        public static bool Prefix(CharacterObject prisoner, Hero sellerHero, ref int __result)
        {
            if (Support.settings.significant_ransoms)
            {
                __result = Support.CalculateRansom(prisoner);
                return false;
            }
            else
                return true;
        }
    }
}
