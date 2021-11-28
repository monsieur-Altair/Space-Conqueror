using UnityEngine; 
using UnityEngine.UI; 

namespace Planets
{
    [RequireComponent(typeof(Collider))]
    public abstract class Base : MonoBehaviour
    {
   
        public Resources.Rocket rockets;
        protected UI.UnitHandler _uiHandler; 

        protected float Damage { get; set; }
        protected float Defense { get; set; } 
        protected float MaxCount { get; set; }
        protected float Speed { get; set; }
        protected float ProduceCount { get; set; }
        protected float ProduceTime { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            SetRocket();
        }

        // Update is called once per frame
        void Update()
        {
        }

        protected virtual void SetRocket()
        {
            Damage = rockets.damage;
            Defense = rockets.defense;
            MaxCount = rockets.max;
            Speed = rockets.speed;
            ProduceCount = rockets.produceCount;
            ProduceTime = rockets.produceTime;
        }
    }
}
