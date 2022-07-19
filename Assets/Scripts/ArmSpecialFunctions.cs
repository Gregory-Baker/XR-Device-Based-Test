using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmSpecialFunctions : MonoBehaviour
{
    public RadialMenu radialMenu;

    public SpriteRenderer[] spriteRenderers;

    public Sprite[] targetPositionSprites;
    public Sprite[] specialFunctionSprites;
    public Sprite[] yesNoSprites;

    public void ToggleIcons (ChangeArmTargetControl.ControlMode controlMode)
    {
        switch (controlMode)
        {
            case ChangeArmTargetControl.ControlMode.SpecialFunctions:
                ApplySpecialIcons();
                ActivateFunctions();
                break;
            case ChangeArmTargetControl.ControlMode.TargetPositionControl:
                ApplyTargetPositionIcons();
                DeactivateFunctions();
                break;
            case ChangeArmTargetControl.ControlMode.ConfirmActionSuccess:
                ApplyYesNoIcons();
                DeactivateFunctions();
                break;
        }
    }

    public void ActivateFunctions()
    {
        radialMenu.buttonsActive = true;
    }

    public void DeactivateFunctions()
    {
        radialMenu.buttonsActive = false;
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

    private void ApplyYesNoIcons()
    {
        int i = 0;
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.sprite = yesNoSprites[i];
            i++;
        }
    }
}
