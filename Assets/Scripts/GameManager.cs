using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    // Static accsess
    public static GameManager singleManager;

    // Cache
    TileManager tiles;

    // Food
    [SerializeField]
    int startingFood;
    int food;
    // Counted in seconds per food
    [SerializeField]
    private float foodDecrease;
    [SerializeField]
    forageButton cacheButton;
    //>>Forage<<
    // Counted in seconds
    [SerializeField]
    private float forageCooldown;

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

    //>>Timers<<
    Timer foodTimer;
    Timer tideWarningTimer;
    Timer forageTimer;
    float timeSinceStart = 0;

    //>>UI<<
    [SerializeField]
    TextMeshProUGUI forageText;
    [SerializeField]
    TextMeshProUGUI foodText;
    [SerializeField]
    TextMeshProUGUI timeText;

    public bool CanForage = true;

    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.FindObjectsOfType<GameManager>().Length > 1)
        {
            Destroy(gameObject);
            Destroy(this);
        }
        else
        {
            singleManager = this;
            Plant.initPlantList();
            cacheButton.init(this);
            tiles = gameObject.GetComponent<TileManager>();
            tiles.init();
            food = startingFood;
            foodTimer = Timer.Register(foodDecrease, () => decreaseFood(1));
            tideWarningTimer = Timer.Register(tideCooldown, () => doFloodWarning());
            forageTimer = Timer.Register(forageCooldown, () => reinitateForageAval());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (forageTimer.isDone)
        {
            forageText.text = "Ready To Forage!";
        }
        else
        {
            forageText.text = "Time Left Until Next Forage:<br>" + Mathf.Round(forageTimer.GetTimeRemaining());
        }
        foodText.text = "Food Left: " + food;
        timeSinceStart += Time.deltaTime;
        timeText.text = "Time survived:<br>" + Mathf.Round(timeSinceStart);
    }

    // Food functions
    void decreaseFood(int val)
    {
        food = Mathf.Max(food - val, 0);
        Timer.Register(foodDecrease, () => decreaseFood(1));
    }
    public void increaseFood(int val)
    {
        food = food + val;
    }
    // Forage functions
    void reinitateForageAval()
    {
        cacheButton.forageAvaliable();
    }
    public void restartForage()
    {
        forageTimer = Timer.Register(forageCooldown, () => reinitateForageAval());
    }

    // Tide functions
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
        tideWarningTimer = Timer.Register(tideCooldown, () => doFloodWarning());
    }


}
