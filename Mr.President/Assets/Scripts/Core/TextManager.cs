using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    string[] Sentenses;
    TextMeshProUGUI textbox;
    AudioSource textClip;

    private void Start()
    { 
        textbox = GetComponent<TextMeshProUGUI>();
        textClip = GetComponent<AudioSource>();
        Sentenses = textbox.text.Split(' ');
        textbox.text = "";
        StartCoroutine(typemessage());
    }

    IEnumerator typemessage()
    {
        for (int i = 0; i <= Sentenses.Length - 1; i++)
        {
            yield return new WaitForSeconds(0.1f);
            textbox.text += Sentenses[i];
            textbox.text += " ";
            textClip.Play(0);

            //string pretext = textbox.text;
            //textbox.text = Sentenses[i]+ ' ' + pretext;




        }
    }

}
