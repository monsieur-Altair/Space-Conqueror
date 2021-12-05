using TMPro;
using UnityEngine;

namespace Planets
{
    public class Scientific : Base
    {
        [SerializeField] private Resources.Scientific scientific;
        
        
   
        protected override void LoadResources()
        {
            base.LoadResources();
            LoadScientificRes();
        }

        private void LoadScientificRes()
        {
            //load info about scientific resources
        }



        protected override void IncreaseResources()
        {
            base.IncreaseResources();
            IncreaseScientificRes();
        }

        private void IncreaseScientificRes()
        {
            //add later
        }


        
        protected override void DisplayUI()
        {
            base.DisplayUI();
            DisplayScientificBar();
        }

        private void DisplayScientificBar()
        {
            //add later
        }
    }
}