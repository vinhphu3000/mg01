/* ==============================================================================
 * 主入口
 * @author jr.zeng
 * 2016/6/22 15:05:48
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using UnityEngine;

using mg.org;

public class MainEntry : CCModule
{

    static MainEntry __me;

    public MainEntry()
    {
        __me = this;
    }

    public static MainEntry Me
    {
        get { return __me; }
    }
    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="params_">
    /// [0] 游戏GameObj根 
    /// </param>
    override protected void __Setup(params object[] params_)
    {
        base.__Setup();

        GameObject root = params_[0] as GameObject;

        Log.Info("MainEntry Setup", this);
        
        CCApp.resMgr = new ResMgr();
        CCApp.soundMgr = new SoundMgr(root);
        CCApp.Setup(root);
        
        LoadMgr.Setup();
    }

    override protected void __Clear()
    {

        QuitGame();
        
        LoadMgr.Clear();

        CCApp.Clear();

        Log.Info("MainEntry Clear", this);
    }

    override protected void SetupEvent()
    {


    }

    override protected void ClearEvent()
    {

    }



    //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

        

    //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

    /// <summary>
    /// 退出游戏
    /// </summary>
    public virtual void QuitGame()
    {

        Log.Info("QuitGame", this);


    }

    /// <summary>
    /// 进入游戏
    /// </summary>
    public virtual void EnterGame()
    {
        Log.Info("EnterGame", this);


        string RES_PATH_SPINE = "Assets/Resources/Spine/";

        string path = "Assets/Resources/Spine/jiehunjiemian/jiehunjiemian.atlas.txt";
        string dirname = Path.GetDirectoryName(path);
        string spineName = dirname.Replace(RES_PATH_SPINE, "");
        string changePath = RES_PATH_SPINE + spineName + "/" + spineName + "_SkeletonData.asset";    //spine的定级文件


        Log.Debug("GetDirectoryName " + changePath);


    }


}
