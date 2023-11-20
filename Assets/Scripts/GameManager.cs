using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    
    // 정적으로 사용하겠다는 키워드. 바로 메모리에 얹어버림.
    // 초기화 없이 다른 코드에서 사용 가능 (앞에 자료형은 붙여줘야 함)
    public static GameManager instance;

    void Awake()
    {
        instance = this;
    }
}
