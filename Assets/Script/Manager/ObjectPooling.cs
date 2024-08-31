using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    private static ObjectPooling instance;

    private Transform preliminaryParent;
    private List<GameObject> objectList;

    private GroundItem _groundItem;
    private List<GroundItem> activeItem;
    private List<GroundItem> deactiveItem;

    // �ٸ� ��ũ��Ʈ���� �� �ν��Ͻ��� ������ �� �ִ� �Ӽ�
    public static ObjectPooling Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ObjectPooling>();

                // ���� ������ ã�� �� ������ ���� ����
                if (instance == null)
                {
                    GameObject obj = new ("ObjectPooling");
                    instance = obj.AddComponent<ObjectPooling>();
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

    // ��Ÿ ��� ���� �� �޼���
    void Awake()
    {
        objectList = new();
        activeItem = new();
        deactiveItem = new();
        _groundItem = Resources.Load<GroundItem>("World/GroundItem");
    }


    public GameObject CreateObject(GameObject pre, Transform parent, Vector3 position, Quaternion quaternion)
    {
        if (preliminaryParent == null)
        {
            preliminaryParent = new GameObject("preliminaryParent").transform;
            DontDestroyOnLoad(preliminaryParent);
        }
        if (pre == null)
            return null;
        for(int i = 0; i < objectList.Count; i++)
        {
            if (objectList[i].name.Equals(pre.name) && !objectList[i].activeSelf)
            {
                objectList[i].SetActive(true);
                objectList[i].transform.SetParent(parent == null ? preliminaryParent : parent, false);
                objectList[i].transform.SetLocalPositionAndRotation(position, quaternion);
                return objectList[i];
            }
        }
        GameObject obj = Instantiate(pre, position, quaternion, parent == null ? preliminaryParent : parent);
        obj.SetActive(true);
        obj.name = pre.name;
        objectList.Add(obj);

        return obj;
    }

    public void DestroyObject(GameObject obj)
    {
        if (obj != null)
        {
            if(!objectList.Contains(obj))
            {
                objectList.Add(obj);
            }
            obj.SetActive(false);
        }
    }

    public GroundItem CreateItem()
    {
        GroundItem item = deactiveItem.Find(x => x.gameObject.activeSelf == true);
        if(item != null)
        {
            deactiveItem.Remove(item);
            activeItem.Add(item);
            item.gameObject.SetActive(true);
            return item;
        }
        else
        {
            if (preliminaryParent == null)
            {
                preliminaryParent = new GameObject("preliminaryParent").transform;
                DontDestroyOnLoad(preliminaryParent);
            }
            item = Instantiate(_groundItem, preliminaryParent.transform);
            activeItem.Add(item);
            item.gameObject.SetActive(true);
            return item;
        }
    }

    public void RemoveItem(GroundItem item)
    {
        activeItem.Remove(item);
        deactiveItem.Add(item);
        item.gameObject.SetActive(false);
    }

}
