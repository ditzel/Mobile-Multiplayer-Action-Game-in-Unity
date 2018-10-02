using UnityEngine;
using UnityEngine.EventSystems;

namespace DitzeGames.MobileJoystick
{
    /// <summary>
    /// Put it on any Image UI Element
    /// </summary>
    public class TouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [HideInInspector]
        public Vector2 TouchDist;
        [HideInInspector]
        protected Vector2 PointerOld;
        [HideInInspector]
        protected int PointerId;
        [HideInInspector]
        public bool Pressed;

        // Update is called once per frame
        void Update()
        {
            if (Pressed)
            {
                if (PointerId >= 0 && PointerId < Input.touches.Length)
                {
                    TouchDist = Input.touches[PointerId].position - PointerOld;
                    PointerOld = Input.touches[PointerId].position;
                }
                else
                {
                    TouchDist = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - PointerOld;
                    PointerOld = Input.mousePosition;
                }
            }
            else
            {
                TouchDist = new Vector2();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Pressed = true;
            PointerId = eventData.pointerId;
            PointerOld = eventData.position;
        }


        public void OnPointerUp(PointerEventData eventData)
        {
            Pressed = false;
        }

    }
}