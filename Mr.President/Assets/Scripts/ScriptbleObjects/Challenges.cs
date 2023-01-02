using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Challenge_", menuName = "ScriptableObjects/Challenge", order = 3)]
public class Challenges : ScriptableObject
{

    public float SliderValue;
    [Space(5)]
    public float PointTargetTowin;
    [Space(5)]
    public string WinText;
    public string LoseText;
    [Space(5)]
    public string Win_Spell_ID;
    public string Lose_Spell_ID;

    [Space(5)]
    public string Chapter_HID_Win;
    public string Chapter_HID_Lose;


    [Space(5)]
    public List<Challengflow> ChallengeFlow = new List<Challengflow>();
}

[System.Serializable]
public class Challengflow
{
    public string CardID;
    public ChallengeCardType CardType;
    public bool Available = true;
    public NewsCard SimpleCards = new NewsCard();
    public NewsCard FastCards = new NewsCard();
}


[System.Serializable]
public class NewsCard
{
    public string CardID;
    public string Text;
    public Sprite Image;
    [Header("Right And Left Card config")]
    public string LeftText;
    public string RightText;
    public float LeftValue;
    public float RightValue;
    public string Left_Head_ID;
    public string Right_Head_ID;
}

public enum ChallengeCardType
{
    simple,
    Fast,
}
