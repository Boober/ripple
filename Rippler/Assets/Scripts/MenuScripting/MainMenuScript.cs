using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("Level2", LoadSceneMode.Single);
        }
	
	}
}
