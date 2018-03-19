using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

    private int score;      //player's total score
    public PowerUpParent powerUp;       //replace with custom PowerUp class when we add powerUps TODO
    public Text scoreText;
	//powerups bool
	private bool hotPotatoActive;
	private bool allOrNothingActive;
	private bool hardEarthActive;

	// Use this for initialization
	void Start () {
        score = 0;
        powerUp = null;
        hotPotatoActive = false;
        allOrNothingActive = false;
        hardEarthActive = false;
	}

	public bool HotPotatoActive{
		get{ return hotPotatoActive; }
		set{ hotPotatoActive = value; }
	}
	public bool AllOrNothingActive{
		get{ return allOrNothingActive; }
		set{ allOrNothingActive = value; }
	}
	public bool HardEarthActive{
		get{ return hardEarthActive; }
		set{ hardEarthActive = value; }
	}
	
	// Update is called once per frame
	void Update () {
        //print the player's score to the screen TODO
        scoreText.text = score.ToString();
	}

    //animate the player movement. Empty for now, may be moved to GameManager depending on how we handle it TODO
    public void Animate()
    {

    }

    //add points gotten to the player's score
    public void ApplyScore(int points)
    {
        Debug.Log("Gained " + points + " points");
        score += points;
    }

    //apply the bomb effect
    public void Bomb()
    {
        Debug.Log("BOOM! Lose half your score");
        score = (int) Mathf.Ceil((float)(score / 2.0));
    }

    public int getScore()
    {
        return score;
    }

    //If the player has a powerup, use it
    public void ApplyPowerUp()
    {
        //get gamemanager script
        GameManagerScript gameManager = (GameManagerScript) GameObject.FindGameObjectWithTag("GameController").GetComponent("GameManagerScript");

        //send this player as the targeted player to the gamemanager
        gameManager.ApplyPowerup(this.gameObject);
    }
}
