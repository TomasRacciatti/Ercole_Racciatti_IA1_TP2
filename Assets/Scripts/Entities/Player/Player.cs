using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Player : Agent
{
    [SerializeField] protected float _speed;
    [SerializeField] protected LayerMask _victoryLayerMask;
    [SerializeField] protected Vector3 _collisionBoxSize = new Vector3(1, 2, 1);


    protected bool DetectCollision(Vector3 position, Vector3 boxSize, LayerMask collisionMask)
    {
        Collider[] hits = Physics.OverlapBox(position, boxSize / 2, Quaternion.identity, collisionMask);

        foreach (var hit in hits)
        {
            if (((1 << hit.gameObject.layer) & collisionMask) != 0)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 boxCenter = transform.position;
        Gizmos.DrawWireCube(boxCenter, _collisionBoxSize);
    }
}
