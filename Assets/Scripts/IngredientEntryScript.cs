using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientEntryScript : MonoBehaviour
{
    public Ingredient thisIngredient;
    public UIManager uiManager;

    public void ButtonPressed()
    {
        //in inventory
        if(transform.parent == uiManager.inventoryUI)
        {
            if(uiManager.selectedCardEntry != null)
            {
                transform.GetComponent<Image>().enabled = !transform.GetComponent<Image>().enabled;
                transform.SetParent(uiManager.ingredientsUI);
            }
        }
        //in ingredient
        else
        {
            if(transform.GetComponent<Image>().enabled == true)
            {
                transform.SetParent(uiManager.inventoryUI);
                transform.GetComponent<Image>().enabled = !transform.GetComponent<Image>().enabled;
            }
            else
            {
                transform.SetParent(uiManager.inventoryUI);
                transform.GetComponent<Image>().enabled = !transform.GetComponent<Image>().enabled;
            }
        }
    }
}
