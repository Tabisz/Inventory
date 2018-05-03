using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler  {

    private Stack<Item> items;
    public Text stackTxt;

    public Sprite slotEmpty;
    public Sprite slotHiglight;

    public bool IsEmpty
    {
        get { return items.Count == 0; }
    }

    public Item CurrentItem
    {
        get { return items.Peek(); }
    }
    public bool IsAvailable
    {
        get { return CurrentItem.maxSize > items.Count; }
    }

    public Stack<Item> Items
    {
        get { return items;}
        set { items = value; }
    }

    // Use this for initialization
    void Start () {
        items = new Stack<Item>();
        RectTransform slotRect = GetComponent<RectTransform>();
        RectTransform txtRect = stackTxt.GetComponent<RectTransform>();

        int txtScaleFactor = (int)(slotRect.sizeDelta.x * 0.60f);
        stackTxt.resizeTextMinSize = txtScaleFactor;
        stackTxt.resizeTextMaxSize = txtScaleFactor;
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void AddItem(Item item)
    {
        items.Push(item);
        if (items.Count > 1)
            stackTxt.text = items.Count.ToString();

        ChangeSprite(item.spriteNeutral, item.spriteHighlighted);
    }

    public void AddItems(Stack<Item> items)
    {
        this.items = new Stack<Item>(items);

        stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

        ChangeSprite(CurrentItem.spriteNeutral, CurrentItem.spriteHighlighted);
    }

    private void ChangeSprite(Sprite neutral, Sprite highlight)
    {
        GetComponent<Image>().sprite = neutral;
        SpriteState st = new SpriteState();
        st.highlightedSprite = highlight;

        GetComponent<Button>().spriteState = st;
    }

    private void UseItem()
    {
        if(!IsEmpty)
        {
            items.Pop().Use();

            stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;    //dwa razy sprawdza czy slot jest pusty, chyba da sie to lepiej zrobic

            if(IsEmpty)
            {
                ChangeSprite(slotEmpty, slotHiglight);

                Inventory.EmptySlots++;
            }
        }
    }

    public void ClearSlot()
    {
        items.Clear();
        ChangeSprite(slotEmpty, slotHiglight);
        stackTxt.text = string.Empty;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right && !GameObject.Find("Hover"))
        {
            UseItem();
        }
    }
}
