using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class cursorScript : MonoBehaviour
{
    static public cursorScript singleCursor = null;
    [SerializeField]
    string heldPlant = "empty";
    mouseBox lastMouseBox = null;
    SpriteRenderer cacheDisplay;
    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.FindObjectsOfType<cursorScript>().Length > 1)
        {
            Destroy(gameObject);
            Destroy(this);
        }
        else
        {
            singleCursor = this;
            cacheDisplay = gameObject.GetComponent<SpriteRenderer>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        if(heldPlant == "empty")
        {
            if (lastMouseBox != null && lastMouseBox.mouseInBox && lastMouseBox.cacheTile.currentState == Tile.TileState.Planted)
            {
                string onPlant = lastMouseBox.cacheTile.plantID;
                blackboardScript.singleBlackboard.plantShowcase(onPlant, true);
                blackboardScript.singleBlackboard.boostsGive(lastMouseBox.cacheTile.plantScriptAttached);
            }
            else
            {
                blackboardScript.singleBlackboard.tutorialShow();
            }
        }
    }

    //Changes heldplant value
    private void updateHeldPlant(string newPlant)
    {
        heldPlant = newPlant;
        cacheDisplay.color = Color.white;
        cacheDisplay.sprite = seedTypeLoader.singleLoader.getSprite(heldPlant);
        blackboardScript.singleBlackboard.plantShowcase(heldPlant, false);
    }

    // Clears hand of plants
    private void clearHeldPlant()
    {
        heldPlant = "empty";
        cacheDisplay.color = Color.clear;
        blackboardScript.singleBlackboard.tutorialShow();
    }

    // Sets last mouse box, used to inspect plants
    public void setLastMouseBox(mouseBox insertMouseBox)
    {
        lastMouseBox = insertMouseBox;
    }

    // How the cursor interacts with slots
    // Called by button
    public void interactSlot(seedSlot slot)
    {
        if(heldPlant == "empty")
        {
            pickUpPlantSlot(slot);
        }
        else
        {
            dropPlantSlot(slot);
        }
    }
    public void dropPlantSlot(seedSlot slot)
    {
        if (slot.insertDisplay(heldPlant))
        {
            clearHeldPlant();
        }
    }
    public void pickUpPlantSlot(seedSlot slot)
    {
        string slotPlant = slot.getDisplay();
        if (slotPlant != "empty")
        {
            updateHeldPlant(slotPlant);
        }
    }

    // How the cursor interacts with tiles
    // Called by mouseBox script on correct input
    public void plantPlot(Tile plot)
    {
        if(heldPlant != "empty" && plot.createPlant(heldPlant))
        {
            clearHeldPlant();
        }
    }
    public void digPlot(Tile plot)
    {
        plot.destroyPlant();
    }

    // How cursor interacts with forage mechanic
    public void forage(forageButton forage)
    {
        if(heldPlant == "empty")
        {
            updateHeldPlant(forage.getForage());
        }
    }
}
