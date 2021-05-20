using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors.BarterBehaviors;

namespace TruePrisoners
{
    [HarmonyPatch(typeof(DiplomaticBartersBehavior), "ConsiderRansomPrisoner")]
    class PrisonerFreedomOverride
    {
        public static bool Prefix(Hero hero)
        {
            bool result = true;

            if (!Support.settings.allow_ransoms)
                result = false;
            else
            {
                if (hero != null)
                {
                    if (hero.IsPrisoner && hero.PartyBelongedToAsPrisoner != null && hero != Hero.MainHero)
                    {
                        if (hero.PartyBelongedToAsPrisoner == PartyBase.MainParty || (hero.PartyBelongedToAsPrisoner.IsSettlement && hero.PartyBelongedToAsPrisoner.Settlement.OwnerClan == Clan.PlayerClan))
                            result = false;
                        else
                        {
                            if (hero.CaptivityStartTime.ElapsedDaysUntilNow < Support.settings.minimum_days_before_escape_ransom)
                                result = false;
                        }
                    }
                }
            }
            
            return result;
        }
    }
}
