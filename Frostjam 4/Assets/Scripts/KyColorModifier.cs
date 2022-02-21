using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "ColorModifier", menuName = "ScriptableObjects/Color Modifier", order = 0)]
[Serializable]
public class KyColorModifier : ScriptableObject
{
    public enum ModificationMajorType
    {
        Float, Color, None
    }

    private enum ModificationType
    {
        ColorAdd, Blend, SaturationAdd, LuminosityAdd, ColorValueAdd, FloatAdd, FloatReplace
    }
    
    
    public ModificationMajorType ModMajorType 
    {
        get
        {
            if (ColorModTypes.Contains(m_ModType)) return ModificationMajorType.Color;
            if (FloatModTypes.Contains(m_ModType)) return ModificationMajorType.Float;
            return ModificationMajorType.None;
        }
    }
    
    [SerializeField] private ModificationType m_ModType;
    [Tooltip("Used in ColorAdd, Blend")]
    [SerializeField] private Color m_ColorVal;
    [Tooltip("Used in SaturationAdd, ColorValueAdd, FloatAdd, FloatReplace")]
    [SerializeField] private float m_FloatVal;
    

    
    private ModificationType[] ColorModTypes  = new[]
    {
        ModificationType.ColorAdd, 
        ModificationType.Blend,
        ModificationType.SaturationAdd,
        ModificationType.ColorValueAdd,
    };
    
    private ModificationType[] FloatModTypes  = new[]
    {
        ModificationType.FloatAdd,
        ModificationType.FloatReplace,
    };
    
    public static Color Tint(Color color, float tintAmount)
    {
        if (color.r + color.g + color.b > 1.5f)
        {
            color.r -= tintAmount;
            color.g -= tintAmount;
            color.b -= tintAmount;
        }
        else
        {
            color.r += tintAmount;
            color.g += tintAmount;
            color.b += tintAmount;
        }

        return color;
    }
    
    public float Apply(float floatVal)
    {
        if (ModMajorType != ModificationMajorType.Float) return floatVal;

        switch (m_ModType)
        {
            // Float things
            case ModificationType.FloatAdd:
                floatVal += m_FloatVal;
                break;
            case ModificationType.FloatReplace:
                floatVal = m_FloatVal;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return floatVal;
    }


    public Color Apply(Color color)
    {
        if (ModMajorType != ModificationMajorType.Color) return color;
        
        Vector3 colorHSV;

        switch (m_ModType)
        {
            // Color things
            case ModificationType.ColorAdd:
                color += m_ColorVal;
                break;
            case ModificationType.Blend:
                color = Color.Lerp(color, m_ColorVal, m_FloatVal);
                break;
            case ModificationType.SaturationAdd:
                Color.RGBToHSV(color, out colorHSV.x, out colorHSV.y, out colorHSV.z);
                colorHSV.y += m_FloatVal;
                color = Color.HSVToRGB(colorHSV.x, colorHSV.y, colorHSV.z);
                break;
            case ModificationType.LuminosityAdd:
                throw new NotImplementedException();
                break;
            case ModificationType.ColorValueAdd:
                Color.RGBToHSV(color, out colorHSV.x, out colorHSV.y, out colorHSV.z);
                colorHSV.z += m_FloatVal;
                color = Color.HSVToRGB(colorHSV.x, colorHSV.y, colorHSV.z);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return color;
    }
}

