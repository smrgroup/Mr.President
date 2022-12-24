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

    public IEnumerator loadministers(float interval = 1.0f)
    { 
        yield return new WaitForSeconds(interval);
        TopTweens.OpenCloseObjectAnimation();
        LeftTweens.OpenCloseObjectAnimation();
        RightTweens.OpenCloseObjectAnimation();
    }
}
