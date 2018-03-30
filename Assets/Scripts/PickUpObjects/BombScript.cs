using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : PickUpScript {

	// Use this for initialization
	void Start () {
        value = 0;
	}
	
	// Update is called once per frame
	void Update () {
		//display bomb text on the object TODO
	}

    public override void ApplyScore(PlayerScript player)
    {
        player.Bomb();
    }

    public override string GetType()
    {
        return "Bomb";
    }
}
