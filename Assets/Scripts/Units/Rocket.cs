using System.Linq.Expressions;
using UnityEngine;

namespace Units
{
    public class Rocket : Base
    {

        private Planets.Base.UnitInf _unitInf;
        protected override void TargetInRange()
        {
            Target.AttackedByUnit(this);
            Destroy(gameObject);
            Target = null;

        }

        public override void SetData(Planets.Base.UnitInf unitInf)
        {
            _unitInf = unitInf;
        }

        protected override void SetSpeed()
        {
            Agent.speed = _unitInf.Speed;
        }

        public override float CalculateAttack()
        {
            return _unitInf.Damage / 100.0f* _unitInf.UnitCount;
        }
        
        
    }
}