using UnityEngine;

namespace Resources
{
    [CreateAssetMenu (fileName = "new scientific resource",menuName = "Resources/Scientific Resource")]
    public class Scientific:ScriptableObject
    {
        public new string name;

        public int maxCount;
        public float produceCount;
        public float produceTime;

        public Color color;
    }
}