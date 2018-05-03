using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {


    public Canvas canvas;

    private float hoverYOffset;

    private RectTransform inventoryRect;

    private float inventoryWidth, inventoryHeight;

    public int slots;

    public int rows;

    public float slotPaddingLeft, slotPaddingTop;

    public float slotSize;

    public EventSystem eventSystem;

    public GameObject slotPrefab;

    private static Slot from, to;

    private List<GameObject> allSlots;

    public GameObject iconPrefab;

    private static GameObject hoverObject;

    private static int emptySlots;

    public static int EmptySlots            //po co to jak mozna zmienic private na public?
    {
        get {return emptySlots;}
        set {emptySlots = value;}
    }

    public CanvasGroup canvasGroup;

    private bool fadingIn;

    private bool fadingOut;

    public float fadingTime;

    void Start()
    {
        CreateLayout();

    }


	
	// Update is called once per frame
	void Update () {

        if(Input.GetMouseButtonUp(0))
        {
            if(!eventSystem.IsPointerOverGameObject(-1)&& from != null)
            {
                from.GetComponent<Image>().color = Color.white;
                from.ClearSlot();
                Destroy(hoverObject);
                to = null;
                from = null;
                hoverObject = null;
                emptySlots++;

            }
        }
		if(hoverObject !=null)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
            position.Set(position.x, position.y - hoverYOffset);
            hoverObject.transform.position = canvas.transform.TransformPoint(position);
        }

        if(Input.GetKeyDown(KeyCode.B))
        {

            if(canvasGroup.alpha>0)
            {
                StartCoroutine("FadeOut");
            }
            else
            {


                StartCoroutine("FadeIn");
            }
        }
	}

    private void CreateLayout()
    {
        allSlots = new List<GameObject>();

        hoverYOffset = slotSize * 0.02f;

        emptySlots = slots;
        int columns = slots / rows;
        inventoryWidth = (columns) * (slotSize + slotPaddingLeft) + slotPaddingLeft;
        inventoryHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;

        inventoryRect = GetComponent<RectTransform>();

        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,inventoryWidth);
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight);

        
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                GameObject newSlot = (GameObject)Instantiate(slotPrefab);

                RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                newSlot.name = "Slot";
                newSlot.transform.SetParent(this.transform.parent);

                slotRect.localPosition = inventoryRect.localPosition + new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y));

                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * canvas.scaleFactor);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * canvas.scaleFactor);

                allSlots.Add(newSlot);
            }
        }
    }

    public bool AddItem(Item item)
    {
        if (item.maxSize == 1)
        {
            PlaceEmpty(item);
            return true;
        }
        else
        {
            foreach (GameObject slot in allSlots)
            {
                Slot tmp = slot.GetComponent<Slot>();
                if (!tmp.IsEmpty)
                {
                    if(tmp.CurrentItem.type == item.type && tmp.IsAvailable)
                    {
                        tmp.AddItem(item);
                        return true;
                    }
                }
            }
        }
        if (emptySlots > 0)
        {
            PlaceEmpty(item);
        }
        return false;
    }

    private bool PlaceEmpty(Item item)
    {
        if(emptySlots>0)
        {
            foreach (GameObject slot in allSlots)
            {
                Slot tmp = slot.GetComponent<Slot>();
                if(tmp.IsEmpty)
                {
                    tmp.AddItem(item);
                    emptySlots--;
                    return true;
                }
            }
        }
        return false;
    }

    public void MoveItem(GameObject clicked)
    {
        if(from == null)
        {
            if(!clicked.GetComponent<Slot>().IsEmpty)
            {
                from = clicked.GetComponent<Slot>();
                from.GetComponent<Image>().color = Color.gray;

                hoverObject =Instantiate(iconPrefab);
                hoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
                hoverObject.name = "Hover";

                RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>();
                RectTransform clickedTransform = clicked.GetComponent<RectTransform>();

                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);

                hoverObject.transform.SetParent(GameObject.Find("Canvas").transform, true);
                hoverObject.transform.localScale = from.gameObject.transform.localScale;
            }
        }
        else if(to == null)
        {
            to = clicked.GetComponent<Slot>();
            Destroy(hoverObject);
        }
        if(to != null && from !=null)
        {
            Stack<Item> tmpTo = new Stack<Item>(to.Items);
            to.AddItems(from.Items);

            if (tmpTo.Count == 0)
            {
                from.ClearSlot();
            }
            else
            {
                from.AddItems(tmpTo);
            }
            from.GetComponent<Image>().color = Color.white;
            to = null;
            from = null;
            hoverObject = null;
        }
    }


    private IEnumerator FadeOut()
    {
        Debug.Log("ddd");
        if (!fadingOut)
        {
            fadingOut = true;
            fadingIn = false;
            StopCoroutine("FadeIn");

            float startAlpha = canvasGroup.alpha;
            float progress = 0.0f;
            float rate = 1.0f / fadingTime;

            while (progress<1.0)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, progress);
                progress += rate * Time.deltaTime;

                yield return null;
            }

            canvasGroup.alpha = 0;
            fadingOut = false;

        }
    }
    private IEnumerator FadeIn()
    {
        if (!fadingIn)
        {
            fadingIn = true;
            fadingOut = false;
            StopCoroutine("FadeOut");

            float startAlpha = canvasGroup.alpha;
            float progress = 0.0f;
            float rate = 1.0f / fadingTime;

            while (progress < 1.0)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 1, progress);
                progress += rate * Time.deltaTime;

                yield return null;
            }

            canvasGroup.alpha = 1;
            fadingIn = false;

        }
    }
}

