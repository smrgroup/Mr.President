using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/CardData", order = 1)]
public class CardData : ScriptableObject
{
    public List<CardDetails> Cards = new List<CardDetails>();
}

[System.Serializable]
public class CardDetails
{
    public string cardName;
    public Sprite Image;
}
