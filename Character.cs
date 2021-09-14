using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string hat;
    public string weapon;
    public string wallColor;
    public string floorColor;
    public string eyeball;
    public string beak;
    public string eyebrow;
    public string accesory;
    public string bodyColor;
    public string visorColor;
    public string bellyColor;
    public string appliedFullScreenShader;

    public void setAttributeReference(string attribute, string name)
    {
        switch (attribute)
        {
            case "hat":
                {
                    hat = name;
                } break;
            case "weapon":
                {
                    weapon = name;
                }
                break;
            case "eyeball":
                {
                    eyeball = name;
                }
                break;
            case "beak":
                {
                    beak = name;
                }
                break;
            case "eyebrow":
                {
                    eyebrow = name;
                }
                break;
            case "accessory":
                {
                    accesory = name;
                }
                break;
            case "wallcolor":
                {
                    wallColor = name;
                }
                break;
            case "floorcolor":
                {
                    floorColor = name;
                }
                break;
            case "bodycolor":
                {
                    bodyColor = name;
                }
                break;
            case "visorcolor":
                {
                    visorColor = name;
                }
                break;
            case "bellycolor":
                {
                    bellyColor = name;
                }
                break;
            case "appliedFullScreenShader":
                {
                    appliedFullScreenShader = name;
                }
                break;

        }
    }
}
