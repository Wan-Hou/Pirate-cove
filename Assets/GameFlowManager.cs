using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Numerics;


public class GameFlowManager : MonoBehaviour
{
	public static GameFlowManager instance;

	///  Getting the player
    public GameObject player;
	public UnityEngine.Vector3 player_initial_position;
	public UnityEngine.Quaternion player_initial_rotation;

    /// Text that shows up to give the player a tip on what to do next
    public TextMeshProUGUI tipText;
	public GameObject TipTextPanel;

    /// How long the tip text stays on screen
    public float tipTextTime;
	private float tipTextTimer;
	
	/// Text that shows when player is close to something they can interact with.	
	public TextMeshProUGUI promptText;
	public GameObject promptTextPanel;

	/// References to texts on the screen corner that show current objectives
	public TextMeshProUGUI mainObj1Text, mainObj2Text, mainObj3Text;

    /****************************** GAME FLOW CONTROL BOOLEANS AND GAME OBJECTS ******************************/

    /// First action, talking to path guard for first time,
	///  then player is directed to look at a map in the tavern
    public bool firstTalkedToPathGuard = false; 
	public bool canReadTavernMap = false;
	public bool firstReadTavernMap = false;

	/// Artifact GO references
	public GameObject IdolGO;
	public GameObject JewelGO;
	public GameObject SpecialSwordGO;


	/// Bools to indicate each artifact was collected
	public bool idolWasCollected;

	public bool jewelWasCollected;

	public bool specialSwordCollected;


	/// For the special sword, player also needs to talk to one of the pirates on the map
	///  and get the ship quarter keys to open the door
	public bool talkedToPirateFirstTime = false;
	public bool talkedToPirateLastTime = false;
	public GameObject pirateGO;

	public Animator leftPirateDoorAnimator, rightPirateDoorAnimator;
	public bool openedQuartersDoors = false;

	/// Used when talking to path guard for the last time once you have all the items required;
	///  turns off path barrier
	public bool lastTalkedToPathGuard = false;

	/// GameObj reference to path barrier
	public GameObject pathBarrierGO;

	public GameObject endGamePanel;

    /*********************************** ACTIVATION METHODS ***********************************/


    public void TalkedToPathGuard_FirstTime()
	{
		Debug.Log("Talked to guard for the first time!");
		StartCoroutine(LoadTempText("Sorry mate, before I let you pass you need to find " +
			"the things your crewmate hid around the beach. " +
			"I heard him saying around the tavern that he drew a map " +
			"describing where he left each object, why don't you go take a look?"));
		firstTalkedToPathGuard = true;
		canReadTavernMap = true;
	}



	public void TalkedToPathGuard_LastTime()
	{
		Debug.Log("Talked to guard for the last time!");
		StartCoroutine(LoadTempText("Ok mate, you've found all the items around the coast, " +
			"you're free to pass!"));
		lastTalkedToPathGuard = true;
		Destroy(pathBarrierGO);
	}




	public void ReadTavernMap()
	{
		Debug.Log("Read tavern map!");
		StartCoroutine(LoadTempText("You found the map with the locations of the three items! " +
			"Go search for them!"));
		firstReadTavernMap = true;
		canReadTavernMap = false;
		///Turn on all special items
		IdolGO.SetActive(true);
		JewelGO.SetActive(true);
		SpecialSwordGO.SetActive(true);

		mainObj1Text.text = "Objective: Find the Sacred Idol. " +
			"Hint: 'That was too heavy so I hid it close-by'. ";
		mainObj2Text.text = "Objective: Find the Crystal Jewel. " +
			"Hint: 'Three twisted trunks, one flag to rule them all; An anchor, a barrel, and a crystal to round the haul! ";
		mainObj3Text.text = "Objective: Find the Special Sword. " +
			"Hint: 'This pretty sword is coming with me! But I don't have anywhere to put it..." +
			"maybe I'll keep it in the captain's quarters since I hold the key!'. ";
		promptText.text = "";
	}

	public void CollectedIdol()
	{
		Debug.Log("Collected idol!");
		idolWasCollected = true;
		Destroy(IdolGO);
		mainObj1Text.text = "Find the Sacred Idol: Completed! ";
		mainObj1Text.color = new Color(0, 1, 0, 1);
		promptText.text = "";

	}

	public void CollectedJewel()
	{
		Debug.Log("Collected jewel!");
		jewelWasCollected = true;
		Destroy(JewelGO);
		mainObj2Text.text = "Find the Crystal Jewel: Completed! ";
		mainObj2Text.color = new Color(0, 1, 0, 1);
		promptText.text = "";
	}

	/// Talked to pirate to get the keys to the captain's quarters
	public void TalkedToPirateFirstTime()
	{
        promptText.text = GameObject.Find("Basic_BanditPrefab").GetComponent<GameFlowInteractor>().objectPrompt;
        promptTextPanel.SetActive(true);
        Debug.Log("Talked to pirate!");
		talkedToPirateFirstTime = true;
		StartCoroutine(LoadTempText("Hey Capt'n! What? A sword? Yeah, I remember stashing it, but I can't remember where I put the keys" +
			"...I'm so thirsty I can't think.... Can you find me something to drink while I look for the key? Coconut water would be perfect! "));
        pirateGO.tag = "Pirate";
		mainObj3Text.text = "Objective: Find the Special Sword. Look for a drink, maybe coconut water, to quench the pirate's thirst.";
	}

	/// Talked to pirate to get the keys to the captain's quarters
	public void TalkedToPirateLastTime()
	{
		Debug.Log("Talked to pirate after getting coconut!");
		talkedToPirateLastTime = true;
		StartCoroutine(LoadTempText("Hey Capt'n! Thanks for the coconut water! I found the key to the captain quarters while you were gone, here it is!"));
		
		mainObj3Text.text = "Objective: Find the Special Sword. Investigate the captains quarters.";
	}

	/// Open captain's quarters
	public void OpenQuarterDoors()
	{
		Debug.Log("Opened Quarters' Doors!");
		openedQuartersDoors = true;
		leftPirateDoorAnimator.SetTrigger("DoorOpen");
		rightPirateDoorAnimator.SetTrigger("DoorOpen");
		//mainObj3Text.text = "Objective: Find the Special Sword. Investigate the captains quarters.";
	}

	/// Get special sword
	public void CollectedSword()
	{
		
		Debug.Log("Collected sword!");
		specialSwordCollected = true;
		Destroy(SpecialSwordGO);
		mainObj3Text.text = "Objective: Find the Special Sword. Completed!";
		mainObj3Text.color = new Color(0, 1, 0, 1);
		promptText.text = "";

	}

	public IEnumerator LoadTempText(string tempText)
	{
		TipTextPanel.SetActive(true);
        if (tipText.text != tempText)
		{ 
			tipText.text = tempText;
			tipTextTimer = tipTextTime;
		}
		yield return new WaitForSeconds(tipTextTimer);
		TipTextPanel.SetActive(false);
	}

	public IEnumerator EndGameFadeIn()
	{
		endGamePanel.SetActive(true);
		float alpha = 0;
		Image img = endGamePanel.GetComponent<Image>();
		img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);

		while (alpha < 1) 
		{
			alpha += Time.deltaTime;
			img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
			yield return new WaitForSeconds(Time.deltaTime);
		}
		img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
	}

	void Awake()
	{
		if (instance==null)
		{
			instance = this;
		}
		else
			Destroy(gameObject);
	}

    // Start is called before the first frame update
    void Start()
    {
		player_initial_position = player.transform.position;
        player_initial_rotation = player.transform.rotation;
        mainObj1Text.text = "You have to go into town. Follow the path leading out of the beach.";
		mainObj2Text.text = "";
		mainObj3Text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y <= -30) 
		{
			Debug.Log("Fell out of map");
			player.transform.position = player_initial_position;
            player.transform.rotation = player_initial_rotation;
            StartCoroutine(LoadTempText("Now, how did you fall out of the map?"));
        }
		if (Input.GetKeyDown(KeyCode.Escape)) 
		{
			Application.Quit();
		}
    }
}
