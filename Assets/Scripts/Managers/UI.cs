using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    [DefaultExecutionOrder(1000)]
    public class UI : MonoBehaviour
    {
        public static UI Instance { get; private set; }
        
        
        [SerializeField] private GameObject counterPrefab;
        [SerializeField] private GameObject planetsLay;
        [SerializeField] private Canvas canvas;
        
        private List<Planets.Base> _allPlanets;
        //private List<GameObject> _counter=new List<GameObject>();
        //private List<TextMeshProUGUI> _foregrounds=new List<TextMeshProUGUI>();
        //private List<Image> _backgrounds = new List<Image>();
        
        private Dictionary<int,GameObject> _counter=new Dictionary<int, GameObject>();
        private Dictionary<int,Image> _backgrounds=new Dictionary<int, Image>();
        private Dictionary<int,TextMeshProUGUI> _foregrounds=new Dictionary<int, TextMeshProUGUI>();

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        private Vector3 _offset; 


        //private float _number = 0;
        private void Start()
        {
            /*float colliderLength = GetComponent<SphereCollider>().radius;
            Debug.Log(colliderLength);*/
            _offset = new Vector3(0, -30.0f, 0);
            //find out how to calculate offset for counter
            FillLists();
        }
        
        private void FillLists()
        {
            Vector3 pos;
            GameObject counter;
            int index;
            _allPlanets = planetsLay.GetComponentsInChildren<Planets.Base>().ToList();
            foreach (var planet in _allPlanets)
            {
                pos = planet.transform.position;
                counter = Instantiate(counterPrefab, canvas.transform);
                counter.transform.position = Camera.main.WorldToScreenPoint(pos) + _offset;
                index = pos.GetHashCode();
                _counter.Add(index,counter);
                _foregrounds.Add(index, counter.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>());
                _backgrounds.Add(index, counter.GetComponentInChildren<Image>());
                //_counter.Add(counter);
                //_foregrounds.Add(counter.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>());
                //_backgrounds.Add(counter.GetComponentInChildren<Image>());
                
            }
        }

        public void SetCounter(Planets.Base planet, int value)
        {
            //int index = _allPlanets.IndexOf(planet);
            int index = planet.transform.position.GetHashCode();
            _foregrounds[index].text = value.ToString();
        }
    }
}