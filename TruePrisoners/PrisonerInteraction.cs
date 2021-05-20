using HarmonyLib;
using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using System.Linq;

namespace TruePrisoners
{
    /************/
    /* Override */
    /************/

    [HarmonyPatch(typeof(PrisonerRecruitCampaignBehavior), "AddDialogs")]
    class PrisonerDialogOverride
    {
        public static bool Prefix(ref PrisonerRecruitCampaignBehavior __instance, CampaignGameStarter campaignGameStarter)
        {
            campaignGameStarter.AddDialogLine("conversation_prisoner_chat_start", "start", "prisoner_recruit_start_player", "{=k7ebznzr}{HLC_INTRO}", new ConversationSentence.OnConditionDelegate(PrisonerInteraction.IsPlayerPrisonerHero), (ConversationSentence.OnConsequenceDelegate)null, 120, (ConversationSentence.OnClickableConditionDelegate)null);
            campaignGameStarter.AddPlayerLine("conversation_prisoner_chat_player", "prisoner_recruit_start_player", "hlc_player_prisoner_ransom", "{=HLC11000}{HLC_CHOICE1}", new ConversationSentence.OnConditionDelegate(PrisonerInteraction.IsPlayerPrisonerHero), new ConversationSentence.OnConsequenceDelegate(PrisonerInteraction.HeroPayRansom));
            campaignGameStarter.AddPlayerLine("conversation_prisoner_chat_player", "prisoner_recruit_start_player", "hlc_player_prisoner_release", "{=HLC11010}{HLC_CHOICE2}", new ConversationSentence.OnConditionDelegate(PrisonerInteraction.IsPlayerPrisonerHero), new ConversationSentence.OnConsequenceDelegate(PrisonerInteraction.HeroUnconditionalRelease));
            campaignGameStarter.AddPlayerLine("conversation_prisoner_chat_player", "prisoner_recruit_start_player", "prisoner_recruit_start_response", "{=ksZXyDJG}Don't do anything stupid. I'm watching you.", (ConversationSentence.OnConditionDelegate)null, (ConversationSentence.OnConsequenceDelegate)null, 100, (ConversationSentence.OnClickableConditionDelegate)null, (ConversationSentence.OnPersuasionOptionDelegate)null);

            campaignGameStarter.AddDialogLine("hlc_conversation_prisoner_chat_player_ransom", "hlc_player_prisoner_ransom", "close_window", "{=HLC11001}{HLC_RESULT}", (ConversationSentence.OnConditionDelegate)null, (ConversationSentence.OnConsequenceDelegate)null);
            campaignGameStarter.AddDialogLine("hlc_conversation_prisoner_chat_player_release", "hlc_player_prisoner_release", "close_window", "{=HLC11011}{HLC_RESULT}", (ConversationSentence.OnConditionDelegate)null, (ConversationSentence.OnConsequenceDelegate)null);
            campaignGameStarter.AddDialogLine("hlc_conversation_prisoner_chat_response", "prisoner_recruit_start_response", "close_window", "{=Oe1bTJp6}{HLC_RESULT}", (ConversationSentence.OnConditionDelegate)null, (ConversationSentence.OnConsequenceDelegate)null, 100, (ConversationSentence.OnClickableConditionDelegate)null);

            campaignGameStarter.AddDialogLine("hlc_conversation_prisoner_ransom_thankful", "start", "close_window", "{=k7ebznzr}{HLC_RESULT}", new ConversationSentence.OnConditionDelegate(PrisonerInteraction.PlayerRescueHero), (ConversationSentence.OnConsequenceDelegate)null, 120, (ConversationSentence.OnClickableConditionDelegate)null);


            // Seemingly unused code //

           
            /*campaignGameStarter.AddDialogLine("conversation_prisoner_recruit_start_1", "start", "prisoner_recruit_start", "{=!}I'm going to take a chance on you, to give you a chance to walk free, if you like.", new ConversationSentence.OnConditionDelegate(PrisonerInteraction.IsPlayerPrisonerNonHero), (ConversationSentence.OnConsequenceDelegate)null, 100, (ConversationSentence.OnClickableConditionDelegate)null);
            campaignGameStarter.AddPlayerLine("conversation_prisoner_recruit_start", "prisoner_recruit_start", "prisoner_recruit", "{=!}Are you willing to join us? To fight alongside us?", (ConversationSentence.OnConditionDelegate)null, (ConversationSentence.OnConsequenceDelegate)null, 100, (ConversationSentence.OnClickableConditionDelegate)null, (ConversationSentence.OnPersuasionOptionDelegate)null);
            campaignGameStarter.AddDialogLine("prisoner_recruit_1", "prisoner_recruit", "close_window", "{=!}Aye. I would do that.", new ConversationSentence.OnConditionDelegate(__instance.conversation_prisoner_recruit_on_condition), (ConversationSentence.OnConsequenceDelegate)null, 100, (ConversationSentence.OnClickableConditionDelegate)null);
            campaignGameStarter.AddDialogLine("prisoner_recruit_2", "prisoner_recruit", "close_window", "{=!}No. I'm no traitor.", new ConversationSentence.OnConditionDelegate(__instance.conversation_prisoner_recruit_no_on_condition), (ConversationSentence.OnConsequenceDelegate)null, 100, (ConversationSentence.OnClickableConditionDelegate)null);
            campaignGameStarter.AddDialogLine("prisoner_recruit_3", "prisoner_recruit", "close_window", "{=!}You heard me the first time. You know where to stick your offer.", (ConversationSentence.OnConditionDelegate)null, (ConversationSentence.OnConsequenceDelegate)null, 100, (ConversationSentence.OnClickableConditionDelegate)null);*/

            // Ransom Options //

            campaignGameStarter.AddGameMenuOption("town", "town_prison", "Ransom prisoners", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.CheckForRansomPrisoners), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.GoToPrisonerRansomList), false, 9, false);
            //campaignGameStarter.AddGameMenuOption("castle_dungeon", "town_prison", "Ransom prisoners", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.CheckForRansomPrisoners), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.GoToPrisonerRansomList), false, 1, false);

            //campaignGameStarter.AddGameMenuOption("town_keep_dungeon", "town_prison", "Ransom prisoners", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.CheckForRansomPrisoners), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.GoToPrisonerRansomList), false, 1, false);
            //campaignGameStarter.AddGameMenuOption("castle_dungeon", "town_prison", "Ransom prisoners", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.CheckForRansomPrisoners), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.GoToPrisonerRansomList), false, 1, false);

            campaignGameStarter.AddGameMenu("town_keep_dungeon_ransom_hlc", "{HLC_PRISONER_RANSOM}", new OnInitDelegate(PrisonerInteraction.SelectPrisonersToRansom), GameOverlays.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.none, (object)null);

            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_ransom_hlc", "town_keep_dungeon_ransom_hlc_01", "{=HLC10100}{HLC_RANSOM01}", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlayerRansomPrisoner01), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlayerRansomPrisonerRelease01), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_ransom_hlc", "town_keep_dungeon_ransom_hlc_02", "{=HLC10100}{HLC_RANSOM02}", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlayerRansomPrisoner02), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlayerRansomPrisonerRelease02), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_ransom_hlc", "town_keep_dungeon_ransom_hlc_03", "{=HLC10100}{HLC_RANSOM03}", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlayerRansomPrisoner03), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlayerRansomPrisonerRelease03), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_ransom_hlc", "town_keep_dungeon_ransom_hlc_04", "{=HLC10100}{HLC_RANSOM04}", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlayerRansomPrisoner04), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlayerRansomPrisonerRelease04), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_ransom_hlc", "town_keep_dungeon_ransom_hlc_05", "{=HLC10100}{HLC_RANSOM05}", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlayerRansomPrisoner05), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlayerRansomPrisonerRelease05), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_ransom_hlc", "town_keep_dungeon_ransom_hlc_06", "{=HLC10100}{HLC_RANSOM06}", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlayerRansomPrisoner06), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlayerRansomPrisonerRelease06), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_ransom_hlc", "town_keep_dungeon_ransom_hlc_07", "{=HLC10100}{HLC_RANSOM07}", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlayerRansomPrisoner07), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlayerRansomPrisonerRelease07), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_ransom_hlc", "town_keep_dungeon_ransom_hlc_08", "{=HLC10100}{HLC_RANSOM08}", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlayerRansomPrisoner08), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlayerRansomPrisonerRelease08), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_ransom_hlc", "town_keep_dungeon_ransom_hlc_09", "{=HLC10100}{HLC_RANSOM09}", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlayerRansomPrisoner09), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlayerRansomPrisonerRelease09), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_ransom_hlc", "town_keep_dungeon_ransom_hlc_10", "{=HLC10100}{HLC_RANSOM10}", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlayerRansomPrisoner10), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlayerRansomPrisonerRelease10), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_ransom_hlc", "town_keep_dungeon_ransom_hlc_11", "{=HLC10100}{HLC_RANSOM11}", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlayerRansomPrisoner11), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlayerRansomPrisonerRelease11), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_ransom_hlc", "town_keep_dungeon_ransom_hlc_12", "{=HLC10100}{HLC_RANSOM12}", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlayerRansomPrisoner12), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlayerRansomPrisonerRelease12), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_ransom_hlc", "town_keep_dungeon_ransom_hlc_13", "{=HLC10100}{HLC_RANSOM13}", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlayerRansomPrisoner13), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlayerRansomPrisonerRelease13), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_ransom_hlc", "town_keep_dungeon_ransom_hlc_14", "{=HLC10100}{HLC_RANSOM14}", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlayerRansomPrisoner14), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlayerRansomPrisonerRelease14), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_ransom_hlc", "town_keep_dungeon_ransom_hlc_15", "{=HLC10100}{HLC_RANSOM15}", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlayerRansomPrisoner15), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlayerRansomPrisonerRelease15), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_ransom_hlc", "town_keep_dungeon_ransom_hlc_16", "{=HLC10100}{HLC_RANSOM16}", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlayerRansomPrisoner16), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlayerRansomPrisonerRelease16), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_ransom_hlc", "town_keep_dungeon_ransom_hlc_17", "{=HLC10100}{HLC_RANSOM17}", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlayerRansomPrisoner17), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlayerRansomPrisonerRelease17), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_ransom_hlc", "town_keep_dungeon_ransom_hlc_18", "{=HLC10100}{HLC_RANSOM18}", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlayerRansomPrisoner18), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlayerRansomPrisonerRelease18), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_ransom_hlc", "town_keep_dungeon_ransom_hlc_19", "{=HLC10100}{HLC_RANSOM19}", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlayerRansomPrisoner19), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlayerRansomPrisonerRelease19), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_ransom_hlc", "town_keep_dungeon_ransom_hlc_20", "{=HLC10100}{HLC_RANSOM20}", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlayerRansomPrisoner20), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlayerRansomPrisonerRelease20), false, -1, false);

            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_ransom_hlc", "town_keep_dungeon_ransom_hlc_back", "{=esSm5V6t}Consider other options", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.SelectPrisonersToRansomReturn), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.SelectPrisonersToRansomLeave), false, -1, false);

            // Rescue Quest //

            campaignGameStarter.AddGameMenuOption("town", "town_prison", "Plan prisoners rescue", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.CheckForPrisoners), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.GoToPrisonersRescue), false, 9, false);
            //campaignGameStarter.AddGameMenuOption("castle", "town_prison", "Plan prisoner rescue", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.Dummy), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.GoToPrisonersRescue), false, 6, false);

            //campaignGameStarter.AddGameMenuOption("town_keep_dungeon", "town_prison", "Plan rescue", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.CheckForPrisoners), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.GoToPrisonersRescue), false, 2, false);
            //campaignGameStarter.AddGameMenuOption("castle_dungeon", "town_prison", "Plan rescue", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.CheckForPrisoners), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.GoToPrisonersRescue), false, 2, false);

            campaignGameStarter.AddGameMenu("town_keep_dungeon_rescue_start_hlc", "{HLC_PRISONER_RESCUE}", new OnInitDelegate(PrisonerInteraction.StartPrisonerRescue), GameOverlays.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.none, (object)null);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_start_hlc", "town_prison", "Begin planning", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.StartPrisonerRescuePlanning), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.RedirectToPrisonerRescuePlanning), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_start_hlc", "town_prison", "Cancel the plan", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.StartPrisonerRescueReturn), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.StartPrisonerRescueLeave), false, -1, false);


            campaignGameStarter.AddGameMenu("town_keep_dungeon_rescue_plan_hlc", "{HLC_PRISONER_RESCUE_DETAILS}", new OnInitDelegate(PrisonerInteraction.PlanPrisonerRescue), GameOverlays.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.none, (object)null);

            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_plan_hlc", "town_prison", "Distraction", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueDistraction), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueDistractionSelect), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_plan_hlc", "town_prison", "Approach", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueApproach), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueApproachSelect), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_plan_hlc", "town_prison", "Escape", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueEscape), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueEscapeSelect), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_plan_hlc", "town_prison", "Execute plan", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueExecute), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueExecuteSelect), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_plan_hlc", "town_prison", "Start over", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueReturn), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueLeave), false, -1, false);

            campaignGameStarter.AddGameMenu("town_keep_dungeon_rescue_plan_distraction_hlc", "{HLC_PRISONER_RESCUE_DETAILS}", new OnInitDelegate(PrisonerInteraction.PlanPrisonerRescueDistractionStart), GameOverlays.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.none, (object)null);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_plan_distraction_hlc", "town_prison", "Bribe the guards", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueOption), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueOptionBribe), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_plan_distraction_hlc", "town_prison", "Poison the water", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueOption), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueOptionPoison), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_plan_distraction_hlc", "town_prison", "Start a fire in town", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueTownOption), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueOptionFireInside), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_plan_distraction_hlc", "town_prison", "Start a fire outside the walls", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueOption), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueOptionFireOutside), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_plan_distraction_hlc", "town_prison", "Hire bandits", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueCastleOption), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueOptionBandits), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_plan_distraction_hlc", "town_prison", "Hire mercenaries", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueTownOption), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueOptionMercs), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_plan_distraction_hlc", "town_prison", "Hire entertainers", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueTownOption), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueOptionEntertainers), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_plan_distraction_hlc", "town_prison", "Review the plan", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueDetailsReturn), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueDetailsLeave), false, -1, false);

            campaignGameStarter.AddGameMenu("town_keep_dungeon_rescue_plan_approach_hlc", "{HLC_PRISONER_RESCUE_DETAILS}", new OnInitDelegate(PrisonerInteraction.PlanPrisonerRescueApproachStart), GameOverlays.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.none, (object)null);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_plan_approach_hlc", "town_prison", "Use stealth", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueOption), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueOptionStealth), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_plan_approach_hlc", "town_prison", "Pay the guards to escort", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueOption), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueOptionApproachBribe), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_plan_approach_hlc", "town_prison", "Use soldier disguises", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueOption), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueOptionSoldiers), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_plan_approach_hlc", "town_prison", "Use officer disguises", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueOption), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueOptionOfficers), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_plan_approach_hlc", "town_prison", "Pretend to be construction workers", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueTownOption), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueOptionBuilders), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_plan_approach_hlc", "town_prison", "Sneak inside the latrine barrels", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueOption), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueOptionLatrine), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_plan_approach_hlc", "town_prison", "Review the plan", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueDetailsReturn), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueDetailsLeave), false, -1, false);

            campaignGameStarter.AddGameMenu("town_keep_dungeon_rescue_plan_escape_hlc", "{HLC_PRISONER_RESCUE_DETAILS}", new OnInitDelegate(PrisonerInteraction.PlanPrisonerRescueEscapeStart), GameOverlays.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.none, (object)null);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_plan_escape_hlc", "town_prison", "Bribe the gate guards", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueOption), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueOptionEscapeBribe), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_plan_escape_hlc", "town_prison", "Sneak onto a goods cart", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueTownOption), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueOptionGoods), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_plan_escape_hlc", "town_prison", "Pretend to be villagers", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueTownOption), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueOptionVillagers), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_plan_escape_hlc", "town_prison", "Have a trader smuggle the prisoners", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueTownOption), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueOptionTraders), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_plan_escape_hlc", "town_prison", "Play the role of search party", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueOption), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueOptionSearch), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_plan_escape_hlc", "town_prison", "Disguise fugitives as messengers", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueOption), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueOptionMessengers), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_plan_escape_hlc", "town_prison", "Scale down the walls", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueOption), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueOptionWall), false, -1, false);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_plan_escape_hlc", "town_prison", "Review the plan", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueDetailsReturn), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueDetailsLeave), false, -1, false);

            campaignGameStarter.AddWaitGameMenu("town_keep_dungeon_rescue_execute_escape_hlc", "Executing the escape plan...", (OnInitDelegate)null, new OnConditionDelegate(PrisonerInteraction.ExecuteEscapeWait), new OnConsequenceDelegate(PrisonerInteraction.ExecuteEscapeComplete), new OnTickDelegate(PrisonerInteraction.ExecuteEscapeTick), GameMenu.MenuAndOptionType.WaitMenuShowOnlyProgressOption, GameOverlays.MenuOverlayType.None, 0.0f, GameMenu.MenuFlags.none, (object)null);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_execute_escape_hlc", "town_prison", "Cancel and reconsider", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueDetailsReturn), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueDetailsLeave), false, -1, false);

            campaignGameStarter.AddGameMenu("town_keep_dungeon_rescue_execute_escape_result_hlc", "{HLC_PRISONER_RESCUE_DETAILS}", new OnInitDelegate(PrisonerInteraction.PlanPrisonerRescueResult), GameOverlays.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.none, (object)null);
            campaignGameStarter.AddGameMenuOption("town_keep_dungeon_rescue_execute_escape_result_hlc", "town_prison", "{HLC_CHOICE1}", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.PlanPrisonerRescueExecuteReturn), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.PlanPrisonerRescueExecuteLeave), false, -1, false);

            return false;
        }

        [GameMenuInitializationHandler("town_keep_dungeon_ransom_hlc")]
        [GameMenuInitializationHandler("town_keep_dungeon_rescue_start_hlc")]
        [GameMenuInitializationHandler("town_keep_dungeon_rescue_plan_hlc")]
        [GameMenuInitializationHandler("town_keep_dungeon_rescue_plan_distraction_hlc")]
        [GameMenuInitializationHandler("town_keep_dungeon_rescue_plan_approach_hlc")]
        [GameMenuInitializationHandler("town_keep_dungeon_rescue_plan_escape_hlc")]
        [GameMenuInitializationHandler("town_keep_dungeon_rescue_execute_escape_hlc")]
        [GameMenuInitializationHandler("town_keep_dungeon_rescue_execute_escape_result_hlc")]
        public static void GameMenuBackgroundSet(MenuCallbackArgs args)
        {
            Settlement settlement = Support.GetCurrentSettlement();
            args.MenuContext.SetBackgroundMeshName(settlement.GetComponent<SettlementComponent>().WaitMeshName); 
        }
    }

    [HarmonyPatch(typeof(EncounterGameMenuBehavior), "AddGameMenus")]
    class CastleOutsideDialogOverride
    {
        public static void Postfix(CampaignGameStarter gameSystemInitializer)
        {
            gameSystemInitializer.AddGameMenuOption("castle_outside", "town_prison", "Ransom prisoners", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.CheckForRansomPrisoners), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.GoToPrisonerRansomList), false, 2, false);
            gameSystemInitializer.AddGameMenuOption("castle_outside", "town_prison", "Plan prisoners rescue", new GameMenuOption.OnConditionDelegate(PrisonerInteraction.CheckForPrisoners), new GameMenuOption.OnConsequenceDelegate(PrisonerInteraction.GoToPrisonersRescue), false, 2, false);
        }
    }

    /****************/
    /* Interactions */
    /****************/

    public static class PrisonerInteraction
    {
        /***********/
        /* General */
        /***********/

        /*** General ***/

        public static int ransomState = 0;
        public static int ransomValue = 0;

        /*** Is Player Prisoner ***/

        public static Settlement settlement;
        public static List<CharacterObject> prisoners;

        public static bool playerRescuedHero = false;

        public static double rescueChance = 0.1;
        public static int rescueChanceAssumption = 0;


        public static int rescueSecurity = 0;
        public static int rescueCost = 0;

        public static int rescueDistractionCost = 0;
        public static int rescueApproachCost = 0;
        public static int rescueEscapeCost = 0;

        public static bool rescueEscapeSuccess = false;
        public static bool rescueEscapePostSuccess = false;

        public static bool rescueDistractionBribe = false;
        public static bool rescueDistractionPoison = false;
        public static bool rescueDistractionFireInside = false;
        public static bool rescueDistractionFireOutside = false;
        public static bool rescueDistractionBandits = false;
        public static bool rescueDistractionMercs = false;
        public static bool rescueDistractionEntertainers = false;

        public static int rescueDistractionBribeCost = 0;
        public static int rescueDistractionPoisonCost = 0;
        public static int rescueDistractionFireInsideCost = 0;
        public static int rescueDistractionFireOutsideCost = 0;
        public static int rescueDistractionBanditsCost = 0;
        public static int rescueDistractionMercsCost = 0;
        public static int rescueDistractionEntertainersCost = 0;

        public static bool rescueApproachStealth = false;
        public static bool rescueApproachBribe = false;
        public static bool rescueApproachSoldiers = false;
        public static bool rescueApproachOfficers = false;
        public static bool rescueApproachBuilders = false;
        public static bool rescueApproachLatrine = false;

        public static int rescueApproachStealthCost = 0;
        public static int rescueApproachBribeCost = 0;
        public static int rescueApproachSoldiersCost = 0;
        public static int rescueApproachOfficersCost = 0;
        public static int rescueApproachBuildersCost = 0;
        public static int rescueApproachLatrineCost = 0;

        public static bool rescueEscapeBribe = false;
        public static bool rescueEscapeGoods = false;
        public static bool rescueEscapeVillagers = false;
        public static bool rescueEscapeTraders = false;
        public static bool rescueEscapeSearch = false;
        public static bool rescueEscapeMessengers = false;
        public static bool rescueEscapeWall = false;

        public static int rescueEscapeBribeCost = 0;
        public static int rescueEscapeGoodsCost = 0;
        public static int rescueEscapeVillagersCost = 0;
        public static int rescueEscapeTradersCost = 0;
        public static int rescueEscapeSearchCost = 0;
        public static int rescueEscapeMessengersCost = 0;
        public static int rescueEscapeWallCost = 0;


        public static float rescueExecutionHours = 4;
        public static float rescueExecutionStartTime;
        public static CampaignTime rescueExecutionTime;

        /***************************/
        /* Player Prisoners Ransom */
        /***************************/

        /*** Is Player Prisoner ***/

        public static bool IsPlayerPrisoner()
        {
            bool result = false;

            try
            {
                if (CharacterObject.OneToOneConversationCharacter != null)
                {
                    if (CharacterObject.OneToOneConversationCharacter.Occupation != Occupation.PrisonGuard && CharacterObject.OneToOneConversationCharacter.Occupation != Occupation.Guard && CharacterObject.OneToOneConversationCharacter.Occupation != Occupation.CaravanGuard)
                    {
                        bool isPlayerPrisoner = false;

                        if (CharacterObject.OneToOneConversationCharacter.IsHero && CharacterObject.OneToOneConversationCharacter.HeroObject != null)
                        {
                            Hero hero = CharacterObject.OneToOneConversationCharacter.HeroObject;

                            if (hero.IsPrisoner)
                            {
                                if (hero.PartyBelongedToAsPrisoner != null)
                                {
                                    if (hero.PartyBelongedToAsPrisoner == PartyBase.MainParty)
                                        isPlayerPrisoner = true;
                                    else if (hero.PartyBelongedToAsPrisoner.IsSettlement)
                                    {
                                        if (hero.PartyBelongedToAsPrisoner.Settlement != null)
                                        {
                                            if (hero.PartyBelongedToAsPrisoner.Settlement.OwnerClan != null)
                                            {
                                                if (hero.PartyBelongedToAsPrisoner.Settlement.OwnerClan == Clan.PlayerClan)
                                                    isPlayerPrisoner = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (MobileParty.MainParty != null)
                            {
                                if (MobileParty.MainParty.PrisonRoster != null)
                                {
                                    if (MobileParty.MainParty.PrisonRoster.Contains(CharacterObject.OneToOneConversationCharacter))
                                    {
                                        if (Hero.OneToOneConversationHero.PartyBelongedTo == null)
                                            isPlayerPrisoner = true;
                                        else
                                        {
                                            if (!Hero.OneToOneConversationHero.PartyBelongedTo.IsActive)
                                                isPlayerPrisoner = true;
                                        }

                                        if (!isPlayerPrisoner)
                                        {
                                            if (MobileParty.ConversationParty != null)
                                            {
                                                if (MobileParty.ConversationParty.IsMainParty)
                                                    isPlayerPrisoner = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        result = isPlayerPrisoner;
                    }
                }
            }
            catch (Exception EX) { }

            return result;
        }

        /*** Is Player Prisoner Non-Hero ***/

        public static bool IsPlayerPrisonerNonHero()
        {
            bool result = false;

            if (IsPlayerPrisoner())
            {
                if (!CharacterObject.OneToOneConversationCharacter.IsHero || CharacterObject.OneToOneConversationCharacter.HeroObject == null)
                {
                    string intro = "...";

                    switch (Support.Random(1, 20))
                    {
                        case 1:
                            intro = "What do you want?";
                            break;
                        case 2:
                            intro = "Hm?";
                            break;
                        case 3:
                            intro = "Need Something?";
                            break;
                        case 4:
                            intro = "Come to gloat or to stare?";
                            break;
                        case 5:
                            intro = "If you're waiting for me to do a dance, it's not gonna happen.";
                            break;
                        case 6:
                            intro = "Well if it ain't the warden.";
                            break;
                        case 7:
                            intro = "I have nothing to say to you.";
                            break;
                        case 8:
                            intro = "What is it?";
                            break;
                        case 9:
                            intro = "Come to check on your captives?";
                            break;
                        case 10:
                            intro = "What?";
                            break;
                        case 11:
                            intro = "Hm...";
                            break;
                        case 12:
                            intro = "*coughs*";
                            break;
                        case 13:
                            intro = "*sniffle*";
                            break;
                        case 14:
                            intro = "*stare*";
                            break;
                        case 15:
                            intro = "Come to take my life along with my freedom now?";
                            break;
                        case 16:
                            intro = "Ehem...";
                            break;
                        case 17:
                            intro = "Want someting?";
                            break;
                        case 18:
                            intro = "Nothing to report";
                            break;
                        case 19:
                            intro = "All is well here";
                            break;
                        default:
                            intro = "...";
                            break;
                    }

                    MBTextManager.SetTextVariable("HLC_INTRO", intro, false);
                    return true;
                }
            }

            return result;
        }

        /*** Player Prisoner Hero ***/

        public static bool IsPlayerPrisonerHero()
        {
            bool result = false;

            if (IsPlayerPrisoner())
            {
                if (CharacterObject.OneToOneConversationCharacter.IsHero && CharacterObject.OneToOneConversationCharacter.HeroObject != null)
                {
                    // INTRO //

                    string intro = "Yes?";
                    string response = "...";

                     Hero prisoner = CharacterObject.OneToOneConversationCharacter.HeroObject;
                        CharacterTraits prisonerTraits = prisoner.GetHeroTraits();
                        TraitObject prisonerPersonality = CharacterObject.OneToOneConversationCharacter.GetPersona();

                        int playerRelation = CharacterRelationManager.GetHeroRelation(Hero.MainHero, prisoner);

                    /* Hero Intro */

                    if (playerRelation > 15)
                    {
                        if (prisonerPersonality == DefaultTraits.PersonaEarnest)
                        {
                            switch (Support.Random(1, 4))
                            {
                                case 1:
                                    intro = "Greetings to you.";
                                    break;

                                case 2:
                                    intro = "My friend, come to visit me at last?";
                                    break;

                                case 3:
                                    intro = "Do you need something from me?";
                                    break;

                                default:
                                    intro = "Yes?";
                                    break;
                            }
                            switch (Support.Random(1, 4))
                            {
                                case 1:
                                    response = "Very well.";
                                    break;

                                case 2:
                                    response = "As you say.";
                                    break;

                                case 3:
                                    response = "Whatever you say.";
                                    break;

                                default:
                                    response = "Right...";
                                    break;
                            }
                        }
                        else if (prisonerPersonality == DefaultTraits.PersonaIronic)
                        {
                            switch (Support.Random(1, 4))
                            {
                                case 1:
                                    intro = "Have you come for a game of talbut?";
                                    break;

                                case 2:
                                    intro = "My friend! Come to join me in captivity?";
                                    break;

                                case 3:
                                    intro = "The warden has come to check on my lowly self. I'm honored.";
                                    break;

                                default:
                                    intro = "Coming to check up on me are you?";
                                    break;
                            }
                            switch (Support.Random(1, 4))
                            {
                                case 1:
                                    response = "I would never dream of being stupid.";
                                    break;

                                case 2:
                                    response = "I wouldn't dare dream of it.";
                                    break;

                                case 3:
                                    response = "But ofcourse, I am your obedient prisoner.";
                                    break;

                                default:
                                    response = "Huh...";
                                    break;
                            }

                        }
                        else if (prisonerPersonality == DefaultTraits.PersonaCurt)
                        {
                            switch (Support.Random(1, 4))
                            {
                                case 1:
                                    intro = "It is good to see you friend, despite the circumstances.";
                                    break;

                                case 2:
                                    intro = "Ah, my jailor. I bid you a good day.";
                                    break;

                                case 3:
                                    intro = "Yes? Come for a little chat have we.";
                                    break;

                                default:
                                    intro = "What can I do for you dear friend?";
                                    break;
                            }
                            switch (Support.Random(1, 4))
                            {
                                case 1:
                                    response = "Very well then.";
                                    break;

                                case 2:
                                    response = "But ofcourse, your word is law here.";
                                    break;

                                case 3:
                                    response = "Certainly.";
                                    break;

                                default:
                                    response = "I see...";
                                    break;
                            }
                        }
                        else
                        {
                            switch (Support.Random(1, 4))
                            {
                                case 1:
                                    intro = "Come to talk? I'm all ears.";
                                    break;

                                case 2:
                                    intro = "Can I do something for you?";
                                    break;

                                case 3:
                                    intro = "Greetings.";
                                    break;

                                default:
                                    intro = "I bid you a good day.";
                                    break;
                            }
                            switch (Support.Random(1, 4))
                            {
                                case 1:
                                    response = "Aha...";
                                    break;

                                case 2:
                                    response = "Ok...";
                                    break;

                                case 3:
                                    response = "Whatever you say.";
                                    break;

                                default:
                                    response = "...";
                                    break;
                            }
                        }

                        response += "[if:confident2][rb:unsure]";
                    }
                    else if (playerRelation > -15)
                    {
                        if (prisonerPersonality == DefaultTraits.PersonaEarnest)
                        {
                            switch (Support.Random(1, 4))
                            {
                                case 1:
                                    intro = "Well now, are you going to stare or say something?";
                                    break;

                                case 2:
                                    intro = "Do you need something?";
                                    break;

                                case 3:
                                    intro = "Yes?";
                                    break;

                                default:
                                    intro = "Hmmm?";
                                    break;
                            }
                            switch (Support.Random(1, 4))
                            {
                                case 1:
                                    response = "You enjoy wasting my time, don't you?";
                                    break;

                                case 2:
                                    response = "Aha...";
                                    break;

                                case 3:
                                    response = "...";
                                    break;

                                default:
                                    response = "Well... Alright.";
                                    break;
                            }
                        }
                        else if (prisonerPersonality == DefaultTraits.PersonaIronic)
                        {
                            switch (Support.Random(1, 4))
                            {
                                case 1:
                                    intro = "Oh look, a guest. Would you care for a cup of wine?";
                                    break;

                                case 2:
                                    intro = "Yes? Did you miss me so much that you just had to see me?";
                                    break;

                                case 3:
                                    intro = "What can this lowly prisoner do for you?";
                                    break;

                                default:
                                    intro = "Hmmm? Did you need something?";
                                    break;
                            }
                            switch (Support.Random(1, 4))
                            {
                                case 1:
                                    response = "Until our next pointless conversation then.";
                                    break;

                                case 2:
                                    response = "Whatever you say.";
                                    break;

                                case 3:
                                    response = "Alright...";
                                    break;

                                default:
                                    response = "Ok...";
                                    break;
                            }
                        }
                        else if (prisonerPersonality == DefaultTraits.PersonaCurt)
                        {
                            switch (Support.Random(1, 4))
                            {
                                case 1:
                                    intro = "I bid you a good day " + (Hero.MainHero.IsFemale ? "madame" : "sir") + ".";
                                    break;

                                case 2:
                                    intro = "Is there something you require?";
                                    break;

                                case 3:
                                    intro = "Good day.";
                                    break;

                                default:
                                    intro = "Do you wish to converse?";
                                    break;
                            }
                            switch (Support.Random(1, 4))
                            {
                                case 1:
                                    response = "I see...";
                                    break;

                                case 2:
                                    response = "Alright...";
                                    break;

                                case 3:
                                    response = "Very well then I suppose.";
                                    break;

                                default:
                                    response = "Understood.";
                                    break;
                            }
                        }
                        else
                        {
                            switch (Support.Random(1, 4))
                            {
                                case 1:
                                    intro = (Hero.MainHero.IsFemale ? "Madame" : "Sir") + "?";
                                    break;

                                case 2:
                                    intro = "Do you need something?";
                                    break;

                                case 3:
                                    intro = "Yes?";
                                    break;

                                default:
                                    intro = "What seems to be the matter?";
                                    break;
                            }
                            switch (Support.Random(1, 4))
                            {
                                case 1:
                                    response = "Ok...";
                                    break;

                                case 2:
                                    response = "I see. Very well.";
                                    break;

                                case 3:
                                    response = "As you say.";
                                    break;

                                default:
                                    response = "...";
                                    break;
                            }
                        }

                        response += "[if:idle_pleased][rb:unsure]";
                    }
                    else
                    {
                        if (prisonerPersonality == DefaultTraits.PersonaEarnest)
                        {
                            switch (Support.Random(1, 4))
                            {
                                case 1:
                                    intro = "I have nothing to say to you.";
                                    break;

                                case 2:
                                    intro = "...";
                                    break;

                                case 3:
                                    intro = "What do you want?";
                                    break;

                                default:
                                    intro = "What?";
                                    break;
                            }
                            switch (Support.Random(1, 4))
                            {
                                case 1:
                                    response = "Sod off.";
                                    break;

                                case 2:
                                    response = "Go plough yourself.";
                                    break;

                                case 3:
                                    response = (Hero.MainHero.IsFemale ? "Bitch" : "Bastard");
                                    break;

                                default:
                                    response = "...";
                                    break;
                            }
                        }
                        else if (prisonerPersonality == DefaultTraits.PersonaIronic)
                        {
                            switch (Support.Random(1, 4))
                            {
                                case 1:
                                    intro = "Are you lost or something?";
                                    break;

                                case 2:
                                    intro = "So, have you finally decided to let me put these bindings on you?";
                                    break;

                                case 3:
                                    intro = "Are you here to offer me your surrender?";
                                    break;

                                default:
                                    intro = "Spit it out, what do you want?";
                                    break;
                            }
                            switch (Support.Random(1, 4))
                            {
                                case 1:
                                    response = "You're a moron.";
                                    break;

                                case 2:
                                    response = "Are you sure you should be speaking to me about stupidity.";
                                    break;

                                case 3:
                                    response = "Off with you.";
                                    break;

                                default:
                                    response = "What an excellently pointless conversation.";
                                    break;
                            }
                        }
                        else if (prisonerPersonality == DefaultTraits.PersonaCurt)
                        {
                            switch (Support.Random(1, 4))
                            {
                                case 1:
                                    intro = "If you wish to speak, then I would suggest commencing.";
                                    break;

                                case 2:
                                    intro = "Go on then, say your peace.";
                                    break;

                                case 3:
                                    intro = "If you wish to speak then go ahead, I'm listening.";
                                    break;

                                default:
                                    intro = "If you have something to say, then by all means say it.";
                                    break;
                            }
                            switch (Support.Random(1, 4))
                            {
                                case 1:
                                    response = "...";
                                    break;

                                case 2:
                                    response = "That was a waste of words.";
                                    break;

                                case 3:
                                    response = "I see...";
                                    break;

                                default:
                                    response = "I don't quite believe you are qualified to judge stupidity.";
                                    break;
                            }
                        }
                        else
                        {
                            switch (Support.Random(1, 4))
                            {
                                case 1:
                                    intro = "What is it?";
                                    break;

                                case 2:
                                    intro = "If you're here to talk, then just talk already.";
                                    break;

                                case 3:
                                    intro = "I have no interest in speaking to you.";
                                    break;

                                default:
                                    intro = "What do you want?";
                                    break;
                            }
                            switch (Support.Random(1, 4))
                            {
                                case 1:
                                    response = "...";
                                    break;

                                case 2:
                                    response = "Aha...";
                                    break;

                                case 3:
                                    response = "Right...";
                                    break;

                                default:
                                    response = "Whatever.";
                                    break;
                            }
                        }

                        response += "[if:idle_angry][rb:very_negative]";
                    }
                    
                    MBTextManager.SetTextVariable("HLC_INTRO", intro, false);
                    MBTextManager.SetTextVariable("HLC_RESULT", response, false);

                    // RANSOM //

                    string choice1 = "You are to be ransomed";
                    string choice2 = "You are free to go";
                    ransomValue = Support.CalculateRansom(CharacterObject.OneToOneConversationCharacter);

                    if (CharacterObject.OneToOneConversationCharacter.HeroObject.Gold >= ransomValue)
                    {
                        ransomState = 1;
                        choice1 = "I have chosen to allow your ransom for " + ransomValue + "{GOLD_ICON}";
                    }
                    else
                    {
                        if (CharacterObject.OneToOneConversationCharacter.HeroObject.Gold > 100)
                        {
                            ransomState = 2;
                            choice1 = "I am willing to release you, in exchange for all the gold you have";
                        }
                        else
                        {
                            ransomState = 3;
                            choice1 = "I have decided to release you without a ransom, since you cannot afford one";
                        }
                    }

                    choice2 = "You are free to go, unconditionally";

                    MBTextManager.SetTextVariable("HLC_CHOICE1", choice1, false);
                    MBTextManager.SetTextVariable("HLC_CHOICE2", choice2, false);
                    result = true;
                }
            }

            return result;
        }

        /*** Hero Pay Ransom ***/

        public static void HeroPayRansom()
        {
            if (CharacterObject.OneToOneConversationCharacter != null)
            {
                if (CharacterObject.OneToOneConversationCharacter.IsHero)
                {
                    Hero prisoner = CharacterObject.OneToOneConversationCharacter.HeroObject;
                    CharacterTraits prisonerTraits = prisoner.GetHeroTraits();
                    TraitObject prisonerPersonality = CharacterObject.OneToOneConversationCharacter.GetPersona();

                    int playerRelation = CharacterRelationManager.GetHeroRelation(Hero.MainHero, prisoner);

                    string response = "I thank you.";

                    switch (ransomState)
                    {
                        case 1:

                            GiveGoldAction.ApplyBetweenCharacters(prisoner, Hero.MainHero, ransomValue, false);

                            Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, 10);
                            Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 220);
                            Support.LogFriendlyMessage(Hero.MainHero.Name.ToString() + " has shown mercy");

                            if (playerRelation > 15 || prisonerTraits.Valor < 0)
                            {
                                if (prisonerPersonality == DefaultTraits.PersonaEarnest)
                                {
                                    switch (Support.Random(1, 4))
                                    {
                                        case 1:
                                            response = "A fair trade. I accept.";
                                            break;

                                        case 2:
                                            response = "Thank you friend. I have grown tired of these bindings.";
                                            break;

                                        case 3:
                                            response = "So be it then, you shall have your coin.";
                                            break;

                                        default:
                                            response = "I see no reason to object. You will have your pouch of gold.";
                                            break;
                                    }
                                }
                                else if (prisonerPersonality == DefaultTraits.PersonaIronic)
                                {
                                    switch (Support.Random(1, 4))
                                    {
                                        case 1:
                                            response = "These are some expensive bindings it seems. Oh well.";
                                            break;

                                        case 2:
                                            response = "And here I thought freedom was free. Guess everything does in fact have a price.";
                                            break;

                                        case 3:
                                            response = "To stay in shackles or lose my gold? Any third option? No? Darn it.";
                                            break;

                                        default:
                                            response = "Apparently a pouch of gold is heavy enough to lift freedom. Such is life. I accept.";
                                            break;
                                    }
                                }
                                else if (prisonerPersonality == DefaultTraits.PersonaCurt)
                                {
                                    switch (Support.Random(1, 4))
                                    {
                                        case 1:
                                            response = "Your offer is favorable to me, I accept it.";
                                            break;

                                        case 2:
                                            response = "This ransom is reasonable, I have no objection.";
                                            break;

                                        case 3:
                                            response = "Your conditions are acceptable. The deal is done.";
                                            break;

                                        default:
                                            response = "This bargain is ultimately in my favor. I accept your offer.";
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (Support.Random(1, 4))
                                    {
                                        case 1:
                                            response = "Very well friend. I accept this ransom.";
                                            break;

                                        case 2:
                                            response = "Then you will have your gold, and I will have my freedom once more.";
                                            break;

                                        case 3:
                                            response = "I see no reason to object.";
                                            break;

                                        default:
                                            response = "It is a deal then, the denars are yours.";
                                            break;
                                    }
                                }

                                response += "[rf:happy][rb:positive]";
                            }
                            else if (prisonerTraits.Valor > 0 || prisonerTraits.Generosity < 0)
                            {
                                if (prisonerPersonality == DefaultTraits.PersonaEarnest)
                                {
                                    switch (Support.Random(1, 4))
                                    {
                                        case 1:
                                            response = "We will meet again, and I will make you pay me back what I am owed.";
                                            break;

                                        case 2:
                                            response = "I have no choice but to accept, but know that vengeance will be mine.";
                                            break;

                                        case 3:
                                            response = "I will return for you, and I will reclaim what is mine.";
                                            break;

                                        default:
                                            response = "A duel would have been preferable to coin, but this will do.";
                                            break;
                                    }
                                }
                                else if (prisonerPersonality == DefaultTraits.PersonaIronic)
                                {
                                    switch (Support.Random(1, 4))
                                    {
                                        case 1:
                                            response = "I will see you on the battlefield again, your corpse better have my coin.";
                                            break;

                                        case 2:
                                            response = "Freedom should be won with blood, not coin. But alas I don't have enough blood to drown you.";
                                            break;

                                        case 3:
                                            response = "Then you will have your coin, until I reclaim it that is.";
                                            break;

                                        default:
                                            response = "Enjoy the denars while you can, I plan on retaking every coin back from you.";
                                            break;
                                    }
                                }
                                else if (prisonerPersonality == DefaultTraits.PersonaCurt)
                                {
                                    switch (Support.Random(1, 4))
                                    {
                                        case 1:
                                            response = "Very well, but know that you shall pay with blood for every coin you take.";
                                            break;

                                        case 2:
                                            response = "We will meet again my friend, and there will be consequences.";
                                            break;

                                        case 3:
                                            response = "You offer is reasonable, hence I will tolerate it.";
                                            break;

                                        default:
                                            response = "Do not take my acceptance today as weakness, for it is your coffers that will ultimately compensate me.";
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (Support.Random(1, 4))
                                    {
                                        case 1:
                                            response = "I have no choice but to accept, though I do so reluctantly.";
                                            break;

                                        case 2:
                                            response = "Then I will swallow my pride and accept.";
                                            break;

                                        case 3:
                                            response = "My freedom is to be bought with coin? I do not like it, but what other choice do I have";
                                            break;

                                        default:
                                            response = "Very well, you will have your coin. I hope you choke on it.";
                                            break;
                                    }
                                }

                                response += "[if:idle_pleased][rb:unsure]";
                            }
                            else
                            {
                                if (prisonerPersonality == DefaultTraits.PersonaEarnest)
                                {
                                    switch (Support.Random(1, 4))
                                    {
                                        case 1:
                                            response = "A reasonable offer, I accept.";
                                            break;

                                        case 2:
                                            response = "I acknowledge my defeat. A ransom is not unjust.";
                                            break;

                                        case 3:
                                            response = "While I don't enjoy giving away my coin, this is the consequence of my own failure.";
                                            break;

                                        default:
                                            response = "Very well, I see no alternative.";
                                            break;
                                    }
                                }
                                else if (prisonerPersonality == DefaultTraits.PersonaIronic)
                                {
                                    switch (Support.Random(1, 4))
                                    {
                                        case 1:
                                            response = "Gold is truly always the answer, is it not?";
                                            break;

                                        case 2:
                                            response = "A pouch of gold for a pound of freedom. Fair enough.";
                                            break;

                                        case 3:
                                            response = "No one enjoys losing denars. Then again, no one enjoys captivity either.";
                                            break;

                                        default:
                                            response = "The deal is on and the coin is yours. Use it wisely, or don't, not my concern.";
                                            break;
                                    }
                                }
                                else if (prisonerPersonality == DefaultTraits.PersonaCurt)
                                {
                                    switch (Support.Random(1, 4))
                                    {
                                        case 1:
                                            response = "Your offer is acceptable to me. It is done.";
                                            break;

                                        case 2:
                                            response = "I see reason in this offer if yours. So be it.";
                                            break;

                                        case 3:
                                            response = "Very well, the deal stands and you shall have your pound of flesh.";
                                            break;

                                        default:
                                            response = "Agreed. I'll arrange for your coin to be delivered.";
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (Support.Random(1, 4))
                                    {
                                        case 1:
                                            response = "Done.";
                                            break;

                                        case 2:
                                            response = "I have no objection. I accept.";
                                            break;

                                        case 3:
                                            response = "Very well, you'll have your coin.";
                                            break;

                                        default:
                                            response = "Very well then.";
                                            break;
                                    }
                                }

                                response += "[ib:confident2][rb:unsure]";
                            }

                            if (prisonerTraits.Valor < 0)
                                Support.ChangeRelation(Hero.MainHero, prisoner, Support.Random(0, 2));
                            else if (prisonerTraits.Valor > 0 || prisonerTraits.Generosity < 0)
                                Support.ChangeRelation(Hero.MainHero, prisoner, Support.Random(-2, 0));
                            else
                                Support.ChangeRelation(Hero.MainHero, prisoner, Support.Random(-1, 1));

                            break;

                        case 2:

                            GiveGoldAction.ApplyBetweenCharacters(prisoner, Hero.MainHero, prisoner.Gold, false);

                            Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, 20);
                            Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Generosity, 10);
                            Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 150);
                            Support.LogFriendlyMessage(Hero.MainHero.Name.ToString() + " has shown mercy and generosity");

                            if (playerRelation > 15 || prisonerTraits.Valor < 0)
                            {
                                if (prisonerPersonality == DefaultTraits.PersonaEarnest)
                                {
                                    switch (Support.Random(1, 4))
                                    {
                                        case 1:
                                            response = "A bit harsh, but reasonable I suppose.";
                                            break;

                                        case 2:
                                            response = "Not exactly happy about emptying my pockets. But I suppose it better to be free and penniless, then wealthy in shackles.";
                                            break;

                                        case 3:
                                            response = "I'm not the most fond of this deal, but I accept.";
                                            break;

                                        default:
                                            response = "Very well, I shall accept my defeat then. My coin is yours friend.";
                                            break;
                                    }
                                }
                                else if (prisonerPersonality == DefaultTraits.PersonaIronic)
                                {
                                    switch (Support.Random(1, 4))
                                    {
                                        case 1:
                                            response = "No better offer for a friend? Oh well, such is the nature of war.";
                                            break;

                                        case 2:
                                            response = "Fine. My pockets are yours, lining and all.";
                                            break;

                                        case 3:
                                            response = "Very well then.";
                                            break;

                                        default:
                                            response = "Freedom or poverty? What an impossibly obvious choice.";
                                            break;
                                    }
                                }
                                else if (prisonerPersonality == DefaultTraits.PersonaCurt)
                                {
                                    switch (Support.Random(1, 4))
                                    {
                                        case 1:
                                            response = "A fairly one sided arrangement, but alas I am in no position to negotiate.";
                                            break;

                                        case 2:
                                            response = "So be it, I approve of your conditions.";
                                            break;

                                        case 3:
                                            response = "I accept this offer of yours, though it lacks fairness in its essence.";
                                            break;

                                        default:
                                            response = "I fear I have no option but to accept.";
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (Support.Random(1, 4))
                                    {
                                        case 1:
                                            response = "So be it.";
                                            break;

                                        case 2:
                                            response = "It's not as if I have much of a choice. I accept.";
                                            break;

                                        case 3:
                                            response = "A steep price, but with little choice.";
                                            break;

                                        default:
                                            response = "You offer is accepted. As harsh as it might be.";
                                            break;
                                    }
                                }

                                response += "[ib:idle_pleased][rb:unsure]";
                            }
                            else if (prisonerTraits.Valor > 0 || prisonerTraits.Generosity < 0)
                            {
                                if (prisonerPersonality == DefaultTraits.PersonaEarnest)
                                {
                                    switch (Support.Random(1, 4))
                                    {
                                        case 1:
                                            response = "All that I have is yours, but know that I will return for it.";
                                            break;

                                        case 2:
                                            response = "I am to be left with nothing? Take it you " + (Hero.MainHero.IsFemale ? "bitch" : "bastard") + ".";
                                            break;

                                        case 3:
                                            response = "First you take my freedom, and now my wealth. What will you take next, my life?";
                                            break;

                                        default:
                                            response = "You're no better than a brigand. I will hunt you down for this.";
                                            break;
                                    }
                                }
                                else if (prisonerPersonality == DefaultTraits.PersonaIronic)
                                {
                                    switch (Support.Random(1, 4))
                                    {
                                        case 1:
                                            response = "I can't decide if this is cruelty or mercy, but so be it.";
                                            break;

                                        case 2:
                                            response = "Very well, but may your road take you far from me.";
                                            break;

                                        case 3:
                                            response = "I will accept, but if we ever meet again, it'll be too soon.";
                                            break;

                                        default:
                                            response = "You'll have your coin and I hope we never cross paths again.";
                                            break;
                                    }
                                }
                                else if (prisonerPersonality == DefaultTraits.PersonaCurt)
                                {
                                    switch (Support.Random(1, 4))
                                    {
                                        case 1:
                                            response = "Then I am defeated and poor. The humiliation.";
                                            break;

                                        case 2:
                                            response = "So I am to be shamed and made poor? Fate can be such a cruel mistress.";
                                            break;

                                        case 3:
                                            response = "I will accept your offer, though I do so with a bowed head.";
                                            break;

                                        default:
                                            response = "My wealth for my freedom then. Take it and begone.";
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (Support.Random(1, 4))
                                    {
                                        case 1:
                                            response = "Just take the coin and sod off.";
                                            break;

                                        case 2:
                                            response = "Do not think this will break me, I will gather my wealth and face you once more.";
                                            break;

                                        case 3:
                                            response = "I feel nothing now but shame for myself, and hatred for you.";
                                            break;

                                        default:
                                            response = "Have what you will, though you will pay for this one day.";
                                            break;
                                    }
                                }

                                response += "[if:idle_angry][rb:very_negative]";
                            }
                            else
                            {
                                if (prisonerPersonality == DefaultTraits.PersonaEarnest)
                                {
                                    switch (Support.Random(1, 4))
                                    {
                                        case 1:
                                            response = "Then my coin is yours.";
                                            break;

                                        case 2:
                                            response = "Take what you will then.";
                                            break;

                                        case 3:
                                            response = "It seems my wealth is now your own.";
                                            break;

                                        default:
                                            response = "All I have is yours then. I hope you choke on it.";
                                            break;
                                    }
                                }
                                else if (prisonerPersonality == DefaultTraits.PersonaIronic)
                                {
                                    switch (Support.Random(1, 4))
                                    {
                                        case 1:
                                            response = "Very well then. I would offer you more, but there is nothing left to offer.";
                                            break;

                                        case 2:
                                            response = "Take good care of my coin for me.";
                                            break;

                                        case 3:
                                            response = "Well, I suppose my coin is yours now. Nothing else needs to be said.";
                                            break;

                                        default:
                                            response = "Then the coin is yours, now give me my freedom.";
                                            break;
                                    }
                                }
                                else if (prisonerPersonality == DefaultTraits.PersonaCurt)
                                {
                                    switch (Support.Random(1, 4))
                                    {
                                        case 1:
                                            response = "Very well then.";
                                            break;

                                        case 2:
                                            response = "So be it.";
                                            break;

                                        case 3:
                                            response = "Nothing else can be done it seems, such is my agony.";
                                            break;

                                        default:
                                            response = "The coin is yours then, spend it in good health.";
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (Support.Random(1, 4))
                                    {
                                        case 1:
                                            response = "Then it is done.";
                                            break;

                                        case 2:
                                            response = "I'll arrange for the gold to be delivered. Now, time to loosen these shackles.";
                                            break;

                                        case 3:
                                            response = "And so my fortune is reduced to nothing. The price of defeat I suppose.";
                                            break;

                                        default:
                                            response = "Then take the coin and release me. I wish for this day to end swiftly.";
                                            break;
                                    }
                                }

                                response += "[if:idle_pleased][rb:unsure]";
                            }

                            if (prisonerTraits.Valor < 0)
                                Support.ChangeRelation(Hero.MainHero, prisoner, Support.Random(-1, 1));
                            else if (prisonerTraits.Valor > 0 || prisonerTraits.Generosity < 0)
                                Support.ChangeRelation(Hero.MainHero, prisoner, Support.Random(-5, -1));
                            else
                                Support.ChangeRelation(Hero.MainHero, prisoner, Support.Random(-3, 0));

                            break;

                        default:

                            Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, 30);
                            Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Generosity, 20);
                            Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 100);
                            Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 180);
                            Support.LogFriendlyMessage(Hero.MainHero.Name.ToString() + " has shown great mercy and generosity");

                            if (prisonerPersonality == DefaultTraits.PersonaEarnest)
                            {
                                switch (Support.Random(1, 4))
                                {
                                    case 1:
                                        response = "While this feels a bit humiliating, I am glad to be free.";
                                        break;

                                    case 2:
                                        response = "Well, while I dislike my own poverty, it does seem to have some perks.";
                                        break;

                                    case 3:
                                        response = "This was not expected. I Thank you.";
                                        break;

                                    default:
                                        response = "I thank you. By my honor, I will never forget this.";
                                        break;
                                }
                            }
                            else if (prisonerPersonality == DefaultTraits.PersonaIronic)
                            {
                                switch (Support.Random(1, 4))
                                {
                                    case 1:
                                        response = "Then I bid you farewell, until battle brings us together once more.";
                                        break;

                                    case 2:
                                        response = "Hahaha, you beautiful " + (Hero.MainHero.IsFemale ? "bitch" : "bastard") + ". I won't soon forget this.";
                                        break;

                                    case 3:
                                        response = "It seems having low funds is not always a curse.";
                                        break;

                                    default:
                                        response = "For once I'm glad I have little coin.";
                                        break;
                                }
                            }
                            else if (prisonerPersonality == DefaultTraits.PersonaCurt)
                            {
                                switch (Support.Random(1, 4))
                                {
                                    case 1:
                                        response = "You have shown me great kindness. I thank you.";
                                        break;

                                    case 2:
                                        response = "Then I wish long life and fortune for you. You have my thanks.";
                                        break;

                                    case 3:
                                        response = "Most nobles would not release a prisoner without compensation or force. You have shown yourself their better.";
                                        break;

                                    default:
                                        response = "Unexpected, but certainly a welcomed gesture.";
                                        break;
                                }
                            }
                            else
                            {
                                switch (Support.Random(1, 4))
                                {
                                    case 1:
                                        response = "Then I am free to go? Well then, may we meet on the field of battle once more.";
                                        break;

                                    case 2:
                                        response = "You are truly honorable. I thank you.";
                                        break;

                                    case 3:
                                        response = "I feel almost ashamed, yet I am glad that you have chosen this.";
                                        break;

                                    default:
                                        response = "You have given me back my freedom, for that I am in your debt.";
                                        break;
                                }
                            }

                            response += "[rf:happy][rb:positive]";

                            if (prisonerTraits.Valor < 0 || prisonerTraits.Generosity < 0)
                                Support.ChangeRelation(Hero.MainHero, prisoner, Support.Random(2, 4));
                            else if (prisonerTraits.Valor > 0 || prisonerTraits.Honor > 0)
                                Support.ChangeRelation(Hero.MainHero, prisoner, Support.Random(0, 2));
                            else
                                Support.ChangeRelation(Hero.MainHero, prisoner, Support.Random(1, 3));

                            break;
                    }

                    ReleasePrisonerThroughDialog();
                    MBTextManager.SetTextVariable("HLC_RESULT", response, false);
                }
            }
        }



        /*** Hero Unconditional Release ***/

        public static void HeroUnconditionalRelease()
        {
            if (CharacterObject.OneToOneConversationCharacter != null)
            {
                if (CharacterObject.OneToOneConversationCharacter.IsHero && CharacterObject.OneToOneConversationCharacter.HeroObject != null)
                {
                    Hero prisoner = CharacterObject.OneToOneConversationCharacter.HeroObject;
                    CharacterTraits prisonerTraits = prisoner.GetHeroTraits();
                    TraitObject prisonerPersonality = CharacterObject.OneToOneConversationCharacter.GetPersona();

                    string response = "I thank you.";

                    Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, 40);
                    Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Generosity, 20);
                    Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, 10);
                    Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 200);
                    Support.LogFriendlyMessage(Hero.MainHero.Name.ToString() + " has shown great mercy, generosity and honor");

                    if (prisonerPersonality == DefaultTraits.PersonaEarnest)
                    {
                        switch (Support.Random(1, 4))
                        {
                            case 1:
                                response = "You are among the most honorable leaders I have met. I thank you.";
                                break;

                            case 2:
                                response = "Well then, this is a pleasant surprise.";
                                break;

                            case 3:
                                response = "You are far too kind for this horrid world. I humbly thank you.";
                                break;

                            default:
                                response = "I Thank you. May your ancestors forever smile upon you.";
                                break;
                        }
                    }
                    else if (prisonerPersonality == DefaultTraits.PersonaIronic)
                    {
                        switch (Support.Random(1, 4))
                        {
                            case 1:
                                response = "Your heart is kinder than mine it seems. I thank you for that.";
                                break;

                            case 2:
                                response = "Then until we meet again on the field of battle.";
                                break;

                            case 3:
                                response = "You are no enemy of mine on this day. I salute your honor.";
                                break;

                            default:
                                response = "Anyone else would have stripped me of my wealth. You are superior to most.";
                                break;
                        }
                    }
                    else if (prisonerPersonality == DefaultTraits.PersonaCurt)
                    {
                        switch (Support.Random(1, 4))
                        {
                            case 1:
                                response = "Your gesture is appreciated, and noted.";
                                break;

                            case 2:
                                response = "You are as kind as you are strong " + (Hero.MainHero.IsFemale ? "madame" : "sir") + ". I commend you.";
                                break;

                            case 3:
                                response = "I shall take my leave then. May the earth beneath your feet be as soft as your heart.";
                                break;

                            default:
                                response = "You could have kept me prisoner, yet you allow my liberty? You are rare amongst the nobles of this world.";
                                break;
                        }
                    }
                    else
                    {
                        switch (Support.Random(1, 4))
                        {
                            case 1:
                                response = "Then I am free to go? Well then, may we meet on the field of battle once more.";
                                break;

                            case 2:
                                response = "You are truly honorable. I thank you.";
                                break;

                            case 3:
                                response = "You have my eternal gratitude.";
                                break;

                            default:
                                response = "You have shown me such kindness. I can only hope to return it one day.";
                                break;
                        }
                    }

                    if (prisonerTraits.Valor < 0 || prisonerTraits.Generosity < 0 || prisonerTraits.Mercy > 0)
                        Support.ChangeRelation(Hero.MainHero, prisoner, Support.Random(3, 6));
                    else
                        Support.ChangeRelation(Hero.MainHero, prisoner, Support.Random(1, 4));

                    ReleasePrisonerThroughDialog();
                    MBTextManager.SetTextVariable("HLC_RESULT", response + "[rf:happy][rb:very_positive]", false);
                }
            }
        }

        /*** Release Prisoner Through Dialog ***/

        public static void ReleasePrisonerThroughDialog()
        {
            if (CharacterObject.OneToOneConversationCharacter != null)
            {
                if (CharacterObject.OneToOneConversationCharacter.IsHero && CharacterObject.OneToOneConversationCharacter.HeroObject != null)
                {
	                var rooster = new FlattenedTroopRoster(1);
	                rooster.Add(CharacterObject.OneToOneConversationCharacter);
                    EndCaptivityAction.ApplyByReleasedFromPartyScreen(rooster);
                    Support.ChangeFamilyRelation(Hero.MainHero, CharacterObject.OneToOneConversationCharacter.HeroObject, 0, 1);
                }
            }
        }

        /*** Player Rescue Hero ***/

        public static bool PlayerRescueHero()
        {
            if (CharacterObject.OneToOneConversationCharacter != null)
            {
                if (CharacterObject.OneToOneConversationCharacter.IsHero && CharacterObject.OneToOneConversationCharacter.HeroObject != null)
                {
                    Hero prisoner = CharacterObject.OneToOneConversationCharacter.HeroObject;
                    CharacterTraits prisonerTraits = prisoner.GetHeroTraits();
                    TraitObject prisonerPersonality = CharacterObject.OneToOneConversationCharacter.GetPersona();

                    int playerRelation = CharacterRelationManager.GetHeroRelation(Hero.MainHero, prisoner);

                    string response = "I thank you for having me released.";

                    if (prisoner.IsPlayerCompanion)
                    {
                        string playerTitle = (Hero.MainHero.IsFemale ? "lady" : "lord");

                        switch (Support.EvaluatePersonality(prisonerTraits))
                        {
                            case 1:
                                switch (Support.Random(1, 4))
                                {
                                    case 1:
                                        response = "I am honored to return to your service once more.";
                                        break;

                                    case 2:
                                        response = "My " + playerTitle + ", I am ready to serve once more.";
                                        break;

                                    case 3:
                                        response = "I thank you my " + playerTitle + ".";
                                        break;

                                    default:
                                        response = "Your " + playerTitle + "ship, I am honored to be in your service once again.";
                                        break;
                                }

                                Support.ChangeRelation(Hero.MainHero, prisoner, Support.Random(11, 19));
                                break;

                            case 2:
                                switch (Support.Random(1, 4))
                                {
                                    case 1:
                                        response = "Well, I'm certainly glad to be out of bindings. Thank you my " + playerTitle + ".";
                                        break;

                                    case 2:
                                        response = "My " + playerTitle + ", you are a beautiful creature. What new tasks have you got in store for me?";
                                        break;

                                    case 3:
                                        response = "I'm back! What are your orders my " + playerTitle + ".";
                                        break;

                                    default:
                                        response = "Your " + playerTitle + "ship, never doubted that you'd be back for me.";
                                        break;
                                }

                                Support.ChangeRelation(Hero.MainHero, prisoner, Support.Random(13, 20));
                                break;

                            case 3:
                                switch (Support.Random(1, 4))
                                {
                                    case 1:
                                        response = "My " + playerTitle + ", I return to do your bidding. Command me.";
                                        break;

                                    case 2:
                                        response = "Your " + playerTitle + "ship, I expected you would return for me, but I rejoice non the less.";
                                        break;

                                    case 3:
                                        response = "So you have finally come for me my " + playerTitle + ". I am glad to be in your service once more.";
                                        break;

                                    default:
                                        response = "I salute your honor my " + playerTitle + ", for you would not abandon even a lowly servant.";
                                        break;
                                }

                                Support.ChangeRelation(Hero.MainHero, prisoner, Support.Random(5, 12));
                                break;

                            default:

                                switch (Support.Random(1, 4))
                                {
                                    case 1:
                                        response = "My " + playerTitle + ", you are a true noble. I happily return to your service.";
                                        break;

                                    case 2:
                                        response = "I thought I would never know freedom again my " + playerTitle + ". I thank you.";
                                        break;

                                    case 3:
                                        response = "I proudly return to serve you my " + playerTitle + ". You have my gratitude for releasing me.";
                                        break;

                                    default:
                                        response = "Finally! I am free to serve my " + playerTitle + " once more. Command me, and I will gladly obey.";
                                        break;
                                }

                                Support.ChangeRelation(Hero.MainHero, prisoner, Support.Random(10, 15));
                                break;
                        }
                    }
                    else
                    {
                        if (prisonerPersonality == DefaultTraits.PersonaEarnest)
                        {
                            switch (Support.Random(1, 4))
                            {
                                case 1:
                                    response = "Either you are quite wealthy, or incredibly honorable. In either case, I salute you.";
                                    break;

                                case 2:
                                    response = "You've done me a great service today. I will remember this.";
                                    break;

                                case 3:
                                    response = "Once more I am free. I thank you for this.";
                                    break;

                                default:
                                    response = "You honor me with your action. I will not forget this day.";
                                    break;
                            }
                        }
                        else if (prisonerPersonality == DefaultTraits.PersonaIronic)
                        {
                            switch (Support.Random(1, 4))
                            {
                                case 1:
                                    response = "I would have broken out of that cell eventually. But I thank you, truly.";
                                    break;

                                case 2:
                                    response = "And here I was getting comfortable being a prisoner. Oh well.";
                                    break;

                                case 3:
                                    response = "I am free again, until I get captured in some other battle that is. You have my thanks.";
                                    break;

                                default:
                                    response = "You beautiful " + (Hero.MainHero.IsFemale ? "bitch" : "bastard") + ". I owe you a debt of gratitude.";
                                    break;
                            }
                        }
                        else if (prisonerPersonality == DefaultTraits.PersonaCurt)
                        {
                            switch (Support.Random(1, 4))
                            {
                                case 1:
                                    response = "I thank you for my liberty. May the heavens strike down any who would dare imprison you friend.";
                                    break;

                                case 2:
                                    response = "Your actions here have humbled me, I can only hope to return the favor one day.";
                                    break;

                                case 3:
                                    response = "May the heavens never cease to smile upon you friend.";
                                    break;

                                default:
                                    response = "Your deeds today will not be soon forgotten.";
                                    break;
                            }
                        }
                        else
                        {
                            switch (Support.Random(1, 4))
                            {
                                case 1:
                                    response = "You have returned my freedom to me. What you have done today will not soon be forgotten.";
                                    break;

                                case 2:
                                    response = "You are a true noble among the heartless masses. I thank you.";
                                    break;

                                case 3:
                                    response = "What you have done here today... It will not be forgotten.";
                                    break;

                                default:
                                    response = "I am forever in your debt.";
                                    break;
                            }
                        }

                        if (prisonerTraits.Valor < 0 || prisonerTraits.Mercy > 0)
                            Support.ChangeRelation(Hero.MainHero, prisoner, Support.Random(2, 6));
                        else if (prisonerTraits.Valor > 0)
                            Support.ChangeRelation(Hero.MainHero, prisoner, Support.Random(1, 3));
                        else
                            Support.ChangeRelation(Hero.MainHero, prisoner, Support.Random(2, 4));
                    }

                    MBTextManager.SetTextVariable("HLC_RESULT", response + "[rf:happy][rb:very_positive]");
                }
            }

            return playerRescuedHero;
        }

        /*******************************/
        /* Non-Player Prisoners Ransom */
        /*******************************/

        /*** Dummy ***/

        public static bool Dummy(MenuCallbackArgs args)
        { return true; }

        /*** Check For Prisoners ***/
        public static bool CheckForPrisoners(MenuCallbackArgs args)
        {
            bool result = false;

            settlement = Support.GetCurrentSettlement();

            if (settlement != null)
            {
                Hero owner = null;

                if (settlement.Town != null)
                {
                    if (settlement.Town.Owner != null)
                        owner = Support.FindHero(settlement.Town.Owner);
                }

                if (settlement.GetComponent<SettlementComponent>().GetPrisonerHeroes().Count > 0)
                {
                    args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
                    result = true;
                }

                if (owner != null)
                {
                    if (owner.IsHumanPlayerCharacter)
                        result = false;
                }
            }

            return result;
        }

        /*** Check For Prisoners ***/
        public static bool CheckForPrisonersInHostileCastle(MenuCallbackArgs args)
        {
            bool result = false;

            settlement = Support.GetCurrentSettlement();

            if (CheckForPrisoners(args))
            {
                SettlementAccessModel.AccessDetails accessDetails;
                Campaign.Current.Models.SettlementAccessModel.CanMainHeroEnterKeep(Settlement.CurrentSettlement, out accessDetails);

                if (accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.NoAccess)
                    return true;
            }

            return result;
        }

        public static bool CheckForRansomPrisoners(MenuCallbackArgs args)
        {
            bool result = false;

            if (Support.settings.allow_ransoms)
            {
                bool sameFaction = false;

                settlement = Support.GetCurrentSettlement();

                if (settlement != null)
                {
                    if (Hero.MainHero.MapFaction != null)
                    {
                        if(settlement.Town != null)
                        {
                            if (Hero.MainHero.MapFaction == settlement.MapFaction || Hero.MainHero.MapFaction == settlement.Town.MapFaction)
                                sameFaction = true;
                        }
                    }
                }
                
                if(!sameFaction)
                    result = CheckForPrisoners(args);
            }

            return result;
        }

        /*** Go To Prisoner Ransom List ***/
        public static void GoToPrisonerRansomList(MenuCallbackArgs args)
        { GameMenu.SwitchToMenu("town_keep_dungeon_ransom_hlc"); }

        /*** Select Prisoners to Ransom ***/
        public static void SelectPrisonersToRansom(MenuCallbackArgs args)
        {
            settlement = Support.GetCurrentSettlement();

            if (settlement != null)
            {
                PrisonLocationClear();
                prisoners = settlement.GetComponent<SettlementComponent>().GetPrisonerHeroes();

                args.MenuTitle = new TextObject("{=x04UGQDn}Prisoners");
                string text = "There are no notable prisoners in this " + (settlement.IsCastle ? "castle" : "town") + ".";

                if (prisoners != null)
                {
                    if (prisoners.Count == 1)
                    {
                        if (prisoners[0] != null)
                        {
                            if (prisoners[0].HeroObject != null && prisoners[0].IsHero)
                                text = prisoners[0].Name.ToString() + " is the only notable character imprisoned in this " + (settlement.IsCastle ? "castle" : "town") + ".";
                        }
                    }

                    if (prisoners.Count > 1)
                        text = "There are " + prisoners.Count.ToString() + " notable characters that can be ransomed in this " + (settlement.IsCastle ? "castle" : "town") + ".";
                }

                MBTextManager.SetTextVariable("HLC_PRISONER_RANSOM", text);
            }
        }

        /*** Select Prisoners to Ransom Return ***/
        public static bool SelectPrisonersToRansomReturn(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.Leave;
            return true;
        }
        public static void SelectPrisonersToRansomLeave(MenuCallbackArgs args)
        {
            if (settlement != null)
            {
                if (settlement.IsCastle)
                    GameMenu.SwitchToMenu("castle_outside");
                else
                    GameMenu.SwitchToMenu("town");
            }
        }

        /*** Player Ransom Prisoner ***/
        public static bool PlayerRansomPrisoner(int id, MenuCallbackArgs args)
        {
            bool result = false;

            if (prisoners.Count >= id)
            {
                CharacterObject prisoner = prisoners[id - 1];

                if (prisoner != null)
                {
                    if (prisoner.HeroObject != null && prisoner.IsHero)
                    {
                        int ransom = Support.CalculatePlayerToHeroRansom(Support.CalculateRansom(prisoner, false));
                        args.optionLeaveType = GameMenuOption.LeaveType.RansomAndBribe;

                        if (Hero.MainHero.Gold < ransom)
                        {
                            args.Tooltip = new TextObject("{=ETKyjOkJ}You don't have enough denars to pay the ransom.");
                            args.IsEnabled = false;
                        }

                        string idText = id.ToString();

                        if (id < 10)
                            idText = "0" + idText;

                        MBTextManager.SetTextVariable("HLC_RANSOM" + idText, prisoner.Name.ToString() + " [" + (Support.CalculatePlayerToHeroRansom(Support.CalculateRansom(prisoner, false))).ToString() + " {GOLD_ICON}]");
                        result = true;
                    }
                }
            }

            return result;
        }

        public static bool PlayerRansomPrisoner01(MenuCallbackArgs args)
        { return PlayerRansomPrisoner(1, args); }
        public static bool PlayerRansomPrisoner02(MenuCallbackArgs args)
        { return PlayerRansomPrisoner(2, args); }
        public static bool PlayerRansomPrisoner03(MenuCallbackArgs args)
        { return PlayerRansomPrisoner(3, args); }
        public static bool PlayerRansomPrisoner04(MenuCallbackArgs args)
        { return PlayerRansomPrisoner(4, args); }
        public static bool PlayerRansomPrisoner05(MenuCallbackArgs args)
        { return PlayerRansomPrisoner(5, args); }
        public static bool PlayerRansomPrisoner06(MenuCallbackArgs args)
        { return PlayerRansomPrisoner(6, args); }
        public static bool PlayerRansomPrisoner07(MenuCallbackArgs args)
        { return PlayerRansomPrisoner(7, args); }
        public static bool PlayerRansomPrisoner08(MenuCallbackArgs args)
        { return PlayerRansomPrisoner(8, args); }
        public static bool PlayerRansomPrisoner09(MenuCallbackArgs args)
        { return PlayerRansomPrisoner(9, args); }
        public static bool PlayerRansomPrisoner10(MenuCallbackArgs args)
        { return PlayerRansomPrisoner(10, args); }
        public static bool PlayerRansomPrisoner11(MenuCallbackArgs args)
        { return PlayerRansomPrisoner(11, args); }
        public static bool PlayerRansomPrisoner12(MenuCallbackArgs args)
        { return PlayerRansomPrisoner(12, args); }
        public static bool PlayerRansomPrisoner13(MenuCallbackArgs args)
        { return PlayerRansomPrisoner(13, args); }
        public static bool PlayerRansomPrisoner14(MenuCallbackArgs args)
        { return PlayerRansomPrisoner(14, args); }
        public static bool PlayerRansomPrisoner15(MenuCallbackArgs args)
        { return PlayerRansomPrisoner(15, args); }
        public static bool PlayerRansomPrisoner16(MenuCallbackArgs args)
        { return PlayerRansomPrisoner(16, args); }
        public static bool PlayerRansomPrisoner17(MenuCallbackArgs args)
        { return PlayerRansomPrisoner(17, args); }
        public static bool PlayerRansomPrisoner18(MenuCallbackArgs args)
        { return PlayerRansomPrisoner(18, args); }
        public static bool PlayerRansomPrisoner19(MenuCallbackArgs args)
        { return PlayerRansomPrisoner(19, args); }
        public static bool PlayerRansomPrisoner20(MenuCallbackArgs args)
        { return PlayerRansomPrisoner(20, args); }

        /*** Player Ransom Prisoner Release ***/
        public static void PlayerRansomPrisonerRelease(int id)
        {
            if (prisoners.Count >= id)
            {
                CharacterObject prisoner = prisoners[id - 1];

                if (prisoner != null)
                {
                    if (prisoner.HeroObject != null)
                    {
                        int ransom = Support.CalculatePlayerToHeroRansom(Support.CalculateRansom(prisoner, false));

                        if (Hero.MainHero.Gold >= ransom)
                        {
                            Hero settlementOwner = null;

                            if (settlement != null)
                            {
                                if (settlement.Town != null)
                                    settlementOwner = Support.FindHero(settlement.Town.Owner);
                            }

                            EndCaptivityAction.ApplyByRansom(prisoner.HeroObject, Hero.MainHero);
                            GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, settlementOwner, ransom);

                            Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, 10);
                            Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, 25);
                            Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Generosity, 50);
                            Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 300);

                            RescueHero(prisoner.HeroObject);

                            if (settlement.IsCastle)
                                GameMenu.SwitchToMenu("castle");
                            else
                                GameMenu.SwitchToMenu("town");
                        }
                        else
                            InformationManager.ShowInquiry(new InquiryData("Insufficient funds", "You do not have enough denars to pay this ransom", true, false, "Ok", "Close", (Action)null, (Action)null, ""));
                    }
                }
            }
        }

        /*** Rescue Hero ***/
        public static void RescueHero(Hero prisoner, bool secret = false)
        {

            Support.ChangeRelation(Hero.MainHero, prisoner, Support.Random(4, 7));

            if (!secret)
            {
                Support.ChangeFamilyRelation(Hero.MainHero, prisoner, 1, 2);
                Support.ChangeFactionRelation(Hero.MainHero, prisoner, 0, 1);
            }

            playerRescuedHero = true;
            CampaignMapConversation.OpenConversation(new ConversationCharacterData(Hero.MainHero.CharacterObject), new ConversationCharacterData(prisoner.CharacterObject));
            playerRescuedHero = false;

            if (secret)
                Support.LogFriendlyMessage(Hero.MainHero.Name.ToString() + " has rescued " + prisoner.Name.ToString());
            else
                Support.LogFriendlyMessage(Hero.MainHero.Name.ToString() + " has paid the ransom for " + prisoner.Name.ToString());

            if (prisoner.IsPlayerCompanion)
            {
                if (!Hero.MainHero.PartyBelongedTo.MemberRoster.Contains(prisoner.CharacterObject))
                    Hero.MainHero.PartyBelongedTo.MemberRoster.AddToCounts(prisoner.CharacterObject, 1);
            }
        }

        public static void PlayerRansomPrisonerRelease01(MenuCallbackArgs args)
        { PlayerRansomPrisonerRelease(1); }
        public static void PlayerRansomPrisonerRelease02(MenuCallbackArgs args)
        { PlayerRansomPrisonerRelease(2); }
        public static void PlayerRansomPrisonerRelease03(MenuCallbackArgs args)
        { PlayerRansomPrisonerRelease(3); }
        public static void PlayerRansomPrisonerRelease04(MenuCallbackArgs args)
        { PlayerRansomPrisonerRelease(4); }
        public static void PlayerRansomPrisonerRelease05(MenuCallbackArgs args)
        { PlayerRansomPrisonerRelease(5); }
        public static void PlayerRansomPrisonerRelease06(MenuCallbackArgs args)
        { PlayerRansomPrisonerRelease(6); }
        public static void PlayerRansomPrisonerRelease07(MenuCallbackArgs args)
        { PlayerRansomPrisonerRelease(7); }
        public static void PlayerRansomPrisonerRelease08(MenuCallbackArgs args)
        { PlayerRansomPrisonerRelease(8); }
        public static void PlayerRansomPrisonerRelease09(MenuCallbackArgs args)
        { PlayerRansomPrisonerRelease(9); }
        public static void PlayerRansomPrisonerRelease10(MenuCallbackArgs args)
        { PlayerRansomPrisonerRelease(10); }
        public static void PlayerRansomPrisonerRelease11(MenuCallbackArgs args)
        { PlayerRansomPrisonerRelease(11); }
        public static void PlayerRansomPrisonerRelease12(MenuCallbackArgs args)
        { PlayerRansomPrisonerRelease(12); }
        public static void PlayerRansomPrisonerRelease13(MenuCallbackArgs args)
        { PlayerRansomPrisonerRelease(13); }
        public static void PlayerRansomPrisonerRelease14(MenuCallbackArgs args)
        { PlayerRansomPrisonerRelease(14); }
        public static void PlayerRansomPrisonerRelease15(MenuCallbackArgs args)
        { PlayerRansomPrisonerRelease(15); }
        public static void PlayerRansomPrisonerRelease16(MenuCallbackArgs args)
        { PlayerRansomPrisonerRelease(16); }
        public static void PlayerRansomPrisonerRelease17(MenuCallbackArgs args)
        { PlayerRansomPrisonerRelease(17); }
        public static void PlayerRansomPrisonerRelease18(MenuCallbackArgs args)
        { PlayerRansomPrisonerRelease(18); }
        public static void PlayerRansomPrisonerRelease19(MenuCallbackArgs args)
        { PlayerRansomPrisonerRelease(19); }
        public static void PlayerRansomPrisonerRelease20(MenuCallbackArgs args)
        { PlayerRansomPrisonerRelease(20); }

        /*** Prisoner Location Clear ***/

        public static void PrisonLocationClear()
        {
            Campaign.Current.GameMenuManager.MenuLocations.Clear();
            Campaign.Current.GameMenuManager.MenuLocations.Add(settlement.LocationComplex.GetLocationWithId("prison"));
        }

        /*******************************/
        /* Non-Player Prisoners Rescue */
        /*******************************/

        /*** Go To Prisoners Rescue ***/
        public static void GoToPrisonersRescue(MenuCallbackArgs args)
        {
            rescueDistractionBribeCost = Support.Random(2000, 4500);
            rescueDistractionPoisonCost = Support.Random(500, 1500);
            rescueDistractionFireInsideCost = 0;
            rescueDistractionFireOutsideCost = 0;
            rescueDistractionBanditsCost = Support.Random(2500, 4000);
            rescueDistractionMercsCost = Support.Random(4000, 5590);
            rescueDistractionEntertainersCost = Support.Random(3000, 4000);

            rescueApproachStealthCost = 0;
            rescueApproachBribeCost = Support.Random(5000, 7000);
            rescueApproachSoldiersCost = Support.Random(1100, 2500);
            rescueApproachOfficersCost = Support.Random(3500, 5500);
            rescueApproachBuildersCost = Support.Random(250, 700);
            rescueApproachLatrineCost = 0;

            rescueEscapeBribeCost = Support.Random(3000, 5000);
            rescueEscapeGoodsCost = 0;
            rescueEscapeVillagersCost = Support.Random(1000, 2500);
            rescueEscapeTradersCost = Support.Random(4000, 8000);
            rescueEscapeSearchCost = 0;
            rescueEscapeMessengersCost = 0;
            rescueEscapeWallCost = 0;

            if(Support.PlayerPartyHasPerk(DefaultPerks.Roguery.RansomBroker))
            {
                rescueDistractionBribeCost = rescueDistractionBribeCost / 2;
                rescueApproachBribeCost = rescueApproachBribeCost / 2;
                rescueEscapeBribeCost = rescueEscapeBribeCost / 2;
            }

            GameMenu.SwitchToMenu("town_keep_dungeon_rescue_start_hlc");
        }

        /*** Start Prisoner Rescue Return ***/
        public static bool StartPrisonerRescueReturn(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.Leave;
            return true;
        }
        public static void StartPrisonerRescueLeave(MenuCallbackArgs args)
        {
            if (settlement != null)
            {
                if (settlement.IsCastle)
                    GameMenu.SwitchToMenu("castle_outside");
                else
                    GameMenu.SwitchToMenu("town");
            }
        }

        /*** Start Prisoner Rescue Planning ***/
        public static bool StartPrisonerRescuePlanning(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.Escape;
            return true;
        }
        public static void RedirectToPrisonerRescuePlanning(MenuCallbackArgs args)
        { GameMenu.SwitchToMenu("town_keep_dungeon_rescue_plan_hlc"); }

        /*** Start Prisoner Rescue Distraction ***/
        public static bool PlanPrisonerRescueDistraction(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.OrderTroopsToAttack;
            return true;
        }
        public static void PlanPrisonerRescueDistractionSelect(MenuCallbackArgs args)
        { GameMenu.SwitchToMenu("town_keep_dungeon_rescue_plan_distraction_hlc"); }

        /*** Start Prisoner Rescue Approach ***/
        public static bool PlanPrisonerRescueApproach(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.Continue;
            return true;
        }
        public static void PlanPrisonerRescueApproachSelect(MenuCallbackArgs args)
        { GameMenu.SwitchToMenu("town_keep_dungeon_rescue_plan_approach_hlc"); }

        /*** Start Prisoner Rescue Escape ***/
        public static bool PlanPrisonerRescueEscape(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.Recruit;
            return true;
        }
        public static void PlanPrisonerRescueEscapeSelect(MenuCallbackArgs args)
        { GameMenu.SwitchToMenu("town_keep_dungeon_rescue_plan_escape_hlc"); }

        /*** Start Prisoner Rescue Execute ***/
        public static bool PlanPrisonerRescueExecute(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.HostileAction;

            if (Hero.MainHero.Gold < rescueCost)
            {
                args.Tooltip = new TextObject("{=ETKyjOkJ}You don't have enough denars to execute this plan.");
                args.IsEnabled = false;
            }

            return true;
        }
        public static void PlanPrisonerRescueExecuteSelect(MenuCallbackArgs args)
        { 
            if(Hero.MainHero.Gold >= rescueCost)
                GameMenu.SwitchToMenu("town_keep_dungeon_rescue_execute_escape_hlc"); 
            else
                InformationManager.ShowInquiry(new InquiryData("Insufficient funds", "You cannot afford to fund this rescue", true, false, "Ok", "Close", (Action)null, (Action)null, ""));
        }

        /*** Plan Prisoner Rescue Return ***/
        public static bool PlanPrisonerRescueReturn(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.Leave;
            return true;
        }
        public static void PlanPrisonerRescueLeave(MenuCallbackArgs args)
        { GameMenu.SwitchToMenu("town_keep_dungeon_rescue_start_hlc"); }

        /*** Plan Prisoner Rescue Details Return ***/
        public static bool PlanPrisonerRescueDetailsReturn(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.Leave;
            return true;
        }
        public static void PlanPrisonerRescueDetailsLeave(MenuCallbackArgs args)
        { GameMenu.SwitchToMenu("town_keep_dungeon_rescue_plan_hlc"); }

        /*** Plan Prisoner Rescue Option ***/
        public static bool PlanPrisonerRescueOption(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.RansomAndBribe;
            return true;
        }
        public static bool PlanPrisonerRescueTownOption(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.RansomAndBribe;
            return !settlement.IsCastle;
        }
        public static bool PlanPrisonerRescueCastleOption(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.RansomAndBribe;
            return settlement.IsCastle;
        }

        /*** Plan Prisoner Rescue Distraction Options ***/
        public static void PlanPrisonerRescueOptionBribe(MenuCallbackArgs args)
        {
            PrisonerRescuDistractionClear();
            rescueDistractionBribe = true;

            rescueDistractionCost = rescueDistractionBribeCost;
            GameMenu.SwitchToMenu("town_keep_dungeon_rescue_plan_hlc");
        }
        public static void PlanPrisonerRescueOptionPoison(MenuCallbackArgs args)
        {
            PrisonerRescuDistractionClear();
            rescueDistractionPoison = true;

            rescueDistractionCost = rescueDistractionPoisonCost;
            GameMenu.SwitchToMenu("town_keep_dungeon_rescue_plan_hlc");
        }
        public static void PlanPrisonerRescueOptionFireInside(MenuCallbackArgs args)
        {
            PrisonerRescuDistractionClear();
            rescueDistractionFireInside = true;

            rescueDistractionCost = rescueDistractionFireInsideCost;
            GameMenu.SwitchToMenu("town_keep_dungeon_rescue_plan_hlc");
        }
        public static void PlanPrisonerRescueOptionFireOutside(MenuCallbackArgs args)
        {
            PrisonerRescuDistractionClear();
            rescueDistractionFireOutside = true;

            rescueDistractionCost = rescueDistractionFireOutsideCost;
            GameMenu.SwitchToMenu("town_keep_dungeon_rescue_plan_hlc");
        }
        public static void PlanPrisonerRescueOptionBandits(MenuCallbackArgs args)
        {
            PrisonerRescuDistractionClear();
            rescueDistractionBandits = true;

            rescueDistractionCost = rescueDistractionBanditsCost;
            GameMenu.SwitchToMenu("town_keep_dungeon_rescue_plan_hlc");
        }
        public static void PlanPrisonerRescueOptionMercs(MenuCallbackArgs args)
        {
            PrisonerRescuDistractionClear();
            rescueDistractionMercs = true;

            rescueDistractionCost = Support.Random(2300, 3500);
            GameMenu.SwitchToMenu("town_keep_dungeon_rescue_plan_hlc");
        }
        public static void PlanPrisonerRescueOptionEntertainers(MenuCallbackArgs args)
        {
            PrisonerRescuDistractionClear();
            rescueDistractionEntertainers = true;

            rescueDistractionCost = rescueDistractionEntertainersCost;
            GameMenu.SwitchToMenu("town_keep_dungeon_rescue_plan_hlc");
        }

        /*** Plan Prisoner Rescue Approach Options ***/
        public static void PlanPrisonerRescueOptionStealth(MenuCallbackArgs args)
        {
            PrisonerRescuApproachClear();
            rescueApproachStealth = true;

            rescueApproachCost = rescueApproachStealthCost;
            GameMenu.SwitchToMenu("town_keep_dungeon_rescue_plan_hlc");
        }
        public static void PlanPrisonerRescueOptionApproachBribe(MenuCallbackArgs args)
        {
            PrisonerRescuApproachClear();
            rescueApproachBribe = true;

            rescueApproachCost = rescueApproachBribeCost;
            GameMenu.SwitchToMenu("town_keep_dungeon_rescue_plan_hlc");
        }
        public static void PlanPrisonerRescueOptionSoldiers(MenuCallbackArgs args)
        {
            PrisonerRescuApproachClear();
            rescueApproachSoldiers = true;

            rescueApproachCost = rescueApproachSoldiersCost;
            GameMenu.SwitchToMenu("town_keep_dungeon_rescue_plan_hlc");
        }
        public static void PlanPrisonerRescueOptionOfficers(MenuCallbackArgs args)
        {
            PrisonerRescuApproachClear();
            rescueApproachOfficers = true;

            rescueApproachCost = rescueApproachOfficersCost;
            GameMenu.SwitchToMenu("town_keep_dungeon_rescue_plan_hlc");
        }
        public static void PlanPrisonerRescueOptionBuilders(MenuCallbackArgs args)
        {
            PrisonerRescuApproachClear();
            rescueApproachBuilders = true;

            rescueApproachCost = rescueApproachBuildersCost;
            GameMenu.SwitchToMenu("town_keep_dungeon_rescue_plan_hlc");
        }
        public static void PlanPrisonerRescueOptionLatrine(MenuCallbackArgs args)
        {
            PrisonerRescuApproachClear();
            rescueApproachLatrine = true;

            rescueApproachCost = rescueApproachLatrineCost;
            GameMenu.SwitchToMenu("town_keep_dungeon_rescue_plan_hlc");
        }

        /*** Plan Prisoner Rescue Escape Options ***/
        public static void PlanPrisonerRescueOptionEscapeBribe(MenuCallbackArgs args)
        {
            PrisonerRescuEscapeClear();
            rescueEscapeBribe = true;

            rescueEscapeCost = rescueEscapeBribeCost;
            GameMenu.SwitchToMenu("town_keep_dungeon_rescue_plan_hlc");
        }
        public static void PlanPrisonerRescueOptionGoods(MenuCallbackArgs args)
        {
            PrisonerRescuEscapeClear();
            rescueEscapeGoods = true;

            rescueEscapeCost = rescueEscapeGoodsCost;
            GameMenu.SwitchToMenu("town_keep_dungeon_rescue_plan_hlc");
        }
        public static void PlanPrisonerRescueOptionVillagers(MenuCallbackArgs args)
        {
            PrisonerRescuEscapeClear();
            rescueEscapeVillagers = true;

            rescueEscapeCost = rescueEscapeVillagersCost;
            GameMenu.SwitchToMenu("town_keep_dungeon_rescue_plan_hlc");
        }
        public static void PlanPrisonerRescueOptionTraders(MenuCallbackArgs args)
        {
            PrisonerRescuEscapeClear();
            rescueEscapeTraders = true;

            rescueEscapeCost = rescueEscapeTradersCost;
            GameMenu.SwitchToMenu("town_keep_dungeon_rescue_plan_hlc");
        }
        public static void PlanPrisonerRescueOptionSearch(MenuCallbackArgs args)
        {
            PrisonerRescuEscapeClear();
            rescueEscapeSearch = true;

            rescueEscapeCost = rescueEscapeSearchCost;
            GameMenu.SwitchToMenu("town_keep_dungeon_rescue_plan_hlc");
        }
        public static void PlanPrisonerRescueOptionMessengers(MenuCallbackArgs args)
        {
            PrisonerRescuEscapeClear();
            rescueEscapeMessengers = true;

            rescueEscapeCost = rescueEscapeMessengersCost;
            GameMenu.SwitchToMenu("town_keep_dungeon_rescue_plan_hlc");
        }
        public static void PlanPrisonerRescueOptionWall(MenuCallbackArgs args)
        {
            PrisonerRescuEscapeClear();
            rescueEscapeWall = true;

            rescueEscapeCost = rescueEscapeWallCost;
            GameMenu.SwitchToMenu("town_keep_dungeon_rescue_plan_hlc");
        }

        /*** Prisoner Rescue Clear ***/
        public static void PrisonerRescuDistractionClear()
        {
            rescueDistractionBribe = false;
            rescueDistractionPoison = false;
            rescueDistractionFireInside = false;
            rescueDistractionFireOutside = false;
            rescueDistractionBandits = false;
            rescueDistractionMercs = false;
            rescueDistractionEntertainers = false;

            rescueDistractionCost = 0;
        }
        public static void PrisonerRescuApproachClear()
        {
            rescueApproachStealth = false;
            rescueApproachBribe = false;
            rescueApproachSoldiers = false;
            rescueApproachOfficers = false;
            rescueApproachBuilders = false;
            rescueApproachLatrine = false;

            rescueApproachCost = 0;
        }
        public static void PrisonerRescuEscapeClear()
        {
            rescueEscapeBribe = false;
            rescueEscapeGoods = false;
            rescueEscapeVillagers = false;
            rescueEscapeTraders = false;
            rescueEscapeSearch = false;
            rescueEscapeMessengers = false;
            rescueEscapeWall = false;

            rescueEscapeCost = 0;
        }

        /*** Prisoner Rescue Clear ***/

        public static void PrisonerRescueClear()
        {
            rescueChance = 0;
            rescueChanceAssumption = 0;

            rescueSecurity = 0;
            rescueCost = 0;

            PrisonerRescuDistractionClear();
            PrisonerRescuApproachClear();
            PrisonerRescuEscapeClear();

            rescueEscapeSuccess = false;
            rescueEscapePostSuccess = false;
        }

        /*** Start Prisoner Rescue ***/
        public static void StartPrisonerRescue(MenuCallbackArgs args)
        {
            settlement = Support.GetCurrentSettlement();

            if (settlement != null)
            {
                PrisonLocationClear();
                prisoners = settlement.GetComponent<SettlementComponent>().GetPrisonerHeroes();

                PrisonerRescueClear();

                args.MenuTitle = new TextObject("{=x04UGQDn}Prisoners");
                string text = "There are no notable prisoners in this " + (settlement.IsCastle ? "castle" : "town") + ".";

                if (prisoners != null)
                {
                    if (prisoners.Count > 0)
                    {
                        if (prisoners.Count == 1)
                        {
                            if (prisoners[0] != null)
                            {
                                if (prisoners[0].HeroObject != null && prisoners[0].IsHero)
                                    text = prisoners[0].Name.ToString() + " is the only notable character imprisoned in this " + (settlement.IsCastle ? "castle" : "town") + ".";
                            }
                        }

                        if (prisoners.Count > 1)
                            text = "There are " + prisoners.Count.ToString() + " notable characters that can be rescued from this " + (settlement.IsCastle ? "castle" : "town") + ".";
                    }
                }

                MBTextManager.SetTextVariable("HLC_PRISONER_RESCUE", text);
            }
        }

        /*** Plan Prisoner Rescue ***/
        public static void PlanPrisonerRescue(MenuCallbackArgs args)
        {
            settlement = Support.GetCurrentSettlement();

            if (settlement != null)
            {
                PrisonLocationClear();
                args.MenuTitle = new TextObject("{=x04UGQDn}Plan Rescue");

                int intelligence = Hero.MainHero.GetAttributeValue(CharacterAttributesEnum.Intelligence);
                int roguery = Hero.MainHero.GetSkillValue(DefaultSkills.Roguery);
                int subIntelligence = 0;
                int subRoguery = 0;

                Hero companion = null;

                for(int i=0; i < Hero.MainHero.CompanionsInParty.Count(); i++)
                {
                    companion = Hero.MainHero.CompanionsInParty.ElementAt<Hero>(i);

                    if (companion  != null)
                    {
                        if(companion.IsActive && !companion.IsWounded)
                        {
                            subRoguery = companion.GetSkillValue(DefaultSkills.Roguery);
                            subIntelligence = companion.GetAttributeValue(CharacterAttributesEnum.Intelligence);

                            if (subRoguery > roguery)
                                roguery = subRoguery;

                            if (subIntelligence > intelligence)
                                intelligence = subIntelligence;
                        }
                    }
                }


                string chance = "low";

                rescueChance = (roguery / 300d) * (settlement.IsCastle ? 0.4d : 0.65);
                rescueChance -= Support.escapeCounter / 200f;

                double rescueChanceVariable = 0d;

                double securityDeficit = 0d;
                double guarrisonOverflow = 0d;
                double securityLevel1 = 0d;
                double securityLevel2 = 0d;
                double securityLevel3 = 0d;

                if (settlement.Town.Security > 0)
                    securityDeficit = 1 - (((settlement.Town.GarrisonParty.Party.NumberOfHealthyMembers / 10) * 2) / settlement.Town.Security);

                guarrisonOverflow = settlement.Town.GarrisonParty.Party.NumberOfHealthyMembers / 200d;
                securityLevel1 = settlement.Town.Security / 40d;
                securityLevel2 = settlement.Town.Security / 55d;
                securityLevel3 = settlement.Town.Security / 80d;

                if (securityDeficit < 0.1d)
                    securityDeficit = 0.1d;
                if (guarrisonOverflow < 0.15d)
                    guarrisonOverflow = 0.15d;
                if (securityLevel1 < 0.11d)
                    securityLevel1 = 0.11d;
                if (securityLevel2 < 0.09d)
                    securityLevel2 = 0.09d;
                if (securityLevel3 < 0.08d)
                    securityLevel3 = 0.08d;

                // Distraction //

                if (rescueDistractionBribe)
                {
                    rescueChanceVariable += 0.1d * securityDeficit;

                    if (Support.PlayerPartyHasPerk(DefaultPerks.Roguery.TwoFaced))
                        rescueChanceVariable += 0.1d * securityDeficit;
                }
                else if (rescueDistractionPoison)
                    rescueChanceVariable += (0.08d * guarrisonOverflow) + (0.009 * securityDeficit);
                else if (rescueDistractionFireInside)
                    rescueChanceVariable += (0.06d * securityLevel1) + (0.006 * securityDeficit);
                else if (rescueDistractionFireOutside)
                    rescueChanceVariable += (0.09d * securityLevel2) + (0.004 * securityDeficit);
                else if (rescueDistractionBandits)
                    rescueChanceVariable += (0.11d * securityDeficit) - (0.011 * securityLevel2);
                else if (rescueDistractionMercs)
                    rescueChanceVariable += 0.15d * securityLevel3;
                else if (rescueDistractionEntertainers)
                    rescueChanceVariable += (0.04d * securityDeficit) - (0.025 * securityLevel3);

                // Approach //

                if (rescueApproachStealth)
                    rescueChanceVariable += (0.04d * securityDeficit) - (0.01 * securityLevel1);
                else if (rescueApproachBribe)
                {
                    rescueChanceVariable += 0.075d * securityDeficit;

                    if (Support.PlayerPartyHasPerk(DefaultPerks.Roguery.TwoFaced))
                        rescueChanceVariable += 0.075d * securityDeficit;
                }
                else if (rescueApproachSoldiers)
                    rescueChanceVariable += (0.08d * securityLevel2) + (0.015 * securityDeficit);
                else if (rescueApproachOfficers)
                    rescueChanceVariable += 0.14d * securityLevel3;
                else if (rescueApproachBuilders)
                    rescueChanceVariable += (0.06d * guarrisonOverflow) - (0.02 * securityLevel2);
                else if (rescueApproachLatrine)
                    rescueChanceVariable += (0.05d * securityLevel1) + (0.012 * securityDeficit);

                // Escape //

                if (rescueEscapeBribe)
                {
                    rescueChanceVariable += 0.05d * securityDeficit;

                    if (Support.PlayerPartyHasPerk(DefaultPerks.Roguery.TwoFaced))
                        rescueChanceVariable += 0.05d * securityDeficit;
                }
                else if (rescueEscapeGoods)
                    rescueChanceVariable += 0.06d * securityLevel1 + (0.005 * securityDeficit);
                else if (rescueEscapeVillagers)
                    rescueChanceVariable += (0.05d * guarrisonOverflow) - (0.03 * securityLevel1);
                else if (rescueEscapeTraders)
                    rescueChanceVariable += (0.09d * securityLevel2) + (0.01 * securityDeficit);
                else if (rescueEscapeSearch)
                    rescueChanceVariable += 0.07d * securityDeficit - (0.02 * securityLevel3);
                else if (rescueEscapeMessengers)
                    rescueChanceVariable += 0.013d * securityLevel3;
                else if (rescueEscapeWall)
                    rescueChanceVariable += (0.05d * securityDeficit) - (0.02 * securityLevel2);

                // Calculations //

                rescueChance += rescueChanceVariable;

                if (rescueChance > 1)
                    rescueChance = 1;
                else if (rescueChance < 0.05d)
                    rescueChance = 0.05d;

                int assumptionOffsert = ((11 - intelligence) * 2);
                rescueChanceAssumption = (int)(rescueChance * 100) + Support.Random(-assumptionOffsert, assumptionOffsert);

                if (rescueChanceAssumption < 1)
                    rescueChanceAssumption = 1;
                else if (rescueChanceAssumption > 100)
                    rescueChanceAssumption = 100;

                if (intelligence < 6)
                {
                    if (rescueChanceAssumption < 20)
                        chance = "fairly low";
                    else if (rescueChanceAssumption < 40)
                        chance = "low";
                    else if (rescueChanceAssumption < 60)
                        chance = "slim";
                    else if (rescueChanceAssumption < 60)
                        chance = "decent";
                    else if (rescueChanceAssumption < 80)
                        chance = "good";
                    else
                        chance = "exceptional";
                }
                else
                    chance = rescueChanceAssumption.ToString() + "%";

                string text = "Currently you believe the chance of success for this plan is " + chance + " .\n \n";
                string distractionText = "There is no distraction in place, you will be facing the full force of the guards.";
                string approachText = "A direct approach will be taken, breaking into the dungeon by force.";
                string escapeText = "Once the deed is done you'll fight your way out if you have to.";

                if (rescueDistractionBribe)
                    distractionText = "Some of the guards have been bribed to leave their posts unattended, or distract others.";
                else if (rescueDistractionPoison)
                    distractionText = "A light poison will be used to render some unlucky guardsmen too ill to man their posts.";
                else if (rescueDistractionFireInside)
                    distractionText = "A fire will be started in town shortly before the operation begins, likely drawing away guards to assist.";
                else if (rescueDistractionFireOutside)
                    distractionText = "Lighting a large bonfire nearby should have guards dispatched to investigate the smoke.";
                else if (rescueDistractionBandits)
                    distractionText = "A group of bandits will fire arrows at the walls, forcing the guards to man the walls in preparation for an assault.";
                else if (rescueDistractionMercs)
                    distractionText = "Hired mercenaries will start fights with guards and harass them in town, drawing away some of their attention.";
                else if (rescueDistractionEntertainers)
                    distractionText = "A paid band of entertainers and ladies of the night will distract the guardsmen.";

                if (rescueApproachStealth)
                    approachText = "Stealth will be used in the approach, trying to stay out of sight.";
                else if (rescueApproachBribe)
                    approachText = "A few cooperative soldiers will escort us to the cells.";
                else if (rescueApproachSoldiers)
                    approachText = "Disguised in soldier uniforms, any infiltrators will blend in with the rest of the prison guards.";
                else if (rescueApproachOfficers)
                    approachText = "Approaching with appearance and authority of officers guarantees prison guards will neither question nor resist.";
                else if (rescueApproachBuilders)
                    approachText = "Dungeon cells are often structures of less quality, peasants with hammers are not an uncommon sight.";
                else if (rescueApproachLatrine)
                    approachText = "Using latrine barrels to enter a dungeon is unexpected, effective and quite disgusting.";

                if (rescueEscapeBribe)
                    escapeText = "As a last step, the gate guards have been paid well to fail closing the gate in-time.";
                else if (rescueEscapeGoods)
                    escapeText = "As for the escape, well... Hiding among the goods of a leaving cart is cheap, though risky.";
                else if (rescueEscapeVillagers)
                    escapeText = "For making an exist however, apparent harmless villagers and vagabonds are unlikely to draw attention during an escape event.";
                else if (rescueEscapeTraders)
                    escapeText = "Now, for the last piece of the puzzle. Traders have smoothed access in and out of cities, they are ideal for smuggling fugitives, albeit expensive.";
                else if (rescueEscapeSearch)
                    escapeText = "So how to get away then? Simple, who would possibly expect fugitives to be hiding among their own search parties.";
                else if (rescueEscapeMessengers)
                    escapeText = "As for making an exist, well, during an escape messengers are dispatched far and wide to inform patrols, such a perfect disguise.";
                else if (rescueEscapeWall)
                    escapeText = "Now the question is, how to actually leave. Scaling down the walls with rope can be dangerous, but also quite effective.";

                text += distractionText + " ";
                text += approachText + " ";
                text += escapeText + "\n \n";

                rescueCost = rescueDistractionCost + rescueApproachCost + rescueEscapeCost;

                text += "The cost of executing this rescue will be " + rescueCost + " {GOLD_ICON}";

                MBTextManager.SetTextVariable("HLC_PRISONER_RESCUE_DETAILS", text);
            }
        }

        /*** Plan Prisoner Rescue Distraction Start ***/
        public static void PlanPrisonerRescueDistractionStart(MenuCallbackArgs args)
        {
            settlement = Support.GetCurrentSettlement();

            if (settlement != null)
            {
                PrisonLocationClear();
                args.MenuTitle = new TextObject("{=x04UGQDn}Distraction");
                string text = "A good distraction can reduce the number of guards and significantly impact the chance of success.\n \nWhat method will you use to pull away the soldiers?";

                MBTextManager.SetTextVariable("HLC_PRISONER_RESCUE_DETAILS", text);
            }
        }

        /*** Plan Prisoner Rescue Approach Start ***/
        public static void PlanPrisonerRescueApproachStart(MenuCallbackArgs args)
        {
            settlement = Support.GetCurrentSettlement();

            if (settlement != null)
            {
                PrisonLocationClear();
                args.MenuTitle = new TextObject("{=x04UGQDn}Approach");
                string text = "Deciding how to approach the rescue is possibly the most important part of the mission.\n \nSo how will you approach the cells?";

                MBTextManager.SetTextVariable("HLC_PRISONER_RESCUE_DETAILS", text);
            }
        }

        /*** Plan Prisoner Rescue Escape Start ***/
        public static void PlanPrisonerRescueEscapeStart(MenuCallbackArgs args)
        {
            settlement = Support.GetCurrentSettlement();

            if (settlement != null)
            {
                PrisonLocationClear();
                args.MenuTitle = new TextObject("{=x04UGQDn}Escape");
                string text = "Grabbing the prisoners means little, if you get caught during the escape.\n \nHow will you be evading capture?";

                MBTextManager.SetTextVariable("HLC_PRISONER_RESCUE_DETAILS", text);
            }
        }

        /******************/
        /* Execute Escape */
        /******************/

        public static bool ExecuteEscapeWait(MenuCallbackArgs args)
        {
	        args.MenuContext.GameMenu.StartWait();//.AllowWaitingAutomatically();
            rescueExecutionTime = CampaignTime.HoursFromNow(rescueExecutionHours);
            rescueExecutionStartTime = rescueExecutionTime.RemainingSecondsFromNow;
            GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, (Hero)null, rescueCost, false);

            return true; 
        }

        public static void ExecuteEscapeComplete(MenuCallbackArgs args)
        {
            prisoners = settlement.GetComponent<SettlementComponent>().GetPrisonerHeroes();
            GameMenu.SwitchToMenu("town_keep_dungeon_rescue_execute_escape_result_hlc");
        }

        public static void ExecuteEscapeTick(MenuCallbackArgs args, CampaignTime dt)
        {
            float timeDifferent = rescueExecutionTime.RemainingSecondsFromNow / rescueExecutionStartTime;
            if (timeDifferent < 0)
                timeDifferent = 0;
            timeDifferent = 1f - timeDifferent;

            args.MenuContext.GameMenu.SetProgressOfWaitingInMenu(timeDifferent);
        }

        /*** Plan Prisoner Rescue Result ***/
        public static void PlanPrisonerRescueResult(MenuCallbackArgs args)
        {
            settlement = Support.GetCurrentSettlement();

            if (settlement != null)
            {
                PrisonLocationClear();
                args.MenuTitle = new TextObject("{=x04UGQDn}Results");

                string text = "The end result";
                string subText = "Leave";

                Hero owner = null;

                if(settlement.Town != null)
                {
                    if (settlement.Town.Owner != null)
                        owner = Support.FindHero(settlement.Town.Owner);
                }

                rescueEscapeSuccess = rescueEscapePostSuccess;

                if (prisoners.Count > 0)
                {
                    if (!rescueEscapeSuccess)
                    {
                        if ((Support.Random(0, 1000) / 1000f) <= rescueChance)
                            rescueEscapeSuccess = true;
                    }

                    bool exposed = false;
                    bool severeCrime = false;
                    bool treason = false;

                    if (rescueEscapeSuccess)
                    {
                        text = "The escape plan was a success! Everything went as planned with minor deviations. " + (prisoners.Count > 1 ? "The prisoners have" : prisoners[0].Name.ToString() + " has") + " been sworn to secrecy. ";

                        if (!rescueEscapePostSuccess)
                        {
                            for (int i = 0; i < prisoners.Count; i++)
                            {
	                            var roster = new FlattenedTroopRoster(1);
	                            roster.Add(prisoners[i]);
                                EndCaptivityAction.ApplyByReleasedFromPartyScreen(roster);

                                Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Honor, -5);
                                Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Mercy, 30);
                                Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 50);
                                Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 150);

                                RescueHero(prisoners[i].HeroObject, true);
                            }
                        }

                        subText = "Best to leave this place soon";
                        rescueEscapePostSuccess = true;
                    }
                    else
                    {
                        if (Support.settings.failed_rescue_consequences)
                        {
                            if (Support.Random(1, 10) == 1)
                            {
                                bool sameFaction = (Hero.MainHero.MapFaction == settlement.MapFaction);
                                bool atWar = (FactionManager.IsAtWarAgainstFaction(settlement.MapFaction, Hero.MainHero.MapFaction));
                                
                                if (sameFaction)
                                {
                                    if (Hero.MainHero.Clan.Kingdom != null)
                                    {
                                        Kingdom kingdom = Hero.MainHero.Clan.Kingdom;

                                        if (kingdom.Leader != null)
                                        {
                                            if (kingdom.Leader.IsAlive)
                                            {
                                                foreach (Settlement settlement in new List<Settlement>(Hero.MainHero.Clan.Settlements))
                                                { ChangeOwnerOfSettlementAction.ApplyByLeaveFaction(kingdom.Leader, settlement); }
                                            }
                                        }

                                        ChangeKingdomAction.ApplyByLeaveKingdomAsMercenaryWithKingDecision(Hero.MainHero.Clan, Hero.MainHero.Clan.Kingdom, false);
                                        treason = true;
                                    }
                                }

                                if (sameFaction || !atWar)
                                {
                                    FactionManager.DeclareWar(settlement.MapFaction, Hero.MainHero.MapFaction);
                                    ChangeCrimeRatingAction.Apply(Settlement.CurrentSettlement.MapFaction, Campaign.Current.Models.CrimeModel.GetCrimeRatingOf(CrimeModel.CrimeType.RaidVillage), true);

                                    severeCrime = true;
                                }

                                PartyBase.MainParty.AddElementToMemberRoster(CharacterObject.PlayerCharacter, -1, true);
                                TakePrisonerAction.Apply(Settlement.CurrentSettlement.Party, Hero.MainHero);


                                exposed = true;
                                GameMenu.SwitchToMenu("taken_prisoner");
                            }
                        }

                        text = "The plan has failed, ";

                        switch (Support.Random(1, 8))
                        {
                            case 1:
                                text += "the prison cells could not be reached, cover was blown during the approach. ";
                                break;

                            case 2:
                                text += "watchmen recognized the " + (prisoners.Count > 1 ? "prisoners" : "prisoner") + " during the escape. " + (prisoners.Count > 1 ? "They" : prisoners[0].Name.ToString()) + " had to be abandoned or risk exposing our involvement. ";
                                break;

                            case 3:
                                text += "the prison guards resisted long enough for reinforcements to arrive. Pressing on would have meant death or capture. ";
                                break;

                            case 4:
                                text += "the " + (prisoners.Count > 1 ? "prisoners were" : "prisoner was") + " too sick to move. " + (prisoners.Count > 1 ? "They" : prisoners[0].Name.ToString()) + " couldn't make it to the exit before more guards arrived. ";
                                break;

                            case 5:
                                text += "guards captured " + (prisoners.Count > 1 ? "the prisoners" : prisoners[0].Name.ToString()) + " during the escape. Fighting them would have been suicidal. ";
                                break;

                            case 6:
                                text += (prisoners.Count > 1 ? "the prisoners were" : prisoners[0].Name.ToString() + " was") + " erratic. " + (prisoners.Count > 1 ? "They" : (prisoners[0].IsFemale ? "She" : "He")) + " refused help, likely confused or disoriented. ";
                                break;

                            case 7:
                                text += (prisoners.Count > 1 ? "the prisoners were" : prisoners[0].Name.ToString() + " was") + " wounded during the escape. Could not be recovered in time. ";
                                break;

                            default:
                                text += " the prison cells were never reached. Guardsmen attempted to intercept us prior to arrival, and failed. ";
                                break;
                        }

                        subText = "We can try again another time";
                    }

                    if (owner != null)
                    {
                        if (Support.Random(1, 15) == 1 || exposed)
                        {
                            if (severeCrime)
                            {
                                Support.ChangeRelation(Hero.MainHero, owner, Support.Random(-8, -4));
                                Support.ChangeFamilyRelation(Hero.MainHero, owner, Support.Random(-1, 0));
                                Support.ChangeFactionRelation(Hero.MainHero, owner, -4, 0);
                            }
                            else
                            {
                                Support.ChangeRelation(Hero.MainHero, owner, Support.Random(-3, -1));
                                Support.ChangeFactionRelation(Hero.MainHero, owner, -1, 0);
                            }

                            if(treason)
                                Support.ChangeFactionRelation(Hero.MainHero, owner, -6, -2);

                            text += "However, while we were not exposed, " + owner.Name.ToString() + " suspects our involvement.";

                            exposed = true;
                        }
                    }

                    if (!exposed)
                        text += "Our involvement was not directly revealed.";

                }
                else
                    text = "It seems all the prisoners have already been moved... This was quite a waste of time.";

                Support.escapeCounter += 50;

                MBTextManager.SetTextVariable("HLC_PRISONER_RESCUE_DETAILS", text);
                MBTextManager.SetTextVariable("HLC_CHOICE1", subText);
            }
        }

        /*** Plan Prisoner Rescue Result Leave ***/
        public static bool PlanPrisonerRescueExecuteReturn(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.Leave;
            return true;
        }
        public static void PlanPrisonerRescueExecuteLeave(MenuCallbackArgs args)
        { PlayerEncounter.Finish(true); }
    }
}
