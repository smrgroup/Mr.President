using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersianText : MonoBehaviour
{
    [ContextMenu("Convert To Persian")]
    void ConvertToPersian()
    {
        GetComponent<Text>().text = Fa.faConvert(GetComponent<Text>().text);
    }
}
