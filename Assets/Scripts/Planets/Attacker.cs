using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Planets
{
    public class Attacker : Base
    {
        private ZoneController _attackZone;
        [SerializeField] private Resources.Unit _resourceBullet;
        private UnitInf _bulletInf;

        public override void Start()
        {
            _bulletInf = new UnitInf();
            
            base.Start();
        }

        public void StartAttackingUnits(Units.Base unit)
        {
            var tr = unit.transform;

            var radiusCurrent = GetComponent<SphereCollider>().radius;

            var currentPos = transform.position;
            var destinationPos = tr.position;

            var offset = (destinationPos - currentPos).normalized;
            
            
            var obj = Pool.GetObject(
                    Planets.Type.Spawner, 
                currentPos+offset*radiusCurrent, 
                tr.rotation)
                .GetComponent<Units.Base>();
            obj.SetData(in _bulletInf);
            //obj.GoTo(destinationPos);
            obj.GoTo(new Vector3(1,0,-3));
        }

        protected override void LoadResources()
        {
            base.LoadResources();

            if (_resourceBullet == null)
                throw new MyException("bullet info = null");
            
            _bulletInf.Damage = _resourceBullet.damage;
            _bulletInf.Speed = _resourceBullet.speed;
            _bulletInf.Team = Team;
            _bulletInf.UnitCount = 1;
            
        }
    }
}