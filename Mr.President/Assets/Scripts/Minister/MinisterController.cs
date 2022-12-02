using System.Collections;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MinisterController : MonoBehaviour
{
    [ReadOnlyAttribute]
    public int id;
    public float value = 1;
    Image borderfill;

    bool startfill = true;
    
    // Start is called before the first frame update
    void Start()
    {
        borderfill = GetComponent<Image>();
        StartCoroutine(StartFill());
    }

    // Update is called once per frame
    void Update()
    {
        if(!startfill && value != borderfill.fillAmount)
        borderfill.fillAmount = Mathf.Lerp(borderfill.fillAmount,value, Time.deltaTime);
    }

    IEnumerator StartFill()
    {

        yield return new WaitForSeconds(0.01f);

        while (borderfill.fillAmount != 1)
        {
            yield return new WaitForSeconds(0.00015f);
            borderfill.fillAmount += 0.005f;
        }

        startfill = false;
    }

    public void setvalue(float incomevalue)
    {
        value = value + incomevalue;
        if (value > 1)
            value = 1;
    }


}
