using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfMadeModelManager : MonoBehaviour
{
	public static SelfMadeModelManager instance;


	public GameObject coconutPile;
	public GameObject cutCoconut;

	public Animator treasureChestAnimator;
	


	public bool interactedWithCoconutPile = false;
	public bool interactedWithCutCoconut = false;
	public bool interactedWithTreasureChest = false;

	public bool treasureChestOpen = false;



	public void InteractWithCoconutPile()
	{
		interactedWithCoconutPile = true;
		//Access all children and give them Rigidbodies to make them roll out of the way
		foreach (Transform child in coconutPile.transform)
		{
			child.gameObject.AddComponent<Rigidbody>();
		}
	}


	public void InteractWithCutCoconut()
	{
		interactedWithCutCoconut = true;
		Destroy(cutCoconut);
		StartCoroutine(GameFlowManager.instance.LoadTempText("Found a cut coconut! Who knows what you can use it for...but at least it quenches your thirst!"));
	}



	public void InteractWithTreasureChest()
	{
		if (!interactedWithTreasureChest)
		{
			interactedWithTreasureChest = true;
			StartCoroutine(GameFlowManager.instance.LoadTempText("You found a hidden treasure! Could this have any use somewhere...?"));
			treasureChestAnimator.SetTrigger("OpenChest");
			treasureChestAnimator.GetComponentInParent<GameFlowInteractor>().objectPrompt = "Press X to close the chest";
            treasureChestOpen = true;
		}
		else
		{
			if (treasureChestOpen)
			{
				treasureChestAnimator.SetTrigger("CloseChest");
                treasureChestAnimator.GetComponentInParent<GameFlowInteractor>().objectPrompt = "Press X to open the chest";
            }
			else
			{
				treasureChestAnimator.SetTrigger("OpenChest");
                treasureChestAnimator.GetComponentInParent<GameFlowInteractor>().objectPrompt = "Press X to close the chest";
                
            }
            
            treasureChestOpen = !treasureChestOpen;
		}
        GameFlowManager.instance.promptText.text = treasureChestAnimator.GetComponentInParent<GameFlowInteractor>().objectPrompt;
        GameFlowManager.instance.promptTextPanel.SetActive(true);

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
