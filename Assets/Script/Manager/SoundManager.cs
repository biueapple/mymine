using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();

                // 만약 씬에서 찾을 수 없으면 새로 생성
                if (instance == null)
                {
                    GameObject obj = new("SoundManager");
                    instance = obj.AddComponent<SoundManager>();
                }

                // 씬 전환 시에 파괴되지 않도록 설정
                if (instance.transform.parent != null)
                    DontDestroyOnLoad(instance.transform.parent);
                else
                    DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }
    public enum SoundClips
    {
        Sowrd,
        Bow,
        Wand,
        Eating,

    }
    [SerializeField]
    private AudioClip[] clips;

    public List<Transform> audioSources = new ();
    public void PlayClip(AudioClip clip, Transform parent)
    {
        audioSources.Add(parent);
        AudioSource source = audioSources[^1].gameObject.AddComponent<AudioSource>();
        source.spatialBlend = 1;
        source.clip = clip;
        source.playOnAwake = false;
        source.loop = true;
        source.Play();
    }
    public void StopClip(AudioClip clip, Transform parent)
    {
        AudioSource[] source = parent.GetComponents<AudioSource>();
        for(int i = 0; i < source.Length; i++)
        {
            if(source[i].clip == clip)
            {
                audioSources.Remove(parent);
                source[i].Stop();
                Destroy(source[i]);
                break;
            }
        }
    }

    //원하는 곳에 사운드를 플레이 시켜줌

    public void CreateSound(Vector3 position, SoundClips clip)
    {
        AudioSource source = new GameObject("AudioSource").AddComponent<AudioSource>();
        source.transform.position = position;
        source.clip = clips[(int)clip];
        source.Play();
        StartCoroutine(DeleteSound(clips[(int)clip].length, source.gameObject));
    }

    private IEnumerator DeleteSound(float timer, GameObject gameObject)
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
