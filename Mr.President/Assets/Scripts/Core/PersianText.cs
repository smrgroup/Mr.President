using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PersianText : MonoBehaviour
{
    [ContextMenu("Convert To Persian")]
    void ConvertToPersian()
    {
        GetComponent<TextMeshProUGUI>().text = Fa.faConvert(GetComponent<TextMeshProUGUI>().text);
    }
}
