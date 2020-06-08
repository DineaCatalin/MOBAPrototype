﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectPool : MonoBehaviour
{
    public static ParticleEffectPool Instance;

    [SerializeField] ParticlePoolData[] particleDatas;

    Dictionary<GameParticle, LocalParticleSystemManager[]> particleMap;
    Dictionary<GameParticle, int> indexMap;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        particleMap = new Dictionary<GameParticle, LocalParticleSystemManager[]>();
        indexMap = new Dictionary<GameParticle, int>();

        foreach (var particleData in particleDatas)
        {
            LocalParticleSystemManager[] particleArray = new LocalParticleSystemManager[particleData.number];

            for (int i = 0; i < particleArray.Length; i++)
            {
                Debug.Log("ParticleEffectPool Awake Instantiating " + particleData.particleSystem.name);
                GameObject particlesGO = Instantiate(particleData.particleSystem, transform);

                LocalParticleSystemManager particleManager;

                particleManager = particlesGO.GetComponent<LocalParticleSystemManager>();

                if (particleManager == null)
                {
                    particleManager = particlesGO.AddComponent<LocalParticleSystemManager>();
                }

                particleArray[i] = particleManager;
            }

            particleMap.Add(particleData.particleName, particleArray);
            indexMap.Add(particleData.particleName, 0);
        }

        particleDatas = null; 
    }

    public void SpawnParticle(GameParticle particleName, Vector2 location)
    {
        Debug.Log("ParticleEffectPool SpawnParticle " + particleName.ToString());

        // Get the current index and length of the specific pool 
        int currentIndex = indexMap[particleName];
        int poolSize = particleMap[particleName].Length;

        // Get the next index
        currentIndex = (currentIndex + 1) % poolSize;
        Debug.Log("ObjectPool GetNext " + name + "Current Index is " + currentIndex);

        LocalParticleSystemManager poolObject = particleMap[particleName][currentIndex];

        // If the object is not is use return this one to be used
        if (!poolObject.InUse)
        {
            Debug.Log("ObjectPool GetNext found object 1st try at index " + currentIndex);
            indexMap[particleName] = currentIndex;
            ShowParticles(poolObject, location);
        }

        // Preferably this part of the should not be called as it is more expensive
        else
        {
            LocalParticleSystemManager[] pool = particleMap[particleName];

            // First we will loop the pool to check for a free spot
            Debug.Log("ObjectPool GetNext Looping objects to find unactive. Length " + pool.Length);
            for (int i = 0; i < pool.Length; i++)
            {
                if (!pool[i].InUse)
                {
                    indexMap[particleName] = i;
                    Debug.Log("ObjectPool GetNext found object at index " + i);
                    ShowParticles(pool[i], location);
                }
            }

            // Worst case there was no object that was found so we need to extend the array
            // so that we can create a new object, this is very expensive so in the ideal case
            // it should be never called
            poolSize++;
            Debug.Log("ObjectPool GetNext Pool size before expansion " + pool.Length);
            Array.Resize<LocalParticleSystemManager>(ref pool, poolSize);
            Debug.Log("ObjectPool GetNext Pool size after expansion " + pool.Length + " original pool size " + particleMap[particleName].Length);
            currentIndex = poolSize - 1;
            Debug.Log("ObjectPool GetNext Adding new object to index " + currentIndex);
            indexMap[particleName] = currentIndex;

            // Set the name of the new object to the name of the object that was used as a template
            // for the Instantiate() otherwise the new object it will have "(Clone)" added to it's name
            pool[currentIndex] = Instantiate(pool[currentIndex - 1], transform);
            pool[currentIndex].name = pool[currentIndex - 1].name;

            particleMap[particleName] = pool;

            Debug.Log("ObjectPool GetNext Adding new object : " + pool[currentIndex].name);
            Debug.Log("ObjectPool GetNext free object not found increasing Pull side to " + poolSize);

            ShowParticles(pool[currentIndex], location);
        }
    }

    void ShowParticles(LocalParticleSystemManager particles, Vector2 position)
    {
        particles.transform.position = position;
        particles.PlayAndStop();
    }
}
