using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    #region vars
    // Checks if plant was initialized
    bool initialized = false;

    // Used to find description
    public string plantName;

    // In order to update plants upon change of position
    // Put only Plant scripts into this
    static ArrayList plantList;

    // Caches
    Tile cacheTile;
    SpriteRenderer cacheRenderer;

    //>>Visuals<<
    // Prefab that appears on the disconnect of the plant
    [SerializeField]
    GameObject uponDisconnect;
    // Prefab that appears on the production of food
    [SerializeField]
    GameObject uponFoodProd;
    // The plant will bounce a set amount
    [SerializeField]
    float bounceAmplitude;
    [SerializeField]
    float bounceSpeed;
    // Tracks initial size
    float initHeight;
    // Tracks the progress of the bouncing
    float bounceTime = 0;

    //>>Production<<
    // Equation for production updates upon the production of food
    // The updated amount follows this equation
    // newTime = (baseTime) / (nutritionModifier * otherModifiers)

    // Base amount of time for production
    public float baseTimeUntilProduce;
    float timeLeftUntilProduce = 0;

    // Susceptibility to nutrition
    // nutrition calculated through equation
    // nutritionModifier = (1 - (0.5-(nutrition/maxNutrition)))^nutritionSus
    [SerializeField]
    float nutritionSusc;
    //>>Modifiers<<
    public class PlantModifier
    {
        public float modifierAmount;
        public string reason;
        // Plant that recieves the modifier
        public Plant recievePlant;
        // Plant that gives the modifier
        public Plant givePlant;
        public bool selfInflicted() { return recievePlant == givePlant; }
        public PlantModifier(float modAm, string reas, Plant recieve, Plant give)
        {
            modifierAmount = modAm;
            reason = reas;
            recievePlant = recieve;
            givePlant = give;
        }
    }
    // In order to keep track of the modifiers present affecting the plant
    // Only add PlantModifier classes
    [SerializeField]
    public ArrayList recievedModifierList = new ArrayList();
    // In order to keep track of the modifiers the plant inflicts on others
    // Only add PlantModifier classes
    [SerializeField]
    public ArrayList givenModifierList = new ArrayList();

    #endregion

    #region functions

    //>>Disconnect and Initialization<<
    // Disconnects plant from tile
    public virtual void disconnect()
    {
        Instantiate(uponDisconnect, transform.position, Quaternion.identity.normalized);
        for(int i = 0; i < givenModifierList.Count; i++)
        {
            deleteModifier((PlantModifier)givenModifierList[i]);
        }
        for(int i = 0; i < recievedModifierList.Count; i++)
        {
            deleteModifier((PlantModifier)recievedModifierList[i]);
        }
        Destroy(gameObject);
        Destroy(this);
    }

    // Initializes the plant
    public virtual void initPlant(Tile insertTile, string givenPlant)
    {
        plantName = givenPlant;
        cacheTile = insertTile;
        cacheRenderer = gameObject.GetComponent<SpriteRenderer>();
        cacheRenderer.sortingOrder = -(int)(transform.position.y * 100);
        initHeight = transform.localScale.y;
        initialized = true;
        plantList.Add(this);
    }

    // Creates new plant list upon initiation
    public static void initPlantList()
    {
        plantList = new ArrayList();
    }

    //>>Special Plant Update<<
    // Notifies all plants when a plant has been placed or destroyed.
    public static void updateAllSpecialPlants()
    {
        for(int i = 0; i < plantList.Count; i++)
        {
            ((Plant)plantList[i]).updateSpecialPlant();
        }
    }
    // For class children of plant
    // Checks if for updates
    public virtual void updateSpecialPlant()
    {

    }

    //>>Modifiers<<
    // Calculates the nutrition modifier based on tile nutrition and nutritionSusc
    public float newNutritionModifier()
    {
        float nutritionMod = (float)(1 - (0.5 - (cacheTile.nutritionLevel / cacheTile.maxNutr)));
        nutritionMod = Mathf.Pow(nutritionMod, nutritionSusc);
        return nutritionMod;
    }
    // Multiplies together all of the extra modifiers
    public float newExtraModifier()
    {
        float extraMod = 1;
        for (int i = 0; i < recievedModifierList.Count; i++)
        {
            extraMod *= ((PlantModifier)recievedModifierList[i]).modifierAmount;
        }
        return extraMod;
    }
    // Uses existing modifiers to produce new time until produce
    public float newProductionTime()
    {
        float nutritionMod = newNutritionModifier();
        float extraMod = newExtraModifier();
        return baseTimeUntilProduce / (extraMod * nutritionMod);
    }
    // Gives a modifier
    public void giveModifier(Plant givenTo, float amount, string description)
    {
        PlantModifier newModifier = new PlantModifier(amount, description, givenTo, this);
        givenModifierList.Add(newModifier);
        newModifier.recievePlant.recievedModifierList.Add(newModifier);
    }

    // Removes a modifier
    public void deleteModifier(PlantModifier modifier)
    {
        modifier.givePlant.givenModifierList.Remove(modifier);
        modifier.recievePlant.recievedModifierList.Remove(modifier);
    }

    // Produces food
    void produceFood()
    {
        timeLeftUntilProduce = newProductionTime();
        Instantiate(uponFoodProd, transform.position, Quaternion.identity.normalized);
        GameManager.singleManager.increaseFood(1);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (initialized)
        {
            // Handles plant bouncing
            bounceTime += Time.deltaTime;
            transform.localScale = new Vector3(transform.localScale.x, initHeight + bounceAmplitude * Mathf.Sin(bounceTime * bounceSpeed));

            // Handles food production
            timeLeftUntilProduce -= Time.deltaTime;
            if(timeLeftUntilProduce <= 0)
            {
                produceFood();
            }
        }
    }
    #endregion
}
