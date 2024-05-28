using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    // Static accsess
    public static GameManager singleManager;

    // Cache
    TileManager tiles;

    //>>Food<<
    [SerializeField]
    int startingFood;
    int food;
    [SerializeField]
    int foodLostPerCycle;
    [SerializeField]
    forageButton cacheButton;

    //>>Tide Manager<<
    // Counted in seconds
    [SerializeField]
    int width;
    [SerializeField]
    private float floodChance;
    [SerializeField]
    private int maxLand;
    [SerializeField]
    private int minLand;

    //>>Timers<<
    Timer foodTimer;
    Timer tideWarningTimer;
    Timer forageTimer;
    float timeSinceStart = 0;

    //>>Time Expressed<<
    // All cooldowns curve up and down
    // Food decrease curve
    [SerializeField]
    private float foodDecreaseCurveAmp;
    [SerializeField]
    private float foodDecreaseCurveBase;
    [SerializeField]
    private float foodDecreaseExpMulti;
    [SerializeField]
    private float foodDecreaseBaseAdd;

    // Forage cooldown curve
    [SerializeField]
    private float forageCooldownCurveAmp;
    [SerializeField]
    private float forageCooldownCurveBase;
    [SerializeField]
    private float forageCooldownExpMulti;
    [SerializeField]
    private float forageCooldownBaseAdd;

    // Tide warning curve
    [SerializeField]
    private float warningCurveAmp;
    [SerializeField]
    private float warningCurveBase;
    [SerializeField]
    private float warningExpMulti;
    [SerializeField]
    private float warningBaseAdd;

    // Tide cooldown curve
    [SerializeField]
    private float tideCooldownCurveAmp;
    [SerializeField]
    private float tideCooldownCurveBase;
    [SerializeField]
    private float tideCooldownExpMulti;
    [SerializeField]
    private float tideCooldownBaseAdd;

    //>>UI<<
    [SerializeField]
    TextMeshProUGUI forageText;
    [SerializeField]
    TextMeshProUGUI foodText;
    [SerializeField]
    TextMeshProUGUI timeText;

    public bool CanForage = true;

    //>>Expressed Cooldown Functions<<
    float getPointInCurve(float time, float curveAmplitude, float curveBase, float exponentMultiplier, float baseAdd)
    {
        return curveAmplitude * Mathf.Pow(curveBase, exponentMultiplier * time) + baseAdd;
    }
    float expressedFoodDecreaseTime()
    {
        return getPointInCurve(timeSinceStart, foodDecreaseCurveAmp, foodDecreaseCurveBase, foodDecreaseExpMulti, foodDecreaseBaseAdd);
    }
    float expressedTideCooldownTime()
    {
        return getPointInCurve(timeSinceStart, tideCooldownCurveAmp, tideCooldownCurveBase, tideCooldownExpMulti, tideCooldownBaseAdd);
    }
    float expressedForageCooldownTime()
    {
        return getPointInCurve(timeSinceStart, forageCooldownCurveAmp, forageCooldownCurveBase, forageCooldownExpMulti, forageCooldownBaseAdd);
    }
    float expressedWarningTime()
    {
        return getPointInCurve(timeSinceStart, warningCurveAmp, warningCurveBase, warningExpMulti, warningBaseAdd);
    }
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
            foodTimer = Timer.Register(expressedFoodDecreaseTime(), () => decreaseFood(foodLostPerCycle));
            tideWarningTimer = Timer.Register(expressedWarningTime(), () => doFloodWarning());
            forageTimer = Timer.Register(expressedForageCooldownTime(), () => reinitateForageAval());
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
        Timer.Register(expressedFoodDecreaseTime(), () => decreaseFood(foodLostPerCycle));
        if(food <= 0)
        {
            onLose();
        }
    }
    public void increaseFood(int val)
    {
        food = food + val;
    }
    public void onLose()
    {
        SceneManager.LoadScene("LoseScreen");
    }

    // Forage functions
    void reinitateForageAval()
    {
        cacheButton.forageAvaliable();
    }
    public void restartForage()
    {
        forageTimer = Timer.Register(expressedForageCooldownTime(), () => reinitateForageAval());
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
        tiles.sendWarning(dir, generateCoord, width, willFlood, expressedWarningTime());
        Timer.Register(expressedWarningTime(), () => doFlood(dir, generateCoord, width, willFlood));
    }
    void doFlood(Tile.Direction dir, int coord, int width, bool flood)
    {
        tiles.sendWave(dir, coord, width, flood);
        tideWarningTimer = Timer.Register(expressedTideCooldownTime(), () => doFloodWarning());
    }


}
