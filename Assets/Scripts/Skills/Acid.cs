using System.Collections;
using UnityEngine;

namespace Skills
{
    public class Acid : Base
    {
        public float HitDuration { get; private set; }
        public float HitDamage { get; private set; }
        public int HitCount { get; private set; }
        protected override void LoadResources()
        {
            base.LoadResources();
            var res = resource as Resources.Acid;
            if (res != null)
            {
                HitCount = res.hitCount;
                HitDuration = res.duration / HitCount;
                HitDamage = res.damage / HitCount;
            }
        }
        
        protected override void ApplySkill(Vector3 pos)
        {
            Planet= RaycastForPlanet(pos);
            if (Planet != null)
            {
                IsOnCooldown = true;
                Planets.Scientific.DecreaseScientificCount(Cost);
                StartCoroutine(nameof(DamagePlanetByRain));
                Invoke(nameof(CancelSkill), Cooldown);
            }
            else
            {
                UnblockButton();
            }
        }

        private IEnumerator DamagePlanetByRain()
        {
            var count = 0;
            while (count != HitCount)
            {
                Planet.DecreaseCounter(HitDamage);
                yield return new WaitForSeconds(HitDuration);
                count++;
            }
            
        }

        protected override void CancelSkill()
        {
            IsOnCooldown = false;
            UnblockButton();
        }
    }
}