using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CardDragController : MonoBehaviour , IBeginDragHandler, IDragHandler, IEndDragHandler
{

 

    Vector3 _startPosition;
    Vector3 _offsetToMouse;
    float _zDistanceToCamera;
    float _XDistanceFromStart;


    public void OnBeginDrag(PointerEventData eventData)
    {
        _startPosition = transform.position;
        _zDistanceToCamera = Mathf.Abs(_startPosition.z - Camera.main.transform.position.z);

        _offsetToMouse = _startPosition - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _zDistanceToCamera));
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Input.touchCount > 1)
            return;

        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _zDistanceToCamera)) + _offsetToMouse;

        _XDistanceFromStart = _startPosition.x - transform.position.x;

        Debug.Log(_XDistanceFromStart);

        if (_XDistanceFromStart > 0.05f)
            _XDistanceFromStart = 0.05f;
        else if (_XDistanceFromStart < -0.05f)
            _XDistanceFromStart = -0.05f;

        if (transform.position.x > _startPosition.x)
        {
            Debug.Log("RIGHT");
        }
        else
        {
            Debug.Log("LEFT");
        }

        transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, _XDistanceFromStart, transform.rotation.w);


    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _offsetToMouse = Vector3.zero;
        transform.position = _startPosition;
        transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y,0, transform.rotation.w);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
