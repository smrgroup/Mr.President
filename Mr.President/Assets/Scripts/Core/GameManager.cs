using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    //current card data
    public CardData cardsdata;
    private Chapter CarrentCard;
    private CardDetails carddetails;
    private State dragstate;

    private List<GameObject> backcardsPrefabs;
    private GameObject PlayableCard;
    private Card_Controller card_Controller;
    private TextManager textarea;

    private static int CardHead = 0;
    private int NextCardID = 0;



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

        Debug.Log("header " + CardHead);


        if (CardHead >= cardsdata.ChapterFlow.Count)
            return;
        else
        {
            //get card by header
            CarrentCard = cardsdata.ChapterFlow[CardHead];

            //get Card type
            carddetails = GetNextCard();

            //init card
            PlayableCard = Instantiate(CardPrefab, CardsArea.transform);

            //fill data
            Text[] ChooseCardTexts = PlayableCard.GetComponentsInChildren<Text>();
            ChooseCardTexts[0].text = carddetails.Left_Choose_Text;
            ChooseCardTexts[1].text = carddetails.Right_Choose_Text;
            PlayableCard.name = carddetails.Name;
            PlayableCard.GetComponent<Image>().sprite = carddetails.Image;
            PlayableCard.GetComponent<CardDragController>().card_details = carddetails;
            textarea.typemsg(carddetails.Text);
            CardDragController.OnDragedCard.AddListener(CardDragEvent);
            CardHead++;

        }
    }

    private CardDetails GetNextCard()
    {

        int next_cardid = cardsdata.ChapterFlow[CardHead].CardID;
        CardType next_cardtype = cardsdata.ChapterFlow[CardHead].CardType;

        if (next_cardtype == CardType.Random)
        {
            return GetRandomCard();
        }
        else if (next_cardtype == CardType.Fast)
        {
            return cardsdata.FastCards[next_cardid];
        }
        else if (next_cardtype == CardType.simple)
        {
            return cardsdata.SimpleCards[next_cardid];
        }
        return null;
    }

    private void CardDragEvent(State state)
    {
        dragstate = state;
        Cardheadmanager();
        CreateCard();
    }

    private void Cardheadmanager()
    {
        if (CarrentCard.CardType == CardType.Fast)
        {
            if (dragstate == State.Left)
            {
                CardHead = carddetails.Left_Head_ID;
            }
            else if (dragstate == State.Right)
            { 
                CardHead = carddetails.Right_Head_ID;
            }
        Debug.Log("FastCard Move ON heade " + CardHead);
        }
    }

    public CardDetails GetRandomCard()
    {
       int rnd = Random.Range(0, cardsdata.RandomCards.Count);
        return cardsdata.RandomCards[rnd];
    } 

}
