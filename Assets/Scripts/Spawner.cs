using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public float levelTime; // ��ȯ ���� ������ �����ϴ� ����

    float timer;
    int level;

    void Awake()
    {
        // �ڽ����� �����ص� ������Ʈ�� ������.
        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.instance.maxGameTime / spawnData.Length; // �ִ� �ð��� ���� ������ ũ��� ������ �ڵ����� ���� �ð� ���
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length - 1); // �ð��� ���� ���� ����
        // Mathf.Min(a, b) : a �� b �� �� ���� ���� ��ȯ 
        // FloorToInt : �Ҽ��� �Ʒ��� ������ Int ������ �ٲ�
        // CeilToInt : �Ҽ��� �Ʒ��� �ø��� Int ������ �ٲ�

        // ���� �ð� ����
        if (timer > (spawnData[level].spawnTime))
        {
            timer = 0f;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy;
        enemy = GameManager.instance.pool.Get(0);

        // GetComponentsInChildren �� �ڱ��ڽŵ� �����̿��� 0 �� �ƴ϶� 1 ���� ����
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;

        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

// ��ȯ �����͸� ����ϴ� Ŭ���� ����
[System.Serializable] // ����ȭ (Serialization) : ��ü�� ���� Ȥ�� �����ϱ� ���� ��ȯ -> �ν�����â���� ����
public class SpawnData
{
    public float spawnTime; // ��ȯ �ð�
    public int spriteType; // ���� Ÿ��
    public int health; // ü��
    public float speed; // �ӵ�
}