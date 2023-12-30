using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;

    Image icon;
    Text textLevel;

    void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1]; // GetComponentsInChildren 의 첫번째 Image 컴포넌트는 자기자신이므로 두번째 값을 가져옴.
        icon.sprite = data.itemIcon; // ItemData 에 설정해둔 itemIcon 값을 가져와서 스프라이트 변경

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0]; // icon 과 달리 GetComponentsInChildren 의 첫번째 Text 컴포넌트는 Text Level 이기에 첫번째 값을 가져옴.
    }

    void LateUpdate()
    {
        textLevel.text = "Lv." + (level + 1);
    }

    public void OnClick()
    {
        switch(data.itemType)
        {
            case ItemData.ItemType.Melee :
            case ItemData.ItemType.Range:
                // 레벨이 0 일 때 (게임 시작 시) 기본 오브젝트 생성
                if(level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    // AddComponent 함수 반환 값을 미리 선언한 변수에 저장
                    weapon = newWeapon.AddComponent<Weapon>(); // AddComponent<T> : 게임오브젝트에 T 컴포넌트를 추가하는 함수
                    weapon.Init(data);
                }
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];

                    weapon.LevelUp(nextDamage, nextCount);
                }
                break;
            case ItemData.ItemType.Glove:
                break;
            case ItemData.ItemType.Shoe:
                break;
            case ItemData.ItemType.Heal:
                break;
        }

        level++;

        // 현재 레벨이 최대 레벨과 같을 시
        if(level == data.damages.Length)
        {
            // 버튼 클릭 제한되게 설정 -> interactable 비활성화 
            GetComponent<Button>().interactable = false;
        }
    }
}
