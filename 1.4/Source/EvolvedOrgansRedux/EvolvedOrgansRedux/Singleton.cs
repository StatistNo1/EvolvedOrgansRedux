using System.Linq;

namespace EvolvedOrgansRedux {
    public sealed class Singleton {
        public Settings settings = Verse.LoadedModManager.GetMod<EvolvedOrgansReduxSettings>().GetSettings<Settings>();
        public System.Collections.Generic.List<System.Tuple<Verse.RecipeDef, Verse.BodyPartDef>> bodyPartsToDelete = new();
        public System.Collections.Generic.List<System.Tuple<Verse.BodyPartTagDef, float>> BodyPartTagsToRecalculate = new();
        public System.Collections.Generic.List<string> forbiddenMods = new System.Collections.Generic.List<string>() {
                "Android tiers",
                "Android tiers - TX Series",
                "Android Tiers Reforged"
            };
        public string NameOfThisMod { get; set; }
        public System.Collections.Generic.List<Verse.RecipeDef> AddLowershouldersToRecipedef { get; set; }
        public System.Collections.Generic.List<Verse.RecipeDef> AddLeftchestcavityToRecipedef { get; set; }
        public System.Collections.Generic.List<Verse.RecipeDef> AddRightchestcavityToRecipedef { get; set; }
        private static readonly Singleton instance = new ();
        static Singleton() { }
        private Singleton() {
            AddLowershouldersToRecipedef = new System.Collections.Generic.List<Verse.RecipeDef>();
            AddLeftchestcavityToRecipedef = new System.Collections.Generic.List<Verse.RecipeDef>();
            AddRightchestcavityToRecipedef = new System.Collections.Generic.List<Verse.RecipeDef>();
        }
        public static Singleton Instance { get { return instance; } }
        private float calculateAmountOfTaggedBodyparts(Verse.BodyPartTagDef tag) {
            float TaggedBodyPart = 0;
            foreach (Verse.BodyPartRecord bpr in Verse.DefDatabase<Verse.BodyDef>.GetNamed("Human").AllParts) {
                foreach (Verse.BodyPartTagDef bptd in bpr.def.tags.Where(x => x == tag)) {
                    TaggedBodyPart++;
                }
            }
            return TaggedBodyPart /= 2;
        }
        public void AddBodyPartTagDefToList(Verse.BodyPartTagDef bodyPartTagDef) {
            BodyPartTagsToRecalculate.Add(new System.Tuple<Verse.BodyPartTagDef, float>(bodyPartTagDef, calculateAmountOfTaggedBodyparts(bodyPartTagDef)));
        }
    }
}
