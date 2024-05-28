using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomPlant : Plant
{
    // Time that mushroom plant has to exist without destroying other plants
    [SerializeField]
    float baseTime;
    // Time added per plant destroyed
    [SerializeField]
    float timeAddPerDelete;
    float timeUntilDestruct = 0;
    public override void initPlant(Tile insertTile, string givenPlant)
    {
        int amountDeleted = 0;
        void checkAndDestroyPlant(Tile givenTile)
        {
            if(givenTile != null && givenTile.currentState == Tile.TileState.Planted)
            {
                givenTile.plantScriptAttached.disconnect();
                amountDeleted += 1;
            }
        }
        checkAndDestroyPlant(insertTile.leftTile);
        checkAndDestroyPlant(insertTile.leftUpTile);
        checkAndDestroyPlant(insertTile.upTile);
        checkAndDestroyPlant(insertTile.rightUpTile);
        checkAndDestroyPlant(insertTile.rightTile);
        checkAndDestroyPlant(insertTile.rightDownTile);
        checkAndDestroyPlant(insertTile.downTile);
        checkAndDestroyPlant(insertTile.leftDownTile);
        timeUntilDestruct = baseTime + timeAddPerDelete * amountDeleted;
        base.initPlant(insertTile, givenPlant);
    }

    public override void updateEffect()
    {
        timeUntilDestruct -= Time.deltaTime;
        if (timeUntilDestruct <= 0)
        {
            disconnect();
        }
        base.updateEffect();
    }
}
