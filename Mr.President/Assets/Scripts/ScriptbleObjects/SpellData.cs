using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SpellsData", menuName = "ScriptableObjects/SpellsData", order = 2)]
public class SpellData : ScriptableObject
{
    public List<Spell> Spells = new List<Spell>();

    private void OnValidate()
    {
        setIds(Spells);
    }

    private void setIds(List<Spell> list)
    {
        if (list.Count > 1)
        {
            list[0].Id = 0;
            for (int i = 1; i < list.Count; i++)
            {
                list[i].Id = list[i - 1].Id + 1;
            }
        }
        else
        if (list.Count == 1)
        {
            list[0].Id = 0;
        }
    }

}

[System.Serializable]
public class Spell 
{

    [Header("IS PowerUp")]
    public bool IsPowrUp = false;
    [Space(15)]
    public int Id;
    public string Name;
    [Space(10)]
    [Header("choose Spell Type")]
    public bool Percent_Effect = false;
    [DrawIf("Percent_Effect", true)]
    public float PercentEffect;
    public bool Count_Effect = false;
    [DrawIf("Count_Effect", true)]
    public float CountEffect;
    [Space(10)]
    [Header("Posibilty To Affect Spell")]
    public float Percent_to_Spell;
    [Header("Time To Repeat Spell in all game")]
    public int Times_to_Spell;
    [Space(10)]
    [Header("Add SpecialMinisters")]
    public List<MinistersID> SpecialMinisters = new List<MinistersID>();
    [Space(10)]
    [Header("Count Card Repeat")]
    public int Cards_Count;
}

[System.Serializable]
public class MinistersID
{
    public string id;
}

