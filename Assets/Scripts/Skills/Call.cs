using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;


namespace Skills
{
    [DefaultExecutionOrder(1000)]
    public class Call : MonoBehaviour, ISkill
    {
        private Control.SkillController _skillController;
        private Camera _mainCamera;
        private Managers.ObjectPool _objectPool;
        public void Start()
        {
            _skillController = Control.SkillController.Instance;
            if (_skillController == null)
                throw new MyException("can't get skill controller");
            _mainCamera=_skillController.MainCamera;
            if(_mainCamera==null)
                throw new MyException("can't get main camera");
            _objectPool = Managers.ObjectPool.Instance;
            if (_objectPool == null)
                throw new MyException("can't get object pool");
        }

        public void Execute(Vector3 pos)
        {
            var planet= RaycastForPlanet(pos);
            if (planet != null)
            {
                planet.isMove = !planet.isMove;
                CallSupply(planet);
            }
        }

        private Planets.Base RaycastForPlanet(Vector3 pos)
        {
            //Debug.Log("vector="+pos);
            //Debug.Log("camera=null"+(_skillController==null));
            var ray = _mainCamera.ScreenPointToRay(pos);
            return Physics.Raycast(ray, out var hit) ? hit.collider.GetComponentInParent<Planets.Base>() : null;
        }

        private void CallSupply(Planets.Base destinationPlanet)
        {
            var launchPos= FindSpawnPoint(destinationPlanet);
            var destPos = CalculateDestPos(launchPos, destinationPlanet);
            var unit = _objectPool.GetObject(destinationPlanet.Type, 
                launchPos, 
                Quaternion.LookRotation(destPos-launchPos))
                .GetComponent<Units.Base>();
            destinationPlanet.AdjustUnit(unit);
            unit.GoTo(destinationPlanet, destPos);
        }

        private Vector3 CalculateDestPos(in Vector3 launchPos, Planets.Base destinationPlanet)
        {
            var destPos = destinationPlanet.transform.position;
            var offset = (destPos - launchPos).normalized;
            /*Debug.Log("dest="+destPos);
            Debug.Log("launch="+launchPos);*/
            return destPos - offset * destinationPlanet.Radius;
        }
        
        //calculate a min way on SCREEN (NOT WORLD) coordinates for supply
        private Vector3 FindSpawnPoint(Planets.Base destination)
        {
            var destPosWorld = destination.transform.position;
            var destPosScreen = _mainCamera.WorldToScreenPoint(destPosWorld);
            var destX = destPosScreen.x;
            var destY = destPosScreen.y;
            var destZ = destPosScreen.z;

            /*
            Debug.Log("screen = "+destPosScreen);
            Debug.Log("world="+destPosWorld);
            Debug.Log("max depth ="+_skillController.MaxDepth);
            Debug.Log("min depth ="+_skillController.MinDepth);
            */
            
            /*
            Vector3[] possiblePoints =
            {
                _mainCamera.ScreenToWorldPoint(new Vector3(0,destY,destZ)) ,//from left side
                _mainCamera.ScreenToWorldPoint(new Vector3(destX,Screen.height,_skillController.MaxDepth)) ,//from top side
                _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width,destY,destZ)) ,//from right side
                _mainCamera.ScreenToWorldPoint(new Vector3(destX,0,_skillController.MinDepth)) //from bottom side
            };
            */
            
            //possible points for spawn on screen coordinates
            Vector3[] possiblePoints =
            {
                new Vector3(0,destY,destZ),                                     //from left side
                new Vector3(destX,Screen.height,_skillController.MaxDepth),   //from top side
                new Vector3(Screen.width,destY,destZ),                          //from right side
                new Vector3(destX,0,_skillController.MinDepth)                //from bottom side
            };


            /*
            foreach (var p in possiblePoints)
            {
                Debug.Log("pos = "+p);
            }*/
            
            var min = float.MaxValue;
            int index = 0;
            int minIndex = 0;
            
            foreach (var point in possiblePoints)
            {
                var distance = Vector2.Distance(point, destPosScreen);
                
                if (distance < min)
                {
                    min = distance;
                    minIndex = index;
                }
                index++;
            }
            
            // return _mainCamera.ScreenToWorldPoint(possiblePoints[minIndex]);
            var result=_mainCamera.ScreenToWorldPoint(possiblePoints[minIndex]);
            result.y = destPosWorld.y;
            return result;
            //return possiblePoints[minIndex];
        }
    }
}