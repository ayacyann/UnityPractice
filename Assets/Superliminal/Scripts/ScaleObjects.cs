using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;
using DG.Tweening;

public class ScaleObjects : MonoBehaviour
{
    public GameObject hitObject;
    private Transform hitObjectOriParent;
    private float interval = 0.05f;
    private Vector3 targetPosition;
    private Tween tween;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInteractable();
        // CheckHitPoint();
    }

    private void CheckHitPoint()
    {
        if(hitObject!=null)
        {
            Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
            Physics.Raycast(Camera.main.ScreenPointToRay(center), out RaycastHit hit, 100);
            if(hit.collider!=null)
            {
                Debug.Log(hit.collider.gameObject.name);
                Vector3 offset = hit.point - hitObject.transform.position;
                int times = (int)(offset.magnitude / interval);
                Vector3 dir = offset.normalized;
                Vector3 c = hitObject.transform.position;
                targetPosition = c;
                for (int i = 0; i < times; i++)
                {
                    c += dir * interval;
                    Collider[] colliders = Physics.OverlapBox(c, hitObject.GetComponent<Renderer>().bounds.size * 0.5f);
                    if (colliders.Count() > 1)
                    {
                        break;
                    }
                    else
                    {
                        targetPosition = c;
                    }
                }
                float distance = (hitObject.transform.position - Camera.main.transform.position).magnitude;
                hitObject.transform.localScale = hitObject.transform.localScale *
                                                 ((targetPosition - Camera.main.transform.position).magnitude / distance);
                hitObject.transform.position = targetPosition;
            }
        }
    }

    private void CheckInteractable()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (hitObject == null)
            {
                Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
                Physics.Raycast(Camera.main.ScreenPointToRay(center), out RaycastHit hit, 100);
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.CompareTag("Interactable"))
                    {
                        hitObject = hit.collider.gameObject;
                        hitObject.GetComponent<Rigidbody>().useGravity = false;
                        Rigidbody rb = hitObject.GetComponent<Rigidbody>();
                        rb.constraints = RigidbodyConstraints.FreezeAll;
                        hitObjectOriParent = hitObject.transform.parent;
                        hitObject.transform.parent = Camera.main.transform;
                        hitObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                        //将物体转移到视线中心
                        Vector3 view = hit.point - Camera.main.transform.position;
                        Vector3 obj = hitObject.transform.position - Camera.main.transform.position;
                        float cos = Vector3.Dot(view.normalized, obj.normalized);
                        Vector3 offset = obj.magnitude * cos * view.normalized;
                        targetPosition = offset + Camera.main.transform.position;
                        tween = hitObject.transform.DOMove(targetPosition, 0.2f).OnComplete(()=>tween=null);
                    }
                }
            }
            else
            {
                hitObject.transform.parent = hitObjectOriParent;
                hitObject.GetComponent<Rigidbody>().useGravity = true;
                hitObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                hitObject.layer = LayerMask.NameToLayer("Default");
                hitObject = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(hitObject!=null)
        {
            Gizmos.color = Color.red;
            Bounds bounds = hitObject.GetComponent<Renderer>().bounds;
            // Debug.Log($"center:{bounds.center},size:{bounds.size}");
            Gizmos.DrawCube(targetPosition, bounds.size);
        }
    }
}
