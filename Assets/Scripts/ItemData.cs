using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptble Object/ItemData")] // CreateAssetMenu : 커스텀 메뉴를 생성하는 속성
public class ItemData : ScriptableObject
{
    // 아이템 타입 (근접, 원거리, 장갑, 신발, 힐)
    public enum ItemType { Melee, Range, Glove, Shoe, Heal }

    [Header("# Main Info")]
    public ItemType itemType; // 아이템 타입
    public int itemId; // 아이템 id
    public string itemName; // 아이템 이름
    public string itemDesc; // 아이템 설명
    public Sprite itemIcon; // 아이템 아이콘


    [Header("# Level Data")]
    public float baseDamage; // 기본 (0 레벨) 데미지
    public int baseCount; // 기본 근접(개수), 원거리(관통)
    public float[] damages; // 레벨 별 데미지
    public int[] counts; // 레벨 별 count

    [Header("# Weapon")]
    public GameObject projectile; // 투사체 (프리팹)

}
