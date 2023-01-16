using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Chapter_", menuName = "ScriptableObjects/NewChapter", order = 1)]
public class CardData : ScriptableObject
{

    [Header("<<Chapter Config>>")]
    public List<Chapter> ChapterFlow = new List<Chapter>();
    public List<RandomCardsList> RandomCards = new List<RandomCardsList>();

    private void OnValidate()
    {

        for (int i = 0; i < ChapterFlow.Count; i++)
        {
            ChapterFlow[i].elementName = ChapterFlow[i].CardID;
        }

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
public class RandomCardsList
{
    public string RandomListID;
    public List<CardDetails> RandomCards = new List<CardDetails>();
}

[System.Serializable]
public class Chapter
{
    [HideInInspector]
    public string elementName;

    public bool _Challenge = false;

    [Header("-------------------------------------------------------")]

    //if Flow Is Challenge
    [DrawIf("_Challenge", true)]
    public Challenges Challenge;


    //if Flow Is Card
    [DrawIf("_Challenge", false)] 
    public bool Available = true;

    [DrawIf("Available",true)]
    public string CardID;
    
    [DrawIf("Available", true)]
    [DrawIf("_Challenge", false)]
    public CardType CardType;
    [Header("if Cardtype is random enter listID below")]
    public string RandomlistID;


    [Space(10)]
    [Header("<<Cards>>")]
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
    public string id;
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
}
