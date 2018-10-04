/* ==============================================================================
 * 安全偏好储存
 * @author jr.zeng
 * 2016/8/24 11:27:57
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;


namespace mg.org
{
    
    public class SecurePref
    {
        private const string RAW_NOT_FOUND = "{not_found}";

        private const string KEY_SEPARATOR = "@";
        private const string DATA_SEPARATOR = "|";
        private const char RAW_SEPARATOR = ':';

        protected string m_prefId = "default";

        public SecurePref()
        {

        }

        /// <summary>
        /// 偏好id
        /// </summary>
        public string PrefID
        {
            set { m_prefId = value; }
            get { return m_prefId; }
        }

        /// <summary>
        /// 转换键名
        /// </summary>
        /// <param name="key_"></param>
        /// <returns></returns>
        public string TransKey(string key_)
        {
            return m_prefId + KEY_SEPARATOR + key_;
        }


        private string TransFieldKey(string key_, Type tp_)
        {
            return key_ + KEY_SEPARATOR + tp_.Name + KEY_SEPARATOR;
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据管理∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public bool HasKey(string key_)
        {
            key_ = TransKey(key_);
            return SecurePrefs.HasKey(key_);
        }



        public void DeleteKey(string key_)
        {
            key_ = TransKey(key_);
            SecurePrefs.DeleteKey(key_);
        }


        //删除自己对应的数, 不影响其他id
        public void DeleteAll()
        {
            //TODO
            //SecurePrefs.DeleteAll();
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽数据操作∽-★-∽--------∽-★-∽------∽-★-∽--------//
        
        //-------∽-★-∽------∽-★-∽--------∽-★-∽SETTER∽-★-∽--------∽-★-∽------∽-★-∽--------//

        /// <summary>
        /// 设置字符串
        /// </summary>
        /// <param name="key_"></param>
        /// <param name="value_"></param>
        public void SetString(string key_, string value_)
        {
            if (value_ != null)
            {
                key_ = TransKey(key_);
                SecurePrefs.SetString(key_, value_);
            }
            else
            {
                DeleteKey(key_);
            }
        }


        /// <summary>
        /// 设置整型
        /// </summary>
        /// <param name="key_"></param>
        /// <param name="value_"></param>
        public void SetInt(string key_, int value_)
        {
            key_ = TransKey(key_);
            SecurePrefs.SetInt(key_, value_);
        }


        /// <summary>
        /// 设置浮点型
        /// </summary>
        /// <param name="key_"></param>
        /// <param name="value_"></param>
        public void SetFloat(string key_, float value_)
        {
            key_ = TransKey(key_);
            SecurePrefs.SetFloat(key_, value_);
        }

        //-------------SETTER扩展-------------

        /// <summary>
        /// 设置无符号整型
        /// </summary>
        /// <param name="key_"></param>
        /// <param name="value_"></param>
        public void SetUInt(string key_, uint value_)
        {
            SetString(key_, value_.ToString());
        }

        /// <summary>
        /// 设置双精度浮点型
        /// </summary>
        /// <param name="key_"></param>
        /// <param name="value_"></param>
        public void SetDouble(string key_, double value_)
        {
            SetString(key_, value_.ToString());
        }

        /// <summary>
        /// 设置长整型
        /// </summary>
        /// <param name="key_"></param>
        /// <param name="value_"></param>
        public void SetLong(string key_, long value_)
        {
            SetString(key_, value_.ToString());
        }

        /// <summary>
        /// 设置布尔值
        /// </summary>
        /// <param name="key_"></param>
        /// <param name="value_"></param>
        public void SetBool(string key_, bool value_)
        {
            SetInt(key_, value_ ? 1 : 0);
        }

        /// <summary>
        /// 设置字节
        /// </summary>
        /// <param name="key_"></param>
        /// <param name="value_"></param>
        public void SetByteArray(string key_, byte[] value_)
        {
            if (value_ != null)
            {
                SetString(key_, Encoding.UTF8.GetString(value_, 0, value_.Length));
            }
            else
            {
                DeleteKey(key_);
            }

        }

        /// <summary>
        /// 设置二维向量
        /// </summary>
        /// <param name="key_"></param>
        /// <param name="value_"></param>
        public void SetVector2(string key_, Vector2 value_)
        {
            if (value_ != null)
            {
                string concatenated = value_.x + DATA_SEPARATOR + value_.y;
                SetString(key_, concatenated);
            }
            else
            {
                DeleteKey(key_);
            }
        }

        /// <summary>
        /// 设置三维向量
        /// </summary>
        /// <param name="key_"></param>
        /// <param name="value_"></param>
        public void SetVector3(string key_, Vector3 value_)
        {
            if (value_ != null)
            {
                string concatenated = value_.x + DATA_SEPARATOR + value_.y + DATA_SEPARATOR + value_.z;
                SetString(key_, concatenated);
            }
            else
            {
                DeleteKey(key_);
            }

        }

        /// <summary>
        /// 设置四元数
        /// </summary>
        /// <param name="key_"></param>
        /// <param name="value_"></param>
        public void SetQuaternion(string key_, Quaternion value_)
        {
            if (value_ != null)
            {
                string concatenated = value_.x + DATA_SEPARATOR + value_.y + DATA_SEPARATOR + value_.z + DATA_SEPARATOR + value_.w;
                SetString(key_, concatenated);
            }
            else
            {
                DeleteKey(key_);
            }
        }


        /// <summary>
        /// 设置颜色
        /// </summary>
        /// <param name="key_"></param>
        /// <param name="value_"></param>
        public void SetColor(string key_, Color value_)
        {
            if (value_ != null)
            {
                string concatenated = value_.r + DATA_SEPARATOR + value_.g + DATA_SEPARATOR + value_.b + DATA_SEPARATOR + value_.a;
                SetString(key_, concatenated);
            }
            else
            {
                DeleteKey(key_);
            }
        }

        /// <summary>
        /// 写入对象
        /// </summary>
        /// <param name="id_"></param>
        /// <param name="value_"></param>
        public void SetObj<T>(string key_, T value_) where T : new()
        {
            __SetObj(key_, value_);
        }

        private void __SetObj(string key_, object value_)
        {
            if (value_ == null)
            {
                DeleteKey(key_);
                return;
            }

            SetString(key_, key_);  //记录已保存此对象

            Type t = value_.GetType();
            string fieldKey = TransFieldKey(key_, t);

            FieldInfo[] fields = t.GetFields();
            FieldInfo fieldInfo;
            string saveName;
            string fieldTpName;
            object fieldValue;

            int len = fields.Length;
            for (int i = 0; i < len; i++)
            {
                fieldInfo = fields[i];
                fieldTpName = fieldInfo.FieldType.Name.ToLower();   //转小写
                fieldValue = fieldInfo.GetValue(value_);    //属性值

                saveName = fieldKey + fieldInfo.Name;   //保存此属性的key

                switch (fieldTpName)
                {
                    case FieldTpName.OBJECT:
                        //暂不支持object类型, 因为获取时无法确定实际类型
                        //TODO
                        
                        break;
                    case FieldTpName.STRING:

                        string strVal = (string)fieldValue;
                        SetString(saveName, strVal);

                        break;
                    case FieldTpName.INT:
                    case FieldTpName.INT32:

                        int intVal = (int)fieldValue;
                        SetInt(saveName, intVal);

                        break;
                    case FieldTpName.UINT:
                    case FieldTpName.UINT32:

                        uint uintVal = (uint)fieldValue;
                        SetUInt(saveName, uintVal);

                        break;
                    case FieldTpName.LONG:
                    case FieldTpName.ULONG:
                    case FieldTpName.INT64:
                    case FieldTpName.UINT64:

                        long longVal = (long)fieldValue;
                        SetLong(saveName, longVal); 

                        break;
                    case FieldTpName.SINGLE:
                    case FieldTpName.FLOAT:

                        float floatVal = (float)fieldValue;
                        SetFloat(saveName, floatVal);

                        break;
                    case FieldTpName.DOUBLE:

                        double doubleVal = (double)fieldValue;
                        SetDouble(saveName, doubleVal);

                        break;
                    case FieldTpName.BOOL:

                        bool boolVal = (bool)fieldValue;
                        SetBool(saveName, boolVal);

                        break;
                    case FieldTpName.BYTE_ARRAY:

                        byte[] byteVal = (byte[])fieldValue;
                        SetByteArray(saveName, byteVal);

                        break;
                    case FieldTpName.COLOR:

                        Color colorVal = (Color)fieldValue;
                        SetColor(saveName, colorVal);

                        break;
                    case FieldTpName.VECTOR2:

                        Vector2 vec2Val = (Vector2)fieldValue;
                        SetVector2(saveName, vec2Val);

                        break;
                    case FieldTpName.VECTOR3:

                        Vector3 vec3Val = (Vector3)fieldValue;
                        SetVector3(saveName, vec3Val);

                        break;
                    case FieldTpName.QUATERNION:

                        Quaternion quaVal = (Quaternion)fieldValue;
                        SetQuaternion(saveName, quaVal);

                        break;
                    default:

                        if (fieldValue != null)
                        {
                            Type _type = fieldValue.GetType();
                            if (!_type.IsPrimitive)
                            {
                                //不是原生对象
                                __SetObj(saveName, fieldValue);
                            }
                        }
                        else
                        {
                            __SetObj(saveName, null);
                        }
                        
                        break;
                }
            }
        }



        //-------∽-★-∽------∽-★-∽--------∽-★-∽GETTER∽-★-∽--------∽-★-∽------∽-★-∽--------//


        public float GetFloat(string key_, float defaultValue_=0)
        {
            key_ = TransKey(key_);
            return SecurePrefs.GetFloat(key_, defaultValue_);
        }


        public int GetInt(string key_, int defaultValue_=0)
        {
            key_ = TransKey(key_);
            return SecurePrefs.GetInt(key_, defaultValue_);
        }


        public string GetString(string key_, string defaultValue_=null)
        {
            key_ = TransKey(key_);
            return SecurePrefs.GetString(key_, defaultValue_);
        }


        //-------------GETTER扩展-------------

        /// <summary>
        /// 获取无符号整型
        /// </summary>
        /// <param name="key_"></param>
        /// <param name="defaultValue_"></param>
        /// <returns></returns>
        public uint GetUInt(string key_, uint defaultValue_ = 0)
        {
            string rawData = GetString(key_, defaultValue_.ToString());
            uint result;
            uint.TryParse(rawData, out result);
            return result;

        }
        /// <summary>
        /// 获取双精度浮点型
        /// </summary>
        /// <param name="key_"></param>
        /// <param name="defaultValue_"></param>
        /// <returns></returns>
        public double GetDouble(string key_, double defaultValue_=0)
        {
            string rawData = GetString(key_, defaultValue_.ToString());
            double result;
            double.TryParse(rawData, out result);
            return result;
        }

        /// <summary>
        /// 获取长整型
        /// </summary>
        /// <param name="key_"></param>
        /// <param name="defaultValue_"></param>
        /// <returns></returns>
        public long GetLong(string key_, long defaultValue_=0)
        {
            string rawData = GetString(key_, defaultValue_.ToString());
            long result;
            long.TryParse(rawData, out result);
            return result;
        }

        /// <summary>
        /// 获取布尔值
        /// </summary>
        /// <param name="key_"></param>
        /// <param name="defaultValue_"></param>
        /// <returns></returns>
        public bool GetBool(string key_, bool defaultValue_=false)
        {
            int defValue = defaultValue_ ? 1 : 0;
            int result = GetInt(key_, defValue);
            return result == 1;
        }

        /// <summary>
        /// 获取字节流
        /// </summary>
        /// <param name="key_"></param>
        /// <param name="defaultValue_"></param>
        /// <returns></returns>
        public byte[] GetByteArray(string key_, byte[] defaultValue_=null)
        {
            key_ = TransKey(key_);

            byte[] result;
            string rawData = GetString(key_, RAW_NOT_FOUND);
            if (rawData == RAW_NOT_FOUND)
            {
                result = defaultValue_;
            }
            else
            {
                result = Encoding.UTF8.GetBytes(rawData);
            }
            return result;
        }


        /// <summary>
        /// 获取2维向量
        /// </summary>
        /// <param name="key_"></param>
        /// <returns></returns>
        public Vector2 GetVector2(string key_) { return GetVector2(key_, Vector2.zero);  }
        public Vector2 GetVector2(string key_, Vector2 defaultValue_)
        {
            Vector2 result;
            string rawData = GetString(key_, RAW_NOT_FOUND);
            if (rawData == RAW_NOT_FOUND)
            {
                result = defaultValue_;
            }
            else
            {
                string[] values = rawData.Split(DATA_SEPARATOR[0]);
                float x;
                float y;
                float.TryParse(values[0], out x);
                float.TryParse(values[1], out y);

                result = new Vector2(x, y);
            }
            return result;
        }

        /// <summary>
        /// 获取三维向量
        /// </summary>
        /// <param name="key_"></param>
        /// <returns></returns>
        public Vector3 GetVector3(string key_)  { return GetVector3(key_, Vector3.zero);  }
        public Vector3 GetVector3(string key_, Vector3 defaultValue_)
        {
            Vector3 result;
            string rawData = GetString(key_, RAW_NOT_FOUND);
            if (rawData == RAW_NOT_FOUND)
            {
                result = defaultValue_;
            }
            else
            {
                string[] values = rawData.Split(DATA_SEPARATOR[0]);
                float x;
                float y;
                float z;
                float.TryParse(values[0], out x);
                float.TryParse(values[1], out y);
                float.TryParse(values[2], out z);

                result = new Vector3(x, y, z);
            }
            return result;
        }

        /// <summary>
        /// 获取四元素
        /// </summary>
        /// <param name="key_"></param>
        /// <returns></returns>
        public Quaternion GetQuaternion(string key_) { return GetQuaternion(key_, Quaternion.identity); }
        public Quaternion GetQuaternion(string key_, Quaternion defaultValue_)
        {

            Quaternion result;
            string rawData = GetString(key_, RAW_NOT_FOUND);
            if (rawData == RAW_NOT_FOUND)
            {
                result = defaultValue_;
            }
            else
            {
                string[] values = rawData.Split(DATA_SEPARATOR[0]);
                float x;
                float y;
                float z;
                float w;
                float.TryParse(values[0], out x);
                float.TryParse(values[1], out y);
                float.TryParse(values[2], out z);
                float.TryParse(values[3], out w);

                result = new Quaternion(x, y, z, w);
            }
            return result;
        }

        /// <summary>
        /// 获取颜色
        /// </summary>
        /// <param name="key_"></param>
        /// <returns></returns>
        public Color GetColor(string key_) {  return GetColor(key_, Color.black); }
        public Color GetColor(string key_, Color defaultValue_)
        {

            Color result;
            string rawData = GetString(key_, RAW_NOT_FOUND);
            if (rawData == RAW_NOT_FOUND)
            {
                result = defaultValue_;
            }
            else
            {
                string[] values = rawData.Split(DATA_SEPARATOR[0]);
                float r;
                float g;
                float b;
                float a;
                float.TryParse(values[0], out r);
                float.TryParse(values[1], out g);
                float.TryParse(values[2], out b);
                float.TryParse(values[3], out a);

                result = new Color(r, g, b, a);
            }
            return result;
        }


        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key_"></param>
        /// <param name="defaultValue_"></param>
        /// <returns></returns>
        public T GetObj<T>(string key_, T defaultValue_ = default(T)) where T : new()
        {
           Type t = typeof(T);
           return (T)__GetObj(t, key_, defaultValue_);
        }

        private object __GetObj(Type t_, string key_, object defaultValue_ = null)
        {
            string hasKey = GetString(key_);
            if (hasKey == null || hasKey != key_)
            {
                //没有保存过对象
                return defaultValue_;
            }

            Type t = t_;
            string fieldKey = TransFieldKey(key_, t);

            object newObj = Activator.CreateInstance(t);

            FieldInfo fieldInfo;
            string saveName;
            string fieldTpName;
            object fieldValue;
            object defaultValue;

            FieldInfo[] fiedls = t.GetFields();
            int len = fiedls.Length;
            for (int i = 0; i < fiedls.Length; i++)
            {
                fieldInfo = fiedls[i];
                saveName = fieldKey + fieldInfo.Name;
                if (!HasKey(saveName))
                {
                    continue;
                }

                fieldValue = null;
                fieldTpName = fieldInfo.FieldType.Name.ToLower();   //转小写
                defaultValue = fieldInfo.GetValue(newObj);


                switch (fieldTpName)
                {
                    case FieldTpName.OBJECT:
                        //暂不支持object类型, 因为获取时无法确定实际类型
                        //TODO

                        break;
                    case FieldTpName.STRING:

                        fieldValue = GetString(saveName, (string)defaultValue);

                        break;
                    case FieldTpName.INT:
                    case FieldTpName.INT32:

                        fieldValue = GetInt(saveName, (int)defaultValue);

                        break;
                    case FieldTpName.UINT:
                    case FieldTpName.UINT32:

                        fieldValue = GetUInt(saveName, (uint)defaultValue);

                        break;
                    case FieldTpName.LONG:
                    case FieldTpName.ULONG:
                    case FieldTpName.INT64:
                    case FieldTpName.UINT64:

                        fieldValue = GetLong(saveName, (long)defaultValue);

                        break;
                    case FieldTpName.SINGLE:
                    case FieldTpName.FLOAT:

                        fieldValue = GetFloat(saveName, (float)defaultValue);

                        break;
                    case FieldTpName.DOUBLE:

                        fieldValue = GetDouble(saveName, (double)defaultValue);

                        break;
                    case FieldTpName.BOOL:

                        fieldValue = GetBool(saveName, (bool)defaultValue);

                        break;
                    case FieldTpName.BYTE_ARRAY:

                        fieldValue = GetByteArray(saveName, (byte[])defaultValue);

                        break;
                    case FieldTpName.COLOR:

                        fieldValue = GetColor(saveName, (Color)defaultValue);

                        break;
                    case FieldTpName.VECTOR2:

                        fieldValue = GetVector2(saveName, (Vector2)defaultValue);

                        break;
                    case FieldTpName.VECTOR3:

                        fieldValue = GetVector3(saveName, (Vector3)defaultValue);

                        break;
                    case FieldTpName.QUATERNION:

                        fieldValue = GetQuaternion(saveName, (Quaternion)defaultValue);

                        break;
                    default:

                        Type _type = fieldInfo.FieldType;
                        if (!_type.IsPrimitive)
                        {
                            fieldValue = __GetObj(_type, saveName);
                        }

                        break;
                }

                if (fieldValue != null)
                {
                    fieldInfo.SetValue(newObj, fieldValue);
                }

            }

            return newObj;
        }
       
    }

}