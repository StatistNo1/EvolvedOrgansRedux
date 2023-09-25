namespace EvolvedOrgansRedux {
    public class Settings : Verse.ModSettings {
        public bool BodyPartAffinity;
        public string ChosenWorkbench;
        public bool ImportantMessage20320905;
        public int AmountOfArms;
        public System.Collections.Generic.List<string> ChoicesOfWorkbenches = new() { "Evolved Organs Redux" };
        //public System.Collections.Generic.List<int> ChoicesForAmountOfArms = new (){ 2, 4, 6, 8 };
        public override void ExposeData() {
            Verse.Scribe_Values.Look(ref BodyPartAffinity, "BodyPartAffinity");
            Verse.Scribe_Values.Look(ref ChosenWorkbench, "ChosenWorkbench");
            Verse.Scribe_Values.Look(ref ImportantMessage20320905, "RemovedSetting-CombatibilitySwitchEORVersionMidSave", false, true);
            //Verse.Scribe_Values.Look(ref AmountOfArms, "AmountOfArms", defaultValue: 4);
            base.ExposeData();
        }
        public Settings() {
            if (ChoicesOfWorkbenches.Contains(ChosenWorkbench))
                ChosenWorkbench = ChoicesOfWorkbenches[0];
        }
    }

    public class EvolvedOrgansReduxSettings : Verse.Mod {
        readonly Settings settings;
        public EvolvedOrgansReduxSettings(Verse.ModContentPack content) : base(content) {
            this.settings = GetSettings<Settings>();
        }
        public override void DoSettingsWindowContents(UnityEngine.Rect inRect) {
            Verse.Listing_Standard listingStandard = new Verse.Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Label("Check this option if you want to put archotech arms on lower shoulders or additional non-Evor lungs into your body.\nThe game has to be restarted for this change to take effect.");
            listingStandard.Gap();
            listingStandard.CheckboxLabeled("BodyPartAffinity", ref settings.BodyPartAffinity);
            listingStandard.GapLine();
            listingStandard.Label("Check of which mod the organ workbench should be used to create EvolvedOrgansRedux implants.\nThe game has to be restarted for this change to take effect.");
            listingStandard.Gap();
            for (int i = 0; i < settings.ChoicesOfWorkbenches.Count; i++) {
                if (listingStandard.RadioButton(settings.ChoicesOfWorkbenches[i], settings.ChosenWorkbench == settings.ChoicesOfWorkbenches[i], tabIn: 30f)) {
                    settings.ChosenWorkbench = settings.ChoicesOfWorkbenches[i];
                }
            }
            //listingStandard.GapLine();
            //listingStandard.Label("Choose up to how many arms your pawns should have.");
            //listingStandard.Gap();
            //for (int i = 0; i < settings.ChoicesForAmountOfArms.Count; i++) {
            //    if (listingStandard.RadioButton(settings.ChoicesForAmountOfArms[i].ToString(), settings.AmountOfArms == settings.ChoicesForAmountOfArms[i], tabIn: 30f)) {
            //        settings.AmountOfArms = settings.ChoicesForAmountOfArms[i];
            //    }
            //}
            listingStandard.End();
        }
        public override string SettingsCategory() {
            return "EvolvedOrgansRedux";
        }
    }
}