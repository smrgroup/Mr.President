using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GameData
{
    public string Chapter;
    public int CardHead;
    public string CardHeadID;
    public List<Chapter> Lastchapterstate;
    public List<MinistersSaveData> ministers;
}

[System.Serializable]
public class MinistersSaveData
{
    public string id;
    public float value;

    public MinistersSaveData(string id, float value)
    {
        this.id = id;
        this.value = value;
    }
}
