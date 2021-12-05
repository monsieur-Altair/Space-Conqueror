using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Resources;
using UnityEngine;
using Type = Planets.Type;

namespace Managers
{    
    [DefaultExecutionOrder(1000)]
    public class Outlook : MonoBehaviour
    {
        /*private MeshRenderer _planetRenderer;
        private MeshRenderer _torchRenderer;*/
        //public Material planetMaterial;
        //public Material torchMaterial;

        [SerializeField] private List<Material> scientificMaterials;
        [SerializeField] private List<Material> lanternsMaterials;
        [SerializeField] private List<Material> rocketsMaterials;
        [SerializeField] private List<Color> counterBackground;
        [SerializeField] private List<Color> counterForeground;
        [SerializeField] private GameObject planetsLay;
        private List<Planets.Base> _allPlanets;
        private List<MeshRenderer> _planetsRenderer=new List<MeshRenderer>();
        private List<MeshRenderer> _lanternsRenderer=new List<MeshRenderer>();

        public static Outlook Instance { get; private set; }

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private Outlook()
        {
            
        }
        
        public void Start()
        {
            /*var planetObject = transform.GetChild(0).gameObject;
            _planetRenderer = planetObject.GetComponent<MeshRenderer>();
            if(_planetRenderer==null)
                throw new MyException("can't use mesh renderer: "+name);

            _torchRenderer = planetObject.transform.GetChild(0).GetComponent<MeshRenderer>();
            if(_torchRenderer==null)
                throw new MyException("can't use torch renderer: "+name);*/
            
            FillList();
        }

        private void FillList()
        {
            _allPlanets = planetsLay.GetComponentsInChildren<Planets.Base>().ToList();
            foreach (var planet in _allPlanets)
            {
                var circle = planet.transform.GetChild(0);
                _planetsRenderer.Add(circle.GetComponent<MeshRenderer>());
                _lanternsRenderer.Add(circle.transform.GetChild(0).GetComponent<MeshRenderer>());
                SetOutlook(planet);
            }
        }
        
        public void SetOutlook(Planets.Base planet)
        {
            var team = (int)planet.Team;
            int index = _allPlanets.IndexOf(planet);
            
            
            switch (planet.Type)
            {
                case Type.Scientific:
                    _planetsRenderer[index].material = scientificMaterials[team];
                    _lanternsRenderer[index].material = lanternsMaterials[team];
                    break;
                case Type.Spawner:
                    break;
                case Type.Attacker:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            /*_planetRenderer.material = planetMaterial;
            _torchRenderer.material = torchMaterial;*/

        }

        public void SetOutlook(Planets.Base planet, Units.Base unit)
        {
            var team = (int) planet.Team;
            
            unit.transform.GetChild(0).GetComponent<MeshRenderer>().material = rocketsMaterials[team];
        }
        
    }
}