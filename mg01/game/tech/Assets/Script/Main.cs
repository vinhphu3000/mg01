/* ==============================================================================
 * Main
 * @author jr.zeng
 * 2016/8/23 16:15:53
 * ==============================================================================*/

//#define RELEASE       //发布版
//#define USE_BUNDLE    //使用bundle资源



using UnityEngine;
using System.Collections;

namespace mg
{
    public class Main : MonoBehaviour
    {

        public enum MainEntryTp
        {
            NONE = 0,
            LUA,
            KUI,
        }

        public MainEntryTp entryType = MainEntryTp.NONE;

        void Awake()
        {

            if (!Application.isEditor)
            {

            }

            if (Application.isPlaying)
            {
                MainEntry a = null;

#if USE_BUNDLE

            a = new MainEntryR2();

#else
                switch (entryType)
                {
                    case MainEntryTp.LUA:
                        a = new MainEntryR1();
                        break;
                    case MainEntryTp.KUI:
                        a = new MainEntryR2();
                        break;
                    default:
                        a = new MainEntryR1();
                        break;
                }

#endif


                if (a != null)
                    a.Setup(this.gameObject);
            }

        }

        // Use this for initialization
        //void Start ()  { }

        // Update is called once per frame
        //void Update ()  { }

        private void OnDestroy()
        {

#if UNITY_EDITOR
            //编辑器时, 销毁整个游戏, 以保证各个模块的正确性
            //if (MainEntry.Me != null)
            //{
            //    MainEntry.Me.Clear();
            //}
#endif

        }

    }


}
