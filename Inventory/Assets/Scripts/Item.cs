using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType { MANA,HEALTH};
public class Item : MonoBehaviour {
    public ItemType type;
    public Sprite spriteNeutral;

    public Sprite spriteHighlighted;

    public int maxSize;


        public void Use()
    {
        switch (type)
        {
            case ItemType.MANA:
                Debug.Log("I just a mana potion");
                break;
            case ItemType.HEALTH:
                Debug.Log("I just a health potion");
                break;
        }
    }
}
