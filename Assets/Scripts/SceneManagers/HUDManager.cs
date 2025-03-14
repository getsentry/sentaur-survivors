using UI;
using UnityEngine;
using UnityEngine.UI;

namespace SceneManagers
{
    public class HUDManager : MonoBehaviour
    {
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
            _tryAgainHighlighter = tryAgainButton.GetComponent<Highlighter>();
            _quitHighlighter = quitButton.GetComponent<Highlighter>();
            _cheatLittleHighlighter = cheatLittleButton.GetComponent<Highlighter>();
            _cheatLotHighlighter = cheatLotButton.GetComponent<Highlighter>();
        }
        
        private void Update()
        {
            if (!_quitHighlighter.isActiveAndEnabled)
            {
                return;
            }
            
            if (_highlightedButton == null)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
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
                else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                {
                    SetHighlightedButton(_quitHighlighter);
                }
            }
            else if (_highlightedButton == quitButton)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
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
                else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                {
                    SetHighlightedButton(_quitHighlighter);
                }
            }
            else if (_highlightedButton == tryAgainButton)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                {
                    SetHighlightedButton(_cheatLittleHighlighter);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                {
                    SetHighlightedButton(_quitHighlighter);
                }
            }
            else if (_highlightedButton == tryAgainButton)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                {
                    SetHighlightedButton(_cheatLittleHighlighter);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                {
                    SetHighlightedButton(_quitHighlighter);
                }
            }
            else if (_highlightedButton == cheatLittleButton || _highlightedButton == cheatLotButton)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                {
                    SetHighlightedButton(_cheatLittleHighlighter);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                {
                    SetHighlightedButton(_cheatLotHighlighter);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
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

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                _highlightedButton.GetComponent<Button>().onClick.Invoke();
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
