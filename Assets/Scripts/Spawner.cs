using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;

    float timer;

    void Awake()
    {
        // �ڽ����� �����ص� ������Ʈ�� ������.
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if(timer > 0.2f)
        {
            timer = 0f;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy;
        enemy = GameManager.instance.pool.Get(Random.Range(0, GameManager.instance.pool.prefabs.Length));

        // GetComponentsInChildren �� �ڱ��ڽŵ� �����̿��� 0 �� �ƴ϶� 1 ���� ����
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
    }
}
