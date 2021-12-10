﻿using UnityEngine;
using UnityEngine.UI;

namespace Skills
{
    [DefaultExecutionOrder(1000)]
    public abstract class Base : MonoBehaviour, ISkill
    {
        [SerializeField] protected Resources.Skill resource;
        
        protected Control.SkillController SkillController;
        protected Camera MainCamera;
        protected Managers.ObjectPool ObjectPool;
        protected Planets.Base Planet;
        protected abstract void ApplySkill();
        protected abstract void CancelSkill();
        protected bool IsOnCooldown = false;

        public float Cooldown { get; private set; }
        public int Cost { get; private set; }

        private Button _button;

        
        public void Start()
        {
            SkillController = Control.SkillController.Instance;
            if (SkillController == null)
                throw new MyException("can't get skill controller");
            MainCamera=SkillController.MainCamera;
            if(MainCamera==null)
                throw new MyException("can't get main camera");
            ObjectPool = Managers.ObjectPool.Instance;
            if (ObjectPool == null)
                throw new MyException("can't get object pool");
            _button = GetComponent<Button>();
            
            _button.onClick.AddListener(() => { SkillController.PressHandler(_button);});
            
            SkillController.CanceledSelection += UnblockButton;
            
            LoadResources();
        }

        protected virtual void LoadResources()
        {
            Cooldown = resource.cooldown;
            Cost = resource.cost;
        }

        public void Execute(Vector3 pos)
        {
            Planet= RaycastForPlanet(pos);
            if (Planet != null && Planets.Scientific.ScientificCount>Cost && !IsOnCooldown)
            {
                ApplySkill();
                Planets.Scientific.DecreaseScientificCount(Cost);
                Invoke(nameof(CancelSkill), Cooldown);
            }
            else
            {
                UnblockButton();
            }
        }
        
        private Planets.Base RaycastForPlanet(Vector3 pos)
        {
            var ray = MainCamera.ScreenPointToRay(pos);
            return Physics.Raycast(ray, out var hit) ? hit.collider.GetComponentInParent<Planets.Base>() : null;
        }
        
        protected void UnblockButton()
        {
            if(!IsOnCooldown)
                _button.image.color=Color.white;
        }
    }
}