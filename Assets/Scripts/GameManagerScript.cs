using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour {

    public PlayerScript[] Players = new PlayerScript[4];
    private int ActivePlayer;
    private List<GameObject> Deck;
    public GameObject activePanel;
    public GameObject waitingPanel;
    public GameObject activeGems;
    public GameObject waitingGems;
    public GameObject ActivePowerup;
    public Sprite AllOrNothingImage;
    public Sprite HardEarthImage;
    public Sprite HotPotatoImage;
    private bool firstTime = true;
    private int roundCounter = 0;
    private bool gameOver = false;
    public GameObject HotPotato;
    public GameObject AllOrNothing;
    public GameObject HardEarth;
    public GameObject targetPlayer;     //player targeted by a powerup

    public GameObject unpauseButton;
    public GameObject toggleSound;
    public GameObject menuButton;
    public Text victoryText;
    public GameObject victoryDwarves;
    
    public GameObject pausePanel;
    public GameObject victoryPanel;

    private const float PAUSE_DUR = 0.8f;
    private const int DECK_CAP = 8;
    private const int NUM_ROUNDS = 5;
    
	public Button take2;
	public Button take3;
	public Button take4;

    private bool playersTargetable;     //used to avoid targeting players during a round

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
        playersTargetable = true;
        targetPlayer = null;

        pausePanel.SetActive(false);
        victoryPanel.SetActive(false);
        ActivePowerup.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SwitchActive()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManagerScript>().numConsec = 0;
		
		//switch active player
        if (ActivePlayer < 3)
        {
            ActivePlayer++;
        }
        else
        {
            roundCounter++;
            ActivePlayer = 0;

            if(roundCounter >= NUM_ROUNDS)
            {
                gameOver = true;
            }

            //give the player a powerup every 2 rounds
            else if ((roundCounter + 1) % 2 == 0)
            {
                //give players a powerup
                for (int i = 0; i < Players.Length; i++)
                {
                    //get a random powerup
                    int powerUpNum = Random.Range(0, 10);

                    if(powerUpNum <= 3)
                    {
                        Players[i].powerUp = (HotPotato) HotPotato.GetComponent("HotPotato");
                        Debug.Log("HotPotato given to player " + i);
                    }
                    else if (powerUpNum <= 7)
                    {
                        Players[i].powerUp = (HardEarth) HardEarth.GetComponent("HardEarth");
                        Debug.Log("HardEarth given to player " + i);
                    }
                    else
                    {
                        Players[i].powerUp = (AllOrNothing) AllOrNothing.GetComponent("AllOrNothing");
                        Debug.Log("AllOrNothing given to player " + i);
                    }
                }
            }
            //turn on takes 2 to 4
			take2.interactable = true;
			take3.interactable = true;
			take4.interactable = true;
        }

        //determine if all or nothing is in effect
        if (Players[ActivePlayer].AllOrNothingActive)
        {
            //turn off takes 2 to 4
            take2.interactable = false;
            take3.interactable = false;
            take4.interactable = false;

            //turn off all or nothing
            Players[ActivePlayer].AllOrNothingActive = false;
        }

        //determine if game ended
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

            PauseGame();
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

        playersTargetable = true;

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

        ArrayList winners = new ArrayList();

        int winScore = 0;

        //report player(s) with most score
        int s1 = Players[0].getScore();
        int s2 = Players[1].getScore();
        int s3 = Players[2].getScore();
        int s4 = Players[3].getScore();

        if(s1 >= s2 && s1 >= s3 && s1 >= s4)
        {
            Debug.Log("RED wins with a score of " + s1);
            winners.Add(GameObject.Find("Player1"));
            winScore = s1;
        }

        if(s2 >= s1 && s2 >= s3 && s2 >= s4)
        {
            Debug.Log("BLUE wins with a score of " + s2);
            winners.Add(GameObject.Find("Player2"));
            winScore = s2;
        }

        if(s3 >= s1 && s3 >= s2 && s3 >= s4)
        {
            Debug.Log("YELLOW wins with a score of " + s3);
            winners.Add(GameObject.Find("Player3"));
            winScore = s3;
        }

        if(s4 >= s1 && s4 >= s2 && s4 >= s3)
        {
            Debug.Log("GREEN wins with a score of " + s4);
            winners.Add(GameObject.Find("Player4"));
            winScore = s4;
        }

        VictoryMenu(winners, winScore);

    }

    private IEnumerator TakeItems(int picks)
    {

        //disable all buttons
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("TakeButton");

        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().interactable = false;
        }

        playersTargetable = false;

        //grab front, reset current front to last one.
        for (int i = 0; i < picks; i++)
        {
            yield return new WaitForSeconds(PAUSE_DUR);

            Transform frontGem = activeGems.transform.GetChild(0);
            Transform waitingGem = waitingGems.transform.GetChild(0);

            //check if player has hardearth active
            if (Players[ActivePlayer].HardEarthActive)
            {
                //remove the front of the deck as if it was taken without applying its effect
                Destroy(Deck[0]);
                Destroy(frontGem);
                Deck.RemoveAt(0);

                //turn off hard earth
                Players[ActivePlayer].HardEarthActive = false;
            }
            //check if target player exists
            else if (targetPlayer != null && targetPlayer.GetComponent<PlayerScript>().HotPotatoActive)
            {
                //check if front of deck is a bomb
                if (Deck[0].GetComponent<PickUpScript>().GetType().Equals("Bomb"))
                {

                    
                    //give the bomb to the target player
                    Deck[0].GetComponent<PickUpScript>().ApplyScore(targetPlayer.GetComponent<PlayerScript>());
                    Destroy(Deck[0]);
                    Destroy(frontGem);
                    Deck.RemoveAt(0);

                    //Hot potato explosion FX here (targetPlayer)
                    ParticleManager.instance.generateParticles("explosion", targetPlayer.transform);

                }
                else
                {
                    

                    //take the gem for the active player
                    Deck[0].GetComponent<PickUpScript>().ApplyScore(Players[ActivePlayer]);
                    Destroy(Deck[0]);
                    Destroy(frontGem);
                    Deck.RemoveAt(0);

                    ParticleManager.instance.generateParticles("shine", Players[ActivePlayer].transform);
                }
            }
            else
            {
                if (Deck[0].GetComponent<PickUpScript>().GetType().Equals("Bomb"))
                {
                    //Regular explosion FX here (ActivePlayer)
                    ParticleManager.instance.generateParticles("explosion", Players[ActivePlayer].transform);
                }
                else
                {
                    ParticleManager.instance.generateParticles("shine", Players[ActivePlayer].transform);
                }



                
                //take items normally
                Deck[0].GetComponent<PickUpScript>().ApplyScore(Players[ActivePlayer]);
                Destroy(Deck[0]);
                Destroy(frontGem);
                Deck.RemoveAt(0);

            }
            

            Deck[2].SetActive(true);


            waitingGem.SetParent(activeGems.transform);
        }

        yield return new WaitForSeconds(PAUSE_DUR);

        //Debug.Log("Player " + (ActivePlayer + 1) + "'s current score is " + Players[ActivePlayer].getScore());

        firstTime = false;

        //turn off hotpotato
        if (targetPlayer != null)
        {
            targetPlayer.GetComponent<PlayerScript>().HotPotatoActive = false;
        }
        

        //reset targetplayer
        targetPlayer = null;

        //call it here or update?
        Refill();

    }

    public void PauseGame()
    {
        //disable all buttons
        Button[] buttons = GameObject.FindObjectsOfType<Button>();

        foreach (Button button in buttons)
        {
            button.GetComponent<Button>().interactable = false;
        }

        //destroy every particle system
        GameObject[] particles = GameObject.FindGameObjectsWithTag("Particle");
        if (particles.Length > 0)
        {
            for (int i = particles.Length - 1; i >= 0; i--)
            {
                Destroy(particles[i]);
            }
        }

        ActivePowerup.SetActive(false);

        pausePanel.SetActive(true);

        unpauseButton.GetComponent<Button>().interactable = true;
        toggleSound.GetComponent<Button>().interactable = true;

    }

    public void Unpause()
    {
        //enable all buttons
        Button[] buttons = GameObject.FindObjectsOfType<Button>();

        foreach (Button button in buttons)
        {
            button.GetComponent<Button>().interactable = true;
        }
        
        //determine current powerup
        if (Players[ActivePlayer].powerUp != null)
        {
            ActivePowerup.SetActive(true);
            if (Players[ActivePlayer].powerUp.GetType().ToString().Equals("HotPotato"))
            {
                ActivePowerup.GetComponent<Image>().sprite = HotPotatoImage;
            }

            else if (Players[ActivePlayer].powerUp.GetType().ToString().Equals("HardEarth"))
            {
                ActivePowerup.GetComponent<Image>().sprite = HardEarthImage;
            }

            else if (Players[ActivePlayer].powerUp.GetType().ToString().Equals("AllOrNothing"))
            {
                ActivePowerup.GetComponent<Image>().sprite = AllOrNothingImage;
            }
        }

        unpauseButton.GetComponent<Button>().interactable = false;
        toggleSound.GetComponent<Button>().interactable = false;

        pausePanel.SetActive(false);

    }

    //apply a powerups effect
    public void ApplyPowerup(GameObject player)
    {
        //check that the active player has a powerup, if not then exit
        if (Players[ActivePlayer].powerUp == null || playersTargetable == false)
        {
            Debug.Log("no powerup");
            return;
        }
        //check that the player isn't the active player
        else if (player == Players[ActivePlayer].gameObject)
        {
            Debug.Log("You can't target yourself");
            //TODO make a visual thing to say you can't target yourself
        }
        //make sure that there is no targetplayer before assigning
        else if (targetPlayer == null)
        {
            targetPlayer = Players[ActivePlayer].powerUp.UsePowerUp(player);
            Players[ActivePlayer].powerUp = null;
            ActivePowerup.SetActive(false);
            ParticleManager.instance.generateParticles("target", targetPlayer.transform);
            Debug.Log(targetPlayer);
        }
    }

    private void VictoryMenu(ArrayList winDwarves, int finalScore)
    {
        victoryPanel.SetActive(true);

        menuButton.GetComponent<Button>().interactable = true;

        foreach(GameObject obj in winDwarves)
        {
            obj.SetActive(true);
            obj.transform.SetParent(victoryDwarves.transform);
        }


        victoryText.text = "Congratulations to the winning player";

        if (winDwarves.Count > 1)
        {
            victoryText.text += "s";
        }

        victoryText.text += "\nWith a final score of: ";
        victoryText.text += finalScore.ToString();
    }

    public void backButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
