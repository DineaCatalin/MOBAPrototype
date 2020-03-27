using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenSprite : MonoBehaviour
{
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
       // transform = GetComponent<Transform>();

        if (sr == null) return;

        transform.localScale = new Vector3(1, 1, 1);

        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        Vector3 lScale = new Vector3(worldScreenWidth / width, worldScreenHeight / height, 1);

        transform.localScale = lScale;
    }
}
