using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class plantDisplay : MonoBehaviour
{   
    Image cacheSprite;
    // Start is called before the first frame update
    public void init()
    {
        cacheSprite = gameObject.GetComponent<Image>();
    }

    public void deactivateSprite()
    {
        cacheSprite.color = Color.clear;
    }
    void reactivateSprite()
    {
        cacheSprite.color = Color.white;
    }
    public void insertNewSprite(string plantID)
    {
        cacheSprite.sprite = seedTypeLoader.singleLoader.getSprite(plantID);
        cacheSprite.SetNativeSize();
        reactivateSprite();
    }
}
