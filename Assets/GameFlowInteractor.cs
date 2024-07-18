using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameFlowInteractor : MonoBehaviour
{

	//Pre-condition boolean to be able to interact with the object; some objects can be interactable from the start
	public bool relevantBool;

	public string objectPrompt;


	public bool closeToObject;


	


	private void OnTriggerEnter(Collider other)
    {

		Debug.Log("Working any trigger");
        if (other.gameObject.tag == "Player" && relevantBool)
        {
			Debug.Log("Trigger player");
			if (gameObject.tag == "EndGame")
			{
				Debug.Log("Working");
				StartCoroutine(GameFlowManager.instance.EndGameFadeIn());
			}
			else
			{
                GameFlowManager.instance.promptTextPanel.SetActive(true);
                GameFlowManager.instance.promptText.text = objectPrompt;
				closeToObject = true;

			}
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameFlowManager.instance.promptTextPanel.SetActive(false);
            GameFlowManager.instance.promptText.text = "";
			closeToObject = false;
        }
    }


	/// <summary>
	/// Depending on the GameObject, the relevant pre-condition bool is something different
	/// </summary>
public void UpdateRelevantBool()
{
	switch(gameObject.tag)
	{
		case "Guard":
		{
			relevantBool = true;
			break;
		}

		case "Map":
		{
			relevantBool = GameFlowManager.instance.canReadTavernMap;
			break;
		}
		
		case "Idol":
		{
			relevantBool = GameFlowManager.instance.firstReadTavernMap;
			break;
		}
		
		case "Jewel":
		{
			relevantBool = GameFlowManager.instance.firstReadTavernMap;
			break;
		}
		
		case "ThirstyPirate":
		{
			relevantBool = GameFlowManager.instance.firstReadTavernMap;
			break;
		}

		case "Pirate":
		{
			relevantBool = SelfMadeModelManager.instance.interactedWithCutCoconut;
			break;
		}
		
		case "ShipDoors":
		{
			relevantBool = GameFlowManager.instance.talkedToPirateLastTime && 
						   !GameFlowManager.instance.openedQuartersDoors;
			break;
		}

		case "Sword":
		{
			relevantBool = GameFlowManager.instance.firstReadTavernMap;
			break;
		}


		case "EndGame":
		{
			relevantBool = GameFlowManager.instance.talkedToPirateLastTime;
			break;
		}


		case "CoconutPile":
		{
			relevantBool = !SelfMadeModelManager.instance.interactedWithCoconutPile;
			break;
		}

		case "CoconutCup":
		{
			relevantBool = !SelfMadeModelManager.instance.interactedWithCutCoconut;
			break;
		}

		case "Treasure Chest":
		{
			relevantBool = true;
			break;
		}

		default:

		{
			Debug.Log("TAG ERROR IN GAME FLOW INTERACTOR: " + gameObject.tag + gameObject.name);
			break;
		}
	}
}


public void ActivateObjectInteraction()
{
	switch(gameObject.tag)
	{
		case "Guard":
		{
			if(GameFlowManager.instance.firstTalkedToPathGuard==false)
			{
				GameFlowManager.instance.TalkedToPathGuard_FirstTime();
			}
			else if(GameFlowManager.instance.idolWasCollected && 
					GameFlowManager.instance.jewelWasCollected && 
					GameFlowManager.instance.specialSwordCollected)
			{
				GameFlowManager.instance.TalkedToPathGuard_LastTime();
			}
			break;
		}

		case "Map":
		{
			GameFlowManager.instance.ReadTavernMap();
			break;
		}
		
		case "Idol":
		{
			GameFlowManager.instance.CollectedIdol();
			break;
		}
		
		case "Jewel":
		{
			GameFlowManager.instance.CollectedJewel();
			break;
		}

		case "ThirstyPirate":
		{
			GameFlowManager.instance.TalkedToPirateFirstTime();
			break;
		}

		case "Pirate":
		{
			GameFlowManager.instance.TalkedToPirateLastTime();
			break;
		}
		
		case "ShipDoors":
		{
			GameFlowManager.instance.OpenQuarterDoors();
			break;
		}

		case "Sword":
		{
			GameFlowManager.instance.CollectedSword();
			break;
		}

		case "EndGame":
		{
			break;
		}

		case "CoconutPile":
		{ 
			SelfMadeModelManager.instance.InteractWithCoconutPile();
			break;
        }

        case "CoconutCup":
		{
			SelfMadeModelManager.instance.InteractWithCutCoconut();
			break;
		}

		case "Treasure Chest":
		{
			SelfMadeModelManager.instance.InteractWithTreasureChest();
			break;
		}

		default:
		{
			Debug.Log("TAG ERROR IN GAME FLOW INTERACTOR: " + gameObject.tag);
			break;
		}
	}
}



    // Start is called before the first frame update
    void Start()
    {
		UpdateRelevantBool();
		closeToObject = false;
    }

    // Update is called once per frame
    void Update()
    {
		UpdateRelevantBool();
        if (Input.GetKeyDown(KeyCode.X) && closeToObject && relevantBool) // Change to your desired input
        {
            GameFlowManager.instance.promptTextPanel.SetActive(false);
            GameFlowManager.instance.promptText.text = "";
            ActivateObjectInteraction();
        }
    }
}
