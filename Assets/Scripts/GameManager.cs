using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("# Player Info")] // �ν������� �Ӽ����� �̻ڰ� ���н����ִ� Ÿ��Ʋ
    public int playerId;
    public int level;
    public int kill;
    public int exp;
    public float health;
    public float maxHealth = 100;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 }; // �� ������ �ʿ� ����ġ�� ������ �迭 ���� ���� �� �ʱ�ȭ

    [Header("# Game Control")]
    public float gameTime; // ���ӽð�
    public float maxGameTime; // �ִ���ӽð�
    public bool isLive;

    [Header("# Game Object")]
    public Player player;
    public PoolManager pool;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public GameObject enemyCleaner;
    
    // �������� ����ϰڴٴ� Ű����. �ٷ� �޸𸮿� ������.
    // �ʱ�ȭ ���� �ٸ� �ڵ忡�� ��� ���� (�տ� �ڷ����� �ٿ���� ��)
    public static GameManager instance;

    void Awake()
    {
        if (instance == null)instance = this;
    }

    public void GameStart(int id)
    {
        playerId = id;
        health = maxHealth;

        player.gameObject.SetActive(true);
        uiLevelUp.Select(playerId % 2);
        Resume();

        AudioManager.instance.PlayerBgm(true);
        AudioManager.instance.PlayerSfx(AudioManager.Sfx.Select);
    }

    public void GameOver()
    {
        StartCoroutine("GameOverRoutine");
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();

        AudioManager.instance.PlayerBgm(false);
        AudioManager.instance.PlayerSfx(AudioManager.Sfx.Lose);
    }

    public void GameVictory()
    {
        StartCoroutine("GameVictoryRoutine");
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();

        AudioManager.instance.PlayerBgm(false);
        AudioManager.instance.PlayerSfx(AudioManager.Sfx.Win);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    void Update()
    {
        if (!isLive)
            return;

        gameTime += Time.deltaTime;

        if(gameTime >= maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();
        }
    }

    public void GetExp()
    {
        if (!isLive)
            return;

        exp++;

        if(exp == nextExp[Mathf.Min(level, nextExp.Length - 1)]) // Min �Լ��� ����Ͽ� �ְ� ����ġ�� �״�� ���
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
