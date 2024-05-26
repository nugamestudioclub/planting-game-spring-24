using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class blackboardScript : MonoBehaviour
{
    // Static
    public static blackboardScript singleBlackboard = null;

    // General Text
    [SerializeField]
    TextMeshProUGUI title;

    //>>Plant Text<<
    [SerializeField]
    TextMeshProUGUI plantDescription;
    [SerializeField]
    Image plantDisplays;
    [SerializeField]
    Vector2 imageUpScale;
    [SerializeField]
    TextMeshProUGUI boostsLabel;
    [SerializeField]
    TextMeshProUGUI boostsField;

    //>>Tutorial<<
    [SerializeField]
    TextMeshProUGUI tutorialMain;

    // Adds single blackboard
    void Start()
    {
        if (GameObject.FindObjectsOfType<blackboardScript>().Length > 1)
        {
            Destroy(gameObject);
            Destroy(this);
        }
        else
        {
            singleBlackboard = this;
            
        }
    }

    // Showcases a plant
    public void plantShowcase(string plantID, bool placedPlant)
    {
        // Switches enabled UI
        tutorialMain.enabled = false;
        plantDescription.enabled = true;
        plantDisplays.enabled = true;
        boostsLabel.enabled = true;
        boostsField.enabled = true;

        // Inserts appropiate information to summarize the plant
        title.text = plantID;
        plantDescription.text = seedTypeLoader.singleLoader.getDescript(plantID);
        plantDisplays.color = Color.white;
        plantDisplays.sprite = seedTypeLoader.singleLoader.getSprite(plantID);
        plantDisplays.SetNativeSize();
        plantDisplays.gameObject.transform.localScale = imageUpScale;
        if (placedPlant)
        {
            boostsLabel.text = "Production Rate:";
            boostsField.text = "1x";
        }
        else
        {
            boostsLabel.text = "Plant not placed.";
            boostsField.text = "Place this plant on a plot of unused land! Brown tiles mark nutrient dense tiles that increase food production!";
        }
    }

    // Updates boosts of a plant
    public void boostsGive(Plant givenPlant)
    {
        string retString = (float)Mathf.Round(givenPlant.newNutritionModifier() * 100)/100 + "x <--- Nutrition rate modifier<br>";
        if(givenPlant.recievedModifierList.Count > 0)
        {
            Plant.PlantModifier[] modList = (Plant.PlantModifier[])givenPlant.recievedModifierList.ToArray();
            for (int i = 0; i < modList.Length; i++)
            {
                retString = retString + Mathf.Round((float)modList[i].modifierAmount / 100) * 100;
                retString = retString + "x <--- " + modList[i].reason + "<br>";
            }
        }
        retString = retString + "= " + (float)Mathf.Round((float)givenPlant.newExtraModifier() * givenPlant.newNutritionModifier() * 100) / 100 + "<br><br>";
        retString = retString + "Final Production time Equation<br>";
        retString = retString + "Base production time / Rate modifier =<br>" + (float)Mathf.Round((float)givenPlant.newProductionTime() * 100) / 100;
        boostsField.text = retString;
    }

    // Showcases instructions to play
    public void tutorialShow()
    {
        // Switches enabled UI
        tutorialMain.enabled = true;
        plantDescription.enabled = false;
        plantDisplays.color = Color.clear;
        plantDisplays.enabled = false;
        boostsLabel.enabled = false;
        boostsField.enabled = false;

        // Inserts appropiate tutorial instructions
        title.text = "How to Play";
    }
}
