using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinistersManager : MonoBehaviour
{

    public EasyTween TopTweens;
    public EasyTween LeftTweens;
    public EasyTween RightTweens;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(loadministers());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator loadministers()
    { 
        yield return new WaitForSeconds(1f);
        TopTweens.OpenCloseObjectAnimation();
        LeftTweens.OpenCloseObjectAnimation();
        RightTweens.OpenCloseObjectAnimation();
    }
}
