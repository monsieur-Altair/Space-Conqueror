using UnityEngine;

namespace Resources
{
    [CreateAssetMenu (fileName = "new rocket",menuName = "Resources/Rocket")]
    public class Rocket : ResourceUnit
    {
        public float damage;
        public float defense;
        public float speed;
        //public static GameObject RocketPrefab;
    }
}