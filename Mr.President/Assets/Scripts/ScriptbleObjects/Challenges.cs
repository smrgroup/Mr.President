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
    public List<NewsCard> Cards = new List<NewsCard>();
}



[System.Serializable]
public class NewsCard
{
    public string Text;
    public Sprite Image;
    public string LeftText;
    public string RightText;
    public float LeftValue;
    public float RightValue;
}
