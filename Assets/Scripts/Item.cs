using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Gear gear;

    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;

    void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1]; // GetComponentsInChildren 의 첫번째 Image 컴포넌트는 자기자신이므로 두번째 값을 가져옴.
        icon.sprite = data.itemIcon; // ItemData 에 설정해둔 itemIcon 값을 가져와서 스프라이트 변경

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0]; // icon 과 달리 GetComponentsInChildren 의 첫번째 Text 컴포넌트는 Text Level 이기에 첫번째 값을 가져옴.
        textName = texts[1];
        textDesc = texts[2];
        textName.text = data.itemName;
    }

    void OnEnable()
    {
        textLevel.text = "Lv." + (level + 1);

        switch(data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                break;
            default:
                textDesc.text = string.Format(data.itemDesc);
                break;

        }
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

                level++;
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                if (level == 0)
                {
                    GameObject newGear = new GameObject();
                    // AddComponent 함수 반환 값을 미리 선언한 변수에 저장
                    gear = newGear.AddComponent<Gear>(); // AddComponent<T> : 게임오브젝트에 T 컴포넌트를 추가하는 함수
                    gear.Init(data);
                } else
                {
                    float nextRate = data.damages[level];
                    gear.LevelUp(nextRate);
                }

                level++;
                break;
            case ItemData.ItemType.Heal:
                GameManager.instance.health = GameManager.instance.maxHealth;
                // 일회성 아이템은 레벨업이 필요 없기에 level++ 코드를 제외
                break;
        }

        // 현재 레벨이 최대 레벨과 같을 시
        if(level == data.damages.Length)
        {
            // 버튼 클릭 제한되게 설정 -> interactable 비활성화 
            GetComponent<Button>().interactable = false;
        }
    }
}
