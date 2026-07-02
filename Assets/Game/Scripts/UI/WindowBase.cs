using UnityEngine;

namespace Game.Windows
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class WindowBase : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [field: SerializeField] public bool IsModal { get; private set; }

        public bool IsOpen { get; private set; }

        #if UNITY_EDITOR
        private void Reset()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        #endif

        protected virtual void Awake()
        {
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void Open()
        {
            IsOpen = true;
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            gameObject.SetActive(true);
            OnOpened();
        }

        public virtual void Close()
        {
            IsOpen = false;
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            OnClosed();
        }

        protected virtual void OnOpened()
        {
        }
        protected virtual void OnClosed()
        {
        }
    }
}