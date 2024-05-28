using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Uses enum to load scripts and load images
public class seedTypeLoader : MonoBehaviour
{
    [SerializeField]
    public static seedTypeLoader singleLoader;

    // Method of setting plants
    [SerializeField]
    string[] plantID;
    [SerializeField]
    string[] plantDescriptions;
    [SerializeField]
    GameObject[] plantPrefabs;
    [SerializeField]
    Sprite[] plantSprites;

    // Struct to avoid creating multiple dictionaries
    private struct plantValues
    {
        public GameObject prefab;
        public Sprite sprite;
        public string description;
    }


    // Actually stores information of plants
    Dictionary<string, plantValues> loaderDict = new Dictionary<string, plantValues>();

    // Creates hashmap
    void Awake()
    {
        if(GameObject.FindObjectsOfType<seedTypeLoader>().Length > 1)
        {
            Destroy(gameObject);
            Destroy(this);
        }
        else
        {
            singleLoader = this;
            for(int i = 0; i < plantID.Length; i++)
            {
                plantValues newValues = new plantValues();
                newValues.description = plantDescriptions[i];
                newValues.prefab = plantPrefabs[i];
                newValues.sprite = plantSprites[i];

                loaderDict.Add(plantID[i], newValues);
            }
        }
    }

    // Gets script, assumes ID is actually a valid ID
    public GameObject getPrefab(string id)
    {
        return loaderDict[id].prefab;
    }

    // Gets sprite, assumes ID is actually a valid ID
    public Sprite getSprite(string id)
    {
        return loaderDict[id].sprite;
    }

    // Gets description, assumes ID is actually a valid ID
    public string getDescript(string id)
    {
        return loaderDict[id].description;
    }

    // Gets random id
    public string getRandomID()
    {
        int idKey = Random.Range(0, plantID.Length);
        return plantID[idKey];
    }
}
