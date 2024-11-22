using UnityEngine;

namespace UI
{
    public class LeafletMenu : MonoBehaviour
    {
        public RectTransform menuPanel;
        public float animationDurationY = 0.3f;
        public float animationDurationX = 0.3f;
        public Vector2 closedSize = new Vector2(0, 0);
        public Vector2 intermediateSize = new Vector2(0, 300);
        public Vector2 openSize = new Vector2(400, 300);

        public bool isOpen = false;
        private Coroutine animationCoroutine;

        private void Start()
        {
            menuPanel.sizeDelta = Vector2.zero;
        }

        public void ToggleMenu()
        {
            if (animationCoroutine != null)
                StopCoroutine(animationCoroutine);

            if (isOpen)
                animationCoroutine = StartCoroutine(CloseMenu());
            else
                animationCoroutine = StartCoroutine(OpenMenu());

            isOpen = !isOpen;
        }

        private System.Collections.IEnumerator OpenMenu()
        {
            yield return AnimateSize(menuPanel.sizeDelta, intermediateSize, animationDurationY);

            yield return AnimateSize(menuPanel.sizeDelta, openSize, animationDurationX);
        }

        private System.Collections.IEnumerator CloseMenu()
        {
            yield return AnimateSize(menuPanel.sizeDelta, intermediateSize, animationDurationX);

            yield return AnimateSize(menuPanel.sizeDelta, closedSize, animationDurationY);
        }

        private System.Collections.IEnumerator AnimateSize(Vector2 startSize, Vector2 endSize, float duration)
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                menuPanel.sizeDelta = Vector2.Lerp(startSize, endSize, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            menuPanel.sizeDelta = endSize;
        }
    }
}