namespace EvolvedOrgansRedux {
    [Verse.StaticConstructorOnStartup]
    public static class Initializer {
        static Initializer() {
            try {
                new AddDefsForEachModdedRace();
            } catch (System.Exception e) {
                string errorMessage = "EvolvedOrgansRedux Error: Step two";
                errorMessage += "\n" + e;
                Verse.Log.Error(errorMessage);
            }
            try {
                if (Verse.LoadedModManager.GetMod<EvolvedOrgansReduxSettings>().GetSettings<Settings>().BodyPartAffinity) {
                    new BodyPartAffinity();
                }
            } catch (System.Exception e) {
                string errorMessage = "EvolvedOrgansRedux Error: Step three";
                errorMessage += "\n" + e;
                Verse.Log.Error(errorMessage);
            }
            Singleton.Instance.AddBodyPartTagDefToList(RimWorld.BodyPartTagDefOf.ManipulationLimbCore);
            Singleton.Instance.AddBodyPartTagDefToList(RimWorld.BodyPartTagDefOf.MovingLimbCore);
        }
    }
    public class GameComponent : Verse.GameComponent {
        public GameComponent(Verse.Game game) { }
        public override void StartedNewGame() {
            base.StartedNewGame();
            if (!Verse.DefDatabase<Verse.ResearchProjectDef>.GetNamed("EVOR_Research_Limbs3").IsFinished) {
                foreach (System.Tuple<Verse.RecipeDef, Verse.BodyPartDef> t in Singleton.Instance.bodyPartsToDelete) {
                    t.Item1.appliedOnFixedBodyParts.Remove(t.Item2);
                    t.Item1.ClearCachedData();
                    t.Item1.ResolveReferences();
                }
            }
            Singleton.Instance.bodyPartsToDelete.Clear();
            if (Singleton.Instance.settings.ChoicesOfWorkbenches.Count > 1 && Singleton.Instance.settings.ChosenWorkbench == Singleton.Instance.settings.ChoicesOfWorkbenches[0]) {
                string label = "EvolvedOrgansRedux";
                string text = "EvolvedOrgansRedux has detected that you have the mod " +
                    Singleton.Instance.settings.ChoicesOfWorkbenches[1] +
                    " active. In the settings menu you can choose the workbench of that mod to reduce the amount of workbenches and ressources.";
                if (Singleton.Instance.settings.ChoicesOfWorkbenches.Count > 2 && Singleton.Instance.settings.ChosenWorkbench == Singleton.Instance.settings.ChoicesOfWorkbenches[0]) {
                    text = "EvolvedOrgansRedux has detected that you have the mods ";
                    for (int i = 1; i < Singleton.Instance.settings.ChoicesOfWorkbenches.Count; i++) {
                        text += "\n\n" + Singleton.Instance.settings.ChoicesOfWorkbenches[i];
                    }
                    text += "\n\nenabled." +
                        "\nIn the settings menu you can choose a workbench of those mods to reduce the amount of workbenches and ressources.";
                }
                Verse.Find.LetterStack.ReceiveLetter(label, text, RimWorld.LetterDefOf.NeutralEvent);
            }

            Verse.Find.LetterStack.ReceiveLetter("EvolvedOrgansRedux additional bodyparts",
                "To install implants in the LowerShoulders, Tail and Back you need to research the Limb-Torso Support Structures project." +
                "\nTo install implants in the BodyCavities you need to research the Additional Organs project.",
                RimWorld.LetterDefOf.NeutralEvent);
        }
        public override void FinalizeInit() {
            base.FinalizeInit();
            if (!Singleton.Instance.settings.ImportantMessage20320905) {
                Verse.Find.LetterStack.ReceiveLetter("EvolvedOrgansRedux info",
                    "I have removed the setting 'CombatibilitySwitchEORVersionMidSave'. This setting was only there for someone that transitioned from the original EVOR version to this one. It's been a few years now since my version is the only one up to date, so everyone should have disabled that setting by now. If you get problems with your implants switching positions or stuff like that, please let me know on the Steam page.",
                    RimWorld.LetterDefOf.NeutralEvent);
                Singleton.Instance.settings.ImportantMessage20320905 = true;
                Verse.LoadedModManager.GetMod<EvolvedOrgansReduxSettings>().WriteSettings();

            }
        }
    }
}