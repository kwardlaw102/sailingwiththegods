using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeOfPerson : MonoBehaviour
{

    public enum person { Seer, Helmsman };

    public person People;
    public bool inZone;
    private TestForQuests Quests;


    // Start is called before the first frame update
    void Start()
    {
        Quests = FindObjectOfType<TestForQuests>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inZone) {
            if (Input.GetKeyDown(KeyCode.Z)) {
                switch (People) {
                    case person.Seer:
                        if(Quests.Ritual == TestForQuests.Problem.Sacrifice) {
                            Quests.GiveSacrifice();
                        }
                        break;

                    case person.Helmsman:
                        break;

                }
            }
        }

    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            inZone = true;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            inZone = false;
        }
    }
}
