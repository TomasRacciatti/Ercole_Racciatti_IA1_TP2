using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public virtual Vector3 Position => transform.position;
    public virtual Vector3 Velocity => Vector3.zero;


}
