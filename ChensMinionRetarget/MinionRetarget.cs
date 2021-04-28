#undef DEBUG

using BepInEx;
using Chen.Helpers;
using Chen.Helpers.GeneralHelpers;
using Chen.Helpers.UnityHelpers;
using R2API.Utils;
using RoR2;
using RoR2.CharacterAI;
using RoR2.UI;
using System.Runtime.CompilerServices;
using UnityEngine;
using static RoR2.UI.PingIndicator;

[assembly: InternalsVisibleTo("ChensMinionRetarget.Tests")]

namespace Chen.MinionRetarget
{
    /// <summary>
    /// The Unity plugin of the mod.
    /// </summary>
    [BepInPlugin(ModGuid, ModName, ModVer)]
    [BepInDependency(R2API.R2API.PluginGUID, R2API.R2API.PluginVersion)]
    [BepInDependency(HelperPlugin.ModGuid, HelperPlugin.ModVer)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    public class MinionRetargetPlugin : BaseUnityPlugin
    {
        /// <summary>
        /// This mod's version.
        /// </summary>
        public const string ModVer =
#if DEBUG
            "0." +
#endif
            "1.0.7";

        /// <summary>
        /// This mod's name.
        /// </summary>
        public const string ModName = "ChensMinionRetarget";

        /// <summary>
        /// This mod's GUID.
        /// </summary>
        public const string ModGuid = "com.Chen.ChensMinionRetarget";

        private void Awake()
        {
#if DEBUG
            Chen.Helpers.GeneralHelpers.MultiplayerTest.Enable(Logger, "Running test build with debug enabled! Report to CHEN if you're seeing this!");
#endif

            On.RoR2.UI.PingIndicator.RebuildPing += PingIndicator_RebuildPing;
            On.RoR2.CharacterAI.BaseAI.EvaluateSkillDrivers += BaseAI_EvaluateSkillDrivers;
            On.RoR2.CharacterAI.BaseAI.OnBodyDamaged += BaseAI_OnBodyDamaged;
        }

        private BaseAI.SkillDriverEvaluation BaseAI_EvaluateSkillDrivers(On.RoR2.CharacterAI.BaseAI.orig_EvaluateSkillDrivers orig, BaseAI self)
        {
            BaseAI.SkillDriverEvaluation skillDriverEval = orig(self);
            Obedience obeyComponent = self.gameObject.GetComponent<Obedience>();
            if (obeyComponent && obeyComponent.target)
            {
                Vector3 targetPosition = obeyComponent.target.transform.position;
                Vector3 selfPosition = self.gameObject.transform.position;
                if (skillDriverEval.dominantSkillDriver.maxDistanceSqr > (targetPosition - selfPosition).sqrMagnitude && self.HasLOS(targetPosition))
                {
                    self.currentEnemy.gameObject = obeyComponent.target;
                    self.currentEnemy.bestHurtBox = self.GetBestHurtBox(obeyComponent.target);
                    self.leader.gameObject = self.currentEnemy.gameObject;
                    self.leader.bestHurtBox = self.currentEnemy.bestHurtBox;
                }
            }
            return skillDriverEval;
        }

        private void BaseAI_OnBodyDamaged(On.RoR2.CharacterAI.BaseAI.orig_OnBodyDamaged orig, BaseAI self, DamageReport damageReport)
        {
            Obedience obeyComponent = self.gameObject.GetComponent<Obedience>();
            if (obeyComponent) return;
            orig(self, damageReport);
        }

        private void PingIndicator_RebuildPing(On.RoR2.UI.PingIndicator.orig_RebuildPing orig, PingIndicator self)
        {
            orig(self);
            if (!self.pingOwner || self.pingType != PingType.Enemy) return;
            CharacterBody targetBody = self.pingTarget.GetComponent<CharacterBody>();
            GameObject masterObject = self.pingOwner;
            CharacterMaster master = masterObject.GetComponent<CharacterMaster>();
            CharacterBody body = master.GetBody();
            if (!body || !targetBody || targetBody.teamComponent.teamIndex == body.teamComponent.teamIndex) return;

            foreach (CharacterMaster minion in master.GetAllMinionComponents<CharacterMaster>())
            {
                minion.gameObject.GetOrAddComponent<Obedience>(obeyComponent =>
                {
                    obeyComponent.expiration = self.pingDuration;
                    obeyComponent.target = self.pingTarget;
                });
            }
        }

        internal static bool DebugCheck()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}