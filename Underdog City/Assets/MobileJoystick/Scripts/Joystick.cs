using UnityEngine;
using UnityEngine.EventSystems;

namespace DitzeGames.MobileJoystick
{

    /// <summary>
    /// Put it on any Image UI Element
    /// </summary>
    public class Joystick : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        protected Vector2 nullPosition;
        protected RectTransform Background;
        protected bool Pressed;
        protected int PointerId;
        public RectTransform Handle;
        [Range(0f,2f)]
        public float HandleRange = 1f;

        [HideInInspector]
        public Vector2 InputVector = Vector2.zero;
        public Vector2 AxisNormalized { get { return InputVector.magnitude > 0.25f ? InputVector.normalized : (InputVector.magnitude < 0.01f ? Vector2.zero : InputVector * 4f); } }

        void Start()
        {
            if (Handle == null)
                Handle = transform.GetChild(0).GetComponent<RectTransform>();
            Background = GetComponent<RectTransform>();
            Background.pivot = new Vector2(0.5f, 0.5f);
            nullPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, Background.position);
            Pressed = false;
        }

        void Update()
        {
            if (Pressed)
            {
                Vector2 direction = (PointerId >= 0 && PointerId < Input.touches.Length) ? Input.touches[PointerId].position - nullPosition : new Vector2(Input.mousePosition.x, Input.mousePosition.y) - nullPosition;
                InputVector = (direction.magnitude > Background.sizeDelta.x / 2f) ? direction.normalized : direction / (Background.sizeDelta.x / 2f);
                Handle.anchoredPosition = (InputVector * Background.sizeDelta.x / 2f) * HandleRange;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Pressed = true;
            PointerId = eventData.pointerId;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Pressed = false;
            InputVector = Vector2.zero;
            Handle.anchoredPosition = Vector2.zero;
        }
    }
}
