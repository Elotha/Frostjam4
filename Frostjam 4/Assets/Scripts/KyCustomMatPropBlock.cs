using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

[Serializable]
public class KyCustomMatPropBlock
{
    public Renderer Rend;
    [Tooltip("If there are more than one materials, which material will be edited?")]
    public int IndexInRenderer;
    public string PropName;
    public bool UseMaterialCopy = false;
    public MaterialPropertyBlock Block;
    public Material Mat;
    public List<KyColorModifier> Modifiers;
    
    public KyCustomMatPropBlock(Renderer renderer, int indexInRenderer, string propName)
    {
        Rend = renderer;
        IndexInRenderer = indexInRenderer;
        PropName = propName;
    }

    public void Initialize()
    {
        // !!! If MaterialPropertyBlock is set in inspector, this has to be called in script. !!!
        if (UseMaterialCopy)
        {
            // Mat = new Material(Rend.materials[IndexInRenderer]);
            // Rend.materials[IndexInRenderer] = Mat;
            var mats = Rend.materials;
            Mat = new Material(mats[IndexInRenderer]);
            mats[IndexInRenderer] = Mat;
            Rend.materials = mats;
        }
        else
        {
            Block = new MaterialPropertyBlock();
        }
        
        // If renderer is a sprite renderer, set the textures as well?
        // TODO; sprite things are hardcoded
        // Texture2D tex = Rend.GetComponent<SpriteRenderer>().sprite.texture;
        // if (tex != null)
        // {
        //     Rend.GetPropertyBlock(Block, IndexInRenderer);
        //     Block.SetTexture("_MainTex", tex);
        //     Rend.SetPropertyBlock(Block, IndexInRenderer);
        // }
        
    }

    public void SetValue(float value)
    {
        value = ApplyAllModifications(value);

        if (UseMaterialCopy)
        {
            Mat.SetFloat(PropName, value);
        }
        else
        {
            Rend.GetPropertyBlock(Block, IndexInRenderer);
            Block.SetFloat(PropName, value);
            Rend.SetPropertyBlock(Block, IndexInRenderer); 
        }
            
        
        float ApplyAllModifications(float value)
        {
            foreach (KyColorModifier modifier in Modifiers)
                value = modifier.Apply(value);
            return value;
        }
    }
    
    public void SetColor(Color color)
    {
        color = ApplyAllModifications(color);
        
        if (UseMaterialCopy)
        {
            Mat.SetColor(PropName, color);
        }
        else
        {
            Rend.GetPropertyBlock(Block, IndexInRenderer);
            Block.SetColor(PropName, color);
            Rend.SetPropertyBlock(Block, IndexInRenderer);
        }

        
        Color ApplyAllModifications(Color color)
        {
            foreach (KyColorModifier modifier in Modifiers)
                color = modifier.Apply(color);
            return color;
        }
    }
}
