using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.RuleTile.TilingRuleOutput;

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

    private bool IsDraging = false;

    private Vector3 currentAngle;



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

        Vector3 rotation = new Vector3(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z);
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

        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (Input.touchCount > 1 || IsDraging == false)
        {
            Quaternion currentRotation = transform.rotation;
            Quaternion wantedRotation = Quaternion.Euler(0, 0, 0);
            transform.rotation = Quaternion.RotateTowards(currentRotation, wantedRotation, Time.deltaTime * 80);
        }
        else
        { 

            rig.position = Vector3.Lerp(transform.position, (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _zDistanceToCamera)) + _offsetToMouse), Time.deltaTime * 2.0f);


            _XDistanceFromStart = _startPosition.x - transform.position.x;

            Debug.Log(_XDistanceFromStart);

            float max_Rotate = 0.25f;
            if (_XDistanceFromStart > max_Rotate)
                _XDistanceFromStart = max_Rotate;
            else if (_XDistanceFromStart < (max_Rotate * -1))
                _XDistanceFromStart =(max_Rotate * -1);
            else if (_XDistanceFromStart < (max_Rotate * -1))
                _XDistanceFromStart = (max_Rotate * -1);

            _XDistanceFromStart = (_XDistanceFromStart * 50) / 100;

            if (transform.position.x > _startPosition.x)
            {
                Debug.Log("RIGHT");
            }
            else
            {
                Debug.Log("LEFT");
            }

            Quaternion targetrotation = new Quaternion(transform.rotation.x, transform.rotation.y, _XDistanceFromStart, transform.rotation.w);
            transform.rotation = Quaternion.RotateTowards(transform.rotation,targetrotation, Time.deltaTime * 150);

        }


    }
}
