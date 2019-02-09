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

        protected override void OnClickTargets()
        {
            _parent2targets = new Dictionary<GameObject, List<GameObject>>();

            AnimatorRecorder.GenTargetDic(_panelPrefabObj, _parent2targets);

        }

        public static void GenerateRecord(GameObject panel)
        {
            ParticleRecorder.GenerateRecord(panel);
            AnimatorRecorder.GenerateRecord(panel);

        }


        public static void ReadRecord(GameObject panel)
        {
            ParticleRecorder.ReadRecord(panel);
            AnimatorRecorder.ReadRecord(panel);

        }


    }

}