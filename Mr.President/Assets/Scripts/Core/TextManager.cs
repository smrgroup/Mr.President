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

    private IEnumerator coroutine;

    private void Start()
    {
        textbox = GetComponent<TextMeshProUGUI>();
        textClip = GetComponent<AudioSource>();

}

    public void typemsg(string msg)
    {
        Sentenses = msg.Split(' ');
        textbox.text = "";
        coroutine = typemessage(msg);
        StopAllCoroutines();
        StartCoroutine(coroutine);
    }

    IEnumerator typemessage(string msg)
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
