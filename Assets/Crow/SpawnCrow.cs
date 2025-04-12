using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnCrow : MonoBehaviour
{
    public GameObject crowPrefab;
    public int crowCount = 10;
    public float spawnRadius = 6f;

    private void Awake()
    {
        for (int i = 0; i < crowCount; i++)
        {
            Vector3 spawnPos = transform.position + Random.insideUnitSphere * spawnRadius;
            Collider[] colliders = Physics.OverlapSphere(spawnPos, 1f);
            while (colliders.Length > 0)
            {
                spawnPos = transform.position + Random.insideUnitSphere * spawnRadius;
                colliders = Physics.OverlapSphere(spawnPos, 1f);
            }
            GameObject go = Instantiate(crowPrefab, spawnPos, Quaternion.identity,this.transform);
        }
    }
}
