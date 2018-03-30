using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

    public TextAlignment instructions;
    public GameObject BackButton;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void playButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void instructionsButton()
    {
        SceneManager.LoadScene("InstructionScene");
    }

    public void creditsButton()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void backButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void quitButton()
    {
        Application.Quit();
    }

}
