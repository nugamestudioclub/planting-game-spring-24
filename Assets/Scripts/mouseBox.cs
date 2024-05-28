using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseBox : MonoBehaviour
{
    [SerializeField]
    float xBound;
    [SerializeField]
    float yBound;
    SpriteRenderer cacheRender;
    public Tile cacheTile;
    public bool mouseInBox;
    // Loads cache
    public void init(Tile insertTile)
    {
        cacheRender = gameObject.GetComponent<SpriteRenderer>();
        cacheTile = insertTile;
    }
    // Updates sprite color
    public void updateColor(Color insertColor)
    {
        cacheRender.color = insertColor;
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 boxPos = transform.position;
        // If mouse inside box
        if (xBound + boxPos.x > mousePos.x && boxPos.x - xBound< mousePos.x && yBound + boxPos.y > mousePos.y && boxPos.y - yBound < mousePos.y)
        {
            cursorScript.singleCursor.setLastMouseBox(this);
            cacheRender.color = new Color(cacheRender.color.r, cacheRender.color.g, cacheRender.color.b, 1);
            mouseInBox = true;
            if(Input.GetAxisRaw("Plant") != 0)
            {
                cursorScript.singleCursor.plantPlot(cacheTile);
            }
            if (Input.GetAxisRaw("Dig") != 0)
            {
                cursorScript.singleCursor.digPlot(cacheTile);
            }
        }
        // If mouse outside box
        else
        {
            cacheRender.color = new Color(cacheRender.color.r, cacheRender.color.g, cacheRender.color.b, 0);
            mouseInBox = false;
        }
    }
}
