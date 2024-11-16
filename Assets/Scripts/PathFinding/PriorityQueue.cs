using System;
using System.Collections.Generic;

public class PriorityQueue<T>
{
    public enum RuleSet
    {
        MaxHeap,
        MinHeap,
    }

    private List<PriorityItem> _internal;

    public int Count => _internal.Count;
    public bool IsEmpty => _internal.Count == 0;

    private readonly RuleSet _heapRule;

    private const int INVALID_INDEX = -1;

    public PriorityQueue()
    {
        _heapRule = RuleSet.MinHeap;
        _internal = new List<PriorityItem>();
    }

    public PriorityQueue(RuleSet ruleSet)
    {
        _heapRule = ruleSet;
        _internal = new List<PriorityItem>();
    }

    public PriorityQueue(PriorityQueue<T> other)
    {
        _heapRule = other._heapRule;
        _internal = new List<PriorityItem>(other._internal);
    }

    public void Reprioritize()
    {
        var oldInternal = _internal;
        _internal = new List<PriorityItem>();

        for (int i = 0; i < oldInternal.Count; i++)
        {
            Enqueue(oldInternal[i]);
        }
    }

    public void Enqueue(T obj, float priority)
    {
        Enqueue(new PriorityItem(obj, priority));
    }

    public void Enqueue(PriorityItem item)
    {
        _internal.Add(item);

        RunEnqueueRules(_internal.Count - 1);
    }

    public T Dequeue()
    {
        if (Count == 0) return default;

        var temp = _internal[0];
        _internal[0] = _internal[Count - 1];
        _internal.RemoveAt(Count - 1);

        RunDequeueRules(0);

        return temp.Item;
    }

    public bool Contains(T item)
    {
        foreach (var priorityItem in _internal)
        {
            if (priorityItem.Equals(item))
            {
                return true;
            }
        }

        return false;
    }

    private void RunEnqueueRules(int index)
    {
        var parentIndex = GetParent(index);

        if (parentIndex == INVALID_INDEX) return;

        //if (_internal[index].Priority < _internal[parentIndex].Priority)
        if (IsBetter(_internal[index], _internal[parentIndex]))
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
            //if (_internal[leftChild].Priority < _internal[index].Priority)
            if (IsBetter(_internal[leftChild], _internal[index]))
            {
                (_internal[index], _internal[leftChild]) = (_internal[leftChild], _internal[index]);
            }
            return;
        }

        //var biggerChildIndex = _internal[leftChild].Priority < _internal[rightChild].Priority
        var biggerChildIndex = IsBetter(_internal[leftChild], _internal[rightChild])
            ? leftChild
            : rightChild;

        //if (_internal[biggerChildIndex].Priority < _internal[index].Priority)
        if (IsBetter(_internal[biggerChildIndex], _internal[index]))
        {
            (_internal[index], _internal[biggerChildIndex]) = (_internal[biggerChildIndex], _internal[index]);
            RunDequeueRules(biggerChildIndex);
        }
    }

    private bool IsBetter(PriorityItem a, PriorityItem b)
    {
        return _heapRule switch
        {
            RuleSet.MaxHeap => a.Priority > b.Priority,
            RuleSet.MinHeap => a.Priority < b.Priority,
            _ => false
        };
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

    public class PriorityItem : IEquatable<PriorityItem>
    {
        public T Item => _item;
        public float Priority => _priority;

        private readonly T _item;
        private float _priority;

        public PriorityItem(T item, float priority)
        {
            _item = item;
            _priority = priority;
        }

        public bool Equals(T other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<T>.Default.Equals(_item, other);
        }

        public bool Equals(PriorityItem other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<T>.Default.Equals(_item, other._item);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PriorityItem)obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(_item);
        }

        public static bool operator ==(PriorityItem left, PriorityItem right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PriorityItem left, PriorityItem right)
        {
            return !Equals(left, right);
        }
    }
}
