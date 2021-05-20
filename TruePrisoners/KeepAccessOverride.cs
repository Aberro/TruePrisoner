using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;

namespace TruePrisoners
{
    //[HarmonyPatch(typeof(PlayerTownVisitCampaignBehavior), "game_menu_town_go_to_keep_on_condition")]
    class TownKeepEnterCheckOverride
    {
        public static bool Prefix(MenuCallbackArgs args, ref bool __result)
        {
            SettlementAccessModel.AccessDetails accessDetails;
            Campaign.Current.Models.SettlementAccessModel.CanMainHeroEnterKeep(Settlement.CurrentSettlement, out accessDetails);
            /*if (accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.NoAccess)
            {
                args.IsEnabled = false;
                switch (accessDetails.AccessLimitationReason)
                {
                    case SettlementAccessModel.AccessLimitationReason.HostileFaction:
                        args.Tooltip = new TextObject("{=b3shPt8Q}You cannot enter an enemy keep.", (Dictionary<string, TextObject>)null);
                        break;
                    case SettlementAccessModel.AccessLimitationReason.Disguised:
                        args.Tooltip = new TextObject("{=f91LSbdx}You cannot enter the keep while in disguise.", (Dictionary<string, TextObject>)null);
                        break;
                    case SettlementAccessModel.AccessLimitationReason.LocationEmpty:
                        args.Tooltip = new TextObject("{=cojKmfSk}There is no one inside.", (Dictionary<string, TextObject>)null);
                        break;
                }
            }*/
            List<Location> list = Settlement.CurrentSettlement.LocationComplex.FindAll((Func<string, bool>)(x => x == "lordshall" || x == "prison")).ToList<Location>();
            args.OptionIssueType = Campaign.Current.IssueManager.CheckIssueForMenuLocations(list);
            args.OptionQuestStatus = Campaign.Current.QuestManager.CheckQuestForMenuLocations(list);
            args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
            __result = true;
            return false;
        }
    }

    //[HarmonyPatch(typeof(PlayerTownVisitCampaignBehavior), "game_menu_town_go_to_keep_on_consequence")]
    class TownKeepEnterConsequenceOverride
    {
        public static bool Prefix(MenuCallbackArgs args)
        {
            SettlementAccessModel.AccessDetails accessDetails;
            Campaign.Current.Models.SettlementAccessModel.CanMainHeroEnterKeep(Settlement.CurrentSettlement, out accessDetails);
            switch (accessDetails.AccessLevel)
            {
                case SettlementAccessModel.AccessLevel.LimitedAccess:
                    if (accessDetails.LimitedAccessSolution != SettlementAccessModel.LimitedAccessSolution.Bribe)
                        break;
                    GameMenu.SwitchToMenu("town_keep_bribe");
                    break;
                case SettlementAccessModel.AccessLevel.NoAccess:
                    GameMenu.SwitchToMenu("town_keep_bribe");
                    break;
                case SettlementAccessModel.AccessLevel.FullAccess:
                    GameMenu.SwitchToMenu("town_keep");
                    break;
            }
            return false;

            //if (FactionManager.IsAtWarAgainstFaction(Settlement.CurrentSettlement.MapFaction, Hero.MainHero.MapFaction))
        }
    }
}
