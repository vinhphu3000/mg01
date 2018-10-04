/* ==============================================================================
 * SecurePrefs
 * @author jr.zeng
 * 2016/8/24 14:16:32
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace mg.org
{

    public class UserPrefs
    {

        public const string PREF_ID_GLOBAL = "PREF_ID_GLOBAL";

        private static Dictionary<string, SecurePref> m_id2pref = new Dictionary<string, SecurePref>();

        private static SecurePref m_glPref = null;
        private static SecurePref m_usrPref = null;

        public UserPrefs()
        {

        }


        public static void Setup()
        {
            CreateGlPref();
        }

        public static void Clear()
        {

        }


        public static SecurePref GlPref
        {
            get { return m_glPref; }
        }

        public static SecurePref UsrPref
        {
            get { return m_usrPref; }
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 创建一个偏好
        /// </summary>
        /// <param name="id_"></param>
        /// <returns></returns>
        public static SecurePref CreatePref(string id_)
        {
            SecurePref pref = null;
            if (m_id2pref.ContainsKey(id_))
            {
                pref = m_id2pref[id_];
                return pref;
            }

            pref = new SecurePref();
            pref.PrefID = id_;

            m_id2pref[id_] = pref;
            return pref;
        }

        /// <summary>
        /// 获取偏好
        /// </summary>
        /// <param name="id_"></param>
        /// <returns></returns>
        public static SecurePref GetPref(string id_)
        {
            SecurePref pref = null;
            if (m_id2pref.ContainsKey(id_))
            {
                pref = m_id2pref[id_];
            }
            return pref;
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//


        public static void SaveAll()
        {

            PlayerPrefs.Save();
        }


        public static void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }


        //public static void DeleteKey(string key_)
        //{
        //    PlayerPrefs.DeleteKey(key_);
        //}

        /// <summary>
        /// 创建全局偏好
        /// </summary>
        /// <returns></returns>
        public static SecurePref CreateGlPref()
        {
            m_glPref = CreatePref(PREF_ID_GLOBAL);
            return m_glPref;
        }


        /// <summary>
        /// 创建当前用户偏好
        /// </summary>
        /// <param name="usr_id_"></param>
        /// <returns></returns>
        public static SecurePref CreateUsrPref(string usr_id_)
        {
            m_usrPref = CreatePref(usr_id_);
            return m_usrPref;
        }



    }

}