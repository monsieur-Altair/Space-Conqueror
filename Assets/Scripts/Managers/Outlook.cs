using System;
using System.Collections.Generic;
using UnityEngine;
using Type = Planets.Type;

namespace Managers
{    
    [DefaultExecutionOrder(1000)]
    public class Outlook : MonoBehaviour
    {
        /*[Serializable]
        class PlanetTextures
        {
            public List<Texture> Textures;
        }
        
        [SerializeField] private List<PlanetTextures> textures;*/

        private List<List<Texture>> _allTextures=new List<List<Texture>>();
        
        [SerializeField] private List<Texture> scientificTextures;
        [SerializeField] private List<Material> lanternsMaterials;
        [SerializeField] private List<Texture> rocketsTextures;
        
        [SerializeField] private Material buffedPlanetMaterial;
        [SerializeField] private Material buffedRocketMaterial;
        
        [SerializeField] private Material basePlanetMaterial;
        [SerializeField] private Material baseRocketMaterial;
        
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
            _allTextures.Add(scientificTextures);
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
            _planetsRenderer[index].material.mainTexture=scientificTextures[team];
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

        public void SetUnitOutlook(Planets.Base planet, Units.Base unit)
        {
            var team = (int) planet.Team;
            //also we can add all rockets materials to list 
            var material = planet.IsBuffed ? buffedRocketMaterial : baseRocketMaterial;
            material.mainTexture = rocketsTextures[team];
            unit.transform.GetChild(0).GetComponent<MeshRenderer>().material = material;
        }

        public void SetBuff(Planets.Base planet)
        {
            int index = planet.ID.GetHashCode();
            var team = (int) planet.Team;
            var type = (int) planet.Type;
            var material = buffedPlanetMaterial;
            material.mainTexture = _allTextures[type][team];
            _planetsRenderer[index].material=material;
        }
        
        public void UnSetBuff(Planets.Base planet)
        {
            int index = planet.ID.GetHashCode();
            var team = (int) planet.Team;
            var type = (int) planet.Type;
            var material = basePlanetMaterial;
            material.mainTexture = _allTextures[type][team];
            _planetsRenderer[index].material = material;
        }
        
    }
}