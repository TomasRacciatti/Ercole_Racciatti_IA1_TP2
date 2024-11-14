using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T> where T : IWeighted
{
    private List<T> _internal;

    public int Count => _internal.Count;
    public bool IsEmpty => _internal.Count == 0;

    private const int INVALID_INDEX = -1;

    public PriorityQueue()
    {
        _internal = new List<T>();
    }
    public PriorityQueue(PriorityQueue<T> collection)
    {
        _internal = new List<T>(collection._internal);
    }

    public void Reprioritize()
    {
        var oldInternal = _internal;
        _internal = new List<T>();

        for (int i = 0; i < oldInternal.Count; i++)
        {
            Enqueue(oldInternal[i]);
        }
    }

    public void Enqueue(T storeObject)
    {
        _internal.Add(storeObject);

        RunEnqueueRules(_internal.Count - 1);
    }

    public T Dequeue()
    {
        if (Count == 0) return default;

        var temp = _internal[0];
        _internal[0] = _internal[Count - 1];
        _internal.RemoveAt(Count - 1);

        RunDequeueRules(0);

        return temp;
    }

    public bool Contains(T item)
    {
        return _internal.Contains(item);
    }

    private void RunEnqueueRules(int index)
    {
        var parentIndex = GetParent(index);

        if (parentIndex == INVALID_INDEX) return;

        if (_internal[index].Weight < _internal[parentIndex].Weight)
        {
            (_internal[parentIndex], _internal[index]) = (_internal[index], _internal[parentIndex]);
            RunEnqueueRules(parentIndex);
        }
    }
    private void RunDequeueRules(int index)
    {
        var leftChild = GetLeftDescendant(index);
        var rightChild = GetRightDescendant(index);

        if (leftChild == INVALID_INDEX) return;
        if (rightChild == INVALID_INDEX)
        {
            if (_internal[leftChild].Weight < _internal[index].Weight)
            {
                (_internal[index], _internal[leftChild]) = (_internal[leftChild], _internal[index]);
            }
            return;
        }

        var biggerChildIndex = _internal[leftChild].Weight < _internal[rightChild].Weight
            ? leftChild
            : rightChild;

        if (_internal[biggerChildIndex].Weight < _internal[index].Weight)
        {
            (_internal[index], _internal[biggerChildIndex]) = (_internal[biggerChildIndex], _internal[index]);
            RunDequeueRules(biggerChildIndex);
        }
    }

    private int GetParent(int index)
    {
        var value = (index - 1) / 2;
        return value >= 0 ? value : INVALID_INDEX;
    }

    private int GetLeftDescendant(int index)
    {
        var value = (index * 2) + 1;
        return value < Count ? value : INVALID_INDEX;
    }
    private int GetRightDescendant(int index)
    {
        var value = (index * 2) + 2;
        return value < Count ? value : INVALID_INDEX;
    }

    public override string ToString()
    {
        string rString = "Elements Are: ";

        for (int i = 0; i < Count; i++)
        {
            rString += $"\n {i} - {_internal[i]}";
        }
        return rString;
    }

}

public interface IWeighted
{
    float Weight { get; }
}
