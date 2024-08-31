using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private static TimeManager instance;
    public static TimeManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TimeManager>();

                // ���� ������ ã�� �� ������ ���� ����
                if (instance == null)
                {
                    GameObject obj = new("TimeManager");
                    instance = obj.AddComponent<TimeManager>();
                }

                if (instance.transform.parent != null)
                    DontDestroyOnLoad(instance.transform.parent);
                else
                    DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    readonly List<Coroutine> coroutineList = new ();

    /// <summary>
    /// �ð� ������ �ѹ� ȣ��
    /// </summary>
    /// <param name="timer"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public Coroutine TimerCallback(float timer, Action action)
    {
        Coroutine coroutine = StartCoroutine(TimerCoroutine(timer, action));
        coroutineList.Add(coroutine);   
        return coroutine;
    }
    /// <summary>
    /// �ð����� ��� ȣ��
    /// </summary>
    /// <param name="timer"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public Coroutine TimerCheckCallback(float timer, Action action)
    {
        Coroutine coroutine = StartCoroutine(TimerCheckCoroutine(timer, action));
        coroutineList.Add(coroutine);
        return coroutine;
    }
    public void StopTimer(Coroutine coroutine)
    {
        coroutineList.Remove(coroutine);
        if(coroutine != null)
            StopCoroutine(coroutine);
    }

    private IEnumerator TimerCoroutine(float timer, Action action)
    {
        yield return new WaitForSeconds(timer);
        action();
    }
    private IEnumerator TimerCheckCoroutine(float timer, Action action)
    {
        while(true)
        {
            yield return null;
            timer -= Time.deltaTime;
            if (timer < 0)
                break;
            action();
        }
    }


    public Coroutine CallCoroutine(IEnumerator enumerator)
    {
        return StartCoroutine(enumerator);
    }
}
