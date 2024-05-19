using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// In flashing arrows
public class flashingScript : MonoBehaviour
{
    [SerializeField]
    public bool active = false;
    [SerializeField]
    Color recedeColor;
    [SerializeField]
    Color waveColor;
    [SerializeField]
    float flashMultiplier;
    [SerializeField]
    SpriteRenderer cacheRender;
    [SerializeField]
    float riseSpeed;
    [SerializeField]
    float timeLeftUntilDesctructLeft;
    [SerializeField]
    Sprite[] arrowSprites;
    [SerializeField]
    Tile.Direction flashDir;
    // Caches and initializes component
    public void init(Tile.Direction dir, float timeLeft, bool flood)
    {
        active = true;
        cacheRender = gameObject.GetComponent<SpriteRenderer>();
        timeLeftUntilDesctructLeft = timeLeft;
        if (flood)
        {
            cacheRender.color = waveColor;
        }
        else
        {
            cacheRender.color = recedeColor;
        }
        flashDir = dir;
        if(dir == Tile.Direction.L)
        {
            cacheRender.sprite = arrowSprites[0];
        }
        if (dir == Tile.Direction.U)
        {
            cacheRender.sprite = arrowSprites[1];
        }
        if (dir == Tile.Direction.R)
        {
            cacheRender.sprite = arrowSprites[2];
        }
        if (dir == Tile.Direction.D)
        {
            cacheRender.sprite = arrowSprites[3];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if ((int)(Time.realtimeSinceStartup * flashMultiplier) % 2 == 0)
            {
                transform.position = transform.position + transform.up * riseSpeed * Time.deltaTime;
                cacheRender.color = new Color(cacheRender.color.r, cacheRender.color.g, cacheRender.color.b, 1);
            }
            else
            {
                transform.position = transform.position + transform.up * -riseSpeed * Time.deltaTime;
                cacheRender.color = new Color(cacheRender.color.r, cacheRender.color.g, cacheRender.color.b, 0);
            }
            timeLeftUntilDesctructLeft -= Time.deltaTime;
            if (timeLeftUntilDesctructLeft < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
