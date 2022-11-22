using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinisterController : MonoBehaviour
{

    Image borderfill;

    // Start is called before the first frame update
    void Start()
    {
        borderfill = GetComponent<Image>();
        StartCoroutine(StartFill());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StartFill()
    {

        yield return new WaitForSeconds(0.01f);

        while (borderfill.fillAmount != 1)
        {
            yield return new WaitForSeconds(0.00015f);
            borderfill.fillAmount += 0.005f;
        }
    }

}
