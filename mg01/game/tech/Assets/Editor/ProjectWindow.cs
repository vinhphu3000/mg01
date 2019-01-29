/* ==============================================================================
 * 项目窗口
 * @author jr.zeng
 * 2019/1/5 15:25:54
 * ==============================================================================*/

using UnityEngine;
using UnityEditor;
using System.Threading;
using System.IO;


public class ProjectWindow : EditorWindow
{

    [MenuItem("工具/项目路径")]
    public static void openWindow()
    {
        EditorWindow.GetWindow<ProjectWindow>();
    }




    private void OnGUI()
    {
        try
        {
            string dataPath = Application.dataPath;
            GUILayout.BeginVertical();
            GUILayout.Label("工程路径：" + dataPath, GUILayout.Width(420));

            //GUILayout.BeginHorizontal();
            //GUILayout.Label("标签：", GUILayout.Width(40));
            //input = EditorGUILayout.TextField(input, GUILayout.Width(380));
            //GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("快速打开：", GUILayout.Width(60));
            if (GUILayout.Button("根目录", GUILayout.Width(50)))
            {
                string path_root = dataPath.Replace("/game/tech/Assets", "");
                OpenDirectory(path_root);
            }
            if (GUILayout.Button("配表", GUILayout.Width(40)))
            {
                string path_game = dataPath.Replace("/tech/Assets", "");
                string path_xls = path_game + "/config";
                OpenDirectory(path_xls);
            }
            if (GUILayout.Button("LuaScript", GUILayout.Width(70)))
            {
                string path_lua = dataPath + "/Resources/LuaScript/src";
                OpenDirectory(path_lua);
            }
            if (GUILayout.Button("PSD", GUILayout.Width(40)))
            {
                string path_psd = dataPath + "/PSD/Style";
                OpenDirectory(path_psd);
            }

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }

    public static void OpenDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Debug.Log("No Directory: " + path);
            return;
        }

        // 新开线程防止锁死
        Thread newThread = new Thread(new ParameterizedThreadStart(CmdOpenDirectory));
        newThread.Start(path);
    }


    private static void CmdOpenDirectory(object obj)
    {
        System.Diagnostics.Process p = new System.Diagnostics.Process();
        p.StartInfo.FileName = "cmd.exe";
        p.StartInfo.Arguments = "/c start " + obj.ToString();
        Debug.Log(p.StartInfo.Arguments);
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardInput = true;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardError = true;
        p.StartInfo.CreateNoWindow = true;
        p.Start();

        p.WaitForExit();
        p.Close();
    }


}