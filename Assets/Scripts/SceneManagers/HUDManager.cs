using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace SceneManagers
{
    public class HUDManager : MonoBehaviour
    {
        private InputAction _navigateAction;
        private InputAction _submitAction;
        
        [SerializeField] private GameObject tryAgainButton;
        [SerializeField] private GameObject quitButton;
        [SerializeField] private GameObject cheatLittleButton;
        [SerializeField] private GameObject cheatLotButton;
        
        private Highlighter _tryAgainHighlighter;
        private Highlighter _quitHighlighter;
        private Highlighter _cheatLittleHighlighter;
        private Highlighter _cheatLotHighlighter;

        private GameObject _highlightedButton;

        private void Awake()
        {
            _navigateAction = InputSystem.actions.FindAction("Navigate");
            _submitAction = InputSystem.actions.FindAction("Submit");
            
            // Subscribe to the performed callback only
            _submitAction.performed += OnSubmitPerformed;
            
            _tryAgainHighlighter = tryAgainButton.GetComponent<Highlighter>();
            _quitHighlighter = quitButton.GetComponent<Highlighter>();
            _cheatLittleHighlighter = cheatLittleButton.GetComponent<Highlighter>();
            _cheatLotHighlighter = cheatLotButton.GetComponent<Highlighter>();
        }

        private void OnSubmitPerformed(InputAction.CallbackContext context)
        {
            if (!gameObject.activeSelf)
            {
                return;
            }
            
            _highlightedButton?.GetComponent<Button>().onClick.Invoke();
        }

        public void OnNavigate()
        {
            if (!gameObject.activeSelf)
            {
                return;
            }
            
            if (!_quitHighlighter.isActiveAndEnabled)
            {
                return;
            }
         
            if (!_navigateAction.IsPressed())
            {
                return;
            }
            
            var direction = _navigateAction.ReadValue<Vector2>();
            
            if (_highlightedButton == null)
            {
                if (direction.x < 0)
                {
                    if (_tryAgainHighlighter.isActiveAndEnabled)
                    {
                        SetHighlightedButton(_tryAgainHighlighter);    
                    }
                    else
                    {
                        SetHighlightedButton(_cheatLittleHighlighter);
                    }
                }
                else if (direction.x > 0)
                {
                    SetHighlightedButton(_quitHighlighter);
                }
            }
            else if (_highlightedButton == quitButton)
            {
                if (direction.x < 0)
                {
                    if (_tryAgainHighlighter.isActiveAndEnabled)
                    {
                        SetHighlightedButton(_tryAgainHighlighter);    
                    }
                    else
                    {
                        SetHighlightedButton(_cheatLittleHighlighter);
                    }
                }
                else if (direction.x > 0)
                {
                    SetHighlightedButton(_quitHighlighter);
                }
            }
            else if (_highlightedButton == tryAgainButton)
            {
                if (direction.x < 0)
                {
                    SetHighlightedButton(_cheatLittleHighlighter);
                }
                else if (direction.x > 0)
                {
                    SetHighlightedButton(_quitHighlighter);
                }
            }
            else if (_highlightedButton == tryAgainButton)
            {
                if (direction.x < 0)
                {
                    SetHighlightedButton(_cheatLittleHighlighter);
                }
                else if (direction.x > 0)
                {
                    SetHighlightedButton(_quitHighlighter);
                }
            }
            else if (_highlightedButton == cheatLittleButton || _highlightedButton == cheatLotButton)
            {
                if (direction.y > 0)
                {
                    SetHighlightedButton(_cheatLittleHighlighter);
                }
                else if (direction.y < 0)
                {
                    SetHighlightedButton(_cheatLotHighlighter);
                }
                else if (direction.x > 0)
                {
                    if (_tryAgainHighlighter.isActiveAndEnabled)
                    {
                        SetHighlightedButton(_tryAgainHighlighter);    
                    }
                    else
                    {
                        SetHighlightedButton(_quitHighlighter);
                    }
                }
            }
        }

        public void SetHighlightedButton(Highlighter highlighted)
        {
            _tryAgainHighlighter.Highlight(false);
            _quitHighlighter.Highlight(false);
            _cheatLittleHighlighter.Highlight(false);
            _cheatLotHighlighter.Highlight(false);
        
            highlighted.Highlight();
            _highlightedButton = highlighted.gameObject;
        }
    }
}
