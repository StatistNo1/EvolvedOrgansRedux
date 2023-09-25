using System.Linq;

namespace EvolvedOrgansRedux {
    [Verse.StaticConstructorOnStartup]
    public static class HarmonyPatches {
        private static readonly System.Type patchType = typeof(HarmonyPatches);
        private static HarmonyLib.Harmony harmony = null;

        static HarmonyPatches() {
            try {
                harmony = new HarmonyLib.Harmony("EvolvedOrgansRedux");
                Singleton.Instance.NameOfThisMod = harmony.Id;
                Patch();
            } catch (System.Exception e) {
                string errorMessage = "EvolvedOrgansRedux -> Error: Step one";
                errorMessage += "\n" + e;
                Verse.Log.Error(errorMessage);
            }
        }
        private static void Patch() {
            PatcherPostFix(typeof(Verse.PawnCapacityUtility), nameof(Verse.PawnCapacityUtility.CalculateLimbEfficiency), nameof(CalculateLimbEfficiency_PostFix));
            PatcherPostFix(typeof(RimWorld.AgeInjuryUtility), nameof(RimWorld.AgeInjuryUtility.RandomHediffsToGainOnBirthday), nameof(RandomHediffsToGainOnBirthday_Postfix), new System.Type[] { typeof(Verse.Pawn), typeof(int) });
            PatcherPostFix(typeof(RimWorld.Recipe_RemoveBodyPart), nameof(RimWorld.Recipe_RemoveBodyPart.GetPartsToApplyOn), nameof(Recipe_RemoveBodyPart_Postfix));
        }
        private static void CalculateLimbEfficiency_PostFix(Verse.BodyPartTagDef limbCoreTag, ref float __result) {
            foreach (System.Tuple<Verse.BodyPartTagDef, float> t in Singleton.Instance.BodyPartTagsToRecalculate) {
                if (limbCoreTag == t.Item1) {
                    __result = ((__result - 1f) * t.Item2) + 1f;
                }
            }
        }
        private static void RandomHediffsToGainOnBirthday_Postfix(Verse.Pawn pawn, ref System.Collections.Generic.IEnumerable<Verse.HediffGiver_Birthday> __result) {
            if (pawn.health.hediffSet.HasHediff(Verse.HediffDef.Named("EVOR_Hediff_AbdominalAndChestcavity_RasVacoule"))) {
                if (Verse.Prefs.LogVerbose)
                    Verse.Log.Message("EvolvedOrgansRedux -> Pawn is immortal because of Ras Vacoule.");
                __result = System.Linq.Enumerable.Empty<Verse.HediffGiver_Birthday>();
            }
        }
        private static void Recipe_RemoveBodyPart_Postfix(ref System.Collections.Generic.IEnumerable<Verse.BodyPartRecord> __result) {
            //Remove the Recipe_RemoveBodyPart for the bodyparts added by this mod.
            //I would rather use my custom Recipe_RemoveImplant.
            __result = __result.Where(bodypartrecord => bodypartrecord.def != DefOf.LowerShoulder && bodypartrecord.def != DefOf.Back &&
                bodypartrecord.def != DefOf.BodyCavity1 && bodypartrecord.def != DefOf.BodyCavity2 && bodypartrecord.def != DefOf.BodyCavityA
                && !bodypartrecord.def.defName.ToLower().Contains("tail"));
        }
        private static void PatcherPostFix(System.Type typeOfMethodToPatch, string nameOfMethodToPatch, string nameOfPatcherMethod, System.Type[] parameters = null) {
            if (Verse.Prefs.LogVerbose)
                Verse.Log.Message("EvolvedOrgansRedux -> Currently patching: " + nameOfPatcherMethod);
            harmony.Patch(HarmonyLib.AccessTools.Method(type: typeOfMethodToPatch, name: nameOfMethodToPatch, parameters),
                      postfix: new HarmonyLib.HarmonyMethod(methodType: patchType, methodName: nameOfPatcherMethod));
        }
        //private static void PatcherTranspiler(System.Type typeOfMethodToPatch, string nameOfMethodToPatch, string nameOfPatcherMethod, System.Type[] parameters = null) {
        //    if (Verse.Prefs.LogVerbose)
        //        Verse.Log.Message("EvolvedOrgansRedux -> Currently patching: " + nameOfPatcherMethod);
        //    harmony.Patch(HarmonyLib.AccessTools.Method(type: typeOfMethodToPatch, name: nameOfMethodToPatch, parameters),
        //              transpiler: new HarmonyLib.HarmonyMethod(methodType: patchType, methodName: nameOfPatcherMethod));
        //}

        //static IEnumerable<CodeInstruction> CalculateCapacityLevel_Transpilerfix(IEnumerable<CodeInstruction> instructions) {
        //    bool instructionFound = false;
        //    MethodInfo callToMathfMax = AccessTools.Method(typeof(Mathf), nameof(Mathf.Max), parameters: new System.Type[] { typeof(float), typeof(float) });
        //    foreach (CodeInstruction instruction in instructions) {
        //        yield return instruction;
        //        if (instruction.Calls(callToMathfMax)) {
        //            yield return new CodeInstruction(OpCodes.Ldarg_1);
        //            yield return CodeInstruction.Call(patchType, nameof(FixCapacity));
        //            instructionFound = true;
        //        }
        //    }
        //    if (!instructionFound)
        //        Log.Error("EvolvedOrgansRedux -> Instruction not found in CalculateCapacityLevel_Transpilerfix");
        //
        //}
        //public static float FixCapacity(float capacityValue, BodyPartTagDef limbCoreTag) {
        //    foreach (System.Tuple<Verse.BodyPartTagDef, float> t in Singleton.Instance.BodyPartTagsToRecalculate) {
        //        if (limbCoreTag == t.Item1) {
        //            capacityValue = ((capacityValue - 1f) * t.Item2) + 1f;
        //        }
        //    }
        //    return capacityValue;
        //}
    }
}