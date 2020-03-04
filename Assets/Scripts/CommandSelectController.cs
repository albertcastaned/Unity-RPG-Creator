using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CommandSelectController : MonoBehaviour
{
    public int choiceIndex;
    public Vector2 activeDimention;
    public Vector2 nonActiveDimention;
    public bool ACTIVE;
    private RectTransform myTransform;
    public float Speed = 20f;
    public PlayerMenuChoice[] playerChoices;
    public Transform selector;
    private Vector3 selectorDestination;
    private List<MenuSlotController> slots = new List<MenuSlotController>();
    private Vector3 velocity = Vector3.zero;

    void Awake()
    {
        myTransform = GetComponent<RectTransform>();
        choiceIndex = 0;
        selectorDestination = new Vector3(selector.transform.localPosition.x, 172.3f - (28f * choiceIndex), 0f);

        GameObject slotParent = gameObject.transform.GetChild(0).gameObject;
        foreach(RectTransform transf in slotParent.transform)
        {
            slots.Add(transf.gameObject.GetComponent<MenuSlotController>());
        }


    }

    public List<MenuSlotController> GetSlots()
    {
        return slots;
    }

    void Start()
    {
        slots[choiceIndex].Activate();

    }
    public void ActivateMenu()
    {
        ACTIVE = true;
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

    private IEnumerator MoveCursorUp()
    {

        slots[choiceIndex].Deactivate();
        choiceIndex--;
        if (choiceIndex < 0)
        {
            choiceIndex = 4;
        }
        slots[choiceIndex].Activate();

        selectorDestination = new Vector3(selector.transform.localPosition.x, 172.3f - 28f * choiceIndex, 0f);


        while (selector.transform.localPosition != selectorDestination)
        {
            selector.transform.localPosition = Vector3.SmoothDamp(selector.transform.localPosition, selectorDestination, ref velocity, 0.04f);
            yield return null;
        }


    }
    private IEnumerator MoveCursorDown()
    {

        slots[choiceIndex].Deactivate();

        choiceIndex++;
        if (choiceIndex > 4)
        {
            choiceIndex = 0;
        }
        slots[choiceIndex].Activate();

        selectorDestination = new Vector3(selector.transform.localPosition.x, 172.3f - 28f * choiceIndex, 0f);

        while (selector.transform.localPosition != selectorDestination)
        {
            selector.transform.localPosition = Vector3.SmoothDamp(selector.transform.localPosition, selectorDestination, ref velocity, 0.04f);
            yield return null;
        }

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
        if (ACTIVE)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                StopCoroutine(nameof(MoveCursorUp));
                StopCoroutine(nameof(MoveCursorDown));

                StartCoroutine(nameof(MoveCursorUp));
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                StopCoroutine(nameof(MoveCursorUp));
                StopCoroutine(nameof(MoveCursorDown));

                StartCoroutine(nameof(MoveCursorDown));
            }
        }
    }
    public TypeOfCommand GetSelection()
    {
        return playerChoices[choiceIndex].typeOfCommand;
    }
}