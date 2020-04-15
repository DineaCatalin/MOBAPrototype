using System.Collections.Generic;
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

    Dictionary<PlayerEffect, Image> imageMap;
    Dictionary<PlayerEffect, float> buffDurationMap;
    Dictionary<PlayerEffect, Coroutine> coroutineMap;

    bool locked;

    private void Start()
    {
        imageMap = new Dictionary<PlayerEffect, Image>();
        buffDurationMap = new Dictionary<PlayerEffect, float>();
        coroutineMap = new Dictionary<PlayerEffect, Coroutine>();

        imageMap.Add(PlayerEffect.DOT, dotImage);
        imageMap.Add(PlayerEffect.Speed, speedImage);
        imageMap.Add(PlayerEffect.DoubleDamage, doubleDamageImage);
        imageMap.Add(PlayerEffect.Slow, slowImage);
        imageMap.Add(PlayerEffect.Root, rootImage);
        imageMap.Add(PlayerEffect.ManaBurn, manaChargeImage);
        imageMap.Add(PlayerEffect.Heal, healImage);

        buffDurationMap.Add(PlayerEffect.DOT, 0);
        buffDurationMap.Add(PlayerEffect.Speed, 0);
        buffDurationMap.Add(PlayerEffect.DoubleDamage, 0);
        buffDurationMap.Add(PlayerEffect.Slow, 0);
        buffDurationMap.Add(PlayerEffect.Root, 0);
        buffDurationMap.Add(PlayerEffect.ManaBurn, 0);
        buffDurationMap.Add(PlayerEffect.Heal, 0);

        coroutineMap.Add(PlayerEffect.DOT, null);
        coroutineMap.Add(PlayerEffect.Speed, null);
        coroutineMap.Add(PlayerEffect.DoubleDamage, null);
        coroutineMap.Add(PlayerEffect.Slow, null);
        coroutineMap.Add(PlayerEffect.Root, null);
        coroutineMap.Add(PlayerEffect.ManaBurn, null);
        coroutineMap.Add(PlayerEffect.Heal, null);

        EventManager.StartListening(GameEvent.StartRound, new System.Action(Unlock));
    }

    public void AddBuff(PlayerEffect buff, float duration)
    {
        if(!locked && imageMap.ContainsKey(buff))
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

    IEnumerator BuffTick(PlayerEffect buff)
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

    public void Deactivate(PlayerEffect buff)
    {
        imageMap[buff].enabled = false;
    }

    public void ActivateBuff(PlayerEffect buff)
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
