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

                // ���� ������ ã�� �� ������ ���� ����
                if (instance == null)
                {
                    GameObject obj = new("SoundManager");
                    instance = obj.AddComponent<SoundManager>();
                }

                // �� ��ȯ �ÿ� �ı����� �ʵ��� ����
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

    //���ϴ� ���� ���带 �÷��� ������

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
