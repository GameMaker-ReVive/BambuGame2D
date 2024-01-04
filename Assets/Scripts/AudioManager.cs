using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect; 
    // AudioHighPassFilter 설정된 주파수 이상의 소리의 주파수를 설정된 주파수와 같게 낮춰주는 역할을 함.
    // AudioHighPassFilter 는 Listener Effect 계열이라 Audio Listener 가 있어야 사용가능.

    [Header("#SFX")] // 특수효과(Special Effects)
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels; // 다량의 효과음을 낼 수 있도록 채널 개수 변수 선언
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Sfx { Dead, Hit, LevelUp=3, Lose, Melee, Range=7, Select, Win } // Hit 가 2개라서 LevelUp 을 3번째로 지정, Range 도 똑같음.

    void Awake()
    {
        instance = this;

        Init();
    }

    void Init()
    {
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer"); // BgmPlayer -> Scene 에서 표시되는 오브젝트 이름
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false; // 기본값은 true
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // 효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for(int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].bypassListenerEffects = true; // bypassListenerEffects 를 사용해서 레벨업 효과음은 AudioHighPassFilter 의 영향을 받지 않게 해줌. (-> 배경음만 주파수를 낮춤)
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    public void PlayerBgm(bool isPlayer)
    {
        if(isPlayer)
        {
            bgmPlayer.Play();
        } else
        {
            bgmPlayer.Stop();
        }
    }

    public void EffectBgm(bool isPlayer)
    {
        bgmEffect.enabled = isPlayer;
    }

    public void PlayerSfx(Sfx sfx)
    {
        for(int index = 0; index < sfxPlayers.Length; index++)
        {
            // channelIndex 를 사용해서 사용된 index 의 값을 저장해놔서 만약 마지막으로 5번 Audio Source 사용했을 때, 다시 5번부터 비어있는 자리 탐색 시작.
            int loopIndex = (index + channelIndex) % sfxPlayers.Length; 

            // 해당 index 의 Audio Source 가 이미 사용중이면 다시 for 문으로 돌아가 다음 순서의 Audio Source 체크
            if (sfxPlayers[loopIndex].isPlaying)
                continue;
            
            // 이름이 같은 Sfx 값은 랜덤으로 부여
            int ranIndex = 0;
            if(sfx == Sfx.Hit || sfx == Sfx.Melee)
            {
                ranIndex = Random.RandomRange(0, 2);
            }

            channelIndex = loopIndex; // 사용한 loopIndex 를 저장
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }
}
