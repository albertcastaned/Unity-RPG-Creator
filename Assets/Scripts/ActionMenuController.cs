using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class ActionMenuController : MonoBehaviour
{
    public Vector2 activeDimention;
    public Vector2 nonActiveDimention;
    private RectTransform myTransform;
    public float Speed = 20f;
    private int choiceIndex;

    public bool ACTIVE ;
    public List<string> actionNames;

    public Transform selector;
    private Vector3 selectorDestination;

    public GameObject SlotPrefab;

    public GameObject SlotParent;

    private Vector2 originalRectSize;

    private Vector3 velocity = Vector3.zero;

    private RectTransform SlotParentTransform;
    
    private readonly List<MenuSlotController> slots = new List<MenuSlotController>();

    [HideInInspector] public bool isSpecialMoves;
    void Start()
    {
        myTransform = GetComponent<RectTransform>();
        originalRectSize = SlotParent.GetComponent<RectTransform>().sizeDelta;
        SlotParentTransform = SlotParent.GetComponent<RectTransform>();
        GenerateTextSlots();
        UpdateSelectorDestination();
        DeactivateMenu();
    }

    private IEnumerator UpdateSelectorDestination()
    {
        selectorDestination = new Vector3(choiceIndex % 2 == 0 ? 40f : 240f, -30f - (40 * (choiceIndex / 2)), 0f);

        while(selector.transform.localPosition != selectorDestination)
        {
            selector.transform.localPosition = Vector3.SmoothDamp(selector.transform.localPosition, selectorDestination, ref velocity, 0.08f);
            yield return null;
        }
    }

    public int GetSelection()
    {
        return choiceIndex;
    }
    private void MoveCursorUp()
    {
        slots[choiceIndex].Deactivate();
        choiceIndex -= 2;
        if (choiceIndex < 0)
        {
            choiceIndex = 0;
        }
        slots[choiceIndex].Activate();

        StopCoroutine(nameof(UpdateSelectorDestination));

        StartCoroutine(nameof(UpdateSelectorDestination));
    }
    private void MoveCursorDown()
    {
        slots[choiceIndex].Deactivate();
        choiceIndex += 2;
        if (choiceIndex >= actionNames.Count)
        {
            choiceIndex = actionNames.Count - 1;
        }
        slots[choiceIndex].Activate();

        StopCoroutine(nameof(UpdateSelectorDestination));

        StartCoroutine(nameof(UpdateSelectorDestination));
    }

    private void MoveCursorLeft()
    {

        slots[choiceIndex].Deactivate();
        choiceIndex--;
        if (choiceIndex < 0)
        {
            choiceIndex = 0;
        }
        slots[choiceIndex].Activate();

        StopCoroutine(nameof(UpdateSelectorDestination));

        StartCoroutine(nameof(UpdateSelectorDestination));
    }

    private void MoveCursorRight()
    {
        slots[choiceIndex].Deactivate();
        choiceIndex++;
        if (choiceIndex >= actionNames.Count)
        {
            choiceIndex = actionNames.Count - 1;
        }
        
        slots[choiceIndex].Activate();
        StopCoroutine(nameof(UpdateSelectorDestination));

        StartCoroutine(nameof(UpdateSelectorDestination));
    }


    private void GenerateTextSlots()
    {
        slots.Clear();

        for (int i = 0; i < actionNames.Count; i++)
        {
            GameObject slot = Instantiate(SlotPrefab, SlotParent.transform);
            slot.transform.localScale = new Vector3(1f, 1f, 1f);

            slot.transform.localPosition = new Vector3((i % 2 == 0) ? 140 : 340, -30 - (40 * (i / 2)), 0);
            TextMeshProUGUI textSlot = slot.GetComponent<TextMeshProUGUI>();
            textSlot.text = actionNames[i];

            if (isSpecialMoves && !BattleController.instance.CanUseSpecialMove(i))
            {
                MenuSlotController slotController = slot.GetComponent<MenuSlotController>();
                slotController.SetToDisabled();
            }


            slots.Add(slot.GetComponent<MenuSlotController>());
            
        }

        SlotParentTransform.sizeDelta = new Vector2(originalRectSize.x, 20 + 40 * (actionNames.Count / 2));

    }
    void DeletePreviousTextSlots()
    {

        foreach (Transform child in SlotParent.transform)
        {
            if (child.CompareTag("MenuSlot"))
                Destroy(child.gameObject);
        }
    }
    public void SetActionNames(List<string> newActionNames)
    {
        actionNames = newActionNames;
        DeletePreviousTextSlots();
        GenerateTextSlots();

        if (choiceIndex >= actionNames.Count)
            choiceIndex = actionNames.Count - 1;
        StopCoroutine(nameof(UpdateSelectorDestination));
        StartCoroutine(nameof(UpdateSelectorDestination));

    }

    public void ActivateMenu()
    {
        ACTIVE = true;
        slots[choiceIndex].Activate();

        StopCoroutine(nameof(UpdateSizeActive));
        StopCoroutine(nameof(UpdateSizeUnactive));

        StartCoroutine(nameof(UpdateSizeActive));
    }

    public void DeactivateMenu()
    {
        ACTIVE = false;
        StopCoroutine(nameof(UpdateSizeActive));
        StopCoroutine(nameof(UpdateSizeUnactive));

        StartCoroutine(nameof(UpdateSizeUnactive));
    }

    
    private IEnumerator UpdateSizeActive()
    {
        while (myTransform.sizeDelta != activeDimention)
        {
            myTransform.sizeDelta = Vector2.MoveTowards(myTransform.sizeDelta, activeDimention, Speed);
            yield return null;

        }

    }
    private IEnumerator UpdateSizeUnactive()
    {
        while (myTransform.sizeDelta != nonActiveDimention)
        {
            myTransform.sizeDelta = Vector2.MoveTowards(myTransform.sizeDelta, nonActiveDimention, Speed);
            yield return null;
        }

    }
    void Update()
    {
        if (!ACTIVE) return;
        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveCursorUp();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            MoveCursorDown();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveCursorLeft();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveCursorRight();
        }

    }
}