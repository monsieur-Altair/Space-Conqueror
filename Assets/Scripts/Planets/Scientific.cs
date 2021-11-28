using UnityEngine;

namespace Planets
{
    public class Scientific : Base
    {
        public Resources.ScientificResources scientificResources;
        public float speed = 20.0f;

        private float count = 0.0f;
        
        // Start is called before the first frame update
        void Start()
        {
            _uiHandler = GetComponent<UI.UnitHandler>();
            SetRocket();
            
        }

        // Update is called once per frame
        void Update()
        {
            count += ProduceCount / ProduceTime*Time.deltaTime;
            _uiHandler.SetCounter((int)count);
            transform.Rotate(Vector3.forward, speed*Time.deltaTime);
        }

        protected override void SetRocket()
        {
            base.SetRocket();
            //set rockets according user upgrades
        }
    }
}