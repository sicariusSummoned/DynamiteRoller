using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    private int score;      //player's total score
    private bool powerUp;       //replace with custom PowerUp class when we add powerUps TODO

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//print the player's score to the screen TODO

	}

    //animate the player movement. Empty for now, may be moved to GameManager depending on how we handle it TODO
    public void Animate()
    {

    }

    //add points gotten to the player's score
    public void ApplyScore(int points)
    {
        score += points;
    }

    //apply the bomb effect
    public void Bomb()
    {
        score = (int) Mathf.Ceil((float)(score / 2.0));
    }
}
