using System;
using DG.Tweening;
using GameManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UICanvas: UIElement
    {
        private Tween _fadeTween;

        [SerializeField]
        private TMP_Text titleText;
        
        [SerializeField]
        private CanvasGroup canvasGroup;

        private UISettings UISettings => GameSettings.Instance.UISettings;

        public virtual void Show()
        {
            _fadeTween?.Kill();
            _fadeTween = canvasGroup.DOFade(1f, UISettings.CanvasShowHideDuration)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    canvasGroup.interactable = true;
                    canvasGroup.blocksRaycasts = true;
                });
        }
        
        public virtual void Hide()
        {
            _fadeTween?.Kill();
            _fadeTween = canvasGroup.DOFade(0f, UISettings.CanvasShowHideDuration)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    canvasGroup.interactable = false;
                    canvasGroup.blocksRaycasts = false;
                });
        }
        
        protected virtual void SetTitle(string title)
        {
            if (titleText != null)
            {
                titleText.text = title;
            }
        }

        private void Reset()
        {
            if (titleText == null)
            {
                titleText = GetComponentInChildren<TMP_Text>();
                if (titleText == null)
                {
                    Debug.LogError("UICanvas: Title Text component not found in children.");
                }
            }

            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    Debug.LogError("UICanvas: CanvasGroup component not found on the GameObject.");
                }
            }
        }
    }
}