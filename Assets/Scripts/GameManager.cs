using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("# Player Info")] // 인스펙터의 속성들을 이쁘게 구분시켜주는 타이틀
    public int level;
    public int kill;
    public int exp;
    public int health;
    public int maxHealth = 100;
    public int[] nextExp = { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 }; // 각 레벨의 필요 경험치를 보관할 배열 변수 선언 및 초기화

    [Header("# Game Control")]
    public float gameTime; // 게임시간
    public float maxGameTime = 2 * 10f; // 최대게임시간

    [Header("# Game Object")]
    public Player player;
    public PoolManager pool;
    
    // 정적으로 사용하겠다는 키워드. 바로 메모리에 얹어버림.
    // 초기화 없이 다른 코드에서 사용 가능 (앞에 자료형은 붙여줘야 함)
    public static GameManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        Debug.Log(instance);
        gameTime += Time.deltaTime;

        if(gameTime >= maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }

    public void GetExp()
    {
        exp++;

        if(exp == nextExp[level])
        {
            level++;
            exp = 0;
        }
    }
}
