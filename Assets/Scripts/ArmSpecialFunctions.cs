using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmSpecialFunctions : MonoBehaviour
{
    public RadialMenu radialMenu;

    public SpriteRenderer[] spriteRenderers;

    public Sprite[] targetPositionSprites;
    public Sprite[] specialFunctionSprites;


    public void ToggleIcons (ChangeArmTargetControl.ControlMode controlMode)
    {
        switch (controlMode)
        {
            case ChangeArmTargetControl.ControlMode.SpecialFunctions:
                ApplySpecialIcons();
                radialMenu.buttonsActive = true;
                break;
            case ChangeArmTargetControl.ControlMode.TargetPositionControl:
                ApplyTargetPositionIcons();
                radialMenu.buttonsActive = false;
                break;
        }
    }

    public void ApplySpecialIcons()
    {
        int i = 0;
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.sprite = specialFunctionSprites[i];
            i++;
        }
    }

    public void ApplyTargetPositionIcons()
    {
        int i = 0;
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.sprite = targetPositionSprites[i];
            i++;
        }
    }
}
