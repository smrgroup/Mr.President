using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MinisterController : MonoBehaviour
{
    //[ReadOnlyAttribute]
    public string id;
    public float value = 0.5f;
    public Image borderfill;

    bool startfill = true;
    bool setnewvalue = false;
    float time = 0;

    public static UnityEvent<string> OnEndOfMinister;

    public List<Minister_GameOverCard> GameoverCards;

    // Start is called before the first frame update

    private void Awake()
    {
        OnEndOfMinister = new UnityEvent<string>();
    }

    void Start()
    {
        StartCoroutine(StartFill());
    }

    // Update is called once per frame
    void Update()
    {
        if (setnewvalue && value != borderfill.fillAmount)
        {
            time += Time.deltaTime;
            borderfill.fillAmount = Mathf.Lerp(borderfill.fillAmount, value, time);
            if (value == borderfill.fillAmount)
            { 
                setnewvalue = false;
                time = 0;
            }

        }
    }

    IEnumerator StartFill()
    {

        yield return new WaitForSeconds(2f);

        while (borderfill.fillAmount < 0.5f)
        {
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
        else if (value < 0)
            value = 0;

        if (value == 0 || value == 1)
            if (!StaticData.gameManager.ENDofGame)
                OnEndOfMinister.Invoke(this.id);
    }


}
