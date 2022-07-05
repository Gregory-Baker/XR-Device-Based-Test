using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipVizHandler : MonoBehaviour
{
    [Header("Sprite Renderers")]
    public SpriteRenderer topRenderer;
    public SpriteRenderer rightRenderer;
    public SpriteRenderer bottomRenderer;
    public SpriteRenderer leftRenderer;
    public SpriteRenderer centreRenderer;

    [Header("Sprites")]
    public Sprite greenArrow;
    public Sprite blueArrow;
    public Sprite redArrow;
    public Sprite yellowArrow;
    public Sprite switchToUpDown;
    public Sprite switchToLeftRight;


    public void SetArrowTooltips(ChangeArmTargetControl.ControlMode controlMode)
    {
        switch (controlMode)
        {
            case ChangeArmTargetControl.ControlMode.SpecialFunctions:
                topRenderer.sprite = greenArrow;
                bottomRenderer.sprite = greenArrow;
                leftRenderer.sprite = yellowArrow;
                rightRenderer.sprite = yellowArrow;
                centreRenderer.sprite = switchToLeftRight;
                break;
            case ChangeArmTargetControl.ControlMode.TargetPositionControl:
                topRenderer.sprite = redArrow;
                bottomRenderer.sprite = redArrow;
                leftRenderer.sprite = blueArrow;
                rightRenderer.sprite = blueArrow;
                centreRenderer.sprite = switchToUpDown;
                break;
        }
    }

}
