using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDragController : MonoBehaviour
{

    [Tooltip("Card Target Positions")]
    protected Vector3 _startPosition;
    public Vector3 _RightPosition;
    public Vector3 _LeftPosition;

    protected Vector3 _startPositionLocal;

    Vector3 _offsetToMouse;
    float _zDistanceToCamera;
    float _XDistanceFromStart;
    float ChoosenArea = 0.5f;

    private bool IsDraging = false;
    private Vector3 currentAngle;
    protected State state;

    protected Rigidbody2D rig;
    public EasyTween Tweens;

    [Tooltip("Card Movement Curve")]
    public AnimationCurve EnterAnim;
    public AnimationCurve ExitAnim;

    [Tooltip("Card Help Text")]
    public GameObject LeftText;
    public GameObject RightText;
    protected EasyTween LeftTween;
    protected EasyTween RightTween;


    [Tooltip("Card Flip")]
    public GameObject CardBack;
    private bool CardBackActive = true;

    public static UnityEvent<State> OnDragedCard;

    public CardDetails card_details;

    void OnMouseDown()
    {
        _zDistanceToCamera = Mathf.Abs(_startPosition.z - Camera.main.transform.position.z);
        _offsetToMouse = _startPosition - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _zDistanceToCamera));
    }

    void OnMouseDrag()
    {

        if (CardBackActive)
            return;

        IsDraging = true;
    }

    public void OnMouseUp()
    {
        _offsetToMouse = Vector3.zero;
        IsDraging = false;

        if (state == State.Middle)
            OnMIddle();
        else if (state == State.Left)
            OnLeft();
        else if (state == State.Right)
            OnRight();

    }

    public virtual void Awake()
    {
        OnDragedCard = new UnityEvent<State>();
    }

    // Start is called before the first frame update
    public virtual void Start()
    {

        Tweens = GetComponent<EasyTween>();
        _startPositionLocal = transform.localPosition;
        _startPosition = transform.position;

        LeftTween = LeftText.GetComponent<EasyTween>();
        RightTween = RightText.GetComponent<EasyTween>();

        rig = GetComponent<Rigidbody2D>();



        FlipCard();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        CardMovement();
    }


    public virtual void CardMovement()
    {
        if (Input.touchCount > 1 || IsDraging == false)
        {
            if (!CardBackActive && state == State.Middle)
            {
                Quaternion currentRotation = transform.rotation;
                Quaternion wantedRotation = Quaternion.Euler(0, 0, 0);
                transform.rotation = Quaternion.RotateTowards(currentRotation, wantedRotation, Time.deltaTime * 150);
            }

            HideCardTexts();
            SignMinisters(false);
        }
        else
        {

            _XDistanceFromStart = _startPosition.x - transform.position.x;
            _XDistanceFromStart = (_XDistanceFromStart * 10) / 100;

            if (transform.position.x >= ChoosenArea && state == State.Middle)
            {
                state = State.Right;
                SignMinisters(true,state);
                RightTween.OpenCloseObjectAnimation();
            }
            else if (transform.position.x <= -ChoosenArea && state == State.Middle)
            {
                state = State.Left;
                SignMinisters(true , state);
                LeftTween.OpenCloseObjectAnimation();
            }
            else if (transform.position.x <= ChoosenArea && transform.position.x >= -ChoosenArea)
            {
                state = State.Middle;
                SignMinisters(false);
                HideCardTexts();
            }

            rig.position = Vector3.Lerp(transform.position, (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _zDistanceToCamera)) + _offsetToMouse), Time.deltaTime * 30.0f);
            Quaternion targetrotation = new Quaternion(transform.rotation.x, transform.rotation.y, _XDistanceFromStart, transform.rotation.w);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetrotation, Time.deltaTime * 150);

        }
    }

    public virtual void HideCardTexts()
    {
        if (LeftTween.IsObjectOpened()) LeftTween.OpenCloseObjectAnimation();
        if (RightTween.IsObjectOpened()) RightTween.OpenCloseObjectAnimation();
        LeftTween.ChangeSetState(false);
        RightTween.ChangeSetState(false);
    }

    public void FlipCard()
    {
        StartCoroutine(CalculateFlip());
    }

    IEnumerator CalculateFlip()
    {
        for (int i = -180; i <= 0; i+=10)
        {
            yield return new WaitForSeconds(0);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, i , transform.eulerAngles.z);

            if (i == -90)
            {
                CardBack.SetActive(false);
            }
            if (i >= 0)
            {
                CardBackActive = false;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
            }
        }



    }

    void OnMIddle() 
    {
        Tweens.ChangeSetState(false);
        Tweens.SetAnimationPosition(transform.localPosition, _startPositionLocal, EnterAnim, ExitAnim);
        Tweens.OpenCloseObjectAnimation();
    }

    void OnLeft()
    {
        Tweens.ChangeSetState(false);
        Tweens.SetAnimationPosition(transform.localPosition, _LeftPosition, EnterAnim, ExitAnim);
        Tweens.OpenCloseObjectAnimation();
        StartCoroutine(RLMoveAwait(0.2f, gameObject));
    }

    void OnRight()
    {
        Tweens.ChangeSetState(false);
        Tweens.SetAnimationPosition(transform.localPosition, _RightPosition, EnterAnim, ExitAnim);
        Tweens.OpenCloseObjectAnimation();
        StartCoroutine(RLMoveAwait(0.2f, gameObject));
    }

    public virtual void SignMinisters(bool sign = true , State state = State.Middle)
    {
        if (sign)
        {
            foreach (var targetminister in card_details.Ministers)
            {
                foreach (MinisterController minister in StaticData.gameManager.ministers)
                {
                    if (minister.id == targetminister.id)
                    {

                        if (state == State.Left)
                        {
                            if (targetminister.Left_value > 0)
                                minister.transform.GetChild(0).GetComponent<Image>().color = Color.green;
                            else
                                minister.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                        }
                        else if (state == State.Right)
                        {
                            if (targetminister.Right_value > 0)
                                minister.transform.GetChild(0).GetComponent<Image>().color = Color.green;
                            else
                                minister.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                        }
                    }
                }
            }
        }
        else
        {
            foreach (MinisterController minister in StaticData.gameManager.ministers)
            {
                    minister.transform.GetChild(0).GetComponent<Image>().color = Color.white;
            }
        }


       
    }

    public virtual IEnumerator RLMoveAwait(float second , GameObject obj)
    {
        yield return new WaitForSeconds(second);
        Destroy(obj);
        OnDragedCard.Invoke(state);
    }

}

public enum State
{
    Middle,
    Left,
    Right
}
