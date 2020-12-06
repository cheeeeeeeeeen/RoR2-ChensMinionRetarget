using RoR2;
using UnityEngine;

namespace Chen.MinionRetarget
{
    internal class Obedience : MonoBehaviour
    {
        public float expiration = 0;
        public GameObject target = null;

        private HealthComponent healthComponent = null;
        private CharacterBody body = null;
        private bool init = true;

        private void FixedUpdate()
        {
            if (init)
            {
                init = false;
                body = target.GetComponent<CharacterBody>();
                healthComponent = body.healthComponent;
            }
            if (expiration <= 0 || !target || !healthComponent.alive) Destroy(this);
            expiration -= Time.fixedDeltaTime;
        }
    }
}