using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public void Move(Vector3 velocity)
    {
        transform.position += velocity;
    }
}
