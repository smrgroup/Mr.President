using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDragController : MonoBehaviour
{

 

    Vector3 _startPosition;
    Vector3 _startPositionLocal;


    Vector3 _offsetToMouse;
    float _zDistanceToCamera;
    float _XDistanceFromStart;

    private Rigidbody2D rig;

    public EasyTween Tweens;


    public AnimationCurve EnterAnim;
    public AnimationCurve ExitAnim;

    public GameObject LeftText;
    public GameObject RightText;
    public EasyTween LeftTween;
    public EasyTween RightTween;

    private bool IsDraging = false;

    private Vector3 currentAngle;

    protected State state;


    void OnMouseDown()
    {
        _zDistanceToCamera = Mathf.Abs(_startPosition.z - Camera.main.transform.position.z);
        _offsetToMouse = _startPosition - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _zDistanceToCamera));
    }

    void OnMouseDrag()
    {
        IsDraging = true;
    }

    public void OnMouseUp()
    {
        _offsetToMouse = Vector3.zero;

        Tweens.ChangeSetState(false);
        Tweens.SetAnimationPosition(transform.localPosition, _startPositionLocal, EnterAnim, ExitAnim);
        Tweens.OpenCloseObjectAnimation();

        IsDraging = false;



    }

    // Start is called before the first frame update
    void Start()
    {
        Tweens = GetComponent<EasyTween>();
        _startPositionLocal = transform.localPosition;
        _startPosition = transform.position;

        LeftTween = LeftText.GetComponent<EasyTween>();
        RightTween = RightText.GetComponent<EasyTween>();

        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        CardMovement();
    }


    public void CardMovement()
    {
        if (Input.touchCount > 1 || IsDraging == false)
        {
            Quaternion currentRotation = transform.rotation;
            Quaternion wantedRotation = Quaternion.Euler(0, 0, 0);
            transform.rotation = Quaternion.RotateTowards(currentRotation, wantedRotation, Time.deltaTime * 100);
            HideCardTexts();
        }
        else
        {

            rig.position = Vector3.Lerp(transform.position, (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _zDistanceToCamera)) + _offsetToMouse), Time.deltaTime * 10.0f);


            _XDistanceFromStart = _startPosition.x - transform.position.x;



            _XDistanceFromStart = (_XDistanceFromStart * 10) / 100;


            if (transform.position.x >= 0.2f && state == State.Middle)
            {
                state = State.Right;
                RightTween.OpenCloseObjectAnimation();
            }
            else if (transform.position.x <= -0.2f && state == State.Middle)
            {
                state = State.Left;
                LeftTween.OpenCloseObjectAnimation();
            }
            else if (transform.position.x <= 0.2f && transform.position.x >= -0.2f)
            {
                state = State.Middle;
                HideCardTexts();
            }

            Debug.Log(state + " " + transform.position.x);

            Quaternion targetrotation = new Quaternion(transform.rotation.x, transform.rotation.y, _XDistanceFromStart, transform.rotation.w);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetrotation, Time.deltaTime * 150);

        }
    }


    public void HideCardTexts()
    {
        if (LeftTween.IsObjectOpened()) LeftTween.OpenCloseObjectAnimation();
        if (RightTween.IsObjectOpened()) RightTween.OpenCloseObjectAnimation();
        LeftTween.ChangeSetState(false);
        RightTween.ChangeSetState(false);
    }

}



public enum State
{
    Middle,
    Left,
    Right
}
