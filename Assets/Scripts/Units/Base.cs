using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Base : MonoBehaviour
    {
        private NavMeshAgent _agent;

        private Vector3 _destination= Vector3.positiveInfinity;

        //override this method to adjust functionality, when unit arrives at the target  
        protected abstract void TargetInRange();
        
        private const float MinDistance = 1.0f;

        public void Start()
        {
        }

        public void Update()
        {
            if (_destination != Vector3.positiveInfinity)
            {
                var distance = Vector3.Distance(_destination, transform.position);
                if (distance < MinDistance)
                {
                    _agent.isStopped = true;
                    TargetInRange();
                }
            }
        }

        public void GoTo(Vector3 destinationPos)
        {
            _destination = destinationPos;

            //didn't work if i get component in start
            _agent = GetComponent<NavMeshAgent>();
            if (_agent == null)
                throw new MyException("can't get NavMeshAgent component");
            _agent.SetDestination(destinationPos);
            _agent.isStopped = false;
        }

    }
}