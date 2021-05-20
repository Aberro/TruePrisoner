using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TruePrisoners
{
    public static class Support
    {
        /*** Variables ***/

        public static Settings settings = new Settings(); 
        public static Random random = new Random();
        public static int escapeCounter = 0;

        /***************/
        /*** RANSOMS ***/
        /***************/

        /*** Calculate Ransom ***/
        public static int CalculateRansom(CharacterObject character, bool randomize = true)
        {
            int value = 10;

            if (character != null)
            {
                value = character.Level * 3 + character.Tier * 22;
                int wealthPoint = 120;

                if (character.HeroObject != null)
                {
                    if (settings.significant_ransoms)
                    {
                        /* Custom Formula */

                        if (character.HeroObject.Clan != null)
                        {
                            value += character.HeroObject.Gold;

                            while (value > 1300)
                            { value -= 1000; }

                            if (character.HeroObject.Clan.Kingdom != null)
                            {
                                Town settlement;

                                for (int i = 0; i < character.HeroObject.Clan.Kingdom.Fiefs.Count(); i++)
                                {
                                    settlement = character.HeroObject.Clan.Kingdom.Fiefs.ElementAt<Town>(i);

                                    if (settlement.IsTown)
                                        wealthPoint += 90;
                                    else
                                        wealthPoint += 50;
                                }
                            }

                            if (randomize)
                            {
                                for (int i = 0; i < (character.HeroObject.Clan.Tier + 1); i++)
                                { value += Random(wealthPoint - 100, wealthPoint + 100); }
                            }
                            else
                            {
                                for (int i = 0; i <= character.HeroObject.Clan.Tier; i++)
                                { value += wealthPoint + 53; }
                            }

                            if (character.HeroObject.Clan.Leader == character.HeroObject)
                                value = (int)(value * 1.5f);

                            if (character.HeroObject.IsNoble)
                                value = (int)(value * 1.25f);
                        }
                    }
                    else
                    {
                        /* Native Formula */

                        int troopRecruitmentCost = Campaign.Current.Models.PartyWageModel.GetTroopRecruitmentCost(character, (Hero)null, false);
                        float num1 = 0.0f;
                        float num2 = 0.0f;
                        float num3 = 1f;
                        if (character.HeroObject?.Clan != null)
                        {
                            num1 = (float)((character.HeroObject.Clan.Tier + 2) * 200) * (character.HeroObject.Clan.Leader == character.HeroObject ? 2f : 1f);
                            num2 = (float)Math.Sqrt((double)Math.Max(0, character.HeroObject.Gold)) * 6f;
                            if (character.HeroObject.Clan.Kingdom != null)
                            {
                                int num4 = character.HeroObject.Clan.Kingdom.Fiefs.Count<Town>();
                                num3 = character.HeroObject.Clan.IsKingdomFaction ? (num4 < 8 ? (float)(((double)num4 + 1.0) / 9.0) : (float)(1.0 + Math.Sqrt((double)(num4 - 8)) * 0.100000001490116)) : 1f;
                            }
                            else
                                num3 = 0.5f;
                        }
                        float num5 = character.HeroObject != null ? num1 + num2 : 0.0f;
                        int num6 = (int)(((double)troopRecruitmentCost + (double)num5) * (!character.IsHero ? 0.25 : 1.0) * (double)num3);
                        value = (num6 != 0 ? num6 : 1);
                    }

                    if (character.HeroObject.Clan != null)
                    {
                        if (character.HeroObject.IsPlayerCompanion)
                            value += 1000;
                        else
                        {
                            if (character.HeroObject.Clan == Clan.PlayerClan)
                                value += 2000;
                        }
                    }
                }
            }

            return value;
        }

        /*** Calculate Player to Hero Ransom ***/
        public static int CalculatePlayerToHeroRansom(int ransom)
        {
            float handicap = 1 - ((float)Hero.MainHero.GetSkillValue(DefaultSkills.Charm) / 300f);
            if (handicap < 0)
                handicap = 0;

           return ransom + (int)(ransom * 2 * handicap);
        }

        /*****************/
        /*** RELATIONS ***/
        /*****************/

        /*** Change Relation ***/
        public static void ChangeRelation(Hero lord1, Hero lord2, int change)
        {
            if (lord1 != null && lord2 != null)
            {
                int relation = CharacterRelationManager.GetHeroRelation(lord1, lord2) + change;

                if (relation > 100)
                    relation = 100;
                else
                {
                    if (relation < -100)
                        relation = -100;
                }

                CharacterRelationManager.SetHeroRelation(lord1, lord2, relation);
            }
        }

        /*** Change Family Relation ***/
        public static void ChangeFamilyRelation(Hero lord1, Hero lord2, int change, int offset = 0)
        {
            List<Hero> familyMembers = FindFamily(lord2);

            for (int i = 0; i < familyMembers.Count; i++)
            { ChangeRelation(lord1, familyMembers[i], Random(change, change + offset)); }
        }

        /*** Change Faction Relation ***/
        public static void ChangeFactionRelation(Hero lord1, Hero lord2, int change, int offset = 0)
        {
            List<Hero> factionMembers = FindFaction(lord2);

            for (int i = 0; i < factionMembers.Count; i++)
            { ChangeRelation(lord1, factionMembers[i], Random(change, change + offset)); }
        }

        /**********************************/
        /*** HEROES, FAMILY and FACTION ***/
        /**********************************/

        /*** Find Hero ***/
        public static Hero FindHero(PartyBase party)
        {
            bool found = false;
            Hero leader = null;

            if (party.Owner != null)
            {
                leader = party.Owner;
                found = true;
            }

            if (!found)
            {
                if (party.Leader != null)
                {
                    if (party.Leader.HeroObject != null)
                    {
                        leader = party.Leader.HeroObject;
                        found = true;
                    }
                }
            }

            if (!found)
            {
                if (party.LeaderHero != null)
                    leader = party.LeaderHero;
            }

            return leader;
        }

        /*** Find Family ***/
        public static List<Hero> FindFamily(Hero hero)
        {
            List<Hero> familyMembers = new List<Hero>();

            if(hero != null)
            {
                Hero familyMember;

                for(int i=0; i < hero.Children.Count; i++)
                {
                    familyMember = hero.Children[i];

                    if(familyMember != null)
                    {
                        if(familyMember.IsAlive)
                            familyMembers.Add(familyMember);
                    }
                }

                for (int i = 0; i < hero.Siblings.Count(); i++)
                {
                    familyMember = hero.Siblings.ElementAt<Hero>(i);

                    if (familyMember != null)
                    {
                        if (familyMember.IsAlive)
                            familyMembers.Add(familyMember);
                    }
                }

                if(hero.Spouse != null)
                {
                    if(hero.Spouse.IsAlive)
                        familyMembers.Add(hero.Spouse);
                }

                if (hero.Father != null)
                {
                    if (hero.Father.IsAlive)
                        familyMembers.Add(hero.Father);
                }

                if (hero.Mother != null)
                {
                    if (hero.Mother.IsAlive)
                        familyMembers.Add(hero.Mother);
                }
            }

            return familyMembers;
        }

        /*** Find Faction ***/
        public static List<Hero> FindFaction(Hero hero)
        {
            List<Hero> factionMembers = new List<Hero>();

            if (hero != null)
            {
                Hero factionMember;

                if (hero.MapFaction != null)
                {
                    if (hero.MapFaction.Heroes != null)
                    {
                        for (int i = 0; i < hero.MapFaction.Heroes.Count(); i++)
                        {
                            factionMember = hero.MapFaction.Heroes.ElementAt(i);

                            if (factionMember != null)
                            {
                                if (factionMember.IsAlive)
                                    factionMembers.Add(factionMember);
                            }
                        }
                    }
                }
            }

            return factionMembers;
        }

        /*******************/
        /*** PERSONALITY ***/
        /*******************/

        /*** Evalue Personality ***/
        public static int EvaluatePersonality(CharacterTraits traits)
        {
            int result = 0;

            int honorableValue = 0;
            int rogueValue = 0;
            int logicalValue = 0;
            int kindValue = 0;

            if (traits.Honor > 0)
                honorableValue++;
            else if (traits.Honor < 0)
                logicalValue++;

            if (traits.Valor > 0)
                honorableValue++;
            else if (traits.Valor < 0)
                rogueValue++;

            if (traits.Generosity > 0)
                kindValue++;
            else if (traits.Generosity < 0)
                rogueValue++;

            if (traits.Mercy > 0)
                kindValue++;
            else if (traits.Mercy < 0)
                logicalValue++;

            if (traits.Calculating > 0)
                logicalValue++;
            else if (traits.Calculating < 0)
                honorableValue++;

            if (honorableValue > rogueValue && honorableValue > logicalValue && honorableValue > kindValue)
                result = 1;
            else if (rogueValue > honorableValue && rogueValue > logicalValue && rogueValue > kindValue)
                result = 2;
            else if (logicalValue > honorableValue && logicalValue > rogueValue && logicalValue > kindValue)
                result = 3;
            else
                result = 4;

            return result;
        }

        /*************/
        /*** PERKS ***/
        /*************/

        /*** Has Perk ***/
        public static bool HasPerk(CharacterObject character, PerkObject perk)
        {
            if (character != null && perk != null)
                return (character.GetPerkValue(perk));
            else
                return false;
        }

        /*** Character has Perk ***/
        public static bool CharacterHasPerk(BasicCharacterObject character, PerkObject perk)
        {
            return HasPerk((CharacterObject)character, perk);
        }

        /*** Player Party Has Perk ***/
        public static bool PlayerPartyHasPerk(PerkObject perk)
        {
            bool found = HasPerk(Hero.MainHero.CharacterObject, perk);
            int count = 0;
            int companionCount = Hero.MainHero.CompanionsInParty.Count();
            Hero companion;

            while (!found && count < companionCount)
            {
                companion = Hero.MainHero.CompanionsInParty.ElementAt<Hero>(count);

                if (companion != null)
                {
                    if (companion.CharacterObject != null)
                    {
                        if (HasPerk(companion.CharacterObject, perk))
                            found = true;
                    }
                }

                count++;
            }

            return found;
        }

        /***************/
        /*** GENERAL ***/
        /***************/

        /*** Get Current Settlement ***/
        public static Settlement GetCurrentSettlement()
        { return (Settlement.CurrentSettlement == null ? MobileParty.MainParty.CurrentSettlement : Settlement.CurrentSettlement); }

        /*** Random ***/
        public static int Random(int min = 0, int max = 1)
        {
            if(min > max)
            {
                int temp = max;
                max = min;
                min = temp;
            }

            return random.Next(min * 100, max * 100) / 100; 
        }
        /*** Chance ***/
        public static int Chance(int yes = 0, int no = 1)
        {
            if (random.Next(1, 100) <= 50)
                return yes;
            else
                return no;
        }

        /*** Message ***/
        public static void LogMessage(string message)
        { InformationManager.DisplayMessage(new InformationMessage(message)); }

        /*** Friendly Message ***/
        public static void LogFriendlyMessage(string message)
        { InformationManager.DisplayMessage(new InformationMessage(message, Color.FromUint(4282569842U))); }
    }
}
