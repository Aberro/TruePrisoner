using HarmonyLib;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using System;
using System.Xml.Serialization;
using System.IO;
using MCM.Abstractions.FluentBuilder;
using MCM.Abstractions.Ref;
using TaleWorlds.Core;

namespace TruePrisoners
{
    public class SubModule : MBSubModuleBase
    {
        /*** On Launch ***/
        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();
            new Harmony("HLC.TruePrisoners").PatchAll();
            Support.LogMessage("True Prisoners Loaded");

            try
            {
                using (Stream stream = (Stream)new FileStream(Path.Combine(BasePath.Name, "Modules", "TruePrisoners", "Settings.xml"), FileMode.Open))
                    Support.settings = (Settings)new XmlSerializer(typeof(Settings)).Deserialize(stream);
            }
            catch (Exception EX)
            {
                Support.LogMessage("True Prisoners: Could not read setting, using default values!");
                Support.settings = new Settings();
            }
        }

        /*** On Load ***/
        public override void OnGameInitializationFinished(Game game)
        { 
            Support.escapeCounter = 0;
            PrisonerInteraction.PrisonerRescueClear();
        }
    }
}
