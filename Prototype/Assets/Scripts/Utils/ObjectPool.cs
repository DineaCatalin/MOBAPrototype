using UnityEngine;
using System;

public class ObjectPool : MonoBehaviour
{
    int currentIndex;
    int poolSize;

    GameObject templateGO;

    GameObject[] pool;

    public void Init(GameObject template, int size)
    {
        poolSize = size;
        pool = new GameObject[poolSize];

        templateGO = template;

        for (int i = 0; i < poolSize; i++)
        {
            pool[i] = Instantiate(template, transform);
        }

        currentIndex = 0;
    }

    public GameObject GetNext()
    {
        currentIndex = (currentIndex + 1) % poolSize;

        GameObject poolObject = pool[currentIndex];

        // If the object is not is use return this one to be used
        if (poolObject.activeSelf == false)
        {
            Debug.Log("ObjectPool GetNext found object 1st try at index " + currentIndex);
            return poolObject;
        }

        // Preferably this part of the should not be called as it is more expensive
        else
        {
            // First we will loop the pool to check for a free spot
            for (int i = 0; i < poolSize; i++)
            {
                if (pool[i].activeSelf == false)
                {
                    currentIndex = i;
                    Debug.Log("ObjectPool GetNext found object at index " + currentIndex + " at try " + i);
                    return pool[i];
                }
            }

            // Worst case there was no object that was found so we need to extend the array
            // so that we can create a new object, this is very expensive so in the ideal case
            // it should be never called
            poolSize++;
            Array.Resize<GameObject>(ref pool, poolSize);
            currentIndex = poolSize - 1;

            pool[currentIndex] = Instantiate(templateGO, transform);

            Debug.Log("ObjectPool GetNext free object not found increasing Pull side to " + currentIndex);

            return pool[currentIndex];
        }
    }

}

