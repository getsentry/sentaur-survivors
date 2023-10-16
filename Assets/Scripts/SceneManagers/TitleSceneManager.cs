using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _startButton;

    [SerializeField]
    private GameObject _quitButton;

    void Awake() {
        _startButton.GetComponent<Button>().onClick.AddListener(StartGame);
        _quitButton.GetComponent<Button>().onClick.AddListener(QuitGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartGame() {
        Debug.Log("Start Game");
        SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);

    }

    private void QuitGame() {
        Debug.Log("Quit (Note this won't quit in the editor)");
        Application.Quit();
    }
}
