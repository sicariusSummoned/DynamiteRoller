using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotPotato : PowerUpParent {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override GameObject UsePowerUp(GameObject player)
    {
		if (player.GetComponent<PlayerScript> () != null) {
			player.GetComponent<PlayerScript> ().HotPotatoActive = true;
		}
		return player;
    }
}
