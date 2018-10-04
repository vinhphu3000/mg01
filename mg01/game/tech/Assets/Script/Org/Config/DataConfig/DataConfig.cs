/* ==============================================================================
 * DataConfig
 * @author jr.zeng
 * 2016/9/7 17:48:44
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.IO;

using UnityEngine;

namespace mg.org
{

    public class DataConfig
    {

        static private DataCfgParser m_parser;
        static private DataConfigReg[] m_regs;

        //static Refer m_refer = new Refer(typeof(DataConfig).Name);

        public DataConfig()
        {

        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="parser_">解析器</param>
        /// <param name="regs_">配置表注册</param>
        static public void Init(DataCfgParser parser_, DataConfigReg[] regs_)
        {
            m_parser = parser_;
            m_regs = regs_;
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//


        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//


        //-------∽-★-∽------∽-★-∽--------∽-★-∽配置加载∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 获取加载项
        /// </summary>
        /// <returns></returns>
        static public LoadReq[] GetLoadObjs()
        {
            LoadReq[] result = new LoadReq[m_regs.Length];

            DataConfigReg reg;
            LoadReq req;
            for (int i = 0; i < m_regs.Length; ++i)
            {
                reg = m_regs[i];

                req = new LoadReqRes(CC_RES_ID.CONFIG, reg.file_name);
                req.on_complete = FileLoadBack;
                req.userData = reg;
                result[i] = req;
                //req.SetRefer(m_refer);
            }

            return result;
        }

        //加载完成
        static private void FileLoadBack(LoadReq req_)
        {
            LoadReq req = req_;
            DataConfigReg reg = req.userData as DataConfigReg;

            TextAsset data = req_.data as TextAsset;
            byte[] bytes = data.bytes;

            object[] datas = m_parser.Parse(bytes, reg.data_tp);
            if (datas == null)
                return;

            if (reg.on_complete != null)
            {
                reg.on_complete(datas);
            }
        }
    }

    //-------∽-★-∽------∽-★-∽--------∽-★-∽DataConfigReg∽-★-∽--------∽-★-∽------∽-★-∽--------//

    /// <summary>
    /// 配置注册项
    /// </summary>
    public class DataConfigReg
    {
        //文件名
        public string file_name;
        //数据类的类型
        public Type data_tp;
        //加载完成回调
        public CALLBACK_Objs on_complete;
        
        /// <param name="fileName_">文件名</param>
        /// <param name="dataTp_">解析成的数据类型</param>
        /// <param name="onComplete_">解析完成的回调</param>
        public DataConfigReg(string fileName_, Type dataTp_, CALLBACK_Objs onComplete_ = null)
        {
            file_name = fileName_;
            data_tp = dataTp_;
            on_complete = onComplete_;
        }

    }

    

}