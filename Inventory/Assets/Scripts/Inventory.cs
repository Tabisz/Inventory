using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    private RectTransform inventoryRect;

    private float inventoryWidth, inventoryHeight;

    public int slots;

    public int rows;

    public float slotPaddingLeft, slotPaddingTop;

    public float slotSize;

    public GameObject slotPrefab;

    private List<GameObject> allSlots; 

    void Start()
    {
        CreateLayout();
    }


	
	// Update is called once per frame
	void Update () {
		
	}

    private void CreateLayout()
    {
        allSlots = new List<GameObject>();
        inventoryWidth = (slots / rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft;
        inventoryHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;

        inventoryRect = GetComponent<RectTransform>();

        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,inventoryWidth);
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight);
    }
}

