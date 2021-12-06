using System;
using UnityEngine;

namespace Planets
{
    public class ZoneController : MonoBehaviour
    {
        private SphereCollider _attackZone;

        public void Start()
        {
            _attackZone = GetComponent<SphereCollider>();
            if (_attackZone == null)
                throw new MyException("cannot get attack zone collider");
        }

        /*private void OnTrig
        {
            Debug.Log("Enter");
        }*/


        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Enter");
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log("Exit");
        }
    }
}