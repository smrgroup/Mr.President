using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SpellController : MonoBehaviour
{

    public SpellData spelldata;
    public SpellData Clonespelldata;

    public List<Spell> spells; 
    
    public List<MinistersID> ministers;
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

    public float Spelldecision(float value , int MinisterID)
    {
        foreach (Spell spell in spells)
        {
            float castORnot = Random.Range(0, 100);

            if (castORnot <= spell.Percent_to_Spell && spell.Times_to_Spell > 0)
            {
                spell.Times_to_Spell--;

                if (spell.Count_Effect)
                {
                    if (spell.SpecialMinisters.Count != 0)
                    {
                        bool isexist = spell.SpecialMinisters.Any(x => x.id== MinisterID);
                        if (isexist)
                        {
                            return value += spell.CountEffect; 
                        }
                        else
                            return value;
                    }
                    else
                    {
                        return value += spell.CountEffect;
                    }
                }
                else if (spell.Percent_Effect)
                {

                    if (spell.SpecialMinisters.Count != 0)
                    {
                        bool isexist = spell.SpecialMinisters.Any(x => x.id == MinisterID);
                        if (isexist)
                        {
                            float pernetage = (spell.PercentEffect / 100) * value;
                            return value += pernetage;
                        }
                        else
                            return value;
                    }
                    else
                    {
                        float pernetage = (spell.PercentEffect / 100 ) * value;
                        return value += pernetage;
                    }

                }

            }
        }

        return 0;

    }

}
