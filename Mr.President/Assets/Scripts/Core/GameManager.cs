using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    public List<CardData> chapters;

    public GameData savedgamedata = new GameData();

    public bool EndofGame = false;

    [Header("PreFabs")]
    public GameObject CardsArea;
    public GameObject BackGroundPlay;
    [Header("Animation")]
    public Animator Loading;
    public Animator EndGame;


    public GameObject BackCardPrefab;
    public GameObject CardPrefab;
    public GameObject EndCardPrefab;

    //current card data
    public CardData cardsdata;
    public CardData Clonecardsdata;

    private Chapter CarrentCard;
    public CardDetails carddetails;
    private State dragstate;

    private List<GameObject> backcardsPrefabs;
    private GameObject PlayableCard;
    private Card_Controller card_Controller;
    public TextManager textarea;

    public List<MinisterController> ministers = new List<MinisterController>();

    private static int CardHead = 0;
    private int NextCardID = 0;

    private SpellController spellcontroller;

    public List<Chapter> chapterflow = new List<Chapter>();

    public ChallengeManager challengeManager;
    public bool ActiveChallenge;

    // Start is called before the first frame update
    void Awake()
    {
        StaticData.gameManager = this;

        challengeManager = GetComponent<ChallengeManager>();

        //init CardArea
        backcardsPrefabs = new List<GameObject>();
        StartCoroutine(intialCardArea());

        // Get Ministers And TextArea
        ministers = FindObjectsOfType<MinisterController>().ToList();
        textarea = FindObjectOfType<TextManager>();

        cardsdata = chapters.Find(x => x.name == PlayerPrefs.GetString("Chapter"));

        // make clone of CardsData for Save Main Data
        Clonecardsdata = Instantiate(cardsdata);

        // Chapter Data
        chapterflow = Clonecardsdata.ChapterFlow;

      
        LoadSavedData();

        // Find SPellController
        spellcontroller = FindObjectOfType<SpellController>();

        // Set FPS On Android
        if (Application.platform == RuntimePlatform.Android)
            Application.targetFrameRate = 100;

    }

    private void Start()
    {
        MinisterController.OnEndOfMinister.AddListener(OnministerEnd);
    }

    // Update is called once per frame
    void Update()
    {

    }



    #region CardController
    IEnumerator intialCardArea()
    {

        yield return new WaitForSeconds(1f);

        Loading.SetBool("start", true);

        yield return new WaitForSeconds(1f);

        int CardCount = 5;

        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < CardCount; i++)
        {
            GameObject Bcard = Instantiate(BackCardPrefab, CardsArea.transform);
            backcardsPrefabs.Add(Bcard);
            yield return new WaitForSeconds(0.3f);
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
        // end Game by minister
        if (EndofGame == true)
            return;

        //int chapterCards = chapterflow.FindAll(x => x.Available == true).Count - 1;
        int chapterCards = chapterflow.Count - 1;
        if ((CardHead >= chapterCards || CardHead == -1))
        {
            ChapterEnd();
            return;
        }
        else
        {

            //get Card type
            carddetails = GetNextCard();


            // check for challenge
            ///if TRUE Cut Of From flow and move Player To Challenge
            if (chapterflow[CardHead]._Challenge)
            {
                this.SetactiveChallenge(true);
                challengeManager.StartChallnage(chapterflow[CardHead].Challenge);
            }

            if (carddetails == null) return;

            //get card by header
            CarrentCard = chapterflow[CardHead];

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

    public void SetactiveChallenge(bool active , string cardID = "xxx")
    {
        ActiveChallenge = active;

        // Challnge Done Move On FLow
        if (!active)
        {
            if (cardID == "xxx")
                CardHead++;
            else
                CardHead = chapterflow.FindIndex(card => card.CardID == cardID);

            CreateCard();
        }


    }

    private CardDetails GetNextCard()
    {

        if (!chapterflow[CardHead].Available)
        {
            while (!chapterflow[CardHead].Available)
                CardHead++;
        }

        Debug.Log("header : " + CardHead + " CardID : " + chapterflow[CardHead].CardID);

        string next_cardid = chapterflow[CardHead].CardID;
        CardType next_cardtype = chapterflow[CardHead].CardType;

        if (next_cardtype == CardType.Random)
        {
            return GetRandomCard();
        }
        else if (next_cardtype == CardType.Fast)
        {
            if (Clonecardsdata.ChapterFlow[CardHead]._Challenge)
                return null;
            else
                return Clonecardsdata.ChapterFlow[CardHead].FastCards;
        }
        else if (next_cardtype == CardType.simple)
        {
            return Clonecardsdata.ChapterFlow[CardHead].SimpleCards;
        }
        return null;
    }

    private void CardDragEvent(State state)
    {
        spellcontroller.Spelldecision();
        dragstate = state;
        AddMinistersEffect();
        Cardheadmanager();
        SaveGame();
        CreateCard();
    }

    private void Cardheadmanager()
    {
        if (CarrentCard.CardType == CardType.Fast)
        {
            if (dragstate == State.Left)
            {
                CardHead = chapterflow.FindIndex(card => card.CardID == carddetails.Left_Head_ID);
            }
            else if (dragstate == State.Right)
            {
                CardHead = chapterflow.FindIndex(card => card.CardID == carddetails.Right_Head_ID);
            }
        }
        else
        if (CarrentCard.CardType == CardType.simple)
        {
            if (dragstate == State.Left)
            {
                chapterflow.Find(card => card.CardID == carddetails.Left_Head_ID).Available = true;
            }
            else if (dragstate == State.Right)
            {
                chapterflow.Find(card => card.CardID == carddetails.Right_Head_ID).Available = true;
            }
        }
    }

    private void AddMinistersEffect()
    {

        for (int i = 0; i < carddetails.Ministers.Count; i++)
        {
            MinisterController minister = ministers.Find(x => x.id == carddetails.Ministers[i].id);
            if (dragstate == State.Left)
            {
                if (spellcontroller.IsActiveSpell)
                {
                    spellcontroller.addAffect(carddetails.ID, carddetails.Ministers.Count, minister, carddetails.Ministers[i].Left_value);
                }
                minister.setvalue(carddetails.Ministers[i].Left_value);
            }
            else
            {
                if (spellcontroller.IsActiveSpell)
                {
                    spellcontroller.addAffect(carddetails.ID, carddetails.Ministers.Count, minister, carddetails.Ministers[i].Right_value);
                }
                minister.setvalue(carddetails.Ministers[i].Right_value);
            }
        }

    }

    public CardDetails GetRandomCard()
    {
        int rnd = Random.Range(0, Clonecardsdata.RandomCards.Count);

        if (chapterflow[CardHead].RandomlistID == "")
        { 
            Debug.LogError("RandomlistID Is Empty");
            return null;
        }
        
        CardDetails choosed = Clonecardsdata.RandomCards
            .Find(R => R.RandomListID == chapterflow[CardHead].RandomlistID)
            .RandomCards[rnd];

        Clonecardsdata.RandomCards.
            Find(R => R.RandomListID == chapterflow[CardHead].RandomlistID)
            .RandomCards
            .Remove(choosed);

        return choosed;
    }
    #endregion

    #region EndGame
    MinisterController Targetminister;
    int TargetministerEndCount = 0;

    void OnministerEnd(string MinisterID)
    {
        Debug.Log("END OF THE GAME BY MINISTERID : " + MinisterID);
        Targetminister = ministers.Find(minister => minister.id == MinisterID);
        createMinisterEndsCard();
        EndofGame = true;
    }

    public void createMinisterEndsCard()
    {
        if (TargetministerEndCount != Targetminister.GameoverCards.Count)
        {
            //init card
            GameObject ENDcard = Instantiate(EndCardPrefab, CardsArea.transform);
            ENDcard.name = "ENDCARD-"+TargetministerEndCount;
            ENDcard.GetComponent<Image>().sprite = Targetminister.GameoverCards[TargetministerEndCount].Image;
            textarea.typemsg(Targetminister.GameoverCards[TargetministerEndCount].message);
            TargetministerEndCount++;         
        }
        else
            ChapterEnd();
    }

    void ChapterEnd()
    {
        EndGame.SetBool("play", true);
        EndofGame = true;
    }


    #endregion

    #region SaveData

    public void LoadSavedData()
    {

        if (PlayerPrefs.HasKey(StaticData.SAVE_KEY))
        {
           
            Debug.Log(PlayerPrefs.GetString(StaticData.SAVE_KEY));

            savedgamedata = JsonUtility.FromJson<GameData>(PlayerPrefs.GetString(StaticData.SAVE_KEY));

            if (cardsdata.name == savedgamedata.Chapter)
            {
                CardHead = savedgamedata.CardHead;
                chapterflow = savedgamedata.Lastchapterstate;
            }
 
        }
    }

    public void SaveGame()
    { 
        GameData gamedata = new GameData();
        gamedata.ministers = new List<MinistersSaveData>();

        gamedata.Chapter = cardsdata.name;
        gamedata.CardHead = CardHead;
        gamedata.CardHeadID = Clonecardsdata.ChapterFlow[CardHead].CardID;
        gamedata.Lastchapterstate = chapterflow;

        ministers = FindObjectsOfType<MinisterController>().ToList();

  
        foreach (var minister in ministers)
        {
            MinistersSaveData newministerdata = new MinistersSaveData(minister.id,minister.value);
            gamedata.ministers.Add(newministerdata);
        }

        var json = JsonUtility.ToJson(gamedata);
        PlayerPrefs.SetString(StaticData.SAVE_KEY,json);

        //GameData newgamedata = new GameData();
        //newgamedata = JsonUtility.FromJson<GameData>(PlayerPrefs.GetString(StaticData.SAVE_KEY));

    }

    [ContextMenu("Delete SaveData")]
    public void deleteSaveData()
    {
        PlayerPrefs.DeleteKey(StaticData.SAVE_KEY);
    }

    #endregion

}
