using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour, IWeighted
{
    public List<Node> Neighbours => _neighbours;

    public float Weight { get; set; }
    public float OriginalWeight { get; set; }

    [SerializeField] private List<Node> _neighbours;

}
