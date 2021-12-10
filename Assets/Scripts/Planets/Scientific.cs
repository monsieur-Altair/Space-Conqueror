using TMPro;
using UnityEngine;

namespace Planets
{
    public class Scientific : Base
    {
        [SerializeField] private Resources.Scientific scientific;

        public int MaxCountScientific { get; private set; }
        public float ProduceCountScientific { get; private set; }
        public float ProduceTimeScientific { get; private set; }
        public static float ScientificCount { get; private set; }

        public static void DecreaseScientificCount(float value)
        {
            ScientificCount -= value;
        }
        
        protected override void LoadResources()
        {
            base.LoadResources();
            LoadScientificRes();
        }

        private void LoadScientificRes()
        {
            MaxCountScientific = scientific.maxCount;
            ProduceCountScientific = scientific.produceCount;
            ProduceTimeScientific = scientific.produceTime;
        }



        protected override void IncreaseResources()
        {
            base.IncreaseResources();
            IncreaseScientificRes();
        }

        private void IncreaseScientificRes()
        {
            //CHANGE FOR EVERY TEAM
            if(Team==Team.Blue)
                ScientificCount += ProduceCountScientific / ProduceTimeScientific * Time.deltaTime;
            if (ScientificCount > MaxCountScientific)
                ScientificCount = MaxCountScientific;
        }


        
        protected override void DisplayUI()
        {
            base.DisplayUI();
            DisplayScientificBar();
        }

        private void DisplayScientificBar()
        {
            UI.SetScientificCounter((int)ScientificCount);
        }
    }
}