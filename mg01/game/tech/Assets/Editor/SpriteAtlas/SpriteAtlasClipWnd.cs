/* ==============================================================================
 * 图集切片器
 * @author jr.zeng
 * 2017/11/7 10:45:08
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

using LitJson;

using mg.org;

namespace Edit
{

    public class SpriteAtlasClipWnd : EditorWindow
    {

        

        string m_clipType = null;
        int m_typeIndex = 0;

        //默认切片尺寸
        string m_clipW_df = "80";
        string m_clipH_df = "80";

        string m_input_w = "80";
        string m_input_h = "80";

        Texture2D m_texture = null;

        [MenuItem("Assets/生成图集/自定义图集切片", true)]
        public static bool CanOpenWindow2()
        {
            Object obj = Selection.activeObject;
            if (obj == null || !(obj is Texture))
                return false;
            string path = AssetDatabase.GetAssetPath(obj);
            return SpriteAtlasWnd.IsInAtlasPath(path);
        }

        [MenuItem("Assets/生成图集/自定义图集切片", false, 3)]
        public static void openWindow2() { openWindow(); }


        [MenuItem("工具/生成图集/自定义图集切片", false, 5)]
        public static void openWindow()
        {
            var window = EditorWindow.GetWindow<SpriteAtlasClipWnd>();
            window.texture = (Selection.activeObject is Texture2D) ? Selection.activeObject as Texture2D : null;
        }


        void OnGUI()
        {
            //try
            //{
                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Label("图集文件：", GUILayout.Width(70));
                texture = EditorGUILayout.ObjectField(texture, typeof(Texture2D), false, GUILayout.Width(200)) as Texture2D;
                GUILayout.EndHorizontal();

                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Label("选择尺寸类型：", GUILayout.Width(100));
                m_typeIndex = EditorGUILayout.Popup(m_typeIndex, SpriteAtlasClipUtility.typeList, GUILayout.Width(120));
                clipType = SpriteAtlasClipUtility.typeList[m_typeIndex];
                GUILayout.EndHorizontal();
                
                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Label("请输入切片尺寸：", GUILayout.Width(110));
                GUILayout.EndHorizontal();

                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Label("width：", GUILayout.Width(70));
                m_input_w = EditorGUILayout.TextField(m_input_w, GUILayout.Width(80));
                GUILayout.EndHorizontal();

                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Label("height：", GUILayout.Width(70));
                m_input_h = EditorGUILayout.TextField(m_input_h, GUILayout.Width(80));
                GUILayout.EndHorizontal();

                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("生成切片", GUILayout.Width(80)))
                {
                    if (texture == null)
                    {
                        Debug.Log("没有选择图集文件");
                    }
                    else
                    {
                        //Debug.Log("aaa");
                        //Debug.Log(select_index);
                        //Debug.Log(_typeExts[select_index]);
                        //Debug.Log(_typeNames[select_index]);

                        float width = float.Parse(m_input_w);
                        float height = float.Parse(m_input_h);
                        string path = AssetDatabase.GetAssetPath(texture);
                        SpriteAtlasClipUtility.ClipAtlas(path, width, height, clipType);

                        SpriteAtlasWnd.GenAsset_OneFile(path);   //重新生成图集Asset

                        Debug.Log("切片完成：" + path + " --> " + width + ", " + height + ", " + clipType);
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(15);
                GUILayout.BeginVertical();
                GUILayout.Label("手动修改切片后一定要记得保存记录！", GUILayout.ExpandHeight(false));
                if (GUILayout.Button("保存修改记录", GUILayout.Width(80)))
                {
                    if (texture == null)
                    {
                        Debug.Log("没有选择图集文件");
                    }
                    else
                    {
                        string path = AssetDatabase.GetAssetPath(texture);
                        Debug.Log("开始保存记录：" + path);
                        SpriteAtlasClipUtility.SaveJsonData(path);
                        TypeReadJson();

                    }
                }
                GUILayout.EndVertical();
            //}
            //catch (System.Exception e)
            //{
            //    Debug.LogException(e);
            //}
        }


        void TypeReadJson()
        {
            if (m_texture != null && m_clipType != null)
            {
                //图集改变时,尝试获取json里的尺寸
                string path = AssetDatabase.GetAssetPath(m_texture);
                JsonData data = SpriteAtlasClipUtility.GetClipJsonDataTypical(path, m_clipType);
                if (data != null)
                {
                    JsonData rect = data["rect"];
                    int w = (int)rect["width"];
                    int h = (int)rect["height"];
                    m_input_w = w.ToString();
                    m_input_h = h.ToString();
                    return;
                }
            }

            m_input_w = m_clipW_df;
            m_input_h = m_clipH_df;

        }

        public string clipType
        {
            get { return m_clipType; }
            set
            {
                if (m_clipType == value)
                    return;
                m_clipType = value;
                TypeReadJson();
            }
        }

        public Texture2D texture
        {
            get { return m_texture; }
            set
            {
                if (m_texture == value)
                    return;

                string path = AssetDatabase.GetAssetPath(value);
                if (!SpriteAtlasWnd.IsInAtlasPath(path))
                {
                    Log.Debug(string.Format("图片不在 {0} 内", SpriteAtlasWnd.RAW_PATH_ATLAS) );                    
                    return;
                }

                m_texture = value;
                TypeReadJson();
            }
        }


    }





}

