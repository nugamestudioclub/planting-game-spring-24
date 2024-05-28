using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seedSlot : MonoBehaviour
{

    // If nothing is in the seed slot
    [SerializeField]
    string currentDisplay = "empty";

    [SerializeField]
    bool startWithPlant;
    plantDisplay cacheDisplay;
    // Start is called before the first frame update
    void Start()
    {
        cacheDisplay = transform.GetChild(0).GetComponent<plantDisplay>();
        cacheDisplay.init();
        if (startWithPlant)
        {
            string seedID = seedTypeLoader.singleLoader.getRandomID();
            currentDisplay = seedID;
            cacheDisplay.insertNewSprite(currentDisplay);
        }
    }

    // Returns true if process was successful
    // Inserts plant into slot
    public bool insertDisplay(string newID)
    {
        if(currentDisplay == "empty")
        {
            currentDisplay = newID;
            cacheDisplay.insertNewSprite(currentDisplay);
            return true;
        }
        else
        {
            return false;
        }
    }

    // Returns true if process was successful
    // Gets plant from slot
    public string getDisplay()
    {
        if(currentDisplay != "empty")
        {
            cacheDisplay.deactivateSprite();
            string retString = currentDisplay;
            currentDisplay = "empty";
            return retString;
        }
        else
        {
            return "empty";
        }
    }
}
