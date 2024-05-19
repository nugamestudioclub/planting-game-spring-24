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
    bool mouseInBox;
    // Loads cache
    public void init()
    {
        cacheRender = gameObject.GetComponent<SpriteRenderer>();
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
        if (xBound + boxPos.x > mousePos.x && boxPos.x - xBound< mousePos.x && yBound + boxPos.y > mousePos.y && boxPos.y - yBound < mousePos.y)
        {
            cacheRender.color = new Color(cacheRender.color.r, cacheRender.color.b, cacheRender.color.g, 1);
            mouseInBox = true;
        }
        else
        {
            cacheRender.color = new Color(cacheRender.color.r, cacheRender.color.b, cacheRender.color.g, 0);
            mouseInBox = false;
        }
    }
}
