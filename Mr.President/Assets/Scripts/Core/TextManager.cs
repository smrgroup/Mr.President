using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    string[] Sentenses;
    Text textbox;
    private void Start()
    { 
        textbox = GetComponent<Text>();
        Sentenses = textbox.text.Split(' ');
        textbox.text = "";
        StartCoroutine(typemessage());
    }

    IEnumerator typemessage()
    {
        for (int i = Sentenses.Length - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(0.1f);
            string pretext = textbox.text;
            textbox.text = Sentenses[i]+ ' ' + pretext;

            if (textbox.text.Length >= 46)
                textbox.text += "\n";


        }
    }

}
