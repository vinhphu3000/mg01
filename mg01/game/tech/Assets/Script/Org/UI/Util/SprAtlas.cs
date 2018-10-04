/* ==============================================================================
 * 纹理集
 * @author jr.zeng
 * 2017/10/31 19:22:00
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;


using mg.org;

public class SprAtlas : ScriptableObject, IAsset
{
    public string file_name;

    public Sprite[] sprites;
    public Dictionary<string, Sprite> name2sprite = new Dictionary<string, Sprite>();

    public Texture texture;
    
    public void OnLoaded()
    {

    }

    public void Unload()
    {
        //name2sprite.Clear();

        for (var i = 0; i < sprites.Length; ++i)
        {
            Resources.UnloadAsset(sprites[i]);
        }

        if (texture != null)
            Resources.UnloadAsset(texture);

        Resources.UnloadAsset(this);
    }
    

    public Sprite GetSprite(string name_)
    {
        if (name2sprite.Count == 0 )
        {
            foreach (var kvp in sprites)
                name2sprite[kvp.name] = kvp;
        }

        Sprite sprite;
        if (name2sprite.TryGetValue(name_, out sprite))
        {
            return sprite;
        }
        return null;
    }

}