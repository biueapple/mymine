using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StorageInterface : MonoBehaviour
{
    protected Storage storage;
    public Storage Storage { get { return storage; } }

    public void Interlock(Storage storage)
    {
        this.storage = storage;
        CreateSlot();
    }

    protected abstract void CreateSlot();
}
