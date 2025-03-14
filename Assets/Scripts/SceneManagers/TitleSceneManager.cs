using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SceneManagers
{
    public class TitleSceneManager : MonoBehaviour
    {
        [SerializeField] private GameObject startButton;
        [SerializeField] private GameObject quitButton;
        
        private Highlighter _startHighlighter;
        private Highlighter _quitHighlighter;

        private GameObject _highlightedButton;

        private void Awake()
        {
            _startHighlighter = startButton.GetComponent<Highlighter>();
            _quitHighlighter = quitButton.GetComponent<Highlighter>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                SetHighlightedButton(_startHighlighter);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                SetHighlightedButton(_quitHighlighter);
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                _highlightedButton.GetComponent<Button>().onClick.Invoke();
            }
        }
        
        public void SetHighlightedButton(Highlighter highlighted)
        {
            _startHighlighter.Highlight(false);
            _quitHighlighter.Highlight(false);
        
            highlighted.Highlight();
            _highlightedButton = highlighted.gameObject;
        }
    
        public void StartGame()
        {
            Debug.Log("Start Game");
            SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);
        }

        public void QuitGame()
        {
            Debug.Log("Quit (Note this won't quit in the editor)");
            Application.Quit();
        }
    }
}
