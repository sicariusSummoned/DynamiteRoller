using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem3Script : PickUpScript {

    // Use this for initialization
    void Start()
    {
        value = 3;
    }

    // Update is called once per frame
    void Update()
    {
        //display score text on the object TODO
    }

    public override void ApplyScore(PlayerScript player)
    {
        player.ApplyScore(value);
    }
}
