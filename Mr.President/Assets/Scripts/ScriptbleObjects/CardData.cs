using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chapter_", menuName = "ScriptableObjects/NewChapter", order = 1)]
public class CardData : ScriptableObject
{

    [Header("<<Chapter Config>>")]
    public List<Chapter> ChapterFlow = new List<Chapter>();


    private void OnValidate()
    {
      
    }
}

[System.Serializable]
public class CardDetails
{

    public string ID;

    public string Name;

    public string Text;
  
    public Sprite Image;
   

    [Header("Right And Left Card config")]

    public string Left_Head_ID;
  
    public string Right_Head_ID;
    [Space(1)]

    public string Left_Choose_Text;

    public string Right_Choose_Text;

    [Space(10)]
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

    public bool _Challenge = false;
    public bool _Card = true;

    [Header("-------------------------------------------------------")]

    //if Flow Is Card
    [DrawIf("_Card", true)] 
    public bool Available = true;

    [DrawIf("Available",true)]
    [DrawIf("_Card", true)]
    public string CardID;
    
    [DrawIf("Available", true)]
    [DrawIf("_Card", true)]
    public CardType CardType;

    [Space(10)]
    [Header("<<Cards>>")]
    public List<CardDetails> RandomCards = new List<CardDetails>();
    public CardDetails SimpleCards = new CardDetails();
    public CardDetails FastCards = new CardDetails();


    public Chapter()
    {
        CardID = "-1";
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
