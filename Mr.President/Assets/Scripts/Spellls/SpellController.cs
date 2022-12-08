using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpellController : MonoBehaviour
{

    public SpellData spelldata;
    public SpellData Clonespelldata;
    public List<Spell> spells; 
    public List<MinistersID> ministers;

    //UI Stuff
    public List<SpellItem> spellItems;


    public bool IsActiveSpell = false;
    Spell Activespell;
    int baseCardCount;
    // Start is called before the first frame update
    void Start()
    {
        spells = new List<Spell>();
        Clonespelldata = Instantiate(spelldata);
        spells = Clonespelldata.Spells;

        spellItems = FindObjectsOfType<SpellItem>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float Spelldecision()
    {

        if (!IsActiveSpell)
        {
           // spells = Shuffle(spells);
            foreach (Spell spell in spells)
            {
                float castORnot = Random.Range(0, 100);
                Debug.Log(castORnot+"%");
                if (castORnot <= spell.Percent_to_Spell && spell.Times_to_Spell > 0)
                {
                    if (!spell.IsPowrUp)
                    {
                        addspellItem(spell);
                        SetspellActive(spell);
                        //Because its auo Active
                        Activespell.Cards_Count--;
                        break;
                    }
                    else
                    {
                        addspellItem(spell);
                    }
                }
            }
        }
        else
        {
            Activespell.Cards_Count--;
        }
        return 0;
    }

    public void SetspellActive(Spell spell)
    {
        spell.Times_to_Spell--;
        baseCardCount = spell.Cards_Count;
        IsActiveSpell = true;
        Activespell = spell;
        Debug.Log("Spell Actived");
    }

    void destroyspell()
    {
        IsActiveSpell = false;
        Activespell.Cards_Count = baseCardCount;

        foreach (SpellItem item in spellItems)
        {
            if (item.spell != null)
            {
                if (item.spell.Id == Activespell.Id && item.isActive)
                { 
                item.spell = null;
                item.hasSpell = false;
                item.isActive = false;
                item.GetComponent<Image>().enabled = false;
                item.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    break;
                }
            }

        }

        Activespell = new Spell();
    }

    public void addspellItem(Spell spell)
    {
        for (int i = 0; i < spellItems.Count; i++)
        {
            if (!spellItems[i].hasSpell)
            {
                spellItems[i].GetComponent<Image>().enabled = true;
                spellItems[i].hasSpell = true;
                spellItems[i].spell = spell;

                // Spell Auto Active
                if (!spellItems[i].spell.IsPowrUp)
                    spellItems[i].isActive = true;

                break;
            }
        }
    }

    public float addAffect(MinisterController minister , float value)
    {

        bool EndOfCard = false;
        if (Activespell.Cards_Count <= 0)
        {
            EndOfCard = true;
        }

        addParticleEffect();

        if (Activespell.Count_Effect)
        {
            if (Activespell.SpecialMinisters.Count != 0)
            {
                //if special ministers
                bool isexist = Activespell.SpecialMinisters.Any(x => x.id == minister.id);
                if (isexist)
                {
                    if (EndOfCard) destroyspell();
                    if (Activespell.IsPowrUp)
                        return value += Activespell.CountEffect;
                    else
                        return value -= Activespell.CountEffect;

                }
                else
                {
                    if (EndOfCard) destroyspell();
                    return value;
                }
            }
            else
            {
                if (EndOfCard) destroyspell();
                if (Activespell.IsPowrUp)
                    return value += Activespell.CountEffect;
                else
                    return value -= Activespell.CountEffect;
            }
        }
        else if (Activespell.Percent_Effect)
        {

            if (Activespell.SpecialMinisters.Count != 0)
            {
                //if special ministers
                bool isexist = Activespell.SpecialMinisters.Any(x => x.id == minister.id);
                if (isexist)
                {
                    if (EndOfCard) destroyspell();
                    
                    float pernetage = (Activespell.PercentEffect / 100) * value;
                    if (Activespell.IsPowrUp)
                        return value += pernetage;
                    else
                        return value -= pernetage;
                }
                else
                {
                    if (EndOfCard) destroyspell();
                    return value;
                }
            }
            else
            {
                if (EndOfCard) destroyspell();
                float pernetage = (Activespell.PercentEffect / 100) * value;
                if (Activespell.IsPowrUp)
                    return value += pernetage;
                else
                    return value -= pernetage;
            }

        }

        return 0;

    }

    void addParticleEffect()
    {
        foreach (SpellItem item in spellItems)
        {
            if (item.spell.Id == Activespell.Id && item.isActive)
            {
                ParticleSystem particle;
                if (item.spell.IsPowrUp)
                    particle = item.PowerUp;
                else
                    particle = item.Spell;

                Instantiate(particle, item.transform.position, item.transform.rotation, item.transform);
                break;
            }

        }
    }

    public List<Spell> Shuffle(List<Spell> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, list.Count);
            Spell value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

        return list;
    }

}