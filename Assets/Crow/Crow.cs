using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Crow : MonoBehaviour
{
    public float mass = 1;
    public float gatherDistance = 6f;
    public float seperateDistance = 3f;
    public float alignDistance = 6f;
    public Transform targetPos;
    public float targetWeight = 0.2f;
    public Vector3 targetForce;
    public Vector3 velocity;
    public float speed = 3f;
    public float rotSpeed = 5f;

    public Vector3 sumForce;
    public float gatherWeight = 1f;
    public Vector3 gatherForce;
    public float seperateWeight = 1f;
    public Vector3 seperateForce;
    public float alignWeight = 1f;
    public Vector3 alignForce;
    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        animator.enabled = false;
        targetPos = GameObject.Find("Target").transform;
        Invoke("StartAnimation", Random.Range(0.2f, 1.5f));
        InvokeRepeating("CalculateGatherAndAlignForce", 0, 0.1f);
        InvokeRepeating("CalculateSeperateForce", 0, 0.1f);
        InvokeRepeating("CalculateTargetForce", 0, 0.1f);
    }

    private void StartAnimation()
    {
        animator.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        sumForce = gatherForce + seperateForce + alignForce + targetForce;
        Vector3 acc = sumForce / mass;
        velocity += acc * Time.deltaTime;
        transform.position += velocity * Time.deltaTime * speed;
        Quaternion rot = Quaternion.LookRotation(velocity);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotSpeed * Time.deltaTime);
    }
    
    private void CalculateTargetForce()
    {
        Vector3 targetDir = targetPos.position - transform.position;
        targetForce = targetDir.normalized * targetWeight - velocity -  targetDir.normalized / (targetDir.magnitude + targetWeight);
    }

    private void CalculateGatherAndAlignForce()
    {
        List<Collider> colliders = Physics.OverlapSphere(transform.position, gatherDistance).ToList();
        gatherForce = Vector3.zero;
        Vector3 gatherPos = Vector3.zero;
        colliders.Remove(GetComponent<Collider>());
        if (colliders.Count > 0)
        {
            foreach (var collider in colliders)
            {
                gatherPos += collider.transform.position;
            }
            gatherPos /= colliders.Count;
            gatherForce = (gatherPos - transform.position) * gatherWeight;
        }

        alignForce = Vector3.zero;
        Vector3 alignDir = Vector3.zero;
        if (!Mathf.Approximately(gatherDistance, alignDistance))
        {
            colliders = Physics.OverlapSphere(transform.position, alignDistance).ToList();
        }
        if(colliders.Count > 0)
        {
            foreach (var collider in colliders)
            {
                alignDir += collider.transform.forward;
            }
            alignDir = alignDir.normalized;
            alignForce = (alignDir - transform.forward) * alignWeight;
        }
    }

    private void CalculateSeperateForce()
    {
        List<Collider> colliders = Physics.OverlapSphere(transform.position, seperateDistance).ToList();
        seperateForce = Vector3.zero;
        colliders.Remove(GetComponent<Collider>());
        foreach (var collider in colliders)
        {
            Vector3 dir = transform.position - collider.transform.position;
            seperateForce += dir.normalized / (dir.magnitude + 0.2f) * seperateWeight;
        }
    }
}
