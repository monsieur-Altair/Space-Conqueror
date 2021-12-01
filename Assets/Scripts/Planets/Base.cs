using UnityEngine; 

namespace Planets
{
    [RequireComponent(typeof(Collider))]
    public abstract class Base : MonoBehaviour
    {
        
        [SerializeField] protected Resources.Unit resourceUnit;
        protected UI.UnitHandler UIHandler;
        public float speed = 20.0f;
        private float _count = 0.0f;


        protected float MaxCount { get; private set; }
        protected float ProduceCount { get; private  set; }
        protected float ProduceTime { get; private  set; }
        protected float Defense { get; private  set; }
        protected float Speed { get; private set; }
        protected float Damage { get; private set; }


        // Start is called before the first frame update
        public void Start()
        {
            UIHandler = GetComponent<UI.UnitHandler>();
            if (UIHandler == null) 
                throw new MyException("ui handler is not loaded: "+name);
            if (resourceUnit == null) 
                throw new MyException("resource is not loaded: "+name);
            
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
            MaxCount = resourceUnit.maxCount;
            ProduceCount = resourceUnit.produceCount;
            ProduceTime = resourceUnit.produceTime;
            Defense = resourceUnit.defense;
            Speed = resourceUnit.speed;
            Damage = resourceUnit.damage;
        }

        protected virtual void Move()
        {
            transform.Rotate(Vector3.up, speed*Time.deltaTime,Space.World);
        }

        protected virtual void IncreaseResources()
        {
            _count += ProduceCount / ProduceTime * Time.deltaTime;
        }

        protected virtual void DisplayUI()
        {
            UIHandler.SetCounter((int)_count);
        }

        public void LaunchUnit(Base destination)
        {
            var radiusCurrent = GetComponent<SphereCollider>().radius;
            var currentPos = transform.position;
            var destinationPos = destination.transform.position;
            var offset = (destinationPos - currentPos).normalized*radiusCurrent;
            var radiusDest = destination.GetComponent<SphereCollider>().radius;
            var prefab = resourceUnit.prefab;
            var unit = Instantiate(
                prefab, 
                currentPos+offset, 
                Quaternion.LookRotation(offset))
                .GetComponent<Units.Base>();
            unit.GoTo(destinationPos-offset*radiusDest);
        }
    }
}
