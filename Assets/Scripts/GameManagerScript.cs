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
    public GameObject activeGems;
    public GameObject waitingGems;
    private bool firstTime = true;
    private int roundCounter = 0;
    private bool gameOver = false;

    private const float PAUSE_DUR = 0.8f;
    private const int DECK_CAP = 8;

	// Use this for initialization
	void Start () {
        ActivePlayer = 0;

        Deck = new List<GameObject>
        {
            Capacity = DECK_CAP
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
        GameObject.Find("AudioManager").GetComponent<AudioManagerScript>().numConsec = 0;

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

    //active player gain points
    public void PickSome(int picks)
    {
        StartCoroutine(TakeItems(picks));
    }

    private void Refill()
    {
        //not sure when the value is checked so this way when the count changes we don't mess with the loop
        int initCount = Deck.Count;

        for (int i = 0; i < Deck.Capacity - initCount; i++)
        {
            //make new pickup
            Deck.Add(GenPickup());

            if(i + initCount <= 2)
            {
                Deck[i + initCount].SetActive(true);
                Deck[i + initCount].transform.SetParent(activeGems.transform);
                Deck[i + initCount].transform.localScale = Vector3.one;
            }

            else
            {
                Deck[i + initCount].transform.SetParent(waitingGems.transform);
                Deck[i + initCount].transform.localScale = Vector3.one;
            }
        }

        //enable all buttons
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("TakeButton");

        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().interactable = true;
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

        GameObject newObj;

        int roll = Mathf.CeilToInt(randVal * 100);

        if(roll <= 36)
        {
            newObj = (GameObject)Instantiate(Resources.Load("Prefabs/Gem1"));
        }

        else if(roll > 36 && roll <= 58)
        {
            newObj = (GameObject)Instantiate(Resources.Load("Prefabs/Gem2"));
        }

        else if(roll > 58 && roll <= 80)
        {
            newObj = (GameObject)Instantiate(Resources.Load("Prefabs/Dynamite"));
        }

        else if (roll > 80 && roll <= 94)
        {
            newObj = (GameObject)Instantiate(Resources.Load("Prefabs/Gem3"));
        }

        else if (roll > 94)
        {
            newObj = (GameObject)Instantiate(Resources.Load("Prefabs/Gem5"));
        }

        else
        {
            newObj = (GameObject)Instantiate(Resources.Load("Prefabs/Dynamite"));
        }

        newObj.SetActive(false);
        
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

    private IEnumerator TakeItems(int picks)
    {

        //disable all buttons
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("TakeButton");

        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().interactable = false;
        }

        //grab front, reset current front to last one.
        for (int i = 0; i < picks; i++)
        {
            yield return new WaitForSeconds(PAUSE_DUR);

            Transform frontGem = activeGems.transform.GetChild(0);
            Transform waitingGem = waitingGems.transform.GetChild(0);


            Deck[0].GetComponent<PickUpScript>().ApplyScore(Players[ActivePlayer]);
            Destroy(Deck[0]);
            Destroy(frontGem);
            Deck.RemoveAt(0);

            Deck[2].SetActive(true);


            waitingGem.SetParent(activeGems.transform);
        }

        yield return new WaitForSeconds(PAUSE_DUR);

        //Debug.Log("Player " + (ActivePlayer + 1) + "'s current score is " + Players[ActivePlayer].getScore());

        firstTime = false;

        //call it here or update?
        Refill();

    }
}
