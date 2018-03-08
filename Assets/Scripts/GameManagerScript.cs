using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour {

    public PlayerScript[] Players = new PlayerScript[4];
    private int ActivePlayer;
    private List<GameObject> Deck;
    public GameObject activePanel;
    public GameObject waitingPanel;
    private bool firstTime = true;
    private int roundCounter = 0;
    private bool gameOver = false;
    public GameObject HotPotato;
    public GameObject AllOrNothing;
    public GameObject HardEarth;


	public Button take2;
	public Button take3;
	public Button take4;

	public GameObject targetPlayer; 


	// Use this for initialization
	void Start () {
        ActivePlayer = 0;

        Deck = new List<GameObject>
        {
            Capacity = 5
        };
        Refill();

        //Debug.Log("GAME START!");
        //Debug.Log("It is now Player " + (ActivePlayer + 1) + "'s turn.");
        //Debug.Log("Player " + (ActivePlayer + 1) + "'s current score is " + Players[ActivePlayer].getScore());
        Debug.Log("The visible Gems are: " + Deck[0].name + ", " + Deck[1].name + ", " + Deck[2].name);


        firstTime = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SwitchActive()
    {
		if (targetPlayer.GetComponent<PlayerScript> ().AONActive == true) {
			take2.interactable = false;
			take3.interactable = false;
			take4.interactable = false;
		}
        if (ActivePlayer < 3)
        {
            ActivePlayer++;
        }
        else
        {
            roundCounter++;
            ActivePlayer = 0;

            if(roundCounter >= 4)
            {
                gameOver = true;
            }
            else
            {
                //give players a powerup
                for (int i = 0; i < Players.Length; i++)
                {
                    //get a random powerup
                    int powerUpNum = Random.Range(0, 3);

                    if(powerUpNum == 0)
                    {
                        Players[i].powerUp = (HotPotato) HotPotato.GetComponent("HotPotato");
                    }
                    else if (powerUpNum == 1)
                    {
                        Players[i].powerUp = (HardEarth) HardEarth.GetComponent("HardEarth");
                    }
                    else
                    {
                        Players[i].powerUp = (AllOrNothing) AllOrNothing.GetComponent("AllOrNothing");
                    }
                }
            }
			take2.interactable = true;
			take3.interactable = true;
			take4.interactable = true;
        }


        if (!gameOver)
        {
            //Debug.Log("It is now Player " + (ActivePlayer + 1) + "'s turn.");
            //Debug.Log("Player " + (ActivePlayer + 1) + "'s current score is " + Players[ActivePlayer].getScore());
            Debug.Log("The visible Gems are: " + Deck[0].name + ", " + Deck[1].name + ", " + Deck[2].name);

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

        else if(gameOver)
        {
            endGame();
        }
    }

	public void CallHardEarthP(){
		GameObject.Find ("HardEarth").GetComponent<HardEarth> ().UsePowerUp (targetPlayer);
	}
	public void CallAONP(){
		GameObject.Find ("AllOrNothing").GetComponent<AllOrNothing> ().UsePowerUp (targetPlayer);
	}
	public void CallHotPotatoP(){
		GameObject.Find ("HotPotato").GetComponent<HotPotato> ().UsePowerUp (targetPlayer);
	}

    //active player gain points
    public void PickSome(int picks)
    {
        //grab front, reset current front to last one.
        for(int i = 0; i < picks; i++)
        {
			//hard earth is active, ignore first one priority
			if (targetPlayer.GetComponent<PlayerScript> ().HardEarthActive == true) {
				targetPlayer.GetComponent<PlayerScript> ().HardEarthActive = false;
			} else {
				//I need the type for just checking bombs
				//if (Deck [0].GetType (BombScript) == true) {
				//	Deck[0].GetComponent<PickUpScript>().ApplyScore(Players[targetPlayer]);
				//}
				Deck[0].GetComponent<PickUpScript>().ApplyScore(Players[ActivePlayer]);
			}
            Destroy(Deck[0]);
            Deck.RemoveAt(0);
        }

        //Debug.Log("Player " + (ActivePlayer + 1) + "'s current score is " + Players[ActivePlayer].getScore());

        firstTime = false;

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

    }

    private GameObject GenPickup()
    {
        float randVal = Random.value;

        GameObject newObj = new GameObject("pickup");

        int roll = Mathf.CeilToInt(randVal * 100);

        if(roll <= 36)
        {
            newObj.name = "1";
            newObj.AddComponent<Gem1Script>();
        }

        else if(roll > 36 && roll <= 58)
        {
            newObj.name = "2";
            newObj.AddComponent<Gem2Script>();
        }

        else if(roll > 58 && roll <= 80)
        {
            newObj.name = "BOMB!";
            newObj.AddComponent<BombScript>();
        }

        else if(roll > 80 && roll <= 94)
        {
            newObj.name = "3";
            newObj.AddComponent<Gem3Script>();
        }

        else if(roll > 94)
        {
            newObj.name = "5";
            newObj.AddComponent<Gem5Script>();
        }

        return newObj;
    }

    public PlayerScript getActive()
    {
        return Players[ActivePlayer];
    }

    private void endGame()
    {
        Debug.Log("Game over!");

        //disable all buttons
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("TakeButton");

        foreach(GameObject button in buttons)
        {
            button.GetComponent<Button>().interactable = false;
        }

        //report player(s) with most score
        int s1 = Players[0].getScore();
        int s2 = Players[1].getScore();
        int s3 = Players[2].getScore();
        int s4 = Players[3].getScore();

        if(s1 >= s2 && s1 >= s3 && s1 >= s4)
        {
            Debug.Log("RED wins with a score of " + s1);
        }

        if(s2 >= s1 && s2 >= s3 && s2 >= s4)
        {
            Debug.Log("BLUE wins with a score of " + s2);
        }

        if(s3 >= s1 && s3 >= s2 && s3 >= s4)
        {
            Debug.Log("YELLOW wins with a score of " + s3);
        }

        if(s4 >= s1 && s4 >= s2 && s4 >= s3)
        {
            Debug.Log("GREEN wins with a score of " + s4);
        }

    }
    
    //use the active player's powerup
    //TODO give the player a way to cancel the powerup activation without losing it
    //TODO get the desired target player
    public void usePowerUp()
    {
        Debug.Log("Active player's powerup is " + Players[ActivePlayer].powerUp);
    }
}
