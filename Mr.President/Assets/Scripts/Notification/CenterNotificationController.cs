using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CenterNotificationController : MonoBehaviour
{

    public EasyTween[] CenterNotiftweens;

    public TextMeshProUGUI text;
    public TextMeshProUGUI Shadowtext;


    private void Awake()
    {
        CenterNotiftweens = GetComponents<EasyTween>();
    }


    public void PlayIn(string Text , Color32 color)
    {
        text.text = Text;
        Shadowtext.text = Text;
        text.color = color;
        StartCoroutine(startplay());
    }

    IEnumerator startplay()
    {
        CenterNotiftweens[0].OpenCloseObjectAnimation();
        yield return new WaitForSeconds(1);
        CenterNotiftweens[1].OpenCloseObjectAnimation();
    }
}
