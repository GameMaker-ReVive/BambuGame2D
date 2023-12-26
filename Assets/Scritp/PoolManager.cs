using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    //1 프리펩을 보관할 변수
    public GameObject[] prefabs;

    //2 풀 담당을 하는 리스트들
    List<GameObject>[] pools;

    void Awake(){
        
        pools = new List<GameObject>[prefabs.Length];
        for(int index=0; index<pools.Length; index++){
            pools[index] = new List<GameObject>();
        }

        Debug.Log(pools.Length);
        
    }

    public GameObject Get(int index){
        
        GameObject select = null;
        
        //1 선택한 풀의 놀고(비활성화 된) 있는 게임오브젝트 접근

        

        //2 못 찾은거?
        

        foreach (GameObject item in pools[index])
        {
            if(!item.activeSelf){
                //발견하면 select 변수에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }
        if(!select){
            //새롭게 생성해서 select 변수에 할당\
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }


        return select;
    }


}
