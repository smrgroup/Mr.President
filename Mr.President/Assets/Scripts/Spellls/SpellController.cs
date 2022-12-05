using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SpellController : MonoBehaviour
{

    public SpellData spelldata;
    public SpellData Clonespelldata;

    public List<Spell> spells; 
    
    public List<MinistersID> ministers;

    public bool IsActiveSpell = false;
    Spell Activespell;
    int baseCardCount;
    // Start is called before the first frame update
    void Start()
    {
        spells = new List<Spell>();
        Clonespelldata = Instantiate(spelldata);
        spells = Clonespelldata.Spells;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float Spelldecision()
    {

        if (!IsActiveSpell)
        {
            foreach (Spell spell in spells)
            {
                float castORnot = Random.Range(0, 100);
                Debug.Log(castORnot+"%");
                if (castORnot <= spell.Percent_to_Spell && spell.Times_to_Spell > 0)
                {
                    spell.Times_to_Spell--;
                    baseCardCount = spell.Cards_Count;
                    IsActiveSpell = true;
                    Activespell = spell;
                    Activespell.Cards_Count--;
                    Debug.Log("Spell Actived");
                    break;
                }
            }
        }
        else
        {
            Activespell.Cards_Count--;
        }
        return 0;
    }

    public float addAffect(MinisterController minister , float value)
    {

        if (Activespell.Cards_Count <= 0)
        {
            IsActiveSpell = false;
            Activespell.Cards_Count = baseCardCount;
            return 0; 
        }

        if (Activespell.Count_Effect)
        {
            if (Activespell.SpecialMinisters.Count != 0)
            {
                bool isexist = Activespell.SpecialMinisters.Any(x => x.id == minister.id);
                if (isexist)
                {
                    return value += Activespell.CountEffect;
                }
                else
                    return value;
            }
            else
            {
                return value += Activespell.CountEffect;
            }
        }
        else if (Activespell.Percent_Effect)
        {

            if (Activespell.SpecialMinisters.Count != 0)
            {
                bool isexist = Activespell.SpecialMinisters.Any(x => x.id == minister.id);
                if (isexist)
                {
                    float pernetage = (Activespell.PercentEffect / 100) * value;
                    return value += pernetage;
                }
                else
                    return value;
            }
            else
            {
                float pernetage = (Activespell.PercentEffect / 100) * value;
                return value += pernetage;
            }

        }

        return 0;

    }

}
