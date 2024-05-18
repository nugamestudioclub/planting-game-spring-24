using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    #region vars
    // State of tile
    public enum TileState
    {
        Default,
        Submerged,
        Planted
    }

    // Check if tile has been loaded
    bool active = false;

    [SerializeField]
    TileState currentState;

    // !!TBD until nutrition system is complete pretend it is always at 1!!
    // Stored nutrition levels
    [SerializeField]
    float nutritionLevels = 1;

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
    public void activate(TileState setState, Tile LD, Tile L, Tile LU, Tile U, Tile RU, Tile R, Tile RD, Tile D)
    {
        active = true;
        currentState = setState;
        leftDownTile = LD;
        leftTile = L;
        leftUpTile = LU;
        upTile = U;
        rightUpTile = RU;
        rightTile = R;
        rightDownTile = RD;
        downTile = D;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
