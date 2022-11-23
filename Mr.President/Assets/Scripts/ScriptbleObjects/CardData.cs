using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/CardData", order = 1)]
public class CardData : ScriptableObject
{
    public List<CardDetails> Cards = new List<CardDetails>();

    private void OnValidate()
    {
        if (Cards.Count > 1)
        {
            Cards[0].ID = 0;
            Cards[0].Name = "Card_" + Cards[0].ID;
            for (int i = 1; i < Cards.Count; i++)
            {
                Cards[i].ID = Cards[i - 1].ID + 1;
                Cards[i].Name = "Card_" + Cards[i].ID;

            }
        }
        else
        if (Cards.Count == 1)
        { 
            Cards[0].ID = 0;
            Cards[0].Name = "Card_" + Cards[0].ID;
        }
    }
}

[System.Serializable]
public class CardDetails
{
    public int ID;
    public string Name;
    public string Text;
    public Sprite Image;

    public int Left_Card_ID;
    public int Right_Card_ID;

}
