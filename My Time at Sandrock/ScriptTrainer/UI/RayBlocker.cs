using UnityEngine;
using UnityEngine.UI;

// 使用方法：
// 先 new一个RayBlocker
// 开启时调用OpenBlocker();
// 关闭时调用CloseBlocker();
// 设置大小调用SetSize(Rect rect)

namespace ScriptTrainer
{
    public class RayBlocker
    {
        private RectTransform rt;
        private GameObject canvasObj;

        private RayBlocker()
        {

        }

        public static RayBlocker CreateRayBlock()
        {
            var rayBlocker = new RayBlocker();

            rayBlocker.canvasObj = new GameObject("NextBlockerCanvas");
            rayBlocker.canvasObj.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            rayBlocker.canvasObj.AddComponent<GraphicRaycaster>();
            var gameObject = new GameObject("RayBlocker");
            rayBlocker.rt = gameObject.AddComponent<RectTransform>();
            rayBlocker.rt.SetParent(rayBlocker.canvasObj.transform);
            rayBlocker.rt.pivot = new Vector2(0, 1);
            Image rbImage = gameObject.AddComponent<Image>();
            rbImage.color = Color.clear;
            rbImage.raycastTarget = true;
            rayBlocker.CloseBlocker();

            GameObject.DontDestroyOnLoad(rayBlocker.canvasObj);

            return rayBlocker;
        }

        public void SetSize(Rect rect)
        {
            rt.sizeDelta = rect.size;
            rt.position = new Vector3(rect.position.x, Screen.height - rect.position.y);
        }

        public void OpenBlocker()
        {
            canvasObj.SetActive(true);
        }

        public void CloseBlocker()
        {
            canvasObj.SetActive(false);
        }
    }
}
