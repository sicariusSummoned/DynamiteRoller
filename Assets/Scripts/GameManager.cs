using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public PlayerScript[] Players = new PlayerScript[4];
    private int ActivePlayer;
    private List<PickUpScript> Deck;
    public GameObject activePanel;
    public GameObject waitingPanel;

	// Use this for initialization
	void Start () {
        ActivePlayer = 0;

        Deck = new List<PickUpScript>
        {
            Capacity = 5
        };
        Refill();
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

    public void PickSome(int picks)
    {
        for(int i = 0; i < picks; i++)
        {
            Deck[0].ApplyScore(Players[ActivePlayer]);
            Deck.RemoveAt(0);
        }

        //call it here or update?
        //Refill();
    }

    private void Refill()
    {
        int initCount = Deck.Count;

        for(int i = 0; i < Deck.Capacity - initCount; i++)
        {
            Deck.Add(GenPickup());
        }

        //just call this here?
        //SwitchActive();
    }

    private PickUpScript GenPickup()
    {
        float randVal = Random.value;

        int roll = Mathf.CeilToInt(randVal * 100);

        if(roll <= 36)
        {
            return new Gem1Script();
        }

        else if(roll > 36 && roll <= 58)
        {
            return new Gem2Script();
        }

        else if(roll > 58 && roll <= 80)
        {
            return new BombScript();
        }

        else if(roll > 80 && roll <= 94)
        {
            return new Gem3Script();
        }

        else if(roll > 94)
        {
            return new Gem5Script();
        }

        else
        {
            return new BombScript();
        }
    }

    public PlayerScript getActive()
    {
        return Players[ActivePlayer];
    }
    
}
