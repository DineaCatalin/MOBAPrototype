using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetImageSprite : MonoBehaviour
{
    Image image;

    // Start is called before the first frame update
    void Awake()
    {
        image = GetComponent<Image>();
        Debug.Log("SetImageSprite Awake image " + image);
    }

    private void Start()
    {
        image.sprite = SpriteCache.Instance.GetSprite(name);
        Debug.Log("SetImageSprite Awake Sprite for " + name + " sprite " + SpriteCache.Instance.GetSprite(name));
    }
}
