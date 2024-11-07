using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> Neighbours => _neighbours;
    [SerializeField] private List<Node> _neighbours;

}
