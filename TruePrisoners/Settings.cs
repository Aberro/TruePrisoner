
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Base.Global;
using System;
using System.Collections.Generic;

namespace TruePrisoners
{
    public class Settings : AttributeGlobalSettings<Settings>
    {
	    private const string RansomGroup = "Ransom options";
        private const string ProbabilityGroup = "Escape probabilities";

        public override string Id => nameof(TruePrisoners);
        public override string DisplayName => "True Prisoners";
        public override string FormatType => "xml";
		public override string FolderName => "TruePrisoners";

		[SettingPropertyGroup(ProbabilityGroup)]
        [SettingPropertyFloatingInteger("Basic escape chance (100% - vanilla)", 0, 2, valueFormat: "#0%", HintText = "This is modifier value, meaning that 100% is unchanged vanilla probability, 200% is twice as high escape chance as vanilla", RequireRestart = false)]
        public float general_chance { get; set; }

        [SettingPropertyGroup(ProbabilityGroup)]
        [SettingPropertyFloatingInteger("Escape from party (100% - vanilla)", 0, 2, valueFormat: "#0%", HintText = "This is modifier value, meaning that 100% is unchanged vanilla probability, 200% is twice as high escape chance as vanilla", RequireRestart = false)]
        public float traveling_chance { get; set; }

        [SettingPropertyGroup(ProbabilityGroup)]
        [SettingPropertyFloatingInteger("Escape from settlements (100% - vanilla)", 0, 2, valueFormat: "#0%", HintText = "This is modifier value, meaning that 100% is unchanged vanilla probability, 200% is twice as high escape chance as vanilla", RequireRestart = false)]
        public float settlement_chance { get; set; }

        [SettingPropertyGroup(ProbabilityGroup)]
        [SettingPropertyBool("Apply to AI", HintText = "Does probability modifiers applied to non-player parties.", RequireRestart = false)]
        public bool ai_applied { get; set; }

        [SettingPropertyGroup(RansomGroup)]
        [SettingPropertyInteger("Minimum days before escape is possible", 0, 240, RequireRestart = false)]
        public int minimum_days_before_escape_ransom { get; set; }

        [SettingPropertyGroup(RansomGroup)]
        [SettingPropertyInteger("Relation penalty for keeping lord imprisoned", -100, 100, HintText = "Negative value means that relation increase", RequireRestart = false)]
        public int daily_penalty { get; set; }

        [SettingPropertyGroup(RansomGroup)]
        [SettingPropertyBool("Allow ransom requests", RequireRestart = false)]
        public bool allow_ransoms { get; set; }

        [SettingPropertyGroup(RansomGroup)]
        [SettingPropertyBool("Significantly increase ransom value", RequireRestart = false)]
        public bool significant_ransoms { get; set; }

        [SettingPropertyGroup(RansomGroup)]
        [SettingPropertyBool("Failed rescue has consequences", HintText = "If player fails rescue attempt, it's considered as crime agains captor faction.", RequireRestart = false)]
        public bool failed_rescue_consequences { get; set; }

        public Settings()
        {
            this.traveling_chance = 1;
            this.settlement_chance = 1;
            this.general_chance = 1;

            this.minimum_days_before_escape_ransom = 5;
            this.daily_penalty = 0;

            this.allow_ransoms = true;
            this.significant_ransoms = false;
            this.ai_applied = false;

            this.failed_rescue_consequences = false;
        }
	}
}
