/* ==============================================================================
 * AutoReleasePool
 * @author jr.zeng
 * 2016/12/9 10:28:31
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mg.org
{

    public class AutoRelease : BaseObject
    {

        List<Ref> m_autoList = new List<Ref>();

        HashSet<Ref> m_refHash = new HashSet<Ref>();  //所以ref的总表, add时记录, 要到dispose时才remove, auto release时不移除

        public AutoRelease()
        {

        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public void Add(Ref ref_)
        {
            if (m_refHash.Contains(ref_))
                return;
            m_refHash.Add(ref_);

            ref_.Retain(null);
            m_autoList.Add(ref_);
        }

        public void Remove(Ref ref_)
        {
            if (!m_refHash.Contains(ref_))
                return;
            m_refHash.Remove(ref_);
           
            if (m_autoList.Remove(ref_))
            {   //还在列表里面
                ref_.Release(null);
                Log.Assert("怎么可能还在AutoList里?");
            }
        }


        public void Excute()
        {
            if (m_autoList.Count == 0)
                return;

            Ref ref_;
            for(int i = m_autoList.Count-1;i>=0;--i)
            {
                ref_ = m_autoList[i];
                m_autoList.RemoveAt(i); //先移除再release,不然在remove里又会release一次
                ref_.Release(null);     //全部引用减1
            }

            //m_autoList.Clear();
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//

        //todo
        public void Dump()
        {

        }

    }


}