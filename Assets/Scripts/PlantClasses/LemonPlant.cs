using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Plant;

public class LemonPlant : Plant
{
    // The modifier added for each nearby plant
    [SerializeField]
    float neighborModify;

    // Adds a self imposed modifier upon a tile having a plant
    void addModIfPlant(Tile givenTile)
    {
        if (givenTile != null && givenTile.currentState == Tile.TileState.Planted && givenTile.plantScriptAttached.GetType() == typeof(LemonPlant))
        {
            giveModifier(this, neighborModify, "Being nearby other plants of the same type");
        }
    }

    // Scans nearby tiles for modifiers and applies modifier on self for each planted nearby tile
    // Destroys all old given modifiers and replaces them
    public override void updateSpecialPlant()
    {
        deleteGivenModifiers();
        addModIfPlant(cacheTile.leftTile);
        addModIfPlant(cacheTile.leftUpTile);
        addModIfPlant(cacheTile.upTile);
        addModIfPlant(cacheTile.rightUpTile);
        addModIfPlant(cacheTile.rightTile);
        addModIfPlant(cacheTile.rightDownTile);
        addModIfPlant(cacheTile.downTile);
        addModIfPlant(cacheTile.leftDownTile);
    }
}
