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
        icon = GetComponentsInChildren<Image>()[1]; // GetComponentsInChildren �� ù��° Image ������Ʈ�� �ڱ��ڽ��̹Ƿ� �ι�° ���� ������.
        icon.sprite = data.itemIcon; // ItemData �� �����ص� itemIcon ���� �����ͼ� ��������Ʈ ����

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0]; // icon �� �޸� GetComponentsInChildren �� ù��° Text ������Ʈ�� Text Level �̱⿡ ù��° ���� ������.
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
                // ������ 0 �� �� (���� ���� ��) �⺻ ������Ʈ ����
                if(level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    // AddComponent �Լ� ��ȯ ���� �̸� ������ ������ ����
                    weapon = newWeapon.AddComponent<Weapon>(); // AddComponent<T> : ���ӿ�����Ʈ�� T ������Ʈ�� �߰��ϴ� �Լ�
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

        // ���� ������ �ִ� ������ ���� ��
        if(level == data.damages.Length)
        {
            // ��ư Ŭ�� ���ѵǰ� ���� -> interactable ��Ȱ��ȭ 
            GetComponent<Button>().interactable = false;
        }
    }
}
