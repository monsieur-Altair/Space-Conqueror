
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

        protected override void ApplySkill()
        {
            Planet.BuffUnitAttack(BuffPercent);
            //add VFX
            IsOnCooldown = true;
        }

        protected override void CancelSkill()
        {
            IsOnCooldown = false;
            Planet.CancelBuff(BuffPercent);
            UnblockButton();
        }
    }
}