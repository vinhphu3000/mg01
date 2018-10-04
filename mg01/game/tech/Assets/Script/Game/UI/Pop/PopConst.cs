/* ==============================================================================
 * PopConst
 * @author jr.zeng
 * 2016/9/27 16:07:56
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PopConst
{
    
    
}


/// <summary>
/// 窗口id
/// </summary>
partial class POP_ID
{
    
    static public string TEST_KUI_3
        = "TestKUIPop3";
    
    //加载条
    static public string LOADING_1 
        = "LoadingView1";
    //错误报告
    static public string ERROR_REPORT 
        = "PopErrorReport";

    static public string TEST_POP_4
        = "TestPop4";

    //窗口预制注册
    public static Dictionary<string, string> pop2prefeb = new Dictionary<string, string>()
        {
        
            {TEST_KUI_3,        "Canvas_Test3"},
            {TEST_POP_4,        "Canvas_TestPop4"},
            
            //加载条
            {LOADING_1,         "Canvas_Loading"},
            {ERROR_REPORT,      "Canvas_ErrorReport"},
        };

}