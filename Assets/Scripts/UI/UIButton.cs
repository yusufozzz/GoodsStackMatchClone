using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class UIButton: UIElement
    {
        private Button _button;
        
        private Button Button
        {
            get
            {
                if (_button == null)
                {
                    _button = GetComponent<Button>();
                    if (_button == null)
                    {
                        Debug.LogError("UIButton: Button component not found on the GameObject.");
                    }
                }
                return _button;
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            if (Button != null)
            {
                Button.onClick.AddListener(OnClick);
            }
        }

        protected virtual void OnClick()
        {
            
        }
        
        public override void OnDisable()
        {
            base.OnDisable();
            if (Button != null)
            {
                Button.onClick.RemoveListener(OnClick);
            }
        }
    }
}