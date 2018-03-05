using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

    private int score;      //player's total score
    private bool powerUp;       //replace with custom PowerUp class when we add powerUps TODO
    public Text scoreText;

	// Use this for initialization
	void Start () {
        score = 0;
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
        if (powerUp != null)
        {
            //powerUp.apply();
        }
    }
}
