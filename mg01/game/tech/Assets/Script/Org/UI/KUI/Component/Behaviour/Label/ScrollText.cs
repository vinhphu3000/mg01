using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollText : MonoBehaviour {
    [SerializeField]
    public ScrollRect ScrollRectRef;
    [SerializeField]
    public InputField InputTextRef;

    private Text inputText;
    private RectTransform content;

    private void Start()
    {
        InputTextRef.onValueChanged.AddListener(OnTextValueChange);
        inputText = InputTextRef.textComponent;
        InputTextRef.lineType = InputField.LineType.MultiLineNewline;
        if (ScrollRectRef == null)
        {
            ScrollRectRef = gameObject.GetComponent<ScrollRect>();
        }
        content = ScrollRectRef.content.GetComponent<RectTransform>();
    }

    private void OnTextValueChange(string text)
    {
        float trueHeight = inputText.preferredHeight;
        Rect contentSize = content.rect;
        content.sizeDelta = new Vector2(contentSize.width, trueHeight);

        if (ScrollRectRef != null)
            ScrollRectRef.verticalNormalizedPosition = 0;
    }
}
