﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerBuffs : MonoBehaviour
{
    public Image manaChargeImage;
    public Image slowImage;
    public Image rootImage;
    public Image dotImage;
    public Image speedImage;
    public Image healImage;
    public Image doubleDamageImage;

    Dictionary<PlayerBuff, Image> imageMap;
    Dictionary<PlayerBuff, float> buffDurationMap;
    Dictionary<PlayerBuff, Coroutine> coroutineMap;

    bool locked;

    private void Start()
    {
        imageMap = new Dictionary<PlayerBuff, Image>();
        buffDurationMap = new Dictionary<PlayerBuff, float>();
        coroutineMap = new Dictionary<PlayerBuff, Coroutine>();

        imageMap.Add(PlayerBuff.DOT, dotImage);
        imageMap.Add(PlayerBuff.Speed, speedImage);
        imageMap.Add(PlayerBuff.DoubleDamage, doubleDamageImage);
        imageMap.Add(PlayerBuff.Slow, slowImage);
        imageMap.Add(PlayerBuff.Root, rootImage);
        imageMap.Add(PlayerBuff.ManaBurn, manaChargeImage);
        imageMap.Add(PlayerBuff.Heal, healImage);

        buffDurationMap.Add(PlayerBuff.DOT, 0);
        buffDurationMap.Add(PlayerBuff.Speed, 0);
        buffDurationMap.Add(PlayerBuff.DoubleDamage, 0);
        buffDurationMap.Add(PlayerBuff.Slow, 0);
        buffDurationMap.Add(PlayerBuff.Root, 0);
        buffDurationMap.Add(PlayerBuff.ManaBurn, 0);
        buffDurationMap.Add(PlayerBuff.Heal, 0);

        coroutineMap.Add(PlayerBuff.DOT, null);
        coroutineMap.Add(PlayerBuff.Speed, null);
        coroutineMap.Add(PlayerBuff.DoubleDamage, null);
        coroutineMap.Add(PlayerBuff.Slow, null);
        coroutineMap.Add(PlayerBuff.Root, null);
        coroutineMap.Add(PlayerBuff.ManaBurn, null);
        coroutineMap.Add(PlayerBuff.Heal, null);

        EventManager.StartListening("StartRound", new System.Action(Unlock));
    }

    public void AddBuff(PlayerBuff buff, float duration)
    {
        if(!locked)
        {
            Debug.Log("PlayerBuffs AddBuff " + buff + " duration " + duration);
            buffDurationMap[buff] += duration;
            imageMap[buff].enabled = true;

            if (coroutineMap[buff] == null)
            {
                Debug.Log("PlayerBuffs AddBuff starting coroutine" + buff);
                coroutineMap[buff] = StartCoroutine(BuffTick(buff));
            }
        }
    }

    IEnumerator BuffTick(PlayerBuff buff)
    {
        Debug.Log("PlayerBuffs BuffTick");
        float duration;

        while (buffDurationMap[buff] > 0f)
        {    
            duration = buffDurationMap[buff];
            buffDurationMap[buff] = 0f;

            Debug.Log("PlayerBuffs AddBuff Buff active waiting for " + duration);

            yield return new WaitForSeconds(duration);
        }

        Debug.Log("PlayerBuffs AddBuff Closing coroutine and deactivating for " + buff);

        coroutineMap[buff] = null;
        Deactivate(buff);
    }

    public void Deactivate(PlayerBuff buff)
    {
        imageMap[buff].enabled = false;
    }

    public void ActivateBuff(PlayerBuff buff)
    {
        imageMap[buff].enabled = true;
    }

    public void DeactivateAll()
    {
        foreach (var item in imageMap)
        {
            Debug.Log("PlayerBuffs DeactivateAll deactivating " + item.Key);
            buffDurationMap[item.Key] = 0;

            if(coroutineMap[item.Key] != null)
            {
                StopCoroutine(coroutineMap[item.Key]);
            }

            item.Value.enabled = false;
        }

        Lock();
    }

    void Unlock()
    {
        locked = false;
    }

    void Lock()
    {
        locked = true;
    }
}
