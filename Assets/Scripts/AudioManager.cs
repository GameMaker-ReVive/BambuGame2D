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
    // AudioHighPassFilter ������ ���ļ� �̻��� �Ҹ��� ���ļ��� ������ ���ļ��� ���� �����ִ� ������ ��.
    // AudioHighPassFilter �� Listener Effect �迭�̶� Audio Listener �� �־�� ��밡��.

    [Header("#SFX")] // Ư��ȿ��(Special Effects)
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels; // �ٷ��� ȿ������ �� �� �ֵ��� ä�� ���� ���� ����
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Sfx { Dead, Hit, LevelUp=3, Lose, Melee, Range=7, Select, Win } // Hit �� 2���� LevelUp �� 3��°�� ����, Range �� �Ȱ���.

    void Awake()
    {
        instance = this;

        Init();
    }

    void Init()
    {
        // ����� �÷��̾� �ʱ�ȭ
        GameObject bgmObject = new GameObject("BgmPlayer"); // BgmPlayer -> Scene ���� ǥ�õǴ� ������Ʈ �̸�
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false; // �⺻���� true
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // ȿ���� �÷��̾� �ʱ�ȭ
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for(int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].bypassListenerEffects = true; // bypassListenerEffects �� ����ؼ� ������ ȿ������ AudioHighPassFilter �� ������ ���� �ʰ� ����. (-> ������� ���ļ��� ����)
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
            // channelIndex �� ����ؼ� ���� index �� ���� �����س��� ���� ���������� 5�� Audio Source ������� ��, �ٽ� 5������ ����ִ� �ڸ� Ž�� ����.
            int loopIndex = (index + channelIndex) % sfxPlayers.Length; 

            // �ش� index �� Audio Source �� �̹� ������̸� �ٽ� for ������ ���ư� ���� ������ Audio Source üũ
            if (sfxPlayers[loopIndex].isPlaying)
                continue;
            
            // �̸��� ���� Sfx ���� �������� �ο�
            int ranIndex = 0;
            if(sfx == Sfx.Hit || sfx == Sfx.Melee)
            {
                ranIndex = Random.RandomRange(0, 2);
            }

            channelIndex = loopIndex; // ����� loopIndex �� ����
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }
}
