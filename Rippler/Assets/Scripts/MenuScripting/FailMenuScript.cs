using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class FailMenuScript : MonoBehaviour {

    public Button continueText;
    public Button exitText;
    public EventSystem eventhandler;

    //User pressed the exit button, quit the game.
    public void ExitPress()
    {
        Application.Quit();
    }
    //User pressed the continue button, restart the level.
    public void ContinuePress()
    {
        SceneManager.LoadScene("Level2",LoadSceneMode.Single);
    }

}
