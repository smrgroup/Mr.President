using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    [Tooltip("PreFabs")]
    public GameObject BackCards;
    public GameObject BackGroundPlay;

    public GameObject BackCardPrefab;
    public GameObject CardPrefab;


    private List<GameObject> backcardsPrefabs;
    private GameObject PlayableCard;

    // Start is called before the first frame update
    void Start()
    {
        backcardsPrefabs = new List<GameObject>();
        StartCoroutine(intialCardArea());
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator intialCardArea()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.3f);
            GameObject Bcard = Instantiate(BackCardPrefab,BackCards.transform);
            Bcard.GetComponent<EasyTween>().OpenCloseObjectAnimation();
            backcardsPrefabs.Add(Bcard);
        }

        yield return new WaitForSeconds(0.3f);

        PlayableCard = Instantiate(CardPrefab, BackGroundPlay.transform);

    }

}
