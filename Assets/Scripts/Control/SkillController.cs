using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace Control
{
    internal enum SkillName
    {
        Buff=0,
        Acid,
        Ice,
        Call,
        None
    }
    
    public class SkillController : MonoBehaviour
    {
        [SerializeField] private List<Button> buttons;
        private const int Buff = 0, Acid = 1, Ice = 2, Call = 3;
        private Touch _touch;
        private SkillName _selectedSkillName;
        private Skills.Call _call;
        public static SkillController Instance;
        
        public Camera MainCamera { get; private set; }
        public float MinDepth { get; private set; }
        public float MaxDepth { get; private set; }
        public bool IsClickedSkill { get; private set; }

        public void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        private UnityAction<Button> _pressedButton;

        public void Start()
        {
            foreach (var button in buttons)
            {
                button.onClick.AddListener(() =>
                {
                    OnPressedButton(button);
                });
            }
            
            MainCamera=Camera.main;
            IsClickedSkill = false;
            _selectedSkillName = SkillName.None;
            
            _call = buttons[Call].GetComponent<Skills.Call>();
            
            MinDepth = MaxDepth = 0.0f;
            GetCameraDepths();
        }

        private void GetCameraDepths()
        {
            var plane = new Plane(Vector3.up, new Vector3(0, 0, MainCamera.nearClipPlane));
            var ray = MainCamera.ViewportPointToRay(new Vector3(0,0,0));
            if (plane.Raycast(ray, out var distance))
            {
                var botLeft = ray.GetPoint(distance);
                MinDepth = MainCamera.WorldToViewportPoint(botLeft).z;
            }
            //Debug.Log("left="+MainCamera.WorldToViewportPoint(botLeft));
            
            ray = MainCamera.ViewportPointToRay(new Vector3(1,1,0));
            if (plane.Raycast(ray, out var distance1))
            {
                var topRight = ray.GetPoint(distance1);
                MaxDepth = MainCamera.WorldToViewportPoint(topRight).z;
            }
            //Debug.Log("right="+MainCamera.WorldToViewportPoint(topRight));
        }
        
        public void HandleTouch(Touch touch)
        {
            _touch = touch;
           // Debug.Log(_touch.position);
           // Debug.Log("handle skill touch\n");
            
            switch (_touch.phase)
            {
                /*case TouchPhase.Began:
                {
                    HandleClick();
                    break;
                }*/
                case TouchPhase.Ended:
                {
                    HandleRelease();
                    break;
                }
            }
        }

        private static int value;
        
        private void HandleRelease()
        {
            //if doesn't work after canceling button, will add yield return
            if (IsClickedSkill)
            {
                var skill= ChooseSkill();
                //gameObject.GetComponent<Skills.Call>();
                skill.Execute(_touch.position);
                
                //Debug.Log("worked "+value++ +"\n");
                /////////////////////////////////////////////////////////////////////
                foreach (var button in buttons)
                {
                    UnBlockButton(button);
                }
                IsClickedSkill = false;   
            }
        }

        private Skills.ISkill ChooseSkill()
        {
            return _selectedSkillName switch
            {
                SkillName.Buff => null,
                SkillName.Acid => null,
                SkillName.Ice => null,
                SkillName.Call => _call,
                SkillName.None => null,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        /*
        private void HandleClick()
        {
            //
            
        }*/

        private void OnPressedButton(Button button)
        {
            if (!IsClickedSkill)
            {
                BlockButton(button);
                StartCoroutine(nameof(SwitchWithWaiting));
                //_pressedButton.Invoke(button);
            }
            else
            {
                /////////////////////////////////////////////////////////////////////
                foreach (var b in buttons)
                {
                    UnBlockButton(b);
                }
                IsClickedSkill = false;
            }
        }

        IEnumerator SwitchWithWaiting()
        {
            //if not use waiting, touch "handle release" will be worked immediately
            yield return new WaitForSeconds(0.1f);//0.1s is finger lift time
            IsClickedSkill = true;
        }

        private void BlockButton(Button button)
        {
            var index = buttons.IndexOf(button);
            _selectedSkillName = (SkillName)index;
           // Debug.Log(_selectedSkill);
            button.image.color=Color.red;
        }
        
        private void UnBlockButton(Button button)
        {
            button.image.color=Color.white;
            _selectedSkillName = SkillName.None;
        }
    }
}