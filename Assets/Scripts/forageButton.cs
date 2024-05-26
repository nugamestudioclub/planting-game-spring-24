using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class forageButton : MonoBehaviour
{

    GameManager cacheManager;
    Button cacheButton;
    bool avaliable = false;
    // Start is called before the first frame update
    public void init(GameManager insertManager)
    {
        cacheManager = insertManager;
        cacheButton = gameObject.GetComponent<Button>();
        avaliable = false;
    }

    public void forageAvaliable()
    {
        avaliable = true;
        cacheButton.interactable = true;
    }

    public string getForage()
    {
        avaliable = false;
        cacheButton.interactable = false;
        cacheManager.restartForage();
        return seedTypeLoader.singleLoader.getRandomID();
    }
}
