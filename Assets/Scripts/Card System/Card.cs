using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public static int elementCount = 4;

    public int[] adds = new int[elementCount];
    public int[] dmgPer = new int[elementCount];
    public float[] multiplies = new float[elementCount];

    public List<Ingredient> ingredients = new List<Ingredient>();

    public Card(List<Ingredient> ingredients = null)
    {
        this.ingredients = ingredients;
        RecalculateCard();
    }

    public void AddIngredient(Ingredient ingredient)
    {
        ingredients.Add(ingredient);
        RecalculateCard();
    }

    public void RemoveIngredient(Ingredient ingredient)
    {
        ingredients.Remove(ingredient);
        RecalculateCard();
    }

    public void RecalculateCard()
    {
        if(ingredients == null)
        {
            return;
        }


        adds = new int[elementCount];
        dmgPer = new int[elementCount];
        multiplies = new float[elementCount];

        foreach (var item in ingredients)
        {
            for (int i = 0; i < elementCount; i++)
            {
                adds[i] += item.adds[i];
                dmgPer[i] += item.dmgPer[i];
            }
        }

        foreach (var item in ingredients)
        {
            for (int i = 0; i < elementCount; i++)
            {
                adds[i] = (int)(adds[i] * item.multiplies[i]);
            }
        }
    }

    public override string ToString()
    {
        string output = "Card Info\nAdds: \n";

        int zeros = 0;

        for (int i = 0; i < elementCount; i++)
        {
            if (adds[i] == 0)
            {
                zeros++;
                continue;
            }

            switch (i)
            {
                case 0:
                    output += "Red: ";
                    break;
                case 1:
                    output += "Blue: ";
                    break;
                case 2:
                    output += "Black: ";
                    break;
                case 3:
                    output += "White: ";
                    break;
            }

            output += adds[i] + " ";
        }

        if (zeros == elementCount)
        {
            output += "None";
        }

        zeros = 0;

        output += "\nDmg Per: \n";

        for (int i = 0; i < elementCount; i++)
        {
            if (dmgPer[i] == 0)
            {
                zeros++;
                continue;
            }

            switch (i)
            {
                case 0:
                    output += "Red: ";
                    break;
                case 1:
                    output += "Blue: ";
                    break;
                case 2:
                    output += "Black: ";
                    break;
                case 3:
                    output += "White: ";
                    break;
            }

            output += dmgPer[i] + " ";
        }

        if (zeros == elementCount)
        {
            output += "None";
        }

        output += "\nIngredients:\n";

        if(ingredients != null)
        {
            foreach (var item in ingredients)
            {
                output += item.ingredientName + " ";
            }
        }

        return output;
    }

    public string GetIngredientNames()
    {
        string output = "";

        if(ingredients == null || ingredients.Count == 0)
        {
            return "None";
        }

        foreach(var item in ingredients)
        {
            output += item.ingredientName + " ";
        }

        return output;
    }
}
