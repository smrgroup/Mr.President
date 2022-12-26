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

    private SpellController SpellController;


    private int challenge_Head = 0;


    //slider Challenge_1
    public Slider challenge_Slider;
    // Start is called before the first frame update
    void Start()
    {
        ministersManager = FindObjectOfType<MinistersManager>();
        SpellController = FindObjectOfType<SpellController>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartChallnage(Challenges challenge)
    {
        GameObject CenterNotif = Instantiate(CenterNotifPrefab, StaticData.gameManager.BackGroundPlay.transform);
        CenterNotifcontroller = CenterNotif.GetComponent<CenterNotificationController>();
        this.challenge = challenge;
        Debug.Log("Start Challenge " + challenge.name);
        challenge_Slider.value = challenge.SliderValue;
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

        if (challenge_Head >= challenge.Cards.Count)
        {
            SetChallengeResult();
            Debug.Log("End Of Challnge");
            StartCoroutine(ChallengeUI(true));
            StaticData.gameManager.SetactiveChallenge(false);
            return;
        }
        {

            ChallengeCard = Instantiate(CardPrefab, StaticData.gameManager.CardsArea.transform);
            ChallengeCard.name = challenge.name;
            StaticData.gameManager.textarea.typemsg(challenge.Cards[challenge_Head].Text);
            ChallengeCardDragController.OnDragedChallengeCard.AddListener(CardDragEvent);
            Text[] ChooseCardTexts = ChallengeCard.GetComponentsInChildren<Text>();
            ChooseCardTexts[0].text = challenge.Cards[challenge_Head].LeftText;
            ChooseCardTexts[1].text = challenge.Cards[challenge_Head].RightText;
            ChallengeCard.GetComponent<Image>().sprite = challenge.Cards[challenge_Head].Image;

        }


    }

    private void CardDragEvent(State state)
    {
        if (state == State.Left)
        {
            StartCoroutine(SetNewValue(challenge.Cards[challenge_Head].LeftValue));
        }
        else if (state == State.Right)
        {
            StartCoroutine(SetNewValue(challenge.Cards[challenge_Head].RightValue));
        }
        challenge_Head++;
        CreateChallengeCard();
    }

    private IEnumerator SetNewValue(float newval)
    {
        Debug.Log("income value : " + newval);
        newval = (challenge_Slider.value + newval);
        while (challenge_Slider.value != newval)
        {
            yield return new WaitForSeconds(0.0015f);
            if (challenge_Slider.value < newval)
            {
                challenge_Slider.value += 0.1f;
                if (challenge_Slider.value >= newval)
                    challenge_Slider.value = newval;
            }
            else
            {
                challenge_Slider.value -= 0.1f;
                if (challenge_Slider.value <= newval)
                    challenge_Slider.value = newval;
            }
        }

    }

    private void SetChallengeResult()
    {
        GameObject CenterNotif = Instantiate(CenterNotifPrefab, StaticData.gameManager.BackGroundPlay.transform.position, StaticData.gameManager.BackGroundPlay.transform.rotation, StaticData.gameManager.BackGroundPlay.transform);
        CenterNotifcontroller = CenterNotif.GetComponent<CenterNotificationController>();

        if (challenge_Slider.value >= challenge.PointTargetTowin)
        {
            Debug.Log("Challenge Win");
            CenterNotifcontroller.PlayIn("ﭘﯿﺮﻭﺯی", new Color32(241, 184, 33, 255));
            Spell spell = spelldata.Spells.Find(x => x.Id == challenge.Win_Spell_ID);
            SpellController.addspellItem(spell);
        }
        else
        {
            Debug.Log("Challenge Lose");
            CenterNotifcontroller.PlayIn("ﺑﺎﺧﺘﯽ", new Color32(241, 33, 47, 255));
            Spell spell = spelldata.Spells.Find(x => x.Id == challenge.Lose_Spell_ID);
            SpellController.addspellItem(spell);
            SpellController.SetspellActive(spell);
        }
    }
}
