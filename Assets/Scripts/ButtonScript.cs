using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {

    public int pullNum = 0;
    private GameManagerScript manager;

	// Use this for initialization
	void Start () {
		manager = (GameManagerScript)GameObject.Find("GameManager").GetComponent("GameManagerScript");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void pullFrom()
    {
        Debug.Log("Pulling " + pullNum +" for: " + manager.getActive());

        manager.PickSome(pullNum);
    }
}
