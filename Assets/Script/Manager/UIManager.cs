using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Transform can = FindObjectOfType<Canvas>().transform;
                if (can == null)
                {
                    Debug.Log("캔버스가 없음");
                    return null;
                }
                else
                {
                    _instance = can.gameObject.AddComponent<UIManager>();
                }
            }

            return _instance;
        }
    }
    //GraphicRaycaster
    GraphicRaycaster m_gr;
    PointerEventData m_ped;
    List<RaycastResult> results;

    private Canvas canvas;
    public Canvas Canvas => canvas;

    private readonly List<GameObject> activeUI = new ();
    public List<GameObject> ActiveUI { get { return activeUI; } }   

    void Awake()
    {
        _instance = this;
        canvas = GetComponent<Canvas>();
        m_ped = new PointerEventData(null);
        m_gr = canvas.GetComponent<GraphicRaycaster>();
        results = new List<RaycastResult>();
    }

    public void OpenUI(GameObject ui)
    {
        activeUI.Add(ui);
        ui.gameObject.SetActive(true);
    }
    public void CloseUI(GameObject ui)
    {
        if(activeUI.Contains(ui))
        { activeUI.Remove(ui); }
        ui.gameObject.SetActive(false);
    }
    public GameObject CloseUI()
    {
        if (activeUI.Count > 0)
        {
            GameObject gameObject = activeUI[0];
            gameObject.SetActive(false);
            activeUI.RemoveAt(0);
            return gameObject;
        }
        return null;
    }


    public Coroutine MoveUp(Graphic graphic, Vector3 start, float speed, Camera cam)
    {
        return StartCoroutine(MoveUpCoroutine(graphic, start, speed, cam));
    }

    private IEnumerator MoveUpCoroutine(Graphic graphic, Vector3 start, float speed, Camera cam)
    {
        graphic.rectTransform.anchoredPosition = cam.WorldToScreenPoint(start);
        while (graphic != null)
        {
            graphic.rectTransform.anchoredPosition += Time.deltaTime * speed * Vector2.up;
            yield return null;
        }
    }

    public Coroutine FollowUI(Graphic graphic, GameObject obj, Vector2 plus, Camera cam)
    {
        return StartCoroutine(FollowUICoroutine(graphic, obj, plus, cam));
    }

    private IEnumerator FollowUICoroutine(Graphic graphic, GameObject obj, Vector2 plus, Camera cam)
    {
        while (graphic != null || obj != null)
        {
            graphic.rectTransform.anchoredPosition = cam.WorldToScreenPoint(obj.transform.position);
            graphic.rectTransform.anchoredPosition += plus;

            yield return null;
        }
    }

    public Coroutine MouseFallow(GameObject graphic)
    {
        return StartCoroutine(MouseFallowCoroutine(graphic));
    }

    private IEnumerator MouseFallowCoroutine(GameObject graphic)
    {
        while (true)
        {
            graphic.transform.position = Input.mousePosition;

            yield return null;
        }
    }

    public void TouchUI(GameObject obj)
    {
        Transform tf = obj.transform.parent;
        while(tf != null)
        {
            if(tf == canvas)
            {
                tf.SetAsLastSibling();
                break;
            }
            tf = tf.parent;
        }
    }

    public T GetGraphicRay<T>() where T : MonoBehaviour
    {
        results.Clear();

        m_ped.position = Input.mousePosition;

        m_gr.Raycast(m_ped, results);

        if (results.Count > 0)
        {
            return results[0].gameObject.GetComponent<T>();
        }
        return default;
    }

    public Transform GetGraphicRay()
    {
        results.Clear();

        m_ped.position = Input.mousePosition;

        m_gr.Raycast(m_ped, results);

        if (results.Count > 0)
        {
            return results[0].gameObject.transform;
        }
        return default;
    }

    public bool TryGetGraphicRay<T>(out T behaviour) where T : MonoBehaviour
    {
        results.Clear();

        m_ped.position = Input.mousePosition;

        m_gr.Raycast(m_ped, results);
        if (results.Count > 0)
            behaviour = results[0].gameObject.GetComponent<T>();
        else
            behaviour = null;
        if(behaviour != null)
            return true;
        return false;
    }

    public T GetGraphicRayFind<T>() where T : MonoBehaviour
    {
        results.Clear();

        m_ped.position = Input.mousePosition;

        m_gr.Raycast(m_ped, results);

        if (results.Count > 0)
        {
            for (int i = 0; i < results.Count; i++)
                if (results[i].gameObject.GetComponent<T>() != null)
                    return results[i].gameObject.GetComponent<T>();
        }
        return default;
    }

    public bool TryGetGraphicRayFind<T>(out T behaviour) where T : MonoBehaviour
    {
        results.Clear();

        m_ped.position = Input.mousePosition;

        m_gr.Raycast(m_ped, results);

        behaviour = null;

        if (results.Count > 0)
        {
            for (int i = 0; i < results.Count; i++)
            {
                if (results[i].gameObject.GetComponent<T>() != null)
                {
                    behaviour = results[i].gameObject.GetComponent<T>();
                    return true;
                }
            }     
        }
        return false;
    }

    public void ListSwap<T>(List<T> list, int index1, int index2)
    {
        if (index2 == -1)
            return;

        if (index1 < 0 || index2 >= list.Count)
        {
            return;
        }

        (list[index1], list[index2]) = (list[index2], list[index1]);
    }
    public int FindIndex<T>(List<T> list, T obj)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Equals(obj))
            {
                return i;
            }
        }
        return -1;
    }
}
