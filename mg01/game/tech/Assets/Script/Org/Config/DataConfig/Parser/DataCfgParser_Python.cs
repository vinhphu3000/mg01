/* ==============================================================================
 * DataCfgParser_Python
 * @author jr.zeng
 * 2016/9/17 9:47:37
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.IO;

namespace mg.org
{

    //配置数据的属性类型
    class CFG_PROPERTY_TP
    {
        public const string INT = "int";
        public const string FLOAT = "float";
        public const string STRING = "string";
    }


    public class DataCfgParser_Python : DataCfgParser
    {

        public DataCfgParser_Python()
        {

        }


        /// <summary>
        /// 解析配置文件
        /// </summary>
        /// <param name="source_"></param>
        /// <param name="dataTp_"></param>
        /// <returns></returns>
        override public object[] Parse(object source_, Type dataTp_)
        {

            byte[] bytes = source_ as byte[];

            MemoryStream fsRead = new MemoryStream(bytes);
            BinaryReader reader_ = new BinaryReader(fsRead);

            int tableDataLen = reader_.ReadInt32(); //读整形时,只要遇到10就出错。。
            int dataLen = reader_.ReadInt32();
            //int tableDataLen = (int)ReadValueFrom(reader_, CFG_PROPERTY_TP.INT);
            //int dataLen = (int)ReadValueFrom(reader_, CFG_PROPERTY_TP.INT);

            Type dataTp = dataTp_;
            object data;
            object[] datas = new object[tableDataLen];

            string key;
            string tp;
            object value;

            for (int i = 0; i < tableDataLen; ++i)
            {
                data = Activator.CreateInstance(dataTp);

                for (int j = 0; j < dataLen; ++j)
                {

                    key = ReadStrFrom(reader_);
                    tp = ReadStrFrom(reader_);
                    value = ReadValueFrom(reader_, tp);

                    FieldInfo fieldInfo = dataTp.GetField(key);
                    if (fieldInfo != null)
                    {
                        try
                        {
                            fieldInfo.SetValue(data, value);
                        }
                        catch
                        {
                            Log.Assert(false, "设置属性出错: " + key + " " + tp);
                            reader_.Close();
                            return null;
                        }
                    }
                }

                //尝试调用解析函数(用于在读取配置后进一步加工数据)
                ClassUtil.CallMethod(data, "Analyse");    
                datas[i] = data;
            }

            reader_.Close();
            return datas;
        }


        //从二进制读取字符串
        string ReadStrFrom(BinaryReader reader_)
        {
            int strLen = reader_.ReadInt32();   //读取长度
            if (strLen > 500)
            {
                //出错了
                Log.Assert(false);
            }
            byte[] by = reader_.ReadBytes(strLen);
            string result = System.Text.Encoding.UTF8.GetString(by);
            return result;
        }

        //从二进制读取数值
        object ReadValueFrom(BinaryReader reader_, string tp_)
        {
            object value;
            string str;

            switch (tp_)
            {
                case CFG_PROPERTY_TP.INT:
                    //整型
                    value = reader_.ReadInt32();
                    //str = ReadStrFrom(reader_);
                    //value = int.Parse(str);

                    break;
                case CFG_PROPERTY_TP.FLOAT:
                    //浮点型
                    value = reader_.ReadSingle();
                    //str = ReadStrFrom(reader_);
                    //value = float.Parse(str);

                    break;
                default:
                    //默认字符串
                    str = ReadStrFrom(reader_);
                    value = str;

                    break;
            }

            return value;
        }

    }





}