using System.Collections;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MinisterController : MonoBehaviour
{
    //[ReadOnlyAttribute]
    public string id;
    public float value = 0.5f;
    Image borderfill;

    bool startfill = true;
    bool setnewvalue = false;


    // Start is called before the first frame update
    void Start()
    {
        borderfill = GetComponent<Image>();
        StartCoroutine(StartFill());
    }

    // Update is called once per frame
    void Update()
    {
        if (setnewvalue && value != borderfill.fillAmount)
        {
            borderfill.fillAmount = Mathf.Lerp(borderfill.fillAmount, value, Time.deltaTime);
            if (value == borderfill.fillAmount)
                setnewvalue = false;
        }
    }

    IEnumerator StartFill()
    {

        yield return new WaitForSeconds(0.01f);

        while (borderfill.fillAmount < 0.5f)
        {
            Debug.Log(value);
            yield return new WaitForSeconds(0.0015f);
            borderfill.fillAmount += 0.005f;
            if (borderfill.fillAmount > value)
                borderfill.fillAmount = value;
        }

        startfill = false;
    }

    public void effectvalue()
    {

    }

    public void setvalue(float incomevalue)
    {
        value = value + incomevalue;
        setnewvalue = true;
        if (value > 1)
            value = 1;
    }


}
