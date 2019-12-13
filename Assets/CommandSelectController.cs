using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CommandSelectController : MonoBehaviour
{
    public int choiceIndex = 0;
    public Vector2 activeDimention;
    public Vector2 nonActiveDimention;
    public bool ACTIVE = false;
    private RectTransform myTransform;
    public float Speed = 20f;
    public PlayerMenuChoice[] playerChoices;
    public Transform selector;
    private Vector3 selectorDestination;
    public RectTransform[] slots;
    void Start()
    {
        myTransform = GetComponent<RectTransform>();
        choiceIndex = 0;
        selectorDestination = new Vector3(selector.transform.localPosition.x, 172.3f - (28f * choiceIndex), 0f);

    }

    private void MoveCursorUp()
    {
        choiceIndex--;
        if (choiceIndex < 0)
        {
            choiceIndex = 4;
        }
        selectorDestination = new Vector3(selector.transform.localPosition.x, 172.3f - 28f * choiceIndex, 0f);

    }
    private void MoveCursorDown()
    {

        choiceIndex++;
        if (choiceIndex > 4)
        {
            choiceIndex = 0;
        }
        selectorDestination = new Vector3(selector.transform.localPosition.x, 172.3f - 28f * choiceIndex, 0f);
    }
    void Update()
    {
        if (ACTIVE)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                MoveCursorUp();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                MoveCursorDown();
            }
            myTransform.sizeDelta = Vector2.MoveTowards(myTransform.sizeDelta, activeDimention,  Speed);
            selector.transform.localPosition = Vector3.MoveTowards(selector.transform.localPosition, selectorDestination, 10f);
        }
        else
        {
            myTransform.sizeDelta = Vector2.MoveTowards(myTransform.sizeDelta, nonActiveDimention, Speed);

        }
    }
    public TypeOfCommand GetSelection()
    {
        ACTIVE = !ACTIVE;
        return playerChoices[choiceIndex].typeOfCommand;
    }
}
