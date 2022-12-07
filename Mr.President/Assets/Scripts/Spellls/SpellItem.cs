using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class SpellItem : MonoBehaviour
{

    public bool hasSpell = false;
    public bool isActive = false;

    public ParticleSystem PowerUp;
    public ParticleSystem Spell;
    public Spell spell;
    private SpellController SpellController;

    // Start is called before the first frame update
    void Start()
    {
        SpellController = FindObjectOfType<SpellController>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActiveSpell()
    {
        if (!SpellController.IsActiveSpell)
        {
            SpellController.SetspellActive(spell);
            GetComponent<Image>().color = new Color32(255,0, 172,255);
            isActive = true;
        }
            
    }
}
