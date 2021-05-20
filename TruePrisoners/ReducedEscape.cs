using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using System;
using Helpers;

namespace TruePrisoners
{
    [HarmonyPatch(typeof(PrisonerReleaseCampaignBehavior), "DailyHeroTick")]
    class ReducedEscape
    {
        public static bool Prefix(Hero hero)
        {
            bool result = true;
            bool proceed = false;
            bool playerProceed = false;
            string text = " has escaped";

            try
            {
                bool isPlayer = hero.IsPrisoner && hero.PartyBelongedToAsPrisoner != null &&
                                (hero.PartyBelongedToAsPrisoner.IsMobile &&
                                 hero.PartyBelongedToAsPrisoner == PartyBase.MainParty
                                 || hero.PartyBelongedToAsPrisoner.IsSettlement &&
                                 hero.PartyBelongedToAsPrisoner.Settlement.OwnerClan == Clan.PlayerClan);

                bool enable = isPlayer || Support.settings.ai_applied;
                float generalChance = enable ? (float) Support.settings.general_chance : 1;
                float travelChance = enable ? (float)Support.settings.traveling_chance : 1;
                float settlementChance = enable ? (float)Support.settings.settlement_chance : 1;

                // ***************************************
                // Original method from decompiled source (except for commented lines)
                // ***************************************
                if (!hero.IsPrisoner || hero.PartyBelongedToAsPrisoner == null || hero == Hero.MainHero)
                    return false;
                float baseNumber = 0.05f /**/ * generalChance /**/ ; // general chance modificaiton
                if (hero.PartyBelongedToAsPrisoner.IsMobile && hero.PartyBelongedToAsPrisoner.MobileParty.CurrentSettlement == null)
                {
                    // travel chance modification
                    baseNumber *= (5f - (float)Math.Pow(Math.Min(81, hero.PartyBelongedToAsPrisoner.NumberOfHealthyMembers), 0.25)) /**/ * travelChance /**/; 
                    /**/ text = " slipped away from " + (hero.IsFemale ? "her" : "his") + " guards and escaped"; /**/
                }
                // settlement chance modification begin
                else if (hero.PartyBelongedToAsPrisoner.IsSettlement)
                {
                    baseNumber *= settlementChance;
                    text = " is missing from " + (hero.IsFemale ? "her" : "his") + " cell";
                }
                // end
                if (hero.PartyBelongedToAsPrisoner == PartyBase.MainParty || hero.PartyBelongedToAsPrisoner.IsSettlement && hero.PartyBelongedToAsPrisoner.Settlement.OwnerClan == Clan.PlayerClan || hero.PartyBelongedToAsPrisoner.IsMobile && hero.PartyBelongedToAsPrisoner.MobileParty.CurrentSettlement != null && hero.PartyBelongedToAsPrisoner.MobileParty.CurrentSettlement.OwnerClan == Clan.PlayerClan)
                    baseNumber *= 0.5f;
                ExplainedNumber explainedNumber = new ExplainedNumber(baseNumber);
                if (hero.PartyBelongedToAsPrisoner.IsSettlement)
                {
                    if (hero.PartyBelongedToAsPrisoner.Settlement.IsTown)
                    {
                        PerkHelper.AddPerkBonusForTown(DefaultPerks.Riding.MountedPatrols, hero.PartyBelongedToAsPrisoner.Settlement.Town, ref explainedNumber);
                        PerkHelper.AddPerkBonusForTown(DefaultPerks.Roguery.SweetTalker, hero.PartyBelongedToAsPrisoner.Settlement.Town, ref explainedNumber);
                    }
                    if (hero.PartyBelongedToAsPrisoner.Settlement.IsTown || hero.PartyBelongedToAsPrisoner.Settlement.IsCastle)
                        PerkHelper.AddPerkBonusForTown(DefaultPerks.Engineering.PrisonArchitect, hero.PartyBelongedToAsPrisoner.Settlement.Town, ref explainedNumber);
                }
                if (hero.PartyBelongedToAsPrisoner.IsMobile)
                {
                    if (hero.Clan != null && hero.Clan.Leader != null && hero.Clan.Leader.GetPerkValue(DefaultPerks.Roguery.FleetFooted))
                        explainedNumber.AddFactor(DefaultPerks.Roguery.FleetFooted.SecondaryBonus);
                    if (hero.PartyBelongedToAsPrisoner.MobileParty.HasPerk(DefaultPerks.Riding.MountedPatrols))
                        PerkHelper.AddPerkBonusForParty(DefaultPerks.Riding.MountedPatrols, hero.PartyBelongedToAsPrisoner.MobileParty, true, ref explainedNumber);
                    if (hero.PartyBelongedToAsPrisoner.MobileParty.HasPerk(DefaultPerks.Roguery.RansomBroker))
                        PerkHelper.AddPerkBonusForParty(DefaultPerks.Roguery.RansomBroker, hero.PartyBelongedToAsPrisoner.MobileParty, false, ref explainedNumber);
                }
                if (hero.PartyBelongedToAsPrisoner.IsMobile && hero.PartyBelongedToAsPrisoner.MobileParty.HasPerk(DefaultPerks.Scouting.KeenSight, true))
                    PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.KeenSight, hero.PartyBelongedToAsPrisoner.MobileParty, false, ref explainedNumber);
                if (MBRandom.RandomFloat >= explainedNumber.ResultNumber)
                {
                    // Relation modification begin
                    if (isPlayer && Support.settings.daily_penalty != 0)
                    {
                        Support.ChangeRelation(Hero.MainHero, hero, -Support.settings.daily_penalty);
                        Support.ChangeFamilyRelation(Hero.MainHero, hero, 0, -Support.settings.daily_penalty);
                    }
                    // end
                    return false;
                }
                EndCaptivityAction.ApplyByEscape(hero);
                // Notification modification begin
                if(isPlayer)
                    Support.LogMessage(hero.Name.ToString() + " " + text);
                // end
                return false;
            }
            catch (Exception exe) { }

            return result;
        }
    }
}
