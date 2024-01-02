using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id; // 무기 종류
    public int prefabId; // 프리펩 ID
    public float damage; // 데미지
    public int count; // 개수
    public float speed; // 속도

    float timer; // 총알 발사 타이머
    [SerializeField] Player player;

    void Awake()
    {
        player = GameManager.instance.player;
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                // Vector3.forward -> (0, 0, 1)
                // Vector3.back -> (0, 0, -1)
                break;
            default:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }
    }

    // 레벨업
    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0)
        {
            Batch();
        }

        // player 가 가지고 있는 모든 Gear 에 한해서 모두 ApplyGear 함수를 실행
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver); // BroadcastMessage : 특정 함수 호출을 모든 자식에게 방송하는 함수
        // SendMessageOptions.DontRequireReceiver : 호출을 받는 개체가 꼭 존재하지 않아도 된다라는 옵션을 추가
    }

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Weapon " + data.itemId;
        transform.parent = player.transform; // 생성한 오브젝트를 플레이어 자식으로 설정
        transform.localPosition = Vector3.zero; // 플레이어 안에서 위치를 (0, 0, 0) 으로 맞추기에 지역 위치인 LocalPosition 을 원점으로 변경

        // Property Set
        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;

        for(int index = 0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            // ItemData 에서 설정해둔 프리팹과 pool 에 있는 프리팹이 같을 경우 index 순서를 prefabId 로 설정
            if(data.projectile == GameManager.instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }

        switch (id)
        {
            case 0:
                speed = 150; // Vector3.back 를 사용하기에 양수여야지 시계방향으로 회전
                Batch();
                break;
            default:
                // speed 값은 연사속도를 의미 -> 적을 수록 많이 발사
                speed = 0.3f;
                break;
        }

        // Hand Set
        Hand hand = player.hands[(int)data.itemType]; // enum 값 앞에 (int) 를 붙여줘서 형변환 
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    // 배치
    void Batch()
    {
        for (int index = 0; index < count; index++)
        {
            Transform bullet;

            if (index < transform.childCount) // childCount : 자식 오브젝트 개수 확인
            {
                // count 에 기존에 있던 bullet 까지 포함해서 사용
                // if (index < transform.childCount) 이 없으면 기존 bullet 을 제외하고 count 만큼 다시 생성됨.
                bullet = transform.GetChild(index);
            }
            else
            {
                // 기존의 bullet 보다 count 가 클 시, 그만큼만 pool 에서 가져와서 더 생성
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform; // 새롭게 만들어진 오브젝트의 부모를 자기자신으로 변경
            }

            // 초기화
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            // 회전
            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);

            // 위치
            bullet.Translate(bullet.up * 1.5f, Space.World);

            // 값 설정
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // -1 은 관통력 무한의 의미
        }
    }

    // 총알 발사
    void Fire()
    {
        // 인식된 적이 있는 지 먼저 파악
        if(!player.scanner.nearestTarget)
            return;

        // 목표 위치의 방향 구하기
        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized; // normalized : 현재 벡터의 방향은 유지하고 크기를 1로 변환하는 속성

        // 초기화
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;

        // 오브젝트 날아가는 방향에 맞게 회전 설정
        // FromToRotation : 지정된 축을 중심으로 목표를 향해 회전하는 함수
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        // 값 설정
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }
}
