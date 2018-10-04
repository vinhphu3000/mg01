/* ==============================================================================
 * SecurePrefs
 * @author jr.zeng
 * 2016/8/24 11:48:40
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class SecurePrefs
{
    public SecurePrefs()
    {

    }



    //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

    public static bool HasKey(string key_)
    {
        return PlayerPrefs.HasKey(key_);
    }

    /// <summary>
    /// 写入所有修改参数到硬盘
    /// By default, Unity writes preferences to disk on Application Quit.<br/>
    /// In case when the game crashes or otherwise prematurely exits, you might want to write the preferences at sensible 'checkpoints' in your game.<br/>
    /// This function will write to disk potentially causing a small hiccup, therefore it is not recommended to call during actual game play.
    /// </summary>
    public static void Save()
    {

        PlayerPrefs.Save();
    }

    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }


    public static void DeleteKey(string key_)
    {
        PlayerPrefs.DeleteKey(key_);
    }

    //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

    //-------∽-★-∽------∽-★-∽--------∽-★-∽SETTER∽-★-∽--------∽-★-∽------∽-★-∽--------//

    public static void SetString(string key_, string value_)
    {

        /// 一个游戏存档文件对应一个web播放器URL并且文件大小被限制为1MB
        /// 如果超出这个限制，SetInt、SetFloat和SetString将不会存储值并抛出一个PlayerPrefsException
#if UNITY_EDITOR

#endif

#if UNITY_WEBPLAYER
        
        //    try
        //    {
        //PlayerPrefs.SetString(key_, value_);
        //    }
        //    catch (SecurePrefsException exception)
        //    {
        //        Debug.LogException(exception);
        //    }
#else
        PlayerPrefs.SetString(key_, value_);
#endif

    }

    public static void SetInt(string key_, int value_)
    {
        PlayerPrefs.SetInt(key_, value_);
    }

    public static void SetFloat(string key_, float value_)
    {
        PlayerPrefs.SetFloat(key_, value_);
    }


    //-------∽-★-∽------∽-★-∽--------∽-★-∽GETTER∽-★-∽--------∽-★-∽------∽-★-∽--------//

    public static float GetFloat(string key_)
    {
        return PlayerPrefs.GetFloat(key_);
    }

    public static float GetFloat(string key_, float defaultValue_)
    {
        return PlayerPrefs.GetFloat(key_, defaultValue_);
    }

    public static int GetInt(string key_)
    {
        return PlayerPrefs.GetInt(key_);
    }

    public static int GetInt(string key_, int defaultValue_)
    {
        return PlayerPrefs.GetInt(key_, defaultValue_);
    }


    public static string GetString(string key_)
    {
        return PlayerPrefs.GetString(key_);
    }

    public static string GetString(string key_, string defaultValue_)
    {
        return PlayerPrefs.GetString(key_, defaultValue_);
    }



}
