using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    private static InterfaceManager instance;
    public static InterfaceManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("InterfaceManager").AddComponent<InterfaceManager>();
            }
            return instance;
        }
    }


    //private void Awake()
    //{
    //    generalContainerInterfaces = new();
    //    equipContainerInterfaces = new(); 
    //}

    //private List<GeneralContainerInterface> generalContainerInterfaces;
    //public GeneralContainerInterface GeneralContainerInterface
    //{
    //    get
    //    {
    //        for (int i = 0; i < generalContainerInterfaces.Count; i++)
    //        {
    //            if (generalContainerInterfaces[i].GeneralItemContainer == null)
    //            {
    //                return generalContainerInterfaces[i];
    //            }
    //        }
    //        GeneralContainerInterface generalContainerInterface = Instantiate(Resources.Load<GeneralContainerInterface>("UI/Interface/GeneralInterface"), UIManager.Instance.Canvas.transform);
    //        generalContainerInterfaces.Add(generalContainerInterface);
    //        return generalContainerInterface;
    //    }
    //}
    //private List<EquipContainerInterface> equipContainerInterfaces;
    //public EquipContainerInterface EquipContainerInterface
    //{
    //    get
    //    {
    //        for (int i = 0; i < equipContainerInterfaces.Count; i++)
    //        {
    //            if (equipContainerInterfaces[i].EquipItemContainer == null)
    //            {
    //                return equipContainerInterfaces[i];
    //            }
    //        }
    //        EquipContainerInterface equipContainerInterface = Instantiate(Resources.Load<EquipContainerInterface>("UI/Interface/EquipInterface"), UIManager.Instance.Canvas.transform);
    //        equipContainerInterfaces.Add(equipContainerInterface);
    //        return equipContainerInterface;
    //    }
    //}

}