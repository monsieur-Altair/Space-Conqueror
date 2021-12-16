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
        
        [SerializeField] private List<Material> scientificMaterials;
        [SerializeField] private List<Material> lanternsMaterials;
        [SerializeField] private List<Material> rocketsMaterials;

        private readonly Dictionary<int, MeshRenderer> _planetsRenderer = new Dictionary<int, MeshRenderer>();
        private readonly Dictionary<int, MeshRenderer> _lanternsRenderer = new Dictionary<int, MeshRenderer>();

        private List<Planets.Base> _allPlanets => Main.Instance.AllPlanets;
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
            FillList();
            SetAllOutlooks();
        }

        private void SetAllOutlooks()
        {
            foreach (var planet in _allPlanets)
            {
                SetOutlook(planet);
            }
        }

        private void FillList()
        {
            foreach (var planet in _allPlanets)
            {
                switch (planet.Type)
                {
                    case Type.Scientific:
                        DecomposeScientific(planet);
                        break;
                    case Type.Spawner:
                        break;
                    case Type.Attacker:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void DecomposeScientific(Planets.Base planet)
        {
            var index = planet.ID.GetHashCode();
            var circle = planet.transform.GetChild(0);
            _planetsRenderer.Add(index,circle.GetComponent<MeshRenderer>());
            _lanternsRenderer.Add(index,circle.transform.GetChild(0).GetComponent<MeshRenderer>());
        }

        private void SetScientific(int index, int team)
        {
            _planetsRenderer[index].material = scientificMaterials[team];
            _lanternsRenderer[index].material = lanternsMaterials[team];
        }
        
        public void SetOutlook(Planets.Base planet)
        {
            var team = (int)planet.Team;
            int index = planet.ID.GetHashCode();

            switch (planet.Type)
            {
                case Type.Scientific:
                    SetScientific(index,team);
                    break;
                case Type.Spawner:
                    break;
                case Type.Attacker:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetOutlook(Planets.Base planet, Units.Base unit)
        {
            var team = (int) planet.Team;
            //also we can add all rockets materials to list 
            unit.transform.GetChild(0).GetComponent<MeshRenderer>().material = rocketsMaterials[team];
        }
        
    }
}