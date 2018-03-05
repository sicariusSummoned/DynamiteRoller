﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour {

    public PlayerScript[] Players = new PlayerScript[4];
    private int ActivePlayer;
    private List<GameObject> Deck;
    public GameObject activePanel;
    public GameObject waitingPanel;
    private bool firstTime;

	// Use this for initialization
	void Start () {
        ActivePlayer = 0;

        Deck = new List<GameObject>
        {
            Capacity = 5
        };
        Refill();

        firstTime = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SwitchActive()
    {
        // TODO: move current active down
        if(ActivePlayer < 3)
        {
            ActivePlayer++;
        }
        else
        {
            ActivePlayer = 0;
        }

        Debug.Log("It is now Player " + (ActivePlayer + 1) + "'s turn.");
        Debug.Log("Player " + (ActivePlayer + 1) + "'s current score is " + Players[ActivePlayer].getScore());

        // TODO: move new active up

        //move the gameobjects between holders
        //get the active player currently in activePanel
        Transform activePlayer = activePanel.transform.GetChild(0);

        //get the next player in line in waiting dwarfs
        Transform waitingPlayer = waitingPanel.transform.GetChild(0);

        //move the active player into the waiting queue
        activePlayer.SetParent(waitingPanel.transform);

        //move the waiting player into active
        waitingPlayer.SetParent(activePanel.transform);
    }

    //active player gain points
    public void PickSome(int picks)
    {
        //grab front, reset current front to last one.
        for(int i = 0; i < picks; i++)
        {
            Deck[0].GetComponent<PickUpScript>().ApplyScore(Players[ActivePlayer]);
            Destroy(Deck[0]);
            Deck.RemoveAt(0);
        }

        //call it here or update?
        Refill();
    }

    private void Refill()
    {
        //not sure when the value is checked so this way when the count changes we don't mess with the loop
        int initCount = Deck.Count;

        for(int i = 0; i < Deck.Capacity - initCount; i++)
        {
            //make new pickup
            Deck.Add(GenPickup());
        }

        //just call this here?
        if (!firstTime)
        {
            SwitchActive();
        }

        else
        {
            Debug.Log("It is now Player " + (ActivePlayer + 1) + "'s turn.");

            firstTime = false;
        }

    }

    private GameObject GenPickup()
    {
        float randVal = Random.value;

        GameObject newObj = new GameObject("pickup");

        int roll = Mathf.CeilToInt(randVal * 100);

        if(roll <= 36)
        {
            newObj.AddComponent<Gem1Script>();
        }

        else if(roll > 36 && roll <= 58)
        {
            newObj.AddComponent<Gem2Script>();
        }

        else if(roll > 58 && roll <= 80)
        {
            newObj.AddComponent<BombScript>();
        }

        else if(roll > 80 && roll <= 94)
        {
            newObj.AddComponent<Gem3Script>();
        }

        else if(roll > 94)
        {
            newObj.AddComponent<Gem5Script>();
        }

        return newObj;
    }

    public PlayerScript getActive()
    {
        return Players[ActivePlayer];
    }
    
}