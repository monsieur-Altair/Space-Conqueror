using System;
using System.Collections;
using Managers;
using UnityEngine;

namespace Skills
{
    public class Ice : Base
    {
        [SerializeField] private GameObject actionZone;
        
        private const float Radius = 3.0f;
        public float Duration { get; private set; }

        private GameObject _freezingZone;
        private Plane _plane;

        public static event Action DeletingFreezingZone;
        protected override void LoadResources()
        {
            base.LoadResources();
            var res = resource as Resources.Ice;
            if (res != null)
            {
                Duration = res.duration;
            }

            _freezingZone = Instantiate(actionZone);
            _freezingZone.SetActive(false);
            _plane = new Plane(Vector3.up, new Vector3(0, 0.66f, 0));

        }

        private void SpawnFreezingZone(Vector3 pos)
        {
            _freezingZone.SetActive(true);
            var ray = MainCamera.ScreenPointToRay(pos);
            if(_plane.Raycast(ray, out var distance))
            {
                _freezingZone.transform.position = ray.GetPoint(distance);   
            }
            else
            {
                throw new MyException("cannot calculate zone position");
            }
        }

        protected override void ApplySkill(Vector3 pos)
        {
            IsOnCooldown = true;
            SpawnFreezingZone(pos);
            Planets.Scientific.DecreaseScientificCount(Cost);
            Invoke(nameof(CancelSkill), Cooldown);
        }

        protected override void CancelSkill()
        {
            DeletingFreezingZone?.Invoke();
            _freezingZone.SetActive(false);
            IsOnCooldown = false;
            UnblockButton();
        }
    }
}