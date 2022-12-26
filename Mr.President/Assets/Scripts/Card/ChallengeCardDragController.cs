using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChallengeCardDragController : CardDragController
{

    public static UnityEvent<State> OnDragedChallengeCard;


    private void Awake()
    {
        OnDragedChallengeCard = new UnityEvent<State>();
    }
    // Start is called before the first frame update

    public override void Start()
    {
        base.Start();
    }

    public override void SignMinisters(bool state = true)
    {
    }

    public override void CardMovement()
    {
        base.CardMovement();
    }

    public override IEnumerator RLMoveAwait(float second, GameObject obj)
    {
        yield return new WaitForSeconds(second);
        Destroy(obj);
        Debug.Log("RLMoveAwait");
        OnDragedChallengeCard.Invoke(state);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
