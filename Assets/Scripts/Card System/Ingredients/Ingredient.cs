using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Ingredients are what makes up the cards.
 * Based on what kind of ingredients are contained, they have different effects.
 */
[CreateAssetMenu(fileName = "Ingredient", menuName = "Ingredient/Base", order = 1)]
public class Ingredient : ScriptableObject
{
    public string ingredientName;

    public int count;

    public int[] adds = new int[Card.elementCount];
    public float[] multiplies = new float[Card.elementCount];
    public int[] dmgPer = new int[Card.elementCount];

    public virtual void Effect(UnitScript user, UnitScript target = null)
    {
        return;
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
