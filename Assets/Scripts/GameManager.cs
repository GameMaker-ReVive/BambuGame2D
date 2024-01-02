using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
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
    public bool isLive;

    [Header("# Game Object")]
    public Player player;
    public PoolManager pool;
    public LevelUp uiLevelUp;
    
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

        // 임시 스크립트 (첫번째 캐릭터 선택)
        uiLevelUp.Select(0);
    }

    void Update()
    {
        if (!isLive)
            return;

        gameTime += Time.deltaTime;

        if(gameTime >= maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }

    public void GetExp()
    {
        exp++;

        if(exp == nextExp[Mathf.Min(level, nextExp.Length - 1)]) // Min 함수를 사용하여 최고 경험치를 그대로 사용
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }
}
