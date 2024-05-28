using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatermelonPlant : Plant
{
    // The modifier put on to each plant
    [SerializeField]
    float neighborModify;

    // Adds a self imposed modifier upon a tile having a plant
    void addModIfPlant(Tile givenTile)
    {
        if (givenTile != null && givenTile.currentState == Tile.TileState.Planted)
        {
            giveModifier(givenTile.plantScriptAttached, neighborModify, "Sabatoged by watermelon");
        }
    }

    // Scans nearby tiles for modifiers and applies modifier on self for each planted nearby tile
    // Destroys all old given modifiers and replaces them
    public override void updateSpecialPlant()
    {
        deleteGivenModifiers();
        Tile leftIter = cacheTile.leftTile;
        addModIfPlant(leftIter);
        if(leftIter != null)
        {
            addModIfPlant(leftIter.leftTile);
            leftIter = leftIter.leftTile;
            if(leftIter != null)
            {
                addModIfPlant(leftIter.leftTile);
            }
        }
        Tile rightIter = cacheTile.rightTile;
        addModIfPlant(rightIter);
        if (rightIter != null)
        {
            addModIfPlant(rightIter.rightTile);
            rightIter = rightIter.rightTile;
            if (rightIter != null)
            {
                addModIfPlant(rightIter.rightTile);
            }
        }
    }
}