
using System;
using HarmonyLib;
using UnityEngine;
using Object = UnityEngine.Object;
using UnityEngine.EventSystems;

namespace UnityGameUI
{
    // 窗口拖动相关注入
    public class WindowDragHandler : MonoBehaviour
    {
        public static WindowDragHandler instance;
        private const int NON_EXISTING_TOUCH = -98456;
        private static RectTransform rectTransform;
        private static int pointerId = NON_EXISTING_TOUCH;
        private static Vector2 initialTouchPos;

        public WindowDragHandler()
        {
            instance = this;
        }

        public void Awake()
        {
            rectTransform = gameObject.GetComponent<RectTransform>();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(EventTrigger), nameof(EventTrigger.OnBeginDrag))]
        public static void OnBeginDrag(PointerEventData eventData)
        {

            if (pointerId != NON_EXISTING_TOUCH)
            {
                eventData.pointerDrag = null;
                return;
            }

            pointerId = eventData.pointerId;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, Camera.current, out initialTouchPos);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(EventTrigger), nameof(EventTrigger.OnDrag))]
        public static void OnDrag(PointerEventData eventData)
        {

            if (eventData.pointerId != pointerId)
                return;

            Vector2 touchPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, Camera.current, out touchPos);

            var tmp = touchPos - initialTouchPos;
            rectTransform.gameObject.transform.position += new Vector3(tmp.x, tmp.y, Camera.current.nearClipPlane);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(EventTrigger), nameof(EventTrigger.OnEndDrag))]
        public static void OnEndDrag(PointerEventData eventData)
        {

            if (eventData.pointerId != pointerId)
                return;

            pointerId = NON_EXISTING_TOUCH;
        }

    }
}
