using UnityEngine;

namespace Resources
{
    public abstract class ResourceUnit:ScriptableObject
    {
        public new string name;
    
        public int max;
        public float produceCount;
        public float produceTime;


        
    }
}