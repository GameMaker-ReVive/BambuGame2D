using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        // 월드 좌표와 스크린 좌표가 다르기에 Camera.main.WorldToScreenPoint 를 사용하여 메인 카메라를 가져온 후 좌표를 맞춰 줌.
        // WorldToScreenPoint : 월드 상의 오브젝트 위치를 스크린 좌표로 변환
        rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
    }
}
