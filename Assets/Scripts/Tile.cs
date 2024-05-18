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
    // >>State of tile<<
    public enum TileState
    {
        Default,
        Submerged,
        Planted
    }

    // >>Check if tile has been loaded<<
    bool active = false;

    public TileState currentState;

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
        maxNutr = maxNutrInsert;
        active = true;
        // Caches renderer
        cachedRender = gameObject.GetComponent<SpriteRenderer>();
        // Sets current state
        currentState = setState;
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

    // Updates nutrient value and the sprite color accordingly
    public void updateNutr(float newNutrLev)
    {
        nutritionLevel = newNutrLev;
        cachedRender.color = Color.Lerp(infertileColor, fertileColor, (nutritionLevel / maxNutr));
    }
    // Updates state and handles overlays accordingly
    public void updateState(TileState newState)
    {
        currentState = newState;

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

    // Update is called once per frame
    void Update()
    {
    }
}
