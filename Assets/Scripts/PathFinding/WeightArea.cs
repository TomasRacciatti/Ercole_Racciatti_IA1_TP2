using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightArea : MonoBehaviour
{
    // Si bien no creo que use este script en el proyecto, me viene bien tenerlo y entenderlo.


    [SerializeField] private float _areaRadius;
    [SerializeField] private float _coefficient;
    [SerializeField] private AnimationCurve _droppOffCurve;


    public float GetAreaWeight(Vector3 position)
    {
        float distance = Vector3.Distance(position, transform.position);

        if (distance > _areaRadius)
            return 0;

        float ammount = 1 - Mathf.Clamp01(distance / _areaRadius);

        return _droppOffCurve.Evaluate(ammount) * _coefficient;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _areaRadius);
    }

}
