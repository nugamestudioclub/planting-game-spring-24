using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    #region vars
    // >>Sprite Handlers<<
    [SerializeField]
    Color infertileColor;
    [SerializeField]
    Color fertileColor;
    SpriteRenderer cachedRender;
    OceanAnim cachedAnimHandeler;
    // >>Direction of wave<<
    public enum Direction
    {
        L,
        U,
        R,
        D
    }
    // >>State of tile<<
    public enum TileState
    {
        Default,
        Submerged,
        Planted
    }
    public TileState currentState;
    // Checks if tile has been loaded
    bool active = false;
    GameObject plantObjectAttached;
    Plant plantScriptAttached;

    // >>Nutrient Levels<<
    public float maxNutr;
    // Please only update this through updateNutritionVal func to update sprite
    public float nutritionLevel = 1;

    // Surrounding tiles
    public Tile leftDownTile;
    public Tile leftTile;
    public Tile leftUpTile;
    public Tile upTile;
    public Tile rightUpTile;
    public Tile rightTile;
    public Tile rightDownTile;
    public Tile downTile;
    #endregion
    // Fills in information of tile and activates it
    public void activate(float maxNutrInsert, TileState setState, Tile LD, Tile L, Tile LU, Tile U, Tile RU, Tile R, Tile RD, Tile D)
    {
        active = true;
        // Caches needed components
        cachedRender = gameObject.GetComponent<SpriteRenderer>();
        cachedAnimHandeler = transform.GetChild(0).GetComponent<OceanAnim>();
        // Initialize cached components
        cachedAnimHandeler.initState(setState == TileState.Submerged, this);
        // Sets current state
        currentState = setState;
        maxNutr = maxNutrInsert;
        // Sets adjacent tiles
        leftDownTile = LD;
        leftTile = L;
        leftUpTile = LU;
        upTile = U;
        rightUpTile = RU;
        rightTile = R;
        rightDownTile = RD;
        downTile = D;
    }
    // Attempts to submerge or recede on first non submerged block in a direction
    public void newWaveAttempt(Direction dir, bool flood)
    {
        if(currentState == TileState.Submerged)
        {
            Tile nextTile = null;
            // Find next tile
            if (dir == Direction.L)
            {
                nextTile = leftTile;
            }
            if (dir == Direction.U)
            {
                nextTile = upTile;
            }
            if (dir == Direction.R)
            {
                nextTile = rightTile;
            }
            if (dir == Direction.D)
            {
                nextTile = downTile;
            }
            // Checks if next tile in dir exists
            if (nextTile != null)
            {
                if (nextTile.currentState != TileState.Submerged)
                {
                    if (flood)
                    {
                        updateWave(TileState.Submerged, dir);
                    }
                    else
                    {
                        updateWave(TileState.Default, dir);
                    }
                }
            }
        }
    }
    // Updates nutrient value and the sprite color accordingly
    public void updateNutr(float newNutrLev)
    {
        nutritionLevel = newNutrLev;
        cachedRender.color = Color.Lerp(infertileColor, fertileColor, (nutritionLevel / maxNutr));
    }
    // Updates state from flood and plays anim
    public void updateWave(TileState newState, Direction dir)
    {
        currentState = newState;
        if(newState != TileState.Submerged)
        {
            cachedAnimHandeler.floodObject(dir, false);
        }
        else
        {
            cachedAnimHandeler.floodObject(dir, true);
        }
    }

    // Updates other tiles on animation completion
    public void animComplete()
    {
        void noNullBayUpdate(Tile givenTile)
        {
            if(givenTile != null)
            {
                givenTile.bayUpdate();
            }
        }
        noNullBayUpdate(leftTile);
        noNullBayUpdate(upTile);
        noNullBayUpdate(rightTile);
        noNullBayUpdate(downTile);
    }
    // Updates overlay from adjacent tile states
    public void bayUpdate()
    {
        if (currentState != TileState.Submerged)
        {
            // Submerged tiles with finished animations and edges result in a visible bay line
            bool doesTileComply(Tile givenTile)
            {
                return givenTile == null || givenTile.currentState == TileState.Submerged && !givenTile.getIsInAnim();
            }
            cachedAnimHandeler.updateBay(
                doesTileComply(leftTile),
                doesTileComply(upTile),
                doesTileComply(rightTile),
                doesTileComply(downTile));
        }
        else
        {
            cachedAnimHandeler.updateBay(
                false,
                false,
                false,
                false);
        }
    }

    // The base nutrient calculation
    public float baseNutrientCalculation()
    {
        if(currentState == TileState.Submerged)
        {
            return maxNutr;
        }
        else
        {
            int subOrMissingEdge = 0;
            void addOnTileSubOrMissing(Tile givenTile)
            {
                if (givenTile == null || givenTile.currentState == TileState.Submerged)
                {
                    subOrMissingEdge += 1;
                }
            }
            addOnTileSubOrMissing(leftDownTile);
            addOnTileSubOrMissing(leftTile);
            addOnTileSubOrMissing(leftUpTile);
            addOnTileSubOrMissing(upTile);
            addOnTileSubOrMissing(rightUpTile);
            addOnTileSubOrMissing(rightTile);
            addOnTileSubOrMissing(rightDownTile);
            addOnTileSubOrMissing(downTile);
            return  maxNutr * ((float)subOrMissingEdge / 8);
        }
    }

    // Returns if oceanAnim is in middle of animation
    public bool getIsInAnim()
    {
        return cachedAnimHandeler.getIsInAnim();
    }
    // Update is called once per frame
    void Update()
    {
    }
}
