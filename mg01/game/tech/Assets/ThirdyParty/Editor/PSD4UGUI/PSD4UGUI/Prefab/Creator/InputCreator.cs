using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using LitJson;

using Object = UnityEngine.Object;

using mg.org.KUI;

namespace Edit.PSD4UGUI
{
    public class InputCreator : ContainerCreator
    {
        public const string PARAM_CENTER_FIT = "fitCenter";
        public static Regex PATTERN_CENTER_FIT = new Regex(PARAM_CENTER_FIT);

        public override string Identifier
        {
            get { return ComponentType.Input; }
        }

        public override GameObject Create(JsonData data, GameObject parent)
        {
            GameObject go = base.Create(data, parent);
            AddComponent(go);
            if (HasParam(data, PATTERN_CENTER_FIT))
            {
                //自适应父级的尺寸
                CenterFitChildren(go); 
            }
            return go;
        }

        protected void AddComponent(GameObject go)
        {
            KText[] textList = go.GetComponentsInChildren<KText>();
            KText text = null;
            KText textHolder = null;
            foreach (KText t in textList)
            {
                if (t.name.ToLower().Contains("textholder"))
                {
                    textHolder = t;
                }
                else
                {
                    text = t;
                }
            }

            //TextWrapper text = go.GetComponentInChildren<TextWrapper>();

            RectTransform textRect = text.GetComponent<RectTransform>();
            //textRect.pivot = new Vector2(0.5f, 0.5f);
            //textRect.anchoredPosition = new Vector2(textRect.sizeDelta.x * 0.5f, -textRect.sizeDelta.y * 0.5f);   //为什么要调整位置？
            text.supportRichText = false;

            KInputField input = go.AddComponent<KInputField>();
            input.textComponent = text; //主体文本

            if (textHolder != null)
            {
                input.placeholder = textHolder; //占位文本
            }

            if (text.text == "{0}")
            {
                input.text = "";
            }
            else
            {
                input.text = text.text;
            }
        }



        private void CenterFitChildren(GameObject go)
        {
            KInputField input = go.GetComponent<KInputField>();
            GameObject placeholder = input.placeholder.gameObject;
            GameObject inputtext = input.textComponent.gameObject;

            var parentTransform = go.GetComponent<RectTransform>();     //把inputtext变成这个尺寸， 然后居中对齐
            var placeholderTransform = placeholder.GetComponent<RectTransform>();
            var inputtextTransform = inputtext.GetComponent<RectTransform>();
            //placeholderTransform.sizeDelta = parentTransform.rect.size;
            //placeholderTransform.anchoredPosition = new Vector2(placeholderTransform.anchoredPosition.x, 0);

            inputtextTransform.sizeDelta = parentTransform.rect.size;
            //inputtextTransform.anchoredPosition = new Vector2(inputtextTransform.sizeDelta.x * 0.5f, -inputtextTransform.sizeDelta.y * 0.5f);
            KuiUtil.SetAnchorSmart(inputtextTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
            inputtextTransform.anchoredPosition = new Vector2(-inputtextTransform.sizeDelta.x * 0.5f, inputtextTransform.sizeDelta.y * 0.5f);

        }

    }


}