using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public float levelTime; // 소환 레벨 구간을 결정하는 변수

    float timer;
    int level;

    void Awake()
    {
        // 자식으로 설정해둔 오브젝트들 가져옴.
        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.instance.maxGameTime / spawnData.Length; // 최대 시간에 몬스터 데이터 크기로 나누어 자동으로 구간 시간 계산
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length - 1); // 시간에 따라 레벨 증가
        // Mathf.Min(a, b) : a 와 b 중 더 작은 값을 반환 
        // FloorToInt : 소수점 아래는 버리고 Int 형으로 바꿈
        // CeilToInt : 소수점 아래를 올리고 Int 형으로 바꿈

        // 스폰 시간 설정
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

        // GetComponentsInChildren 는 자기자신도 포함이여서 0 이 아니라 1 부터 시작
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;

        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

// 소환 데이터를 담당하는 클래스 선언
[System.Serializable] // 직렬화 (Serialization) : 개체를 저장 혹은 전송하기 위해 변환 -> 인스펙터창에서 보임
public class SpawnData
{
    public float spawnTime; // 소환 시간
    public int spriteType; // 몬스터 타입
    public int health; // 체력
    public float speed; // 속도
}