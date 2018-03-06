using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class PowerUpParent : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//do the power up action and take reference to whatever player has been picked to have 
	//the actiion preformed upon.
	public abstract void UsePowerUp (int playerNum);
}
