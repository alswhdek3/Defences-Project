using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameobjectPool<T> where T : class
{
    public delegate T CreateDel();

    private int count;
    private Queue<T> queue = new Queue<T>();
    private CreateDel createDel;

    public GameobjectPool(int _count , CreateDel _createDel)
    {
        count = _count;
        createDel = _createDel;

        AllSetting();
    }

    private void AllSetting()
    {
        for(int i=0; i<count; i++)
        {
            var obj = createDel();
            queue.Enqueue(obj);
        }
    }

    public T Get()
    {
        if(queue.Count > 0)
        {
            return queue.Dequeue();
        }
        else
        {
            var obj = createDel();
            return obj;
        }
    }

    public void Set(T _item)
    {
        queue.Enqueue(_item);
    }
}
