using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockCharactor;
    public GameObject[] unlockCharactor;
    public GameObject uiNotice;

    enum Achive { UnlockBean, UnlockPotato }
    Achive[] achives; // 업적 데이터들을 저장해둘 배열

    WaitForSecondsRealtime wait; // 최적화를 위해 변수로 사용

    void Awake()
    {
        // Enum.GetValues : 주어진 열거형의 데이터를 모두 가져오는 함수
        achives = (Achive[])Enum.GetValues(typeof(Achive));

        wait = new WaitForSecondsRealtime(5);

        // 데이터가 없을 경우에만 초기화
        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }

    void Start()
    {
        UnlockCharactor();
    }


    void Init()
    {
        // 데이터 초기화
        PlayerPrefs.SetInt("MyData", 1);

        // 업적 초기화
        foreach(Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0); // 0 -> false 라는 의미로 초기화
        }
    }

    void UnlockCharactor()
    {
        for(int index = 0; index < lockCharactor.Length; index++)
        {
            string achiveName = achives[index].ToString();

            // GetInt 함수로 저장된 업적 상태를 가져와서 버튼 활성화에 적용
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            lockCharactor[index].SetActive(!isUnlock);
            unlockCharactor[index].SetActive(isUnlock);
        }
    }

    void LateUpdate()
    {
        foreach(Achive achive in achives)
        {
            CheckAchive(achive);
        }
    }

    void CheckAchive(Achive achive)
    {
        bool isAchive = false;

        // 각 업적 별 해금 조건 설정
        switch(achive)
        {
            case Achive.UnlockBean:
                isAchive = GameManager.instance.kill >= 10;
                break;
            case Achive.UnlockPotato:
                isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;
        }

        // 업적 달성 및 achive 값이 0 일 경우 1로 전환
        if(isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);

            for(int index = 0; index < uiNotice.transform.childCount; index++)
            {
                // enum Achive 에서 값을 찾아 일치하는 자식 오브젝트 활성화
                if(index == (int)achive)
                {
                    uiNotice.transform.GetChild(index).gameObject.SetActive(true);
                } else
                {
                    uiNotice.transform.GetChild(index).gameObject.SetActive(false);
                }
            }

            StartCoroutine(NoticeRoutine());
        }
    }

    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);

        yield return wait;

        uiNotice.SetActive(false);
    }
}
