using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnDragMove : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    private Transform parent;
    private bool isClickDown;

    private void Start()
    {
        parent = transform.parent;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isClickDown = true;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (isClickDown)
        {
            parent.transform.Translate(eventData.delta);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClickDown = false;
    }

    private void Update()
    {
        if(Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width || Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
        {
            isClickDown = false;
        }
    }
}
