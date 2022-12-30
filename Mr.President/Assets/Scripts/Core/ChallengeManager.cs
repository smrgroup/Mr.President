using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChallengeManager : MonoBehaviour
{

    public GameObject CardPrefab;
    public GameObject CenterNotifPrefab;
    public CenterNotificationController CenterNotifcontroller;
    public SpellData spelldata;


    private MinistersManager ministersManager;
    private GameObject ChallengeCard;
   
    private Challenges challenge;
    private Challenges Clonechallenge;

    private SpellController SpellController;


    private int challenge_Head = 0;

    private IEnumerator fillslider;
    private float actualval = 0;
    NewsCard newcard;
    Challengflow currentCard;
    State dragstate;

    //slider Challenge_1
    float animationTime = 0;
    float animationSpeed = 100f;
    float cureentvelo = 0;

    public Slider challenge_Slider;
    public Image challenge_SliderFill;

    // Start is called before the first frame update
    void Start()
    {
        ministersManager = FindObjectOfType<MinistersManager>();
        SpellController = FindObjectOfType<SpellController>();

        actualval = (int)challenge_Slider.value;
    }

    // Update is called once per frame
    void Update()
    {
        if (actualval != (int)challenge_Slider.value)
        {
            Setslidernewvalue();
            if (FindDifference(actualval, challenge_Slider.value) <= 0.1)
                challenge_Slider.value = actualval;
        }

        if (Clonechallenge != null)
        {
            if (challenge_Slider.value < Clonechallenge.PointTargetTowin)
            {
                challenge_SliderFill.color = new Color32(255, 0, 0, 255);
            }
            else
            {
                challenge_SliderFill.color = new Color32(255, 28, 0, 255);
            }
        };
    }


    public float FindDifference(float nr1, float nr2)
    {
        return Mathf.Abs(nr1 - nr2);
    }

    private void Setslidernewvalue()
    {
        animationTime = Time.deltaTime * animationSpeed;
        challenge_Slider.value = Mathf.SmoothDamp(challenge_Slider.value, actualval,ref cureentvelo, animationTime);

    }

    public void StartChallnage(Challenges challenge)
    {
        GameObject CenterNotif = Instantiate(CenterNotifPrefab, StaticData.gameManager.BackGroundPlay.transform);
        CenterNotifcontroller = CenterNotif.GetComponent<CenterNotificationController>();
        this.challenge = challenge;
        // make clone of CardsData for Save Main Data
        Clonechallenge = Instantiate(this.challenge);
        Debug.Log("Start Challenge " + challenge.name);
        challenge_Slider.value = challenge.SliderValue;
        actualval = challenge_Slider.value;
        StartCoroutine(ChallengeUI());
    }

    IEnumerator ChallengeUI(bool backtoflow = false)
    {
        if (!backtoflow)
        {

            CenterNotifcontroller.PlayIn("ﭼﺎﻟﺶ", new Color32(241, 184, 33, 255));
            yield return new WaitForSeconds(1f);
            StartCoroutine(ministersManager.loadministers(0));
            yield return new WaitForSeconds(0);
            challenge_Slider.GetComponent<EasyTween>().OpenCloseObjectAnimation();
            CreateChallengeCard();
        }
        else
        {
            StartCoroutine(ministersManager.loadministers(0));
            challenge_Slider.GetComponent<EasyTween>().OpenCloseObjectAnimation();
        }
    }

    public void CreateChallengeCard()
    {

        int maxCardCount = Clonechallenge.ChallengeFlow.FindAll(x => x.Available == true).Count;
        if (challenge_Head >= maxCardCount || challenge_Head == -1)
        {
            SetChallengeResult();
            Debug.Log("End Of Challnge");
            StartCoroutine(ChallengeUI(true));
            StaticData.gameManager.SetactiveChallenge(false);
            return;
        }
        {

            newcard = GetNextCard();

            currentCard = Clonechallenge.ChallengeFlow[challenge_Head];

            ChallengeCard = Instantiate(CardPrefab, StaticData.gameManager.CardsArea.transform);
            ChallengeCard.name = Clonechallenge.name;
            StaticData.gameManager.textarea.typemsg(newcard.Text);
            ChallengeCardDragController.OnDragedChallengeCard.AddListener(CardDragEvent);
            Text[] ChooseCardTexts = ChallengeCard.GetComponentsInChildren<Text>();
            ChooseCardTexts[0].text = newcard.LeftText;
            ChooseCardTexts[1].text = newcard.RightText;
            ChallengeCard.GetComponent<Image>().sprite = newcard.Image;

        }


    }

    private NewsCard GetNextCard()
    {

        if (!Clonechallenge.ChallengeFlow[challenge_Head].Available)
        {
            while (!Clonechallenge.ChallengeFlow[challenge_Head].Available)
                challenge_Head++;
        }

        Debug.Log("challenge_Head : " + challenge_Head + " CardID : " + Clonechallenge.ChallengeFlow[challenge_Head].CardID);

        string next_cardid = Clonechallenge.ChallengeFlow[challenge_Head].CardID;
        ChallengeCardType next_cardtype = Clonechallenge.ChallengeFlow[challenge_Head].CardType;

        if (next_cardtype == ChallengeCardType.Fast)
        {
                return Clonechallenge.ChallengeFlow[challenge_Head].FastCards;
        }
        else if (next_cardtype == ChallengeCardType.simple)
        {
            return Clonechallenge.ChallengeFlow[challenge_Head].SimpleCards;
        }
        return null;
    }

    private void CardDragEvent(State state)
    {

        dragstate = state;
        if (state == State.Left)
        {
            actualval = actualval + newcard.LeftValue;
        }
        else if (state == State.Right)
        {
            actualval = actualval + newcard.RightValue;
        }

        Debug.Log("Actual VALUE :  " + actualval);

        challenge_Head++;
        Cardheadmanager();
        CreateChallengeCard();
    }

    private void Cardheadmanager()
    {
        if (currentCard.CardType == ChallengeCardType.Fast)
        {
            if (dragstate == State.Left)
            {
                challenge_Head = Clonechallenge.ChallengeFlow.FindIndex(card => card.CardID == newcard.Left_Head_ID);
            }
            else if (dragstate == State.Right)
            {
                challenge_Head = Clonechallenge.ChallengeFlow.FindIndex(card => card.CardID == newcard.Right_Head_ID);
            }
        }
        else
        if (currentCard.CardType == ChallengeCardType.simple)
        {
            if (dragstate == State.Left)
            {
                Clonechallenge.ChallengeFlow.Find(card => card.CardID == newcard.Left_Head_ID).Available = true;
            }
            else if (dragstate == State.Right)
            {
                Clonechallenge.ChallengeFlow.Find(card => card.CardID == newcard.Right_Head_ID).Available = true;
            }
        }
    }

    private IEnumerator SetNewValue()
    {
        Debug.Log("income value : " + actualval);
        while (challenge_Slider.value != actualval)
        {
            yield return new WaitForSeconds(0.0015f);
            if (challenge_Slider.value < actualval)
            {
                challenge_Slider.value += 0.1f;
                if (challenge_Slider.value >= actualval)
                    challenge_Slider.value = actualval;
            }
            else
            {
                challenge_Slider.value -= 0.1f;
                if (challenge_Slider.value <= actualval)
                    challenge_Slider.value = actualval;
            }
        }

    }

    private void SetChallengeResult()
    {
        GameObject CenterNotif = Instantiate(CenterNotifPrefab, StaticData.gameManager.BackGroundPlay.transform.position, StaticData.gameManager.BackGroundPlay.transform.rotation, StaticData.gameManager.BackGroundPlay.transform);
        CenterNotifcontroller = CenterNotif.GetComponent<CenterNotificationController>();

        if (challenge_Slider.value >= Clonechallenge.PointTargetTowin)
        {
            Debug.Log("Challenge Win");
            CenterNotifcontroller.PlayIn("ﭘﯿﺮﻭﺯی", new Color32(241, 184, 33, 255));
            Spell spell = spelldata.Spells.Find(x => x.Id == Clonechallenge.Win_Spell_ID);
            SpellController.addspellItem(spell);
        }
        else
        {
            Debug.Log("Challenge Lose");
            CenterNotifcontroller.PlayIn("ﺑﺎﺧﺘﯽ", new Color32(241, 33, 47, 255));
            Spell spell = spelldata.Spells.Find(x => x.Id == Clonechallenge.Lose_Spell_ID);
            SpellController.addspellItem(spell);
            SpellController.SetspellActive(spell);
        }
    }
}
