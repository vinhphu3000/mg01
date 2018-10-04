using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;


using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;
using System.Text.RegularExpressions;

namespace Edit.PSD4UGUI
{

    public class InputParam 
    {
        //是否生成图集
        public bool isGenerateAtlas;
        //高质量图集
        public bool isHighQuality;
        //生成ab(没有使用)
        public bool isBuildAssetbundle;
        //用tp打图集
        public bool isTexturePacker;
        //是否patch模式
        public bool isPatch;
    }

    public class PSD4UGUI : EditorWindow
    {

        static Color COLOR_RED = new Color(171.0f / 255.0f, 26.0f / 255.0f, 37.0f / 255.0f);
        static Color COLOR_GREEN = new Color(26.0f / 255.0f, 171.0f / 255.0f, 37.0f / 255.0f);

        const string MODE_FILE = "file";
        const string MODE_FOLDER = "folder";

        static PSD4UGUI _window;

        //菜单模式
        string _mode;
       

        string _error = string.Empty;  
        string _constructError = string.Empty;   //捕获到的系统抛错
        
        //TextAsset[] _jsonAssets;
        JsonAsset[] _jsonAssets;
        int[] _jsonIndices;

        string[] _batchNames;
        Dictionary<string, List<string>> _batch2chlidren;
        int _batchIndice;

        //json的名称菜单
        string[] _popupJsonNames;

        InputParam _inputParam;

        Object _folderObj;


        [MenuItem("PSD4UGUI/文件模式", false, 1)]    //每50个一个组
        public static void StartFileMode()
        {
            _window = EditorWindow.GetWindow<PSD4UGUI>("PSD4UGUI");
            _window._mode = MODE_FILE;
            _window.Show();
            _window.Initialize();
        }

        [MenuItem("PSD4UGUI/文件夹批量模式", false, 2)]
        public static void StartFolderMode()
        {
            _window = EditorWindow.GetWindow<PSD4UGUI>("PSD4UGUI");
            _window._mode = MODE_FOLDER;
            _window.Show();
            _window.Initialize();
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽PSD4UGUI∽-★-∽--------∽-★-∽------∽-★-∽--------//


        void OnGUI()
        {
            if (_inputParam == null)
                return;

            try
            {
                if (_mode == MODE_FILE)
                {
                    ShowFileMode();
                }
                else if (_mode == MODE_FOLDER)
                {
                    ShowFolderMode();
                }
            }
            catch (Exception e)
            {
                _constructError = e.Message;
                Debug.LogException(e);

                Worker.HandleException();   //出错处理
            }
        }


        void Initialize()
        {
            KAssetManager.Initialize();

            _jsonAssets = new JsonAsset[1];
            _jsonIndices = new int[1];
            _popupJsonNames = new string[] { "选择Json文件" };

            _constructError = string.Empty;
            _error = string.Empty;

            _inputParam = new InputParam();
           
            RefreshJsonNames();  //初始化json列表
            InitBatchSetting();
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽视图json相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        //刷新json菜单
        void RefreshJsonNames()
        {
            if (!Directory.Exists(KAssetManager.FOLDER_JSON))
                return;

            //因为json目录放在外部,不能用AssetDatabase
            //string[] guids = AssetDatabase.FindAssets("t:TextAsset", new string[] { KAssetManager.FOLDER_JSON }); 
            DirectoryInfo direction = new DirectoryInfo(KAssetManager.FOLDER_JSON);
            FileInfo[] files = direction.GetFiles("*.json", SearchOption.AllDirectories);

            _popupJsonNames = new string[files.Length + 1];
            _popupJsonNames[0] = "选择Json文件";
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo file = files[i];
                string name = mg.org.FileUtility.GetNameFromFullPath(file.Name, "");
                _popupJsonNames[i + 1] = GetMenuItemLabel(name);
            }
        }
        
        //菜单中显示的名字
        string GetMenuItemLabel(string name)
        {
            string first = name.Substring(0, 1).ToUpper();  //首个字母
            return first + "/" + name;  // -> A/Announce
        }



        //显示json按钮
        void ShowAddJsonBtn()
        {
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("添加Json文件", GUILayout.Width(80)))
            {
                AddJsonAsset();
            }

            if (GUILayout.Button("刷新Json菜单", GUILayout.Width(80)))
            {
                RefreshJsonNames();
            }

            if (GUILayout.Button("清空Json菜单", GUILayout.Width(80)))
            {
                _jsonAssets = new JsonAsset[1];
                _jsonIndices = new int[1];
            }

            if (GUILayout.Button("添加Batch所有文件", GUILayout.Width(160)))
            {
                ShowBatchAssets();
            }
            

            GUILayout.EndHorizontal();
        }

        //添加json按钮
        void AddJsonAsset()
        {
            JsonAsset[] newJsonAssets = new JsonAsset[_jsonAssets.Length + 1];  //增加一个空位
            int[] newJsonIndices = new int[_jsonIndices.Length + 1];
            for (int i = 0; i < _jsonAssets.Length; i++)
            {
                newJsonAssets[i] = _jsonAssets[i];
                newJsonIndices[i] = _jsonIndices[i];
            }
            _jsonAssets = newJsonAssets;
            _jsonIndices = newJsonIndices;
        }


        Vector2 scrollPos;
        void ShowJsonAssets()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(500), GUILayout.Height(400));
            
            if (_jsonAssets != null)
            {
                for (int i = 0; i < _jsonAssets.Length; i++)
                {
                    GUILayout.Space(5);
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Json文件：", GUILayout.Width(70));
                    JsonAsset json = _jsonAssets[i];
                    if (json == null)
                    {
                        GUI.contentColor = COLOR_RED;
                    }

                    //
                    //拖动输入框, json放在外部了,不用支持拖放
                    //TextAsset jsonAsset = EditorGUILayout.ObjectField(_jsonAssets[i], typeof(TextAsset), false, GUILayout.Width(200)) as TextAsset;
                    //int jsonIndex = EditorGUILayout.Popup(_jsonIndices[i], _jsonNames, GUILayout.Width(120));
                    //if (jsonAsset != _jsonAssets[i])
                    //{
                    //    _jsonIndices[i] = Array.IndexOf<string>(_jsonNames, GetMenuItemLabel(jsonAsset.name));
                    //    _jsonAssets[i] = jsonAsset;
                    //}
                    //else if (jsonIndex != _jsonIndices[i])
                    //{
                    //    _jsonAssets[i] = AssetDatabase.LoadAssetAtPath(KAssetManager.FOLDER_JSON + "/" + _jsonNames[jsonIndex].Substring(2) + ".json", typeof(TextAsset)) as TextAsset;
                    //    _jsonIndices[i] = jsonIndex;
                    //}

                    GUILayout.TextArea(json != null ? json.name : _popupJsonNames[0], GUILayout.Width(200));
                    int jsonIndex = EditorGUILayout.Popup(_jsonIndices[i], _popupJsonNames, GUILayout.Width(120));
                    if (jsonIndex != _jsonIndices[i])
                    {
                        if (jsonIndex == 0)
                        {
                            //删除此项
                            _jsonAssets[i] = null;
                            _jsonIndices[i] = 0;
                        }
                        else
                        {
                            string name = _popupJsonNames[jsonIndex].Substring(2);  //去掉"A/"
                            _jsonAssets[i] = KAssetManager.GetUIJson(name);
                            _jsonIndices[i] = jsonIndex;    //记录选中序号
                        }
                    }

                    GUI.contentColor = Color.white;
                    GUILayout.EndHorizontal();
                }
            }
            
            EditorGUILayout.EndScrollView();
        }



        //-------∽-★-∽------∽-★-∽--------∽-★-∽Batch相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        //初始化打包图集设置
        void InitBatchSetting()
        {

            JsonAsset jsonAsset = KAssetManager.GetJson(KAssetManager.AtlasBatchSettingPath);
            if (jsonAsset == null)
            {
                //没找到BatchSetting
                _batchNames = new string[] { "请选择图集名称" };
                _batchIndice = 0;
                return;
            }

            JsonData data = jsonAsset.GetJsonData();
            JsonData setting = data["setting"];

            _batchNames = new string[setting.Count + 1];
            _batchNames[0] = "请选择图集名称";
            _batchIndice = 0;
            _batch2chlidren = new Dictionary<string, List<string>>();

            int i = 1;
            foreach (JsonData part in setting)
            {
                string partname = part[0].ToString();
                _batchNames[i] = partname;
                _batch2chlidren[partname] = new List<string>();
                for (int j = 1; j < part.Count; j++)
                {
                    _batch2chlidren[partname].Add(part[j].ToString());
                }

                ++i;
            }
        }


        void ShowBatchSetting()
        {
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Label("BatchSetting：", GUILayout.Width(80));
            string batchName = _batchNames[_batchIndice];
            if (_batchIndice == 0)
            {
                GUI.contentColor = COLOR_RED;
            }

            GUILayout.TextArea(batchName, GUILayout.Width(200));
            int jsonIndex = EditorGUILayout.Popup(_batchIndice, _batchNames, GUILayout.Width(120));
            if (jsonIndex != _batchIndice)
            {
                _batchIndice = jsonIndex;
            }

            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();
        }

        //显示当前Batch的所有Assets
        void ShowBatchAssets()
        {
            if (_batchIndice == 0)
                return;

            string batchName = _batchNames[_batchIndice];
            List<string> batchChildren = _batch2chlidren[batchName];

            JsonAsset[] newJsonAssets = new JsonAsset[batchChildren.Count];
            int[] newJsonIndices = new int[batchChildren.Count];
            for (int i = 0; i < batchChildren.Count; i++)
            {
                JsonAsset asset = KAssetManager.GetJson(KAssetManager.FOLDER_JSON + "/" + batchChildren[i] + ".json");
                if (asset != null)
                {
                    int indexInArr = Array.IndexOf<string>(_popupJsonNames, GetMenuItemLabel(batchChildren[i]));
                    newJsonAssets[i] = asset;
                    newJsonIndices[i] = indexInArr;
                }
                else
                {
                    newJsonAssets[i] = null;
                    newJsonIndices[i] = 0;
                }
            }
            _jsonAssets = newJsonAssets;
            _jsonIndices = newJsonIndices;
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽Mode通用∽-★-∽--------∽-★-∽------∽-★-∽--------//


        //显示标题
        void ShowLabel()
        {
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            string label = _mode == MODE_FILE ? "文件模式" : "文件夹批量模式";
            GUILayout.Label(label, GUILayout.Width(110));
            GUILayout.EndHorizontal();
        }

        //有选择json时
        void ShowGreenButton()
        {
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();

            Color color = GUI.backgroundColor;
            GUI.backgroundColor = COLOR_GREEN;
            bool isClick = GUILayout.Button("开始", GUILayout.Width(400), GUILayout.Height(80));
            if (isClick == true)
            {
                Execute();
            }

            GUI.backgroundColor = color;
            GUILayout.EndHorizontal();
        }

        //没有选择json时
        void ShowRedButton()
        {
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            Color color = GUI.backgroundColor;
            GUI.backgroundColor = COLOR_RED;
            string tip = _mode == MODE_FILE ? "通过拖拽选择Json文件" : "通过拖拽选择Json文件夹";
            GUILayout.Button(tip, GUILayout.Width(400), GUILayout.Height(80));
            GUI.backgroundColor = color;
            GUILayout.EndHorizontal();
        }


        void ShowLanguageSetting()
        {
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Label("设置语言：", GUILayout.Width(70));
            int index = EditorGUILayout.Popup(LanguageSetting.GetLanguageIndex(KAssetManager.language), LanguageSetting.languages, GUILayout.Width(120));
            KAssetManager.language = LanguageSetting.GetLanguage(index);
            GUILayout.EndHorizontal();
        }


        void ShowToggles()
        {
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();

            _inputParam.isGenerateAtlas = GUILayout.Toggle(_inputParam.isGenerateAtlas, "生成图集");
            _inputParam.isHighQuality = GUILayout.Toggle(_inputParam.isHighQuality, "高质量图集");
            _inputParam.isBuildAssetbundle = GUILayout.Toggle(_inputParam.isBuildAssetbundle, "生成资源");

            GUILayout.EndHorizontal();
        }

        //显示系统抛错
        void ShowConstructError()
        {
            if (_constructError != string.Empty)
            {
                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                Color color = GUI.color;
                GUI.color = COLOR_RED;
                GUILayout.Label("构建发生错误： " + _constructError, EditorStyles.whiteLabel, GUILayout.Width(500));
                GUI.color = color;
                GUILayout.EndHorizontal();
            }
        }



        //-------∽-★-∽------∽-★-∽FileMode文件模式∽-★-∽------∽-★-∽--------//

        void ShowFileMode()
        {
            ShowLabel();
            ShowAddJsonBtn();
            ShowJsonAssets();
            ShowBatchSetting(); //*
            ShowLanguageSetting();
            ShowToggles();
            ShowFileModeStartBtn();
            ShowConstructError();
        }


        bool ValidateFileModeInput()
        {
            if (_jsonAssets == null)
                return false;            

            //for (int i = 0; i < _jsonAssets.Length; i++)
            //{
            //    if (_jsonAssets[i] != null)
            //    {
            //        string jsonPath = AssetDatabase.GetAssetPath(_jsonAssets[i]);
            //        if (jsonPath.Contains(".json") == false)
            //        {
            //            _jsonAssets[i] = null;
            //            Debug.LogError("请选择正确的Json文件： " + jsonPath);
            //        }
            //    }
            //}
            for (int i = 0; i < _jsonAssets.Length; i++)
            {
                if (_jsonAssets[i] != null)
                {
                    return true;
                }
            }
            _error = "请选择Json文件";
            return false;
        }


        void ShowFileModeStartBtn()
        {
            if (ValidateFileModeInput() == true)
            {
                //输入有效
                ShowGreenButton();
            }
            else
            {
                ShowRedButton();
            }
        }

        //-------∽-★-∽------∽-★-∽FolderMode文件夹模式(除了选中的，其他都会导出)∽-★-∽------∽-★-∽--------//
        
        void ShowFolderMode()
        {
            ShowLabel();
            ShowExclusive(); //*
            ShowAddJsonBtn();
            ShowJsonAssets();
            ShowExclusiveEnd(); //*
            ShowFolderInput();  //*
            ShowLanguageSetting();
            ShowToggles();
            ShowFolderModeStartBtn();
            ShowConstructError();
        }
        
        void ShowExclusive()
        {
            GUILayout.Label("黑名单Json");
        }

        void ShowExclusiveEnd()
        {
            GUILayout.Label("-------------------以上是黑名单Json，不导出--------------------");
        }

        void ShowFolderInput()
        {
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Json文件夹：", GUILayout.Width(80));
            //_folderObj = EditorGUILayout.ObjectField(_folderObj, typeof(Object), false, GUILayout.Width(250));

            string folder_path = EditorUtil.ProjectPath(KAssetManager.FOLDER_JSON);
            GUILayout.TextField(folder_path, GUILayout.Width(400));
            GUILayout.EndHorizontal();
        }


        void ShowFolderModeStartBtn()
        {
            if (ValidateFolderModeInput() == true)
            {
                ShowGreenButton();
            }
            else
            {
                ShowRedButton();
            }
        }

        bool ValidateFolderModeInput()
        {
            //_error = string.Empty;
            //if (_folderObj == null)
            //{
            //    _error = "请选择JSON文件夹！";
            //    return false;
            //}
            //if (_folderObj != null)
            //{
            //    string jsonFolderPath = AssetDatabase.GetAssetPath(_folderObj);
            //    if (jsonFolderPath.IndexOf("Json") == -1)
            //    {
            //        _error = "请正确选择JSON文件夹!";
            //        return false;
            //    }
            //}
            return true;
        }
        

        //-------∽-★-∽------∽-★-∽--------∽-★-∽Execute∽-★-∽--------∽-★-∽------∽-★-∽--------//

        void Execute()
        {
            _constructError = string.Empty;
            if (_mode == MODE_FILE)
            {
                ExecuteFileMode();
            }
            else
            {
                ExecuteFolderMode();
            }
        }

        void ExecuteFileMode()
        {
            Worker.ExecuteFileMode(_jsonAssets, _inputParam);
        }

        void ExecuteFolderMode()
        {
            HashSet<string> exclusive = new HashSet<string>();  //排除列表
            foreach (var jsonname in _jsonAssets)
            {
                exclusive.Add(jsonname.name);
            }

            //Woker.ExecuteFolderMode(_folderObj, _isGenerateAtlas, _isHighQuality, _isBuildAssetbundle, exclusive);
        }


    }
}