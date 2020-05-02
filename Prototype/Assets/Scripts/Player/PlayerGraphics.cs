using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGraphics : MonoBehaviour
{
    public GameObject grahpics;

    //public float scaleTime;
    //public LeanTweenType easeType;

    //SpriteRenderer spriteRenderer;

    //Vector2 zeroScale;
    //Vector2 graphicsScale;

    //bool enableHasBeenCalled;
    
    // Start is called before the first frame update
    void Awake()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        //zeroScale = Vector2.zero;
        //graphicsScale = transform.localScale;
    }

    public void SetColor(Color color)
    {
        //spriteRenderer.color = color;
    }

    // Update is called once per frame
    public void Enable()
    {
        Debug.Log("PlayerGraphics Enable");
        //spriteRenderer.enabled = true;
        //enableHasBeenCalled = true;
        //LeanTween.scale(gameObject, graphicsScale, scaleTime).setEase(easeType);
        grahpics.SetActive(true);
    }

    public void Disable()
    {
        Debug.Log("PlayerGraphics Disable");
        //enableHasBeenCalled = false;
        //LeanTween.scale(gameObject, zeroScale, scaleTime).setEase(easeType).setOnComplete(DisableGraphics);
        grahpics.SetActive(false);
    }

    void DisableGraphics()
    {
        //Debug.Log("PlayerGraphics DisableGraphics");
        //if (!enableHasBeenCalled)
        //{
        //    Debug.Log("PlayerGraphics DisableGraphics !enableHasBeenCalled");
        //    spriteRenderer.enabled = false;
        //}
    }


}
