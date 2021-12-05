using TMPro;
using UnityEngine; 

namespace Planets
{
    public enum Team
    {
        Blue=0,
        Red=1,
        White=5
    }
    public enum Type
    {
        Scientific=0,
        Spawner=1,
        Attacker=2
    }
    [RequireComponent(typeof(Collider))]
    public abstract class Base : MonoBehaviour
    {
        [SerializeField] private Team team;
        [SerializeField] private Type type;
        [SerializeField] private Resources.Unit resourceUnit;
        [SerializeField] private Resources.Planet resourcePlanet;

        private const float LaunchCoefficient = 0.5f;

        private Managers.Main _main;
        private Managers.Outlook _outlook;
        private Managers.UI _ui; 
        
        private float speed = 20.0f;
        private float _count;
        private UnitInf _unitInf;
        
        protected float MaxCount { get; private set; }
        protected float ProduceCount { get; private set; }
        protected float ProduceTime { get; private set; }
        protected float Defense { get; private set; }
        public Team Team { get; private set; }
        public Type Type { get; private set; }
        
        
        public struct UnitInf
        {
            public float Speed { get; internal set; }
            public float Damage { get; internal set;}
            public float UnitCount { get; internal set; }
            public Team Team { get; internal set; }
        }
        


        // Start is called before the first frame update
        public void Start()
        {
            _count = 0.0f;

            if (resourceUnit == null) 
                throw new MyException("resource is not loaded: "+name);
            
            _unitInf = new UnitInf();
            _outlook=Managers.Outlook.Instance;
            _ui = Managers.UI.Instance;
            _main=Managers.Main.Instance;
            LoadResources();
        }
        public void Update()
        {
            Move();
            IncreaseResources();
            DisplayUI();
        }

        protected virtual void LoadResources()
        {

            MaxCount = resourcePlanet.maxCount;
            ProduceCount = resourcePlanet.produceCount;
            ProduceTime = resourcePlanet.produceTime;
            Defense = resourcePlanet.defense;
            Team = team;
            Type = type;
            
            _unitInf.Speed = resourceUnit.speed;
            _unitInf.Damage = resourceUnit.damage;
            _unitInf.Team = Team;
        }

        protected virtual void Move()
        {
            transform.Rotate(Vector3.up, speed*Time.deltaTime,Space.World);
        }

        protected virtual void IncreaseResources()
        {
            _count += ProduceCount / ProduceTime * Time.deltaTime;
            if (_count > MaxCount) 
                _count = MaxCount;
        }

        protected virtual void DisplayUI()
        {
            _ui.SetCounter(this,(int)_count);
        }

        public void LaunchUnit(Planets.Base destination)
        {
            var radiusCurrent = GetComponent<SphereCollider>().radius;
            var radiusDest = destination.GetComponent<SphereCollider>().radius;

            var currentPos = transform.position;
            var destinationPos = destination.transform.position;

            var offset = (destinationPos - currentPos).normalized;
            var unit = Instantiate(
                    resourceUnit.prefab, 
                    currentPos+offset*radiusCurrent, 
                    Quaternion.LookRotation(offset)).GetComponent<Units.Base>();

            SetUnitCount();
            _outlook.SetOutlook(this, unit);
            unit.SetData(_unitInf);
            unit.GoTo(destination,destinationPos-offset*radiusDest);
        }

        private void SetUnitCount()
        {
            float unitCount=_count*LaunchCoefficient;
            _unitInf.UnitCount = unitCount;
            _count -= unitCount;
        }


        public void AttackedByUnit(Units.Base unit)
        {
            var unitTeam = unit.getTeam();
            var attack=unit.CalculateAttack();
            attack *= this.Team == unitTeam ? 1 : -1;
            _count += attack;
            if (_count < 0)
            {
                _main.UpdateObjectsCount(Team,unitTeam);
                SwitchTeam(unitTeam);
                _main.CheckGameOver();
            }
        }

        private void SwitchTeam(Planets.Team newTeam)
        {
            
            team = newTeam;
            //reset resources
            LoadResources();

            _outlook.SetOutlook(this);
            _ui.SetCounterColor(this);
            _count *= -1;
        }
    }
}
