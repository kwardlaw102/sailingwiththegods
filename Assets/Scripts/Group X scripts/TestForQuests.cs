using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class TestForQuests : MonoBehaviour
{

    public enum Problem {None, Sacrifice, Helmsman };
    [Space]
    public GameObject Warning = null;
    public GameObject SacrificeMenu = null;
    public Text PromptText;
    public Text FlavorText;
    public Text animalsList;
    public int text;
    public bool MenuUp, SacrificeUp;
    public GameObject cursor;
    public int CodeSelect;
    public int addValue;
    public Transform og;
    private Vector3 add;
    public bool hasSeer;
    
    public string theSacrifice;

    public Problem Ritual;

    private Animals animals;
    private FPSMovement player;
    
    // Start is called before the first frame update
    void Start()
    {
        animals = FindObjectOfType<Animals>();
        player = FindObjectOfType<FPSMovement>();
    }

    // Update is called once per frame

    void Update() {
        if (Input.GetKeyDown(KeyCode.S)) {
            MenuUp = !MenuUp;

            text = Random.Range(1, 5);
            PromptText.text = "A storm has hit!";

            switch (text) {

                case 0:
                    FlavorText.text = "Often along the streaming hair of the gray salt water They pray for sweet homecoming won in spite of the sea (Archilochus fr. 12, 7th c BC)";
                    break;
                case 1:
                    FlavorText.text = "The open sea is churning to a wash of waves. Deep within. A cloud stands upright over the Cyrean caps, signal of a storm, and terror rises from the unforeseen (Archilochus fr. 15, 7th c BC)";
                    break;
                case 2:
                    FlavorText.text = "Long waves surge, churned up on the sea by the east and south winds, hurtling down from heavens; as the west wind sweeps through the high-standing grain  with its violent blast, and the ears all shudder and bow (Adapted from Iliad 2.131)";
                    break;
                case 3:
                    FlavorText.text = "The winds crack and crash,  As when a wave of the loud-roaring sea crashes upon a beach and the surf keeps resounding (Iliad 1.198-200)";
                    break;
                case 4:
                    FlavorText.text = "The Pleiades flee mighty Orion, and plunge into the misty deep: all the gusty winds are raging!  (Hesiod Works and Days 620-621)";
                    break;

            }
        }

        if (MenuUp) {
            Warning.SetActive(true);

            if (Input.GetKeyDown(KeyCode.O)) {
                GiveSacrifice();
            }

            if (Input.GetKeyDown(KeyCode.M)) {
                Magic();
            }

            if (Input.GetKeyDown(KeyCode.N)) {
                SailAway();
            }
        }
        else {
            Warning.SetActive(false);
        }

        

        if (Ritual == Problem.Sacrifice) {
            Sacrifice();
        }
    }
	#region Sacc
	void Sacrifice()
    {
        if(SacrificeUp) {
            if (Input.GetButtonDown("Vertical")) {
                CodeSelect -= (int)Input.GetAxisRaw("Vertical");
                Vector3 Add = new Vector3(0, addValue * (int)Input.GetAxisRaw("Vertical"), 0);
                cursor.transform.position = cursor.transform.position + Add;
            }

            if(Input.GetKeyDown(KeyCode.Z)) {
                if (animals.animals[CodeSelect] == theSacrifice) {
                    CloseSacrifice(true);
                }
                else {
                    CloseSacrifice(false);
                }
                animals.animals.RemoveAt(CodeSelect);
                animalsList.text = string.Join("\n", animals.animals);
                
            }

            if (CodeSelect > animals.animals.Count - 1) { CodeSelect = 0; cursor.transform.position = og.transform.position; }

        }
    }

    public void GiveSacrifice() {
        Ritual = Problem.Sacrifice;
        SacrificeUp = true;
        theSacrifice = animals.animals[Random.Range(1, animals.animals.Count)];
        SacrificeMenu.SetActive(true);
        cursor.SetActive(true);
        animalsList.text = string.Join("\n", animals.animals);
        player.canMove = false;

    }

    public void CloseSacrifice(bool Right) {
        SacrificeMenu.SetActive(false);
        cursor.SetActive(false);
        animalsList.text = null;
        player.canMove = true;
        Warning.SetActive(true);
        MenuUp = true;
        if (!Right) {
            PromptText.text = "Wrong animal!";
        } else {
            PromptText.text = "Correct animal!";
        }
    }
	#endregion

	void Magic() {
        if(!hasSeer) {
            PromptText.text = "No seers?";
            FlavorText.text = null;
            animalsList.text = null;
        } else {
            PromptText.text = "You have a seer?";
            FlavorText.text = null;
            animalsList.text = null;
        }
	}

    void SailAway() {

    }
}
