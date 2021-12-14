using System;
using System.Collections;
using UnityEngine;

namespace Control
{
    public class Zone : MonoBehaviour
    {
        private SphereCollider _zone;
        public void Start()
        {
            _zone = GetComponent<SphereCollider>();
            if (_zone == null)
                throw new MyException("cannot get zone collider");
        }
        
        private void OnTriggerEnter(Collider other)
        {
            var obj = other.gameObject.GetComponent<Skills.IFreezable>();
            if (obj != null)
            {
                //Debug.Log("trigger");
                obj.Freeze();
                Skills.Ice.DeletingFreezingZone += obj.Unfreeze;
            }
        }
    }
    
}