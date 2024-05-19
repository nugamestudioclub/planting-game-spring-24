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
    // Start is called before the first frame update
    void Start()
    {
        cacheRender = gameObject.GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 boxPos = transform.position;
        if (xBound + boxPos.x > mousePos.x && boxPos.x - xBound< mousePos.x && yBound + boxPos.y > mousePos.y && boxPos.y - yBound < mousePos.y)
        {
            cacheRender.color = new Color(cacheRender.color.r, cacheRender.color.b, cacheRender.color.g, 1);
        }
        else
        {
            cacheRender.color = new Color(cacheRender.color.r, cacheRender.color.b, cacheRender.color.g, 0);
        }
    }
}
