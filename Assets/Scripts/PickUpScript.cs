using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

abstract public class PickUpScript : MonoBehaviour {

    protected int value;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public abstract void ApplyScore(PlayerScript player);
    public abstract string GetType();
}
