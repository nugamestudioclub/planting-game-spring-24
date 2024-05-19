using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Causes object attached to flash in and out of visibility
public class flashingScript : MonoBehaviour
{
    [SerializeField]
    float flashMultiplier;
    SpriteRenderer cacheRender;
    float timeLeftUntilDesctruct;
    // Start is called before the first frame update
    void Start()
    {
        cacheRender = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if((int)(Time.realtimeSinceStartup * flashMultiplier) % 2 == 0)
        {
            cacheRender.color = new Color(cacheRender.color.r, cacheRender.color.b, cacheRender.color.g, 1);
        }
        else
        {
            cacheRender.color = new Color(cacheRender.color.r, cacheRender.color.b, cacheRender.color.g, 0);
        }
    }
}
