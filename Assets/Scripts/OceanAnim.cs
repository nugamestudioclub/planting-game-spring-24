using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanAnim : MonoBehaviour
{
    // Whether component has been initialized
    bool active;
    // Whether deep ocean
    [SerializeField]
    bool deepOceanTile;
    // Cached vehicles
    Animator cacheAnimator;
    Tile cacheTile;
    // Bay objects
    GameObject leftBay;
    GameObject upBay;
    GameObject rightBay;
    GameObject downBay;
    // Tracks if animation is in play
    const float animTime = 1;
    bool inAnim = false;
    float animTimeLeft = 0;

    // deepOceanTile allows OceanAnim to partially initialize
    private void Start()
    {
        if (deepOceanTile)
        {
            cacheAnimator = gameObject.GetComponent<Animator>();
            active = true;
            cacheAnimator.SetBool("inWater", true);
        }
    }
    // Initializes the component
    public void initState(bool isUnderwater, Tile attachedTile)
    {
        active = true;
        leftBay = transform.GetChild(0).gameObject;
        upBay = transform.GetChild(1).gameObject;
        rightBay = transform.GetChild(2).gameObject;
        downBay = transform.GetChild(3).gameObject;
        cacheAnimator = gameObject.GetComponent<Animator>();
        cacheTile = attachedTile;
        if (!isUnderwater)
        {
            cacheAnimator.SetTrigger("initSkip");
        }
        cacheAnimator.SetBool("inWater", isUnderwater);
    }

    // Plays animation of object being flooded or water receding
    public void floodObject(Tile.Direction dir, bool isUnderwater)
    {
        inAnim = true;
        animTimeLeft = animTime;
        if(dir == Tile.Direction.L)
        {
            cacheAnimator.SetInteger("MoveDir", 0);
        }
        if (dir == Tile.Direction.U)
        {
            cacheAnimator.SetInteger("MoveDir", 1);
        }
        if (dir == Tile.Direction.R)
        {
            cacheAnimator.SetInteger("MoveDir", 2);
        }
        if (dir == Tile.Direction.D)
        {
            cacheAnimator.SetInteger("MoveDir", 3);
        }
        cacheAnimator.SetBool("inWater", isUnderwater);
    }

    // Update bay visibility
    public void updateBay(bool Lvis, bool Uvis, bool Rvis, bool Dvis)
    {
        leftBay.SetActive(Lvis);
        upBay.SetActive(Uvis);
        rightBay.SetActive(Rvis);
        downBay.SetActive(Dvis);
    }
    // Checks if the object is in animation or not
    public bool getIsInAnim()
    {
        return inAnim;
    }
    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            // Alternates default animations uniformly
            if ((int)Time.realtimeSinceStartup % 2 == 0)
            {
                cacheAnimator.SetInteger("OceanState", 1);
            }
            else
            {
                cacheAnimator.SetInteger("OceanState", 2);
            }
            if (inAnim)
            {
                animTimeLeft -= Time.deltaTime;
                if(animTimeLeft < 0)
                {
                    inAnim = false;
                    cacheTile.animComplete();
                }
            }
        }
    }
}
