using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanAnim : MonoBehaviour
{
    bool active;
    // Cached vehicles
    Animator cacheAnimator;
    // Tracks if animation is in play
    const float animTime = 1;
    float animTimeLeft = 0;

    public void initState(bool isUnderwater)
    {
        active = true;
        cacheAnimator = gameObject.GetComponent<Animator>();
        if (!isUnderwater)
        {
            cacheAnimator.SetTrigger("initSkip");
        }
        cacheAnimator.SetBool("inWater", isUnderwater);
    }
    public void floodObject(Tile.Direction dir, bool isUnderwater)
    {
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

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if ((int)Time.realtimeSinceStartup % 2 == 0)
            {
                cacheAnimator.SetInteger("OceanState", 1);
            }
            else
            {
                cacheAnimator.SetInteger("OceanState", 2);
            }
        }
    }
}
