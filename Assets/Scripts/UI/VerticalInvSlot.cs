using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalInvSlot : MonoBehaviour
{

    PlayerActions playerActions;
    RectTransform rectTransform;

    void Start()
    {
        playerActions = GameObject.Find("Player").GetComponent<PlayerActions>();
        playerActions.OnInvSwitch += HandleInvSwitch;
        rectTransform = GetComponent<RectTransform>();

    }

    void HandleInvSwitch()
    {
        LeanTween.moveY(rectTransform, playerActions.seedSlot * -120 + 5, 0.3f).setEaseOutCubic();
    }
}
