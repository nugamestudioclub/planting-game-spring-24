using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    #region vars
    // >>Animation/Sprite Handlers<<
    // Prefab for wave warnings
    [SerializeField]
    GameObject pointerPrefab;
    [SerializeField]
    Color infertileColor;
    [SerializeField]
    Color fertileColor;
    [SerializeField]
    Color mouseBoxDefaultColor;
    [SerializeField]
    Color mouseBoxPlantedColor;
    [SerializeField]
    Color mouseBoxSubmergedColor;
    SpriteRenderer cachedRender;
    OceanAnim cachedAnimHandeler;
    mouseBox cachedMouseBox;
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
    public Plant plantScriptAttached;
    public string plantID;

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
        cachedMouseBox = transform.GetChild(1).GetComponent<mouseBox>();
        // Initialize cached components
        cachedMouseBox.init(this);
        cachedAnimHandeler.initState(setState == TileState.Submerged, this);
        // Sets current state
        currentState = setState;
        updateMouseBox();
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
    // Warning spawn
    public void spawnWarning(Direction dir, bool flood, float time)
    {
        GameObject pointer = Instantiate(pointerPrefab, transform.position, Quaternion.identity);
        pointer.GetComponent<flashingScript>().init(dir, time, flood);
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
                        nextTile.updateWave(TileState.Submerged, dir);
                    }
                    else
                    {
                        updateWave(TileState.Default, dir);
                    }
                }
                else
                {
                    nextTile.newWaveAttempt(dir, flood);
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
        // Disconnects plants
        if (currentState == TileState.Planted)
        {
            plantScriptAttached.disconnect();
        }
        // Updates state and mouse box
        currentState = newState;
        updateMouseBox();
        // Updates animation
        if(newState != TileState.Submerged)
        {
            cachedAnimHandeler.floodObject(dir, false);
        }
        else
        {
            cachedAnimHandeler.floodObject(dir, true);
        }
        // Updates surrounding tiles
        animComplete();
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
        bayUpdate();
    }

    // Updates mouse box color to current state
    public void updateMouseBox()
    {
        if (currentState == TileState.Submerged)
        {
            cachedMouseBox.updateColor(mouseBoxSubmergedColor);
        }
        if (currentState == TileState.Planted)
        {
            cachedMouseBox.updateColor(mouseBoxPlantedColor);
        }
        if (currentState == TileState.Default)
        {
            cachedMouseBox.updateColor(mouseBoxDefaultColor);
        }
    }

    // Updates overlay from adjacent tile states
    public void bayUpdate()
    {
        if (currentState != TileState.Submerged && !getIsInAnim())
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

    // Creates a plant if plot is avaliable
    // Returns true if process was sucessful
    public bool createPlant(string id)
    {
        if(currentState == TileState.Default)
        {
            plantID = id;
            GameObject loadPrefab = seedTypeLoader.singleLoader.getPrefab(id);
            plantObjectAttached = Instantiate(loadPrefab, transform.position, Quaternion.identity.normalized);
            plantScriptAttached = plantObjectAttached.GetComponent<Plant>();
            plantScriptAttached.initPlant(this, id);
            currentState = TileState.Planted;
            updateMouseBox();
            return true;
        }
        return false;
    }

    // Destroys a plant on the tile if the plot is being used
    // Returns true if process was sucessful
    public bool destroyPlant()
    {
        if(currentState == TileState.Planted)
        {
            plantScriptAttached.disconnect();
            plantObjectAttached = null;
            plantScriptAttached = null;
            currentState = TileState.Default;
            updateMouseBox();
        }
        return false;
    }
    // Update is called once per frame
    void Update()
    {
    }
}
