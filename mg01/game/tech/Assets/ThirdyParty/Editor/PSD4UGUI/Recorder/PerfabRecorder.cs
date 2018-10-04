using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;


namespace Edit.PSD4UGUI
{
   
    /// <summary>
    /// 用于记录UI里面额外的信息, 在生成预制时进行还原
    /// </summary>
    public class PerfabRecorder : BaseRecorder
    {

        static PerfabRecorder _window;


        [MenuItem("PSD4UGUI/综合信息记录")]      
        public static void Main()
        {
            _window = EditorWindow.GetWindow<PerfabRecorder>("综合信息记录");
            _window.Show();

        }

        protected override void OnClickGenerate()
        {
            GenerateRecord(_panelPrefabObj);

        }


        public static void GenerateRecord(GameObject panel)
        {
            //特效
            ParticleRecorder.GenerateRecord(panel);

        }


        public static void ReadRecord(GameObject panel)
        {
            //特效
            ParticleRecorder.ReadRecord(panel);

        }


    }

}