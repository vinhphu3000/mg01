/* ==============================================================================
 * 列表工具
 * @author jr.zeng
 * 2016/6/8 10:40:49
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;


namespace mg.org
{


    /// <summary>
    /// 排序选项
    /// </summary>
    public enum SortOption
    {
        //升序
        ASCENDING = 0,
        //降序
        DESCENDING = 1,
    }


    /// <summary>
    /// List工具
    /// </summary>
    public class ListUtil
    {


        static public T GetValue<T>(List<T> input_, int index_)
        {
            if (index_ <= input_.Count - 1)
            {
                return input_[index_];
            }

            return default(T);
        }


        /// <summary>
        /// 删除数组中最后一个元素，并返回该元素的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list_"></param>
        /// <returns></returns>
        static public T Pop<T>(List<T> input_)
        {
            T obj = default(T);
            int count = input_.Count;
            if (count > 0)
            {
                obj = input_[count - 1];
                input_.RemoveAt(count - 1);
            }

            return obj;
        }

        /// <summary>
        /// 删除数组中第一个元素，并返回该元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list_"></param>
        /// <returns></returns>
        static public T Shift<T>(List<T> input_)
        {
            T obj = default(T);
            if (input_.Count > 0)
            {
                obj = input_[0];
                input_.RemoveAt(0);
            }

            return obj;
        }



        /// <summary>
        /// 转换成object数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr_"></param>
        /// <returns></returns>
        static public object[] TranObjects<T>(List<T> list_)
        {
            object[] result = new object[list_.Count];
            for (int i = 0, len = list_.Count; i < len; ++i)
            {
                result[i] = list_[i];
            }
            return result;
        }


        static public List<T> Clone<T>(List<T> input_)
        {
            List<T> result = new List<T>();
            result.AddRange(input_);
            return result;
        }

        /// <summary>
        /// 剔除列表里的重复项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input_"></param>
        /// <returns></returns>
        static public List<T> RemoveRepeat<T>(List<T> input_)
        {
            HashSet<T> hash = new HashSet<T>();
            List<T> result = new List<T>();
            for (int i = 0, len = input_.Count; i < len; ++i)
            {
                if (!hash.Contains(input_[i]))
                {
                    hash.Add(input_[i]);
                    result.Add(input_[i]);
                }
            }
            return result;
        }


        //-------∽-★-∽------∽-★-∽--------∽-★-∽排序相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input_"></param>
        /// <param name="option_"></param>
        /// <returns></returns>
        static public List<T> Sort<T>(List<T> input_, SortOption option_ = SortOption.ASCENDING) where T : IComparable
        {
            input_.Sort(delegate(T first, T second)
            {
                if (option_ == SortOption.ASCENDING)
                {
                    return first.CompareTo(second);
                }
                else
                {
                    return -first.CompareTo(second);
                }
            });

            //list_.Sort((first, second) => (first.CompareTo(second) );

            return input_;
        }


        /// <summary>
        /// 按属性排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input_"></param>
        /// <param name="keyNames_">键名: string 或 string[] </param>
        /// <param name="sortOptions_">排序opt: ListSortOption 或 ListSortOption[] </param>
        /// <returns></returns>
        static public List<T> SortOn<T>(List<T> input_, object keyNames_, object sortOptions_ = null)
        {
            if (input_.Count == 1)
                return input_;

            string[] names = null;
            if ( keyNames_ is string )
            {
                names = new string[] { (string)keyNames_ };
            }
            else if (keyNames_ is string[] )
            {
                names = keyNames_ as string[];
            }

            if (names == null)
                return input_;

            SortOption[] options;

            if (sortOptions_ is SortOption)
            {
                options = new SortOption[] { (SortOption)sortOptions_ };
            }
            else if (sortOptions_ is SortOption[])
            {
                options = sortOptions_ as SortOption[];
            }
            else
            {
                options = new SortOption[] { SortOption.ASCENDING };
            }

            Type type = typeof(T); //获取类型
            string key;
            IComparable value1 = null;
            IComparable value2 = null;
            int compare = 0;
            SortOption opt = SortOption.ASCENDING;

            input_.Sort(delegate(T first, T second)
            {

                for (int i = 0; i < names.Length; ++i)
                {
                    key = names[i];
                    value1 = ClassUtil.GetProperty(first, type, key) as IComparable;
                    if (value1 == null)
                        continue;
                    value2 = ClassUtil.GetProperty(second, type, key) as IComparable;
                    if (value2 == null)
                        continue;

                    if (i < options.Length)
                        opt = options[i];   //如果没有就取上一次

                    compare = value1.CompareTo(value2);
                    if (compare != 0)
                    {
                        if (opt == SortOption.ASCENDING)
                        {
                            return compare;
                        }
                        else
                        {
                            return -compare;
                        }
                    }
                }

                return 0;
            });

            return input_;
        }



        /// <summary>
        /// 生成一个序列
        /// </summary>
        /// <param name="len_"></param>
        /// <param name="base_"></param>
        /// <param name="increate_"></param>
        /// <returns></returns>
        static public List<int> GenSequence(int len_, int base_=1, int increate_=1)
        {
            return GenSequence(len_, base_, increate_);
        }

        static public List<int> GenSequence( int len_, int base_ = 1, int increate_ = 1, List<int> list_ = null)
        {
            if (list_ == null)
            {
                list_ = new List<int>();
            }
            else
            {
                list_.Clear();
            }


            for (int i = 0; i < len_; ++i)
            {
                list_.Add(base_);
                base_ += increate_;
            }
            return list_;
        }
    }


}