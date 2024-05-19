using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // Cache
    TileManager tiles;

    [SerializeField]
    int startingFood;

    // Counted in seconds per food
    [SerializeField]
    private float foodDecrease;

    //>>Tide Manager<<
    // Counted in seconds
    [SerializeField]
    private float tideCooldown;
    [SerializeField]
    int width;
    [SerializeField]
    private float floodChance;
    [SerializeField]
    private int maxLand;
    [SerializeField]
    private int minLand;
    [SerializeField]
    private float warningTime;
    // Counted in seconds
    [SerializeField]
    private float forageCooldown;

    Timer foodTimer;
    Timer tideTimer;
    Timer forageTimer;

    [SerializeField]
    int food;
    public bool CanForage = true;

    // Start is called before the first frame update
    void Start()
    {
        tiles = gameObject.GetComponent<TileManager>();
        tiles.init();
        food = startingFood;
        foodTimer = Timer.Register(foodDecrease, () => decreaseFood(1));
        tideTimer = Timer.Register(tideCooldown, () => doFloodWarning());
        forageTimer = Timer.Register(forageCooldown, () => CanForage = true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void decreaseFood(int val)
    {
        food = Mathf.Max(food - val, 0);
    }

    void doFloodWarning()
    {
        // Generates direction
        Tile.Direction dir;
        int generate = Random.Range(0,4);
        if(generate == 0)
        {
            dir = Tile.Direction.L;
        }
        else if(generate == 1)
        {
            dir = Tile.Direction.U;
        }
        else if (generate == 2)
        {
            dir = Tile.Direction.R;
        }
        else
        {
            dir = Tile.Direction.D;
        }
        // Generates width
        int generateCoord;
        if (dir != Tile.Direction.L && dir != Tile.Direction.R)
        {
            generateCoord = Random.Range(width, tiles.gridHeight - width + 1);
        }
        else
        {
            generateCoord = Random.Range(width, tiles.gridWidth - width + 1);
        }
        // Generate recede/flood
        int landAvaliable = tiles.getTotalLandTiles();
        bool willFlood = false;
        if(landAvaliable < minLand)
        {
            willFlood = false;
        }
        else if (landAvaliable > maxLand)
        {
            willFlood = true;
        }
        else
        {
            int generateChance = Random.Range(0, 100);
            willFlood = generateChance < floodChance;
        }
        tiles.sendWarning(dir, generateCoord, width, willFlood, warningTime);
        Timer.Register(warningTime, () => doFlood(dir, generateCoord, width, willFlood));
    }
    void doFlood(Tile.Direction dir, int coord, int width, bool flood)
    {
        tiles.sendWave(dir, coord, width, flood);
        tideTimer = Timer.Register(tideCooldown, () => doFloodWarning());
    }


}
