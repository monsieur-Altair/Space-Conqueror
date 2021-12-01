using System.Linq.Expressions;
using UnityEngine;

namespace Units
{
    public class Rocket : Base
    {
        
        
        protected override void TargetInRange()
        {
            Destroy(gameObject);
        }
        
    }
}