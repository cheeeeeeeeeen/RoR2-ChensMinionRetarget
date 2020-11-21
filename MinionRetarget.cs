#undef DEBUG

using BepInEx;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.CharacterAI;
using RoR2.UI;
using UnityEngine;
using static RoR2.UI.PingIndicator;

namespace Chen.MinionRetarget
{
    [BepInPlugin(ModGuid, ModName, ModVer)]
    [BepInDependency(R2API.R2API.PluginGUID, R2API.R2API.PluginVersion)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [R2APISubmoduleDependency(nameof(ResourcesAPI))]
    public class MinionRetargetPlugin : BaseUnityPlugin
    {
        public const string ModVer =
#if DEBUG
            "0." +
#endif
            "1.0.1";

        public const string ModName = "ChensMinionRetarget";
        public const string ModGuid = "com.Chen.ChensMinionRetarget";

        private void Awake()
        {
#if DEBUG
            Logger.LogWarning("Running test build with debug enabled! Report to CHEN if you're seeing this!");
            On.RoR2.Networking.GameNetworkManager.OnClientConnect += (self, user, t) => { };
#endif

            On.RoR2.UI.PingIndicator.RebuildPing += PingIndicator_RebuildPing;
            On.RoR2.CharacterAI.BaseAI.EvaluateSkillDrivers += BaseAI_EvaluateSkillDrivers;
            On.RoR2.CharacterAI.BaseAI.OnBodyDamaged += BaseAI_OnBodyDamaged;
        }

        private BaseAI.SkillDriverEvaluation BaseAI_EvaluateSkillDrivers(On.RoR2.CharacterAI.BaseAI.orig_EvaluateSkillDrivers orig, BaseAI self)
        {
            BaseAI.SkillDriverEvaluation skillDriverEval = orig(self);
            Obedience obeyComponent = self.gameObject.GetComponent<Obedience>();
            if (obeyComponent)
            {
                Vector3 targetPosition = obeyComponent.target.transform.position;
                Vector3 selfPosition = self.gameObject.transform.position;
                if (skillDriverEval.dominantSkillDriver.maxDistanceSqr < (targetPosition - selfPosition).sqrMagnitude && self.HasLOS(targetPosition))
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

            MinionOwnership[] minionOwnerships = FindObjectsOfType<MinionOwnership>();
            foreach (MinionOwnership minionOwnership in minionOwnerships)
            {
                if (!minionOwnership || minionOwnership.ownerMaster != master) continue;
                GameObject minionMasterObject = minionOwnership.gameObject;
                Obedience obeyComponent = Obedience.GetOrCreateComponent(minionMasterObject);
                obeyComponent.expiration = self.pingDuration;
                obeyComponent.target = self.pingTarget;
            }
        }
    }
}