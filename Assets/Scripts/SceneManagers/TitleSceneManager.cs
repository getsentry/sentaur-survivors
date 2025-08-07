using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SceneManagers
{
    public class TitleSceneManager : MonoBehaviour
    {
        private InputAction _navigateAction;
        
        [SerializeField] private GameObject startButton;
        [SerializeField] private GameObject quitButton;
        
        private Highlighter _startHighlighter;
        private Highlighter _quitHighlighter;

        private GameObject _highlightedButton;

        private void Awake()
        {
            _navigateAction = InputSystem.actions.FindAction("Navigate");
            _startHighlighter = startButton.GetComponent<Highlighter>();
            _quitHighlighter = quitButton.GetComponent<Highlighter>();
        }

        public void OnNavigate()
        {
            if (!_navigateAction.IsPressed())
            {
                return;
            }
            
            var direction = _navigateAction.ReadValue<Vector2>();
            if (direction.x < 0)
            {
                SetHighlightedButton(_startHighlighter);
            }
            else if (direction.x > 0)
            {
                SetHighlightedButton(_quitHighlighter);
            }
        }

        public void OnSubmit() => _highlightedButton?.GetComponent<Button>().onClick.Invoke();

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
        
        private void UnityOfBugs()
        {
            Debug.Log("Loading UnityOfBugs");
            SceneManager.LoadScene("1_Bugfarm", LoadSceneMode.Single);
        }
    }
}
