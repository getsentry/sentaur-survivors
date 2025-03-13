using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject _startButton;

    [SerializeField] private GameObject _quitButton;

    private GameObject selectedButton; 

    void Awake()
    {
        _startButton.GetComponent<Button>().onClick.AddListener(StartGame);
        _quitButton.GetComponent<Button>().onClick.AddListener(QuitGame);
    }

    private void Start()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_startButton);
        _startButton = _startButton;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_startButton);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_quitButton);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (selectedButton == _startButton)
            {
                StartGame();
            }
            else
            {
                QuitGame();
            }
        }
    }

    private void StartGame()
    {
        Debug.Log("Start Game");
        SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);
    }

    private void QuitGame()
    {
        Debug.Log("Quit (Note this won't quit in the editor)");
        Application.Quit();
    }
}
