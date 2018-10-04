/* ==============================================================================
 * LayoutUtil
 * @author jr.zeng
 * 2017/8/31 16:19:00
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;


namespace mg.org.KUI
{

    /// <summary>
    /// 布局方向
    /// </summary>
    public enum LayoutDirection
    {
        TopToBottom,
        LeftToRight
    }


    public class LayoutUtil
    {

        /// <summary>
        /// 计算总行列数
        /// </summary>
        /// <param name="param_"></param>
        /// <param name="count_"></param>
        /// <returns></returns>
        public static int CalcTotalLine(LayoutParam param_, int count_)
        {
            if (count_ <= 0)
                return 0;

            return Mathf.CeilToInt(count_ / param_.divNum);
        }


        /// <summary>
        /// 计算单项位置
        /// </summary>
        /// <param name="param_"></param>
        /// <param name="index"></param>
        /// <param name="matchPivot_">匹配中点</param>
        /// <returns></returns>
        public static Vector2 CalcItemPos(LayoutParam param_, int index_, bool matchPivot_=true, LayoutItem item_=null)
        {
            Vector2 pos = new Vector2(param_.origin.x, param_.origin.y);

            Vector2 itemSize = param_.itemSize;
            int divNum = param_.divNum; //每行 / 列的个数

            
            if (param_.dir == LayoutDirection.TopToBottom)
            {
                //垂直
                pos.x = pos.x + param_.padding.left + (index_ % divNum) * (itemSize.x + param_.itemGap.x);
                pos.y = pos.y - param_.padding.top - Mathf.Floor(index_ / divNum) * (itemSize.y + param_.itemGap.y);    //参考系是左上
            }
            else
            {
                pos.x = pos.x + param_.padding.left + Mathf.Floor(index_ / divNum) * (itemSize.x + param_.itemGap.x);
                pos.y = pos.y - param_.padding.top - (index_ % divNum) * (itemSize.y + param_.itemGap.y);               //参考系是左上
            }

            if (item_ != null)
            {
                item_.rect.Set(pos.x, pos.y, itemSize.x, itemSize.y);
                item_.rect.y -= itemSize.y;     //Y轴映射为右下坐标系
            }
            
            //不匹配中心的话, pos是左上角的点
            if(matchPivot_)
            {
                if (!param_.pivot.Equals(LayoutParam.PIVOT_DEFAULT))
                {
                    //中点不是默认的, 要折算一下
                    pos.x = pos.x + (param_.pivot.x - LayoutParam.PIVOT_DEFAULT.x) * itemSize.x;
                    pos.y = pos.y + (param_.pivot.y - LayoutParam.PIVOT_DEFAULT.y) * itemSize.y;
                }
            }

            if (item_ != null)
            {
                item_.pos.Set(pos.x, pos.y);

                if (matchPivot_)
                {
                    item_.pivot.Set(param_.pivot.x, param_.pivot.y);
                }
                else
                {
                    item_.pivot.Set(LayoutParam.PIVOT_DEFAULT.x, LayoutParam.PIVOT_DEFAULT.y);
                }
            }

            return pos;
        }

        /// <summary>
        /// 单项布局
        /// </summary>
        /// <param name="param_"></param>
        /// <param name="index_"></param>
        /// <param name="item_"></param>
        /// <returns></returns>
        public static Vector2 LayItem(LayoutParam param_, int index_, GameObject item_)
        {
            RectTransform rectTrans = item_.GetComponent<RectTransform>();
            //RectTransform rectTrans = item_;

            param_.pivot = rectTrans.pivot;

            if (param_.itemSize.x * param_.itemSize.y == 0) //其中一个为0
            {
                //使用实际尺寸
                param_.itemSize.x = rectTrans.sizeDelta.x;
                param_.itemSize.y = rectTrans.sizeDelta.y;
            }
            
            Vector2 pos = CalcItemPos(param_, index_);
            rectTrans.anchoredPosition = pos;
            return pos;
        }

        /// <summary>
        /// 计算布局的总尺寸
        /// 容器
        /// </summary>
        /// <param name="param_"></param>
        /// <param name="itemNum"></param>
        /// <returns></returns>
        public static Vector2 CalcLayoutSize(LayoutParam param_, int itemNum)
        {
            Vector2 result = new Vector2();

            float col = 0;    //列数
            float row = 0;    //行数
            int num = Mathf.CeilToInt(itemNum / (float)param_.divNum);

            if (param_.dir == LayoutDirection.TopToBottom)
            {
                col = param_.divNum;
                row = num;
            }
            else
            {
                col = num;
                row = param_.divNum;
            }

            if (col * row > 0)
            {
                //有数量
                Padding padding = param_.padding;

                //param_.itemSize 这项一般会在LayItem里面赋值过, 这里再关心正确性
                result.x = padding.left + padding.right + col * param_.itemSize.x + (col - 1) * param_.itemGap.x;
                result.y = padding.top + padding.bottom + row * param_.itemSize.y + (row - 1) * param_.itemGap.y;

                //原点按中心在左上计算, 否则不计入
                if (param_.origin.x > 0)  
                {
                    result.x += param_.origin.x;
                }

                if (param_.origin.y < 0)
                {
                    //param_.origin.y越往下，高度越高
                    result.y -= param_.origin.y;
                }
            }
            
            return result;
        }



        //-------∽-★-∽------∽-★-∽--------∽-★-∽LayoutItem相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        static ClassPool2<LayoutItem> __itemPool = ClassPools.me.CreatePool<LayoutItem>();

        public static void CreateLayoutItems(LayoutParam param_, int totalNum_, List<LayoutItem> items_)
        {
            int remain = items_.Count - totalNum_;

            LayoutItem item;

            for (int i=0; i<totalNum_; ++i)
            {
                if (items_.Count <= i)
                {
                    item = __itemPool.Pop();
                    items_.Add(item);
                }
                item = items_[i];

                CalcItemPos(param_, i, true, item);
            }

            if (remain > 0)
            {

                for(int i = totalNum_ + remain-1; i >= totalNum_; --i)
                {
                    __itemPool.Push(items_[i]);
                    items_.RemoveAt(i);
                }
            }
        }

        public static void RecyleLayoutItems( List<LayoutItem> items_)
        {
            foreach(var item in items_)
            {
                __itemPool.Push(item);
            }

            items_.Clear();
        }

    }


    /// <summary>
    /// 布局参数
    /// </summary>
    public class LayoutParam
    {
        //默认中点
        public static Vector2 PIVOT_DEFAULT = new Vector2(0, 1);       //左上角

        //布局方向
        public LayoutDirection dir = LayoutDirection.TopToBottom;
        //每行/列的个数
        public int divNum = 1;
        //每项的尺寸
        public Vector2 itemSize;
        //每项间距(x水平间距，y垂直间距)
        public Vector2 itemGap = new Vector2(1, 1);
        //原点
        public Vector2 origin;
        //每项的中点
        public Vector2 pivot;
        //外框边距
        public Padding padding;

        public void Init(LayoutDirection dir_, 
                int divNum_,
                float itemSizeX_, float itemSizeY_,
                float itemGapX_, float itemGapY_,
                float originX_, float originY_,
                float pivotX_, float pivotY_,
                float paddingL_, float paddingT_, float paddingR_, float paddingB_)
        {
            dir = dir_;

            divNum = divNum_;

            itemSize.x = itemSizeX_; itemSize.y = itemSizeY_;

            itemGap.x = itemGapX_; itemGap.y = itemGapY_;

            origin.x = originX_;  origin.y = originY_;

            pivot.x = pivotX_; pivot.y = pivotY_;

            padding.left = paddingL_; padding.top = paddingT_; padding.right = paddingR_; padding.bottom = paddingB_;
        }

    }


    public class LayoutItem
    {
        //外框矩形
        public Rect rect;
        //位置
        public Vector2 pos;
        //中点
        public Vector2 pivot;

    }


}