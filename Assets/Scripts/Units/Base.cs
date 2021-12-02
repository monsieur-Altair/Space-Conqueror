using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Base : MonoBehaviour
    {
        protected NavMeshAgent Agent;

        private Vector3 _destination;
        protected Planets.Base Target;

        //override this method to adjust functionality, when unit arrives at the target  
        protected abstract void TargetInRange();
        public abstract void SetData(Planets.Base.UnitInf unitInf);
        protected abstract void SetSpeed();
        public abstract float CalculateAttack();
        
        private const float MinDistance = 1.0f;

        public void Start()
        {
        }

        public void Update()
        {
            if (Target != null)
            {
                /*if (Agent.velocity.sqrMagnitude > Mathf.Epsilon)
                {
                    transform.rotation = Quaternion.LookRotation(Agent.velocity.normalized);
                }*/
                var distance = Vector3.Distance(_destination, transform.position);
                if (distance < MinDistance)
                {
                    Agent.isStopped = true;
                    TargetInRange();
                }
            }
        }

        private void GoTo(Vector3 destinationPos)
        {
            _destination = destinationPos;

            //didn't work if i get component in start
            Agent = GetComponent<NavMeshAgent>();
            if (Agent == null)
                throw new MyException("can't get NavMeshAgent component");
            
            SetSpeed();
            Agent.SetDestination(destinationPos);
            Agent.isStopped = false;
        }
        
        public void GoTo(Planets.Base destination,Vector3 destinationPos)
        {
            Target = destination;
            GoTo(destinationPos);
        }

    }
}