using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Chapter_", menuName = "ScriptableObjects/NewChapter", order = 1)]
public class CardData : ScriptableObject
{

    [Header("<<Chapter Config>>")]
    public List<Chapter> ChapterFlow = new List<Chapter>();
    [Space(5)]
    [Header("<<Cards Config>>")]
    public CardDetails StartCard = new CardDetails();
    public List<CardDetails> RandomCards = new List<CardDetails>();
    public List<CardDetails> SimpleCards = new List<CardDetails>();
    public List<CardDetails> FastCards = new List<CardDetails>();


    private void OnValidate()
    {
        setIds(RandomCards);
        setIds(SimpleCards);
        setIds(FastCards);
        OnRandomCardInChapter();
    }

    private void setIds(List<CardDetails> list)
    {
        if (list.Count > 1)
        {
            list[0].ID = 0;
            for (int i = 1; i < list.Count; i++)
            {
                list[i].ID = list[i - 1].ID + 1;
            }
        }
        else
if (RandomCards.Count == 1)
        {
            RandomCards[0].ID = 0;
        }
    }

    private void OnRandomCardInChapter()
    {
        for (int i = 0; i < ChapterFlow.Count; i++)
        {
            if (ChapterFlow[i].CardType == CardType.Random)
                ChapterFlow[i].CardID = -1;         
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

    public CardType CardType;
   

    [Header("Right And Left Card config")]

    public int Left_Head_ID;
  
    public int Right_Head_ID;
    [Space(1)]

    public string Right_Choose_Text;

    public string Left_Choose_Text;

    public List<Ministers> Ministers = new List<Ministers>();

    public CardDetails()
    {
        this.Right_Choose_Text = "ﻪﻠﺑ";
        this.Left_Choose_Text = "ﺮﯿﺧ";
    }
}

[System.Serializable]
public class Chapter
{

    public bool Available = true;
    [DrawIf("Available",true)]
    public int CardID;
    [DrawIf("Available", true)]
    public CardType CardType;

    public Chapter()
    {
        CardID = -1;
        CardType = CardType.Random;
    }
}

[System.Serializable]
public class Ministers 
{
    public int id;
    public float Left_value = -0.1f;
    public float Right_value = +0.1f;

    public Ministers()
    {
       Left_value = -0.1f;
       Right_value = +0.1f;
    }
}

public enum CardType
{ 
    Random,
    Fast,
    simple,
    StartCard,
}
