using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    [Tooltip("PreFabs")]
    public GameObject CardsArea;
    public GameObject BackGroundPlay;

    public GameObject BackCardPrefab;
    public GameObject CardPrefab;

    public CardData cardsdata;

    private List<GameObject> backcardsPrefabs;
    private GameObject PlayableCard;
    private TextManager textarea;
   



    // Start is called before the first frame update
    void Start()
    {
        backcardsPrefabs = new List<GameObject>();
        StartCoroutine(intialCardArea());

        StaticData.gameManager = this;

        textarea = FindObjectOfType<TextManager>();

        if (Application.platform == RuntimePlatform.Android)
            Application.targetFrameRate = 100;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator intialCardArea()
    {

        int CardCount = 5;

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < CardCount; i++)
        {
            GameObject Bcard = Instantiate(BackCardPrefab,CardsArea.transform);
            backcardsPrefabs.Add(Bcard);
            yield return new WaitForSeconds(0.4f);
            backcardsPrefabs[i].GetComponent<EasyTween>().OpenCloseObjectAnimation();
        }

        yield return new WaitForSeconds(0.5f);

        CreateCard();

        //destroy unneccecery Backs
        for (int j = 0; j < backcardsPrefabs.Count - 1; j++)
        {
            Destroy(backcardsPrefabs[j]);
        }

    }

    public void CreateCard()
    {
        PlayableCard = Instantiate(CardPrefab,CardsArea.transform);
        CardDetails carddetails = GetRandomCard();
        PlayableCard.name = carddetails.ID+"-"+carddetails.Name;
        PlayableCard.GetComponent<Image>().sprite = carddetails.Image;
        textarea.typemsg(carddetails.Text);
    }

    public CardDetails GetRandomCard()
    {
       int rnd = Random.Range(0, cardsdata.Cards.Count);
        return cardsdata.Cards[rnd];
    } 

}
