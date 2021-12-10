using UnityEngine;
using UnityEngine.UI;

namespace Skills
{
    [DefaultExecutionOrder(1000)]
    public class Buff : MonoBehaviour, ISkill
    {
        private Control.SkillController _skillController;
        private Camera _mainCamera;
        private Managers.ObjectPool _objectPool;
        private Planets.Base _planet;
        public Resources.Buff resource;
        private Button _button;
        public float Cooldown { get; private set; }
        public int Cost { get; private set; }
        public float BuffPercent { get; private set; }

        public void Start()
        {
            _skillController = Control.SkillController.Instance;
            if (_skillController == null)
                throw new MyException("can't get skill controller");
            _mainCamera=_skillController.MainCamera;
            if(_mainCamera==null)
                throw new MyException("can't get main camera");
            _objectPool = Managers.ObjectPool.Instance;
            if (_objectPool == null)
                throw new MyException("can't get object pool");
            _button = GetComponent<Button>();
            
            _button.onClick.AddListener(() => { _skillController.HandlePress(_button);});
            
            LoadResources();
        }

        private void LoadResources()
        {
            Cooldown = resource.cooldown;
            BuffPercent = resource.buffPercent;
            Cost = resource.cost;
        }

        public void Execute(Vector3 pos)
        {
            _planet= RaycastForPlanet(pos);
            if (_planet != null && Planets.Scientific.ScientificCount>Cost)
            {
                _planet.isMove = !_planet.isMove;
                BuffThePlanet(_planet);
                Planets.Scientific.DecreaseScientificCount(Cost);
                Invoke(nameof(UnblockButton), Cooldown);
                Invoke(nameof(CancelBuff), Cooldown);
            }
            else
            {
                UnblockButton();
            }
        }

        private void BuffThePlanet(Planets.Base planet)
        {
            planet.BuffUnitAttack(BuffPercent);
            //add VFX
        }

        private void CancelBuff()
        {
            _planet.CancelBuff(BuffPercent);
        }

        private Planets.Base RaycastForPlanet(Vector3 pos)
        {
            //Debug.Log("vector="+pos);
            //Debug.Log("camera=null"+(_skillController==null));
            var ray = _mainCamera.ScreenPointToRay(pos);
            return Physics.Raycast(ray, out var hit) ? hit.collider.GetComponentInParent<Planets.Base>() : null;
        }

        
        private void UnblockButton()
        {
            _skillController.UnBlockButton(_button);
        }
    }
}