/* ==============================================================================
2   * 类名称：ClassUtil
4   * 创建人：jr.zeng
5   * 创建时间：2016/6/8 10:38:06
10   * ==============================================================================*/

//using UnityEngine;

using System;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;


namespace mg.org
{
    public class ClassUtil
    {



        public static object New(Type type_)
        {
            object obj = null;
            try
            {
                obj = Activator.CreateInstance(type_);
            }
            catch (Exception ex)
            {
                Log.Warn(ex);
            }
            return obj;
        }


        public static object NewWithArg(Type type_, params object[] arg_)
        {
            object obj = null;
            try
            {
                obj = Activator.CreateInstance(type_, arg_);
            }
            catch (Exception ex)
            {
                Log.Warn(ex);
            }
            return obj;
        }

        /// <summary>
        /// 创建对象（当前程序集）
        /// </summary>
        /// <param name="typeName">类型名 ClassName / NameSpace.ClassName </param>
        /// <returns>创建的对象，失败返回 null</returns>
        public static object New(string typeName_)
        {
            Type objType = Type.GetType(typeName_, true);
            return New(objType);
        }


        public static object NewWithArg(string typeName_, params object[] arg_)
        {
            Type objType = Type.GetType(typeName_, true);
            return NewWithArg(objType, arg_);
        }


        /// <summary>
        /// 创建对象(外部程序集)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="typeName">类型名</param>
        /// <returns>创建的对象，失败返回 null</returns>
        public static object NewByTypeName(string path_, string typeName_)
        {
            object obj = null;
            try
            {
                obj = Assembly.Load(path_).CreateInstance(typeName_);
            }
            catch (Exception ex)
            {
                Log.Warn(ex);
            }

            return obj;
        }


        /// <summary>
        /// 判断某个类是否继承自某个接口、类
        /// </summary>
        /// <param name="target_"></param>
        /// <param name="parent_"></param>
        /// <returns></returns>
        public static bool IsParentType(Type target_, Type parent_)
        {
            if (target_ == null || parent_ == null || target_ == parent_ || target_.BaseType == null)
            {
                return false;
            }

            if (parent_.IsInterface)
            {
                foreach (var t in target_.GetInterfaces())
                {
                    if (t == parent_)
                    {
                        return true;
                    }
                }
            }
            else
            {
                do
                {
                    if (target_.BaseType == parent_)
                    {
                        return true;
                    }
                    target_ = target_.BaseType;
                }
                while (target_ != null);

            }
            return false;
        }


        
        //-------∽-★-∽------∽-★-∽--------∽-★-∽以下利用反射(效率低下)∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="type_"></param>
        /// <param name="property_"></param>
        /// <param name="target_"></param>
        /// <returns></returns>
        public static object GetProperty(object target_, Type type_, string property_)
        {
            object value = null;

            FieldInfo fieldInfo = type_.GetField(property_); //获取指定名称的属性
            if (fieldInfo != null)
            {
                value = fieldInfo.GetValue(target_);
            }
            else
            {
                PropertyInfo propertyInfo = type_.GetProperty(property_); //获取指定名称的属性
                if (propertyInfo != null)
                {
                    value = propertyInfo.GetValue(target_, null);
                }
            }

            return value;
        }

        public static object GetProperty(object target_, string property_)
        {
            return GetProperty(target_, target_.GetType(), property_);
        }


        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="target_"></param>
        /// <param name="type_"></param>
        /// <param name="property_"></param>
        /// <param name="value_"></param>
        public static void SetField(object target_, Type type_, string property_, object value_)
        {
            FieldInfo fieldInfo = type_.GetField(property_); //获取指定名称的属性
            if (fieldInfo != null)
            {
                fieldInfo.SetValue(target_, value_);
            }
        }


        public static void SetField(object target_, Type type_, string property_, object value_, BindingFlags flags_)
        {
            FieldInfo fieldInfo = type_.GetField(property_, flags_); //获取指定名称的属性
            if (fieldInfo != null)
            {
                fieldInfo.SetValue(target_, value_);
            }
        }

        public static void SetField(object target_, string property_, object value_)
        {
            SetField(target_, target_.GetType(), property_, value_);
        }


        /// <summary>
        /// 设置属性_私有NonPublic
        /// </summary>
        /// <param name="target_"></param>
        /// <param name="type_">target_的类型</param>
        /// <param name="property_"></param>
        /// <param name="value_"></param>
        public static void SetFieldNP(object target_, Type type_, string property_, object value_)
        {
            FieldInfo fieldInfo = type_.GetField(property_, BindingFlags.NonPublic|BindingFlags.Instance);
            if (fieldInfo != null)
            {
                fieldInfo.SetValue(target_, value_);
            }
        }



        /// <summary>
        /// 调用对象的指定函数
        /// </summary>
        /// <param name="obj_"></param>
        /// <param name="methodName_"></param>
        /// <param name="methodParams_"></param>
        /// <returns></returns>
        static public object CallMethod(object obj_, string methodName_, object[] methodParams_ = null)
        {
            Type t = obj_.GetType();
            MethodInfo info = t.GetMethod(methodName_); //根据名称获取函数信息
            if (info == null)
                //没有此函数
                return null;

            return info.Invoke(obj_, methodParams_);
        }



    }


}