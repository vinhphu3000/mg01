/* ==============================================================================
 * ArrayUtil
 * @author jr.zeng
 * 2016/9/7 17:51:19
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mg.org
{

    public class ArrayUtil
    {
        
        static public readonly object[] EMPTY_OBJS = new object[0];

        public ArrayUtil()
        {

        }

        static public T GetValue<T>(T[] arr_, int index_)
        {
            if (index_ >=0 && index_ <= arr_.Length - 1)
            {
                return arr_[index_];
            }

            return default(T);
        }

        static public T[] Add<T>(T[] arr1_, T obj_)
        {
            T[] result = new T[arr1_.Length + 1];
            arr1_.CopyTo(result, 0);
            result[arr1_.Length] = obj_;
            return result;
        }

        /// <summary>
        /// 连接数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr1_"></param>
        /// <param name="arr2_"></param>
        /// <returns></returns>
        static public T[] Concat<T>(T[] arr1_, T[] arr2_)
        {
            if (arr2_ == null)
                return arr1_;

            T[] result = new T[arr1_.Length + arr2_.Length];
            arr1_.CopyTo(result, 0);
            arr2_.CopyTo(result, arr1_.Length);
            return result;
        }



        /// <summary>
        /// 转换成object数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr_"></param>
        /// <returns></returns>
        static public object[] TranObjects<T>(T[] arr_)
        {
            object[] result = new object[arr_.Length];
            for (int i = 0, len = arr_.Length; i < len; ++i)
            {
                result[i] = arr_[i];
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
        static public T[] Sort<T>(T[] input_, SortOption option_ = SortOption.ASCENDING) where T : IComparable
        {
            Array.Sort(input_, delegate(T first, T second)
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
        /// <param name="keyNames_">键名: string | string[] </param>
        /// <param name="sortOptions_">排序opt: ListSortOption | ListSortOption[] </param>
        /// <returns></returns>
        static public T[] SortOn<T>(T[] input_, object keyNames_, object sortOptions_ = null)
        {

            string[] names = null;
            if (keyNames_ is string)
            {
                names = new string[] { (string)keyNames_ };
            }
            else if (keyNames_ is string[])
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
            SortOption opt;

            Array.Sort(input_, delegate(T first, T second)
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

                    opt = ArrayUtil.GetValue<SortOption>(options, i);
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




    }

}