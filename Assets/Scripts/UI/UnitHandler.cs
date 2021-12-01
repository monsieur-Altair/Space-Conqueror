using System;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    // Sets the script to be executed later than all default scripts
    // This is helpful for UI, since other things may need to be initialized before setting the UI
    [DefaultExecutionOrder(1000)]
    public class UnitHandler : MonoBehaviour
    {
        [SerializeField] private GameObject counterPrefab;
        private GameObject _counter;
        private Vector3 _offset; 
        [SerializeField] private Canvas canvas;
        private TextMeshProUGUI _counterText;
        private Image _background;

        //private float _number = 0;
        private void Start()
        {
            /*float colliderLength = GetComponent<SphereCollider>().radius;
            Debug.Log(colliderLength);*/
            _offset = new Vector3(0, -30.0f, 0);
            //find out how to calculate offset for counter
            
            _counter = Instantiate(counterPrefab, canvas.transform);
            
            _counterText= _counter.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
            _background = _counter.GetComponentInChildren<Image>(); 

            if(Camera.main!=null)
                _counter.transform.position = Camera.main.WorldToScreenPoint(transform.position)+_offset;
        }

        public void SetCounter(int value)
        {
            _counterText.text = value.ToString();
        }
        
        public void Update()
        {
            /*_number += 1.5f*Time.deltaTime;
            int o = (int)_number;
            _counterText.text = o.ToString();*/
        }
    }
}