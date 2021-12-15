using UnityEngine;

namespace Skills
{
    public class Call : Base
    {
        public float BuffPercent { get; private set; }

        protected override void LoadResources()
        {
            base.LoadResources();
            var res = resource as Resources.Buff;
            if (res != null)
                BuffPercent = res.buffPercent;
        }
      
        protected override void ApplySkill(Vector3 pos)
        {
            Planet = RaycastForPlanet(pos);
            if (Planet != null)
            {
                CallSupply(Planet);
                IsOnCooldown = true;
                Planets.Scientific.DecreaseScientificCount(Cost);
                Invoke(nameof(CancelSkill), Cooldown);
            }
            else
            {
                UnblockButton();
            }
        }

        protected override void CancelSkill()
        {
            IsOnCooldown = false;
            UnblockButton();
        }
        
        private void CallSupply(Planets.Base destinationPlanet)
        {
            var launchPos= FindSpawnPoint(destinationPlanet);
            var destPos = CalculateDestPos(launchPos, destinationPlanet);
            var unit = ObjectPool.GetObject(destinationPlanet.Type, 
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
            return destPos - offset * destinationPlanet.Radius;
        }
        
        //calculate a min way on SCREEN (NOT WORLD) coordinates for supply
        private Vector3 FindSpawnPoint(Planets.Base destination)
        {
            var destPosWorld = destination.transform.position;
            var destPosScreen = MainCamera.WorldToScreenPoint(destPosWorld);
            var destX = destPosScreen.x;
            var destY = destPosScreen.y;
            var destZ = destPosScreen.z;

            //possible points for spawn on screen coordinates
            Vector3[] possiblePoints =
            {
                new Vector3(0,destY,destZ),                                  //from left side
                new Vector3(destX,Screen.height,Control.SkillController.MaxDepth), //from top side
                new Vector3(Screen.width,destY,destZ),                       //from right side
                new Vector3(destX,0,Control.SkillController.MinDepth)              //from bottom side
            };

            var minWayIndex = FindMinWay(in possiblePoints,in destPosScreen);
            
            var result=MainCamera.ScreenToWorldPoint(possiblePoints[minWayIndex]);
            result.y = destPosWorld.y;
            return result;
        }

        //find launch point by calculating distance between possible points and destination point
        private static int FindMinWay(in Vector3[] possiblePoints, in Vector3 destinationPos)
        {            
            int minIndex = 0;
            var min = float.MaxValue;
            int index = 0;
            foreach (var point in possiblePoints)
            {
                var distance = Vector2.Distance(point, destinationPos);
                
                if (distance < min)
                {
                    min = distance;
                    minIndex = index;
                }
                index++;
            }

            return minIndex;
        }
        
    }
}