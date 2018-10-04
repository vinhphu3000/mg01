/* ==============================================================================
 * FpsTicker
 * @author jr.zeng
 * 2016/7/10 17:26:58
 * ==============================================================================*/

using UnityEngine;
using System.Collections;


namespace mg.org
{

    public class FpsTicker : MonoBehaviour
    {

        public float f_UpdateInterval = 0.7F;

        private float f_LastInterval;

        private int i_Frames = 0;

        private float f_Fps;
        private float f_Ms;
        private string m_fps_str = "";

        private int m_memery;
        private string m_memery_str = "";

        private Rect m_rect_fps;
        private Rect m_rect_memory;
        private GUIStyle m_style;

        float m_labelH = 11;
        float m_marginH = 5f;   //边距

        void Start()
        {
            //Application.targetFrameRate=60;

            int screenH = Screen.height;
            m_rect_memory = new Rect(0, 0, 250, 200);
            m_rect_fps = new Rect(0, 0, 200, 200);

            RefrshLabelY();

            m_style = new GUIStyle();
            m_style.fontSize = (int)m_labelH;
            m_style.normal.textColor = cc.c3b(255, 255, 255);
            f_LastInterval = Time.realtimeSinceStartup;

            i_Frames = 0;
        }

        void RefrshLabelY()
        {
            int screenH = Screen.height;
            m_rect_memory.y = screenH - m_labelH * 2 - m_marginH;
            m_rect_fps.y = screenH - m_labelH * 3 - m_marginH;
        }

        void OnGUI()
        {
            //GUI.Label(m_rect_fps, f_Ms.ToString("f1") + "ms FPS:" + f_Fps.ToString("f2"));
            GUI.Label(m_rect_fps, m_fps_str, m_style);
            //GUI.Label(new Rect(0, 10, 200, 200), "memery: " + m_memery + " MB");
            GUI.Label(m_rect_memory, m_memery_str, m_style);
        }

        void Update()
        {
            ++i_Frames;

            if (Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval)
            {
                f_Fps = i_Frames / (Time.realtimeSinceStartup - f_LastInterval);
                f_Ms = 1000.0f / Mathf.Max(f_Fps, 0.00001f);
                i_Frames = 0;

                f_LastInterval = Time.realtimeSinceStartup;

                m_fps_str = f_Ms.ToString("f1") + "ms FPS:" + f_Fps.ToString("f2");
                //m_fps_str = "FPS:" + f_Fps.ToString("f2");
                m_memery_str = GetMemoryInfo();

                RefrshLabelY();
            }

            //m_memery = SystemInfo.systemMemorySize;
        }


        public const float m_KBSize = 1024.0f * 1024.0f;
        public string GetMemoryInfo()
        {
            float totalMemory = (float)(UnityEngine.Profiling.Profiler.GetTotalAllocatedMemory() / m_KBSize);

            float totalReservedMemory = (float)(UnityEngine.Profiling.Profiler.GetTotalReservedMemory() / m_KBSize);
            //float totalUnusedReservedMemory = (float)(Profiler.GetTotalUnusedReservedMemory() / m_KBSize);
            
            float monoHeapSize = (float)(UnityEngine.Profiling.Profiler.GetMonoHeapSize() / m_KBSize);
            float monoUsedSize = (float)(UnityEngine.Profiling.Profiler.GetMonoUsedSize() / m_KBSize);

            return string.Format("TotalMemory:{0}/{1}MB\nMonoMemory:{2}/{3}MB",
                totalMemory.ToString("f1"),
                //totalUnusedReservedMemory.ToString("f1"),
                totalReservedMemory.ToString("f1"), 
                monoUsedSize.ToString("f1"),
                monoHeapSize.ToString("f1") );

        }
    }

}