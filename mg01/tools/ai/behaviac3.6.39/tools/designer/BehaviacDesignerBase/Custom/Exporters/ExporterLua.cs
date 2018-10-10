// 导出行为树的lua格式的配置

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using Behaviac.Design.Nodes;
using Behaviac.Design.Attributes;
using Behaviac.Design.Properties;
using Behaviac.Design.Attachments;

namespace Behaviac.Design.Exporters
{
    public class ExporterLua2 : Behaviac.Design.Exporters.Exporter
    {


        public static class PropertyDataType
        {
            public static readonly int Number = 1;
            public static readonly int Method = 2;
            public static readonly int Boolean = 3;
            public static readonly int String = 4;
        };

        public enum PropertyType
        {
            Keep,
            Method,
            Property,
        }

        //会导出的属性
        private Dictionary<string, PropertyType> _exportTypeMap = new Dictionary<string, PropertyType>()
        {
            {"Method", PropertyType.Method},
            {"Opl", PropertyType.Method},

            {"BinaryOperator", PropertyType.Keep},
            {"Operator", PropertyType.Keep},
            {"Phase", PropertyType.Keep},
            {"ResultOption", PropertyType.Keep},

            {"DoneWithinFrame", PropertyType.Property},
            {"DecorateWhenChildEnds", PropertyType.Property},
            {"Count", PropertyType.Property},
            {"Frames", PropertyType.Property},
            {"Opr", PropertyType.Property},
            {"Opr2", PropertyType.Property},
            {"Time", PropertyType.Property},
            {"Until", PropertyType.Property},
            {"Weight", PropertyType.Property},

            //NEW
            //--Action
            {"ResultFunctor", PropertyType.Method},
            //--Parallel
            {"ChildFinishPolicy", PropertyType.Keep},
            {"ExitPolicy", PropertyType.Keep},
            {"FailurePolicy", PropertyType.Keep},
            {"SuccessPolicy", PropertyType.Keep},
            //
            {"ReferenceFilename", PropertyType.Keep},
            {"Task", PropertyType.Keep},
            {"TriggeredOnce", PropertyType.Keep},
            {"TriggerMode", PropertyType.Keep},
            {"Prototype", PropertyType.Keep},

        };

        private Dictionary<string, PropertyType> ExportTypeMap
        {
            get { return _exportTypeMap; }
        }

        static bool use_name_trans = true;

        //名称转换
        private Dictionary<string, object> NAME_TRANS = new Dictionary<string, object>()
        {
            //node -> bev
            { "Sequence", "_seq_"},
            { "Parallel", "_paral_"},
            { "Selector", "_sel_"},
            { "IfElse", "_if_else_"},
            { "Wait", "_wait_"},
            { "Action", "_agent_action_"},
            { "Condition", "_agent_cond_"},
            { "DecoratorLoop", "_loop_"},
            { "SelectorProbability", "_rand_sel_"},

            //setting
            { "Count", "count"},
            { "Time", "time"},
            { "Method", "method"},
            { "ResultOption", "resultOpt"},
            { "ResultFunctor", "resultMethod"},
            { "Opl", "opl"},
            { "Opr", "opr"},
            { "Operator", "operator"},
            { "FailurePolicy", "failPolicy"},
            { "SuccessPolicy", "succPolicy"},

            //const
            { "BT_INVALID", 0},
            { "BT_RUNNING", 1},
            { "BT_SUCCESS", 2},
            { "BT_FAILURE", 3},
            { "SUCCEED_ON_ONE", 0},
            { "SUCCEED_ON_ALL", 1},
            { "FAIL_ON_ONE", 0},
            { "FAIL_ON_ALL", 1},
        };

        static string agent_property_evt_flag = "EVT_";

        //转换名称
        private string trans_name(string name)
        {
            if (!use_name_trans)
                return name;
            if (NAME_TRANS.ContainsKey(name))
                return NAME_TRANS[name].ToString();
            return name;
        }

        //转换属性值字符串
        private string trans_property(string name)
        {
            if (!use_name_trans)
                return _quota(name);

            if (NAME_TRANS.ContainsKey(name))
            {
                object value = NAME_TRANS[name];
                if (value is string)
                {
                    //是字符串,加上双引号
                    return _quota(value as string);
                }
                else if (value is bool)
                {
                    return (bool)value ? "true" : "false";
                }
                else
                {
                    return value.ToString();
                }
            }
            return _quota(name);
        }


        protected static string __usedNamespace = "Behaviac.Behaviors";

        /// <summary>
        /// The namespace the behaviours will be exported to.
        /// </summary>
        public static string UsedNamespace
        {
            get { return __usedNamespace; }
            set { __usedNamespace = value; }
        }

        private string _lua = String.Empty;
        public string Lua
        {
            get { return _lua; }
            set { _lua = value; }
        }


        public ExporterLua2(BehaviorNode node, string outputFolder, string filename, List<string> includedFilenames = null)
        : base(node, outputFolder, filename + ".lua", includedFilenames)
        {

        }

        //添加lua内容
        private void AddContent(string content, int indent, bool newLine = true)
        {
            for (int i = 0; i < indent; ++i)
            {
                this.Lua += "    "; // 4 spaces
            }
            this.Lua += content;
            this.Lua += newLine ? "\n" : "";
        }

        //加双引号
        private string _quota(string str)
        {
            return String.Format("\"{0}\"", str);
        }

        private void _err(string msg)
        {
            string str = "ERROR:\n" + msg;
            MessageBox.Show(str);
        }

        private void _debug(string msg, string flag = "")
        {
            MessageBox.Show(flag + "\n" + msg);
        }

        private bool IsNumber(string str)
        {
            System.Text.RegularExpressions.Regex rex = new System.Text.RegularExpressions.Regex(@"^((-{0,1}[0-9]+[\\.]?[0-9]+)|-{0,1}[0-9])$");
            return rex.IsMatch(str);
        }

        private string ParseStruct(string s)
        {
            s = s.Replace(';', ',');

            int bracket_pos = s.IndexOf('{');
            if (bracket_pos != 0)
            {
                _err(String.Format("parse struct err1!!! {0}", s));
                return s;
            }

            string str = String.Empty;
            string[] sList = s.Replace("{", "").Replace("}", "").Split(',');
            if (sList.Length <= 0) // {}
            {
                return s;
            }
            foreach (string mem in sList)
            {
                if (String.IsNullOrEmpty(mem)) // 末尾的逗号
                {
                    continue;
                }

                string[] mList = mem.Split('=');
                if (mList.Length != 2)
                {
                    _err(String.Format("parse struct err2!!! {0}", s));
                    return s;
                }
                string v = mList[1];
                if (String.IsNullOrEmpty(v))
                {
                    _err(String.Format("parse struct err3!!! {0}", s));
                    return s;
                }
                if (v.IndexOf('{') >= 0 || v.IndexOf('(') >= 0 || v.IndexOf("::") >= 0) // struct里面只能包含简单类型
                {
                    _err(String.Format("parse struct err4!!! {0}", s));
                    return s;
                }

                if (v == "true" || v == "false" || v[0] == '"')
                {
                    str += mem;
                }
                else if (IsNumber(v))
                {
                    str += String.Format("{0} = {1}", mList[0], float.Parse(v).ToString());
                }
                else // 枚举？ 加上双引号
                {
                    str += String.Format("{0}=\"{1}\"", mList[0], mList[1]);
                }
                str += ",";
            }
            return str;
        }

        private string ParseArrayElem(string elem)
        {
            // 字符串
            if (String.IsNullOrEmpty(elem))
            {
                return "";
            }
            char c = elem[0];
            if (c == '"')
            {
                return elem;
            }

            // number
            if (IsNumber(elem))
            {
                return float.Parse(elem).ToString();
            }

            // bool
            if (elem == "true" || elem == "false")
            {
                return elem;
            }

            // struct
            if (c == '{')
            {
                return ParseStruct(elem);
            }

            // 枚举？
            return '"' + elem + '"';
        }

        private string ParseMethodParam(string param)
        {
            char c = param[0];
            // number
            if (IsNumber(param))
            {
                return float.Parse(param).ToString();
            }

            // bool
            if (param == "true" || param == "false")
            {
                return param;
            }

            // struct {x=1;y=2;...} struct只应该含有基本类型(不包括数组)
            if (c == '{')
            {
                return ParseStruct(param);
            }

            // string
            if (c == '"')
            {
                return param;
            }

            int colon_pos = param.IndexOf(':');
            if (colon_pos <= 0)
            {
                return '"' + param + '"'; // 枚举?
            }

            // 数组  count:elem1|elem2...
            string elemStr = param.Substring(colon_pos + 1);
            if (String.IsNullOrEmpty(elemStr)) // 没有元素
            {
                return "{}";
            }
            string str = String.Empty;
            string[] elemList = elemStr.Split('|');
            for (int i = 0; i < elemList.Length; i++)
            {
                str += ParseArrayElem(elemList[i]);
                if (i != elemList.Length - 1)
                {
                    str += ",";
                }
            }
            return "{" + str + "}"; // {elem1, elem2, ...}
        }

        private string ParseMethodParams(string param)
        {
            if (String.IsNullOrEmpty(param))
            {
                return "";
            }

            string str = "";
            string[] paramList = param.Split(',');
            for (int i = 0; i < paramList.Length; i++)
            {
                string _param = paramList[i];
                if (_param.IndexOf("::") >= 0)
                {
                    //参数是函数
                    bool b = true;
                    str += ParseMethod(_param, out b);
                }
                else
                {
                    str += ParseMethodParam(paramList[i]);
                }

                if (i != paramList.Length - 1)
                {
                    str += ",";
                }
            }
            return str;
        }

        //解析"函数"
        private string ParseMethod(string method, out bool is_property)
        {
            string result = "";

            is_property = false;

            int colon_pos = method.LastIndexOf("::");
            if (colon_pos > 0) // Self.agent::func(arg1, arg2, ...)
            {
                int bracket_pos = method.LastIndexOf("(");
                if (bracket_pos >= 0)
                {
                    //agent的方法
                    int methodNameLen = bracket_pos - colon_pos - 2;
                    string methodName = method.Substring(colon_pos + 2, bracket_pos - colon_pos - 2);
                    string param = method.Substring(bracket_pos + 1, method.Length - bracket_pos - 2);
                    string MethodParams = ParseMethodParams(param);
                    result = String.Format("agent:{0}({1})", methodName, MethodParams);
                }
                else
                {
                    //agent的属性
                    string property = method.Substring(colon_pos + 2, method.Length - colon_pos - 2);
                    if (property.StartsWith(agent_property_evt_flag))
                    {
                        //事件名称
                        string evtName = property.Substring(agent_property_evt_flag.Length, property.Length-agent_property_evt_flag.Length);
                        result = String.Format("\"{0}\"", evtName);
                    }
                    else
                    {
                        result = String.Format("agent.{0}", property);
                    }

                    is_property = true;
                }
            }
            else
            {
                _err(String.Format("解析函数出错: {0} {1}", method));
            }

            return result;
        }

        private void ExportMethod(string propertyName, string method, int indent)
        {
            string newPropertyName = trans_name(propertyName);

            int colon_pos = method.LastIndexOf("::");
            if (colon_pos > 0) // Self.agent::func(arg1, arg2, ...)
            {
                bool is_property = false;
                string methodStr = ParseMethod(method, out is_property);
                AddContent(String.Format("{0} = function(agent) return {1} end,", newPropertyName, methodStr), indent);
                AddContent(String.Format("{0}Type = {1},", newPropertyName, (int)(is_property ? PropertyType.Property : PropertyType.Method)), indent);
            }
            else
            {
                _err(String.Format("export property:{0}, method:{1}", propertyName, method));
            }
        }

        private void ExportProperty(string propertyName, string property, int indent)
        {
            // method
            int bracket_pos = property.LastIndexOf("::");
            if (bracket_pos > 0)
            {
                ExportMethod(propertyName, property, indent);
                //AddContent(String.Format("{0}Type={1},", propertyName, PropertyDataType.Method), indent);   
                return;
            }

            string newPropertyName = trans_name(propertyName);

            // const [int|float|bool] xxx
            string[] info_list = property.Split(' ');
            if (info_list[0] == "const")
            {
                if (info_list[1] == "int" || info_list[1] == "float")
                {
                    AddContent(String.Format("{0} = {1},", newPropertyName, float.Parse(info_list[2])), indent);
                    //AddContent(String.Format("{0}Type={1},", propertyName, PropertyDataType.Number), indent);
                }
                else if (info_list[1] == "bool")
                {
                    AddContent(String.Format("{0} = {1},", newPropertyName, info_list[2].ToLower()), indent);
                    //AddContent(String.Format("{0}Type={1},", propertyName, PropertyDataType.Boolean), indent);
                }
                else if (info_list[1] == "string")
                {
                    string str = info_list[2];
                    if (!str.StartsWith("\""))
                        str = _quota(str);  //加引号

                    AddContent(String.Format("{0} = {1},", newPropertyName, str), indent);
                    //AddContent(String.Format("{0}Type={1},", propertyName, PropertyDataType.String), indent);
                }
                else
                {
                    _err(String.Format("export property not found1! name:{0} value:{1} ", propertyName, property));
                }

                AddContent(String.Format("{0}Type = {1},", newPropertyName, (int)PropertyType.Keep), indent);
                return;
            }

            // true false
            if (propertyName == "DoneWithinFrame" || propertyName == "DecorateWhenChildEnds" || propertyName == "Until")
            {
                AddContent(String.Format("{0} = {1},", propertyName, property), indent);
                //AddContent(String.Format("{0}Type={1},", propertyName, PropertyDataType.Boolean), indent);
                return;
            }

            _err(String.Format("export property not found2! name:{0} value:{1} ", propertyName, property));
        }

        private void ExportProperties(Node n, int indent)
        {
            IList<DesignerPropertyInfo> properties = n.GetDesignerProperties();

            bool isCustomAction = ExportCustomAction(n, indent);

            foreach (DesignerPropertyInfo p in properties)
            {
                if (p.Attribute.HasFlags(DesignerProperty.DesignerFlags.NoExport))
                {
                    continue;
                }

                object v = p.Property.GetValue(n, null);
                bool bExport = !Plugin.IsExportArray(v);

                if (bExport)
                {
                    string propValue = p.GetExportValue(n);
                    string name = p.Property.Name;

                    if (propValue != string.Empty && propValue != "\"\"")
                    {
                        if (!ExportTypeMap.ContainsKey(name))
                        {
                            _err(String.Format("not support property!!! name:{0} value:{0}", name, propValue));
                        }
                        else if (ExportTypeMap[name] == PropertyType.Method)
                        {
                            if (isCustomAction)
                                continue;

                            ExportMethod(name, propValue, indent);
                        }
                        else if (ExportTypeMap[name] == PropertyType.Property)
                        {
                            ExportProperty(name, propValue, indent);
                        }
                        else
                        {
                            AddContent(String.Format("{0} = {1},", trans_name(name), trans_property(propValue)), indent);
                        }
                    }
                }
            }
        }

        private bool ExportCustomAction(Node n, int indent)
        {
            if (n.ExportClass != "Action")
                return false;

            string propertyName = "Method";

            DesignerPropertyInfo propertyInfo = n.GetPropertyByName(propertyName);
            if (propertyInfo.Property.Name != propertyName)
                return false;

            MethodDef methodDef = propertyInfo.Property.GetValue(n, null) as MethodDef;
            string methodName = methodDef.BasicName;

            if (!(methodName.StartsWith("_") && methodName.EndsWith("_")))
                return false;

            int paramNum = methodDef.Params.Count;

            AddContent(String.Format("CustomClass = {0},", _quota(methodName)), indent);

            MethodDef.Param param;
            for (int i = 0; i < paramNum; ++i)
            {
                param = methodDef.Params[i];
                string value = param.Value.ToString();
                ExportProperty(param.Name, "const " + param.NativeType + " " + value, indent);
            }

            return true;
        }

        private void ExportProperties(Attachments.Attachment a, int indent)
        {
            //DesignerPropertyInfo propertyEffector = new DesignerPropertyInfo();
            IList<DesignerPropertyInfo> properties = a.GetDesignerProperties(true);

            foreach (DesignerPropertyInfo p in properties)
            {
                if (p.Attribute.HasFlags(DesignerProperty.DesignerFlags.NoExport))
                {
                    continue;
                }

                object v = p.Property.GetValue(a, null);
                bool bExport = !Plugin.IsExportArray(v);

                if (bExport)
                {
                    if (p.Property.Name != "Effectors")
                    {
                        string propValue = p.GetExportValue(a);
                        string name = p.Property.Name;

                        if (propValue != string.Empty && propValue != "\"\"")
                        {
                            if (!ExportTypeMap.ContainsKey(name))
                            {
                                _err(String.Format("not support property!!! name:{0} value:{0}", name, propValue));
                            }
                            else if (ExportTypeMap[name] == PropertyType.Method)
                            {
                                ExportMethod(name, propValue, indent);
                            }
                            else if (ExportTypeMap[name] == PropertyType.Property)
                            {
                                ExportProperty(name, propValue, indent);
                            }
                            else
                            {
                                AddContent(String.Format("{0} = {1},", p.Property.Name, _quota(propValue)), indent);
                            }
                        }
                    }
                }
            }
        }

        private void ExportAttachments(Node node, int indent)
        {
            if (node.Attachments.Count <= 0)
            {
                return;
            }

            AddContent("attachment = {", indent);
            foreach (Attachments.Attachment a in node.Attachments)
            {
                if (!a.Enable)
                {
                    continue;
                }


                Type type = a.GetType();
                AddContent("{", indent + 1);
                AddContent(String.Format("class = {0},", _quota(trans_name(node.ExportClass))), indent + 2);
                AddContent(String.Format("id = {0},", a.Id.ToString()), indent + 2);

                // 只支持Precondition和Effector
                //if (!(a.IsEffector || a.IsPrecondition))
                //{
                //    _err(String.Format("not precondition or effector! node:{0}", node.Id.ToString()));
                //}
                this.ExportProperties(a, indent + 2);
                AddContent("},", indent + 1); // end single
            }
            AddContent("},", indent); // end attachment
        }

        private void ExportNode(BehaviorNode behavior, Node node, int indent)
        {
            if (!node.Enable)
            {
                return;
            }

            AddContent("{", indent);
            AddContent(String.Format("class = {0},", _quota(trans_name(node.ExportClass))), indent + 1);
            AddContent(String.Format("id = {0},", node.Id.ToString()), indent + 1);

            this.ExportProperties(node, indent + 1);
            this.ExportAttachments(node, indent + 1);

            // 导出子节点
            if (!node.IsFSM && !(node is ReferencedBehavior) && node.Children.Count > 0)
            {
                AddContent("node = {", indent + 1);
                foreach (Node child in node.Children)
                {
                    if (node.GetConnector(child).IsAsChild)
                    {
                        this.ExportNode(behavior, child, indent + 2);
                    }
                }
                AddContent("},", indent + 1); // end child node
            }

            AddContent("},", indent); // end node
        }

        protected void ExportBehavior(BehaviorNode behavior)
        {
            if (behavior.FileManager == null)
            {
                _err("ERROR: file manager is null !!!");
                return;
            }

            this.Lua = String.Empty;
            int indent = 0;
            AddContent("-- EXPORTED BY TOOL, DON'T MODIFY IT!", indent);
            AddContent("-- Source File: " + behavior.MakeRelative(behavior.FileManager.Filename), indent);
            AddContent("return {", indent);
            AddContent("behavior = {", indent + 1);

            Behavior b = behavior as Behavior;
            Debug.Check(b != null);
            Debug.Check(b.Id == -1);
            if (b.IsFSM)
            {
                _err("cant export FSM!!!");
                return;
            }

            //'\\' ->'/'
            string behaviorName = b.MakeRelative(b.Filename);
            behaviorName = behaviorName.Replace('\\', '/');
            int pos = behaviorName.IndexOf(".lua");
            if (pos != -1)
            {
                behaviorName = behaviorName.Remove(pos);
            }

            AddContent(String.Format("name = {0},", _quota(behaviorName)), indent + 2);
            AddContent(String.Format("agentType = {0},", _quota(b.AgentType.Name)), indent + 2);
            AddContent(String.Format("version = {0},", _quota(b.Version.ToString())), indent + 2);
            this.ExportProperties(b, indent + 2);
            this.ExportAttachments(b, indent + 2);
            // 导出子节点
            AddContent("node = {", indent + 2);
            foreach (Node child in ((Node)behavior).Children)
            {
                this.ExportNode(behavior, child, indent + 3);
            }
            AddContent("},", indent + 2); // end node
            AddContent("},", indent + 1); // end behavior
            AddContent("}", indent); // end return
            return;
        }

        public override FileManagers.SaveResult Export()
        {
            string filename = Path.Combine(_outputFolder, _filename);
            FileManagers.SaveResult result = FileManagers.FileManager.MakeWritable(filename, Resources.ExportFileWarning);
            if (FileManagers.SaveResult.Succeeded != result)
            {
                return result;
            }

            string folder = Path.GetDirectoryName(filename);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            using (StreamWriter file = new StreamWriter(filename))
            {
                ExportBehavior(_node);
                file.Write(this.Lua);
                file.Close();
            }
            return FileManagers.SaveResult.Succeeded;
        }
    }
}
