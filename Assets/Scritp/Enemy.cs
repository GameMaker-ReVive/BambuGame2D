using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;

    public RuntimeAnimatorController[] animCon;

    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;
    Collider2D coll;
    SpriteRenderer spriter;
    Animator anim;
    WaitForFixedUpdate wait;

    void Awake(){
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    void FixedUpdate(){

        if(!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;


        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    void LateUpdate(){

        if(!isLive)
            return;
        spriter.flipX = target.position.x < rigid.position.x;
    }

    void OnEnable(){
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead",false);
        health = maxHealth;

    }

    public void Init(SpawnData data){
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }


    void OnTriggerEnter2D(Collider2D other) {
        if(!other.CompareTag("Bullet") || !isLive)
            return;

        health -= other.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if(health > 0 ){
            anim.SetTrigger("Hit");
        }   
        else {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead",true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
        } 
    }

    IEnumerator KnockBack(){//코루틴
        yield return wait;//1프레임 쉬기
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3f, ForceMode2D.Impulse);

        //yield return new WaitForSeconds(2f);//2초 쉬기

    }

    void Dead(){
        gameObject.SetActive(false);
    }
}