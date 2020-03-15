using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// This will contain all the ability projectile in a pool
// So that abilities will not be instantiated anymore when they are called
public class AbilityProjectilePool : MonoBehaviour
{
    public static AbilityProjectilePool Instance;

    Dictionary<string, GameObject[]> poolMap;
    Dictionary<string, int> poolConfig;

    GameObject templateGO;

    // Use this for initialization
    void Awake()
    {
        Instance = this;

        poolMap = new Dictionary<string, GameObject[]>();
        poolConfig = new Dictionary<string, int>();

        poolConfig.Add("BlastProjectile", 4);
        poolConfig.Add("EarthquakeProjectile", 4);
        poolConfig.Add("FireballProjectile", 4);
        poolConfig.Add("FireStormProjectile", 4);
        poolConfig.Add("IceWallProjectile", 4);
        poolConfig.Add("PushProjectile", 4);
        poolConfig.Add("RootsProjectile", 4);
        poolConfig.Add("SpikesProjectile", 4);
        poolConfig.Add("TornadoProjectile", 4);
        poolConfig.Add("TraceProjectile", 4);
        poolConfig.Add("WaterRainProjectile", 4);
        poolConfig.Add("Mana Sphere", 4);
    }

    public void InitWithObjectList(List<GameObject> pooledObjectTypes)
    {
        foreach(GameObject pooledObject in pooledObjectTypes)
        {
            int size = poolConfig[pooledObject.name];

            Debug.Log("AbilityProjectilePool InitWithObjectList creating pool for " + pooledObject.name + " of size " + size);

            InitPool(pooledObject, size);
        }
    }

    void InitPool(GameObject template, int size)
    {
        GameObject[] pool = new GameObject[size];

        templateGO = template;

        for (int i = 0; i < size; i++)
        {
            pool[i] = Instantiate(template, transform);
            pool[i].SetActive(false);
        }

        // We will set the config value for the object type we just created to 0 so that we can use this Dictionary to store the current index
        poolConfig[template.name] = 0;

        poolMap.Add(template.name, pool);
    }

    public GameObject GetProjectile(string name)
    {
        // Get the current index and length of the specific pool 
        int currentIndex = poolConfig[name];           
        int poolSize = poolMap[name].Length;

        // Get the next index
        currentIndex = (currentIndex + 1) % poolSize;

        GameObject poolObject = poolMap[name][currentIndex];

        // If the object is not is use return this one to be used
        if (poolObject.activeSelf == false)
        {
            Debug.Log("ObjectPool GetNext found object 1st try at index " + currentIndex);
            poolConfig[name] = currentIndex;
            return poolObject;
        }

        // Preferably this part of the should not be called as it is more expensive
        else
        {
            GameObject[] pool = poolMap[name];

            // First we will loop the pool to check for a free spot
            for (int i = 0; i < pool.Length; i++)
            {
                if (pool[i].activeSelf == false)
                {
                    poolConfig[name] = i;
                    Debug.Log("ObjectPool GetNext found object at index " + i);
                    return pool[i];
                }
            }

            // Worst case there was no object that was found so we need to extend the array
            // so that we can create a new object, this is very expensive so in the ideal case
            // it should be never called
            poolSize++;
            Array.Resize<GameObject>(ref pool, poolSize);
            currentIndex = poolSize - 1;
            poolConfig[name] = currentIndex;

            pool[currentIndex] = Instantiate(templateGO, transform);
            
            Debug.Log("ObjectPool GetNext free object not found increasing Pull side to " + currentIndex);

            return pool[currentIndex];
        }
    }

}



