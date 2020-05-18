using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCache : MonoBehaviour
{
    [SerializeField] SpriteContainer[] abilitySprites;

    private Dictionary<string, Sprite> spriteMap;

    //private Sprite sprite;

    public static SpriteCache Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        spriteMap = new Dictionary<string, Sprite>();

        foreach (var spriteContainer in abilitySprites)
        {
            spriteMap.Add(spriteContainer.Name, spriteContainer.sprite);
        }

        abilitySprites = null;
    }

    public Sprite GetSprite(string abilityName)
    {
        abilityName = Utils.Instance.RemoveWhiteSpace(abilityName);

        Sprite sprite;

        if (!spriteMap.TryGetValue(abilityName, out sprite))
        {
            Debug.LogError("AbilitySpriteCache GetSprite No sprite for ability name" + abilityName);
        }

        return sprite;
    }
}
