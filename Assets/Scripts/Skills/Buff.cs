
using UnityEngine;

namespace Skills
{
    public class Buff : Base
    {
        public float BuffPercent { get; private set; }

        protected override void LoadResources()
        {
            base.LoadResources();
            var res = resource as Resources.Buff;
            if(res!=null)
                BuffPercent = res.buffPercent;
        }

        protected override void ApplySkill(Vector3 pos)
        {
            Planet = RaycastForPlanet(pos);
            if (Planet != null)
            {
                
                Planet.BuffUnitAttack(BuffPercent);
                //add VFX
                IsOnCooldown = true;
                Planets.Scientific.DecreaseScientificCount(Cost);
                Invoke(nameof(CancelSkill), Cooldown);
            }
            else
            {
                UnblockButton();
            }
        }

        protected override void CancelSkill()
        {
            IsOnCooldown = false;
            Planet.CancelBuff(BuffPercent);
            UnblockButton();
        }
    }
}