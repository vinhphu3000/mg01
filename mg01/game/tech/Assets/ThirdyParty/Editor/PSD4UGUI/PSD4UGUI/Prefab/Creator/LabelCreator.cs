using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

using LitJson;
using mg.org.KUI;


namespace Edit.PSD4UGUI
{
    public class LabelCreator : ComponentCreator
    {


        /// <summary>
        /// 缩放文本框中文本比例使其在文本框内完全显示
        /// </summary>
        public static Regex PATTERN_BEST_FIT = new Regex(@"bestFit", RegexOptions.IgnoreCase);

        /// <summary>
        /// 强制对不含有粗体的字体显示为粗体效果
        /// </summary>
        public static Regex PATTERN_BOLD = new Regex(@"bold", RegexOptions.IgnoreCase);

        /// <summary>
        /// 斜体
        /// </summary>
        public static Regex PATTERN_ITALIC = new Regex(@"italic", RegexOptions.IgnoreCase);

        /// <summary>
        /// 多行
        /// </summary>
        public static Regex PATTERN_OVERFLOW = new Regex(@"overFlow", RegexOptions.IgnoreCase);
        public static Regex PATTERN_OVERFLOW_V = new Regex(@"overVFlow", RegexOptions.IgnoreCase);

        /// <summary>
        /// 
        /// </summary>
        public static Regex PATTERN_WRAP = new Regex(@"wrap", RegexOptions.IgnoreCase);

        /// <summary>
        /// 文本对齐
        /// </summary>
        public static string PATTERN_ALIGNMENT = "alignment";

        /// <summary>
        /// 描边
        /// </summary>
        public static string PATTERN_STROKE = "stroke";

        /// <summary>
        /// 阴影
        /// </summary>
        public static string PATTERN_SHADOW = "shadow";

        

        public static Regex PATTERN_UNDERLINE = new Regex(@"underline", RegexOptions.IgnoreCase);
        public override string Identifier { get { return ComponentType.Label; } }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽Create∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public override GameObject Create(JsonData data, GameObject parent)
        {
            GameObject go = CreateGameObject(parent, data);
            if (HasOnlyNormalState(data) == true)
            {
                //当文本只有normal态时，直接在go上添加Image组件，不创建名为normal的子go
                AddTextComponent(go, data[STATE_NORMAL]);
            }
            else
            {
                CreateStateList(go, data);
                go.AddComponent<StateText>();
            }
            ShowDefaultState(go);
            ApplyGameObjectParam(go, data);
            return go;
        }

        private void CreateStateList(GameObject go, JsonData data)
        {
            foreach (string key in data.Keys)
            {
                if (data[key].IsObject == true)
                {
                    JsonData stateData = data[key];
                    GameObject stateGo = CreateStateGameObject(go, stateData, key);
                    AddTextComponent(stateGo, stateData);
                    stateGo.SetActive(false);
                }
            }
        }

        protected virtual void AddTextComponent(GameObject go, JsonData stateData)
        {
            KText text = go.AddComponent<KText>();
            text.supportRichText = true;
            JsonData formatData = stateData["format"];
            text.color = GetColor((string)formatData["color"], 100);
            text.font = KAssetManager.GetFont((string)formatData["font"]);
            text.fontSize = (int)formatData["size"];
            text.text = (string)stateData["content"];
            text.material = KAssetManager.GetFontMaterial();
            if (text.text == "{0}")
            {
                text.text = "?";
            }
            //单行文本默认值，超出文本范围不显示
            text.horizontalOverflow = HorizontalWrapMode.Overflow;  //改为自动增长
            text.verticalOverflow = VerticalWrapMode.Truncate;
            //多行文本垂直方向超出范围仍然显示
            float h = 0.0f;
            if (stateData["height"].IsInt)
            {
                h = (float)(int)stateData["height"];
            }
            else
            {
                h = (float)(double)stateData["height"];
            }
            bool isMutilple = IsMutilpleLine(h, (int)formatData["size"]); //多行
            if (isMutilple)
            {
                text.horizontalOverflow = HorizontalWrapMode.Wrap;
                text.verticalOverflow = VerticalWrapMode.Overflow;
            }
            else//单行，检查overflow是否设置
            {
                if (HasParam(stateData, PATTERN_OVERFLOW) == true)
                {
                    text.horizontalOverflow = HorizontalWrapMode.Overflow;  //水平多行
                }
                else if (HasParam(stateData, PATTERN_WRAP) == true)
                {
                    text.horizontalOverflow = HorizontalWrapMode.Wrap;  //超出文本范围不显示
                }

                if (HasParam(stateData, PATTERN_OVERFLOW_V) == true)
                {
                    text.verticalOverflow = VerticalWrapMode.Overflow;      //垂直多行
                    //如果直接指定垂直方向overflow，那横向将自动设为换行
                    text.horizontalOverflow = HorizontalWrapMode.Wrap;
                }
            }
            if (stateData.Keys.Contains("langId") == true)  //语言包,暂时没用
            {
                text.langId = (int)stateData["langId"];
            }
            text.alignment = TextAnchor.UpperLeft;  //文本对齐方式
            if (stateData.Keys.Contains(PATTERN_ALIGNMENT) == true)
            {
                string alignment = (string)stateData[PATTERN_ALIGNMENT];
                text.alignment = (TextAnchor)Enum.Parse(typeof(TextAnchor), alignment, true);
            }
            text.lineSpacing = 1.0f;   
            if (stateData.Keys.Contains("lineSpacing") == true)   //行距
            {
                text.lineSpacing = float.Parse((string)stateData["lineSpacing"]);
            }
            text.resizeTextForBestFit = GetBestFitParam(stateData);   //缩放文本框来适应文本框
            int bold = (int)formatData["bold"]; //粗体
            text.fontStyle = (bold == 1) ? FontStyle.Bold : FontStyle.Normal;
            text.fontStyle = GetFontStyleParam(stateData);  //覆盖了上面。。
            AddUnderline(go, stateData);    //添加下划线
            AddGradientEffect(go, stateData);
            AddStrokeEffect(go, stateData); //添加描边
            AddShadowEffect(go, stateData); //添加阴影
            if (isMutilple == false)
            {
                SetPreferredSize(go);
            }
            if (go.name == STATE_DISABLE)
            {
                text.grey = true;   //置灰
            }

        }

        private bool IsMutilpleLine(float height, int fontSize)
        {
            return height > fontSize * 2;
        }

        private bool GetBestFitParam(JsonData stateData)
        {
            if (HasParam(stateData, PATTERN_BEST_FIT) == true)
            {
                return true;
            }
            return false;
        }

        private FontStyle GetFontStyleParam(JsonData stateData)
        {
            if (HasParam(stateData, PATTERN_BOLD) && HasParam(stateData, PATTERN_ITALIC))
            {   //粗+斜
                return FontStyle.BoldAndItalic;
            }
            if (HasParam(stateData, PATTERN_BOLD) == true)
            {   //粗
                return FontStyle.Bold;
            }
            if (HasParam(stateData, PATTERN_ITALIC) == true)
            {   //斜
                return FontStyle.Italic;
            }
            return FontStyle.Normal;
        }

        private void AddUnderline(GameObject go, JsonData stateData)
        {
            if (HasParam(stateData, PATTERN_UNDERLINE) == true)
            {
                JsonData formatData = stateData["format"];
                Color color = GetColor((string)formatData["color"], 100);
                Underline ul = go.AddComponent<Underline>();
                ul.UnderlineColor = color;
            }
        }

        private void AddStrokeEffect(GameObject go, JsonData stateData)
        {
            //"stroke":{"distance":1,"color":"340303","alpha":100}

            if (stateData.Keys.Contains(PATTERN_STROKE) == true)
            {
                Outline outline = go.AddComponent<Outline>();
                JsonData strokeData = stateData[PATTERN_STROKE];
                int distance = (int)strokeData["distance"];
                //Ps中描边特效参数应用到Unity中时要乘0.5
                outline.effectDistance = new Vector2(distance * 0.5f, -1 * distance * 0.5f);
                outline.effectColor = GetColor(strokeData);
            }
        }

        private void AddShadowEffect(GameObject go, JsonData stateData)
        {
            //"shadow":{"distance":4,"angle":90,"color":"2E1301","alpha":49}

            if (stateData.Keys.Contains(PATTERN_SHADOW) == true)
            {
                Shadow shadow = go.AddComponent<Shadow>();
                JsonData shadowData = stateData[PATTERN_SHADOW];
                int distance = (int)shadowData["distance"];
                int angle = (int)shadowData["angle"];
                shadow.effectDistance = new Vector2(-1 * distance * Mathf.Cos(angle * Mathf.PI / 180.0f), -1 * distance * Mathf.Sin(angle * Mathf.PI / 180.0f));
                shadow.effectColor = GetColor(shadowData);
            }
        }


        private void AddGradientEffect(GameObject go, JsonData stateData)
        {
            if (stateData.Keys.Contains("gradient") == true)
            {
                GradientText gradient = go.AddComponent<GradientText>();
                JsonData gradientData = stateData["gradient"];

                JsonData colorArray = gradientData["colors"];
                JsonData precentArray = gradientData["precents"];
                int count = colorArray.Count;
                List<Color32> colors = new List<Color32>();
                List<float> precents = new List<float>();
                for (int i = 0; i < count; i++)
                {
                    colors.Add(GetColor(colorArray[i].ToString(), 100));
                    precents.Add(1 - float.Parse(precentArray[i].ToString()));
                }
                // hard一下尽量接近ps效果 QAQ
                if (count == 2)
                {
                    colors.Insert(1, colors[1]);
                    precents.Insert(1, 0.5f);
                }
                colors.Reverse();
                precents.Reverse();
                gradient.gradientColors = colors.ToArray();
                gradient.gradientPoses = precents.ToArray();
            }
        }

        private void SetPreferredSize(GameObject go)
        {
            RectTransform rect = go.GetComponent<RectTransform>();
            float width = rect.sizeDelta.x;
            float height = rect.sizeDelta.y;

            float preWidth = LayoutUtility.GetPreferredWidth(rect);
            if (preWidth > width)
            {
                width = (float)Math.Ceiling(preWidth);
                rect.sizeDelta = new Vector2(width, height);
            }
            //preferredHeight在修改width的时候会动态变化
            //2017.3.16在psd解析脚本解决了高度问题，以下代码暂时不执行
            //float preHeight = LayoutUtility.GetPreferredHeight(rect);
            //if (preHeight > height)
            //{
            //    height = (float)Math.Ceiling(preHeight);
            //    rect.sizeDelta = new Vector2(width, height);
            //}

        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽ApplyGameObjectParam∽-★-∽--------∽-★-∽------∽-★-∽--------//

        public override void ApplyGameObjectParam(GameObject go, JsonData data)
        {

            if (HasParam(data, PATTERN_ANCHOR) == true)     //为什么label不判断锚点？先加上看看 jr.zeng@20180714
            {
                //锚点
                SetAnchor(go, data);
            }

            if (HasParam(data, ComponentCreator.PATTERN_HIDE) == true)
            {
                go.SetActive(false);
            }

            if (HasParam(data, ComponentCreator.PATTERN_STATIC) == true)
            {
                AddBuildHelper(go, PARAM_STATIC);
            }
        }


    }

}