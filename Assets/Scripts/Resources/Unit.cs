using UnityEngine;

namespace Resources
{
    [CreateAssetMenu (fileName = "new unit",menuName = "Resources/Unit")]
    public class Unit:ScriptableObject
    {
        public new string name;
    
        public int maxCount;
        public float produceCount;
        public float produceTime;
        public float defense;
        
        public float damage;
        public float speed;
        public GameObject prefab;
    }
}