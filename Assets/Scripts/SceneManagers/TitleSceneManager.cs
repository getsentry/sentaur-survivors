using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SceneManagers
{
    public class TitleSceneManager : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button quitButton;

        private Button _highlightedButton;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                SetHighlightedButton(startButton);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                SetHighlightedButton(quitButton);
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                _highlightedButton.onClick.Invoke();
            }
        }
        
        public void SetHighlightedButton(Button button)
        {
            startButton.GetComponent<Highlighter>().Highlight(false);
            quitButton.GetComponent<Highlighter>().Highlight(false);
        
            button.GetComponent<Highlighter>().Highlight();
            _highlightedButton = button;
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
