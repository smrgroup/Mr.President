using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndGameCardDrageController : CardDragController
{

    private void Awake()
    {
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
        StaticData.gameManager.createMinisterEndsCard();
    }


    // Update is called once per frame
    void Update()
    {

    }
}
