using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace mg.org
{
    /// <summary>
    /// 对象池接口
    /// </summary>
    public interface IPool
    {
        //获取对象
        object Pop();
        //回收对象
        void Push(object obj_);
        //剩余数量
        int RemainCount { get; }
        //清空全部空闲
        void ClearAllIdles();
        //清空对象池
        void Clear();

    }

}