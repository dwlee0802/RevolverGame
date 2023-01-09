using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardEntryScript : MonoBehaviour
{
    public Card card;
    public UIManager uiManager;

    public void ButtonPressed()
    {
        uiManager.SelectCard(card);
    }
}
