/* ==============================================================================
 * DisplayUtil
 * @author jr.zeng
 * 2016/6/12 15:19:18
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;


namespace mg.org
{
    public class DisplayUtil
    {


        /// <summary>
        /// 添加到父节点
        /// </summary>
        /// <param name="parent_">null时, 为根节点</param>
        /// <param name="child_"></param>
        static public bool AddChild(GameObject parent_, GameObject child_)
        {
            Transform transParent = null;

            if (parent_)
                transParent = parent_.transform;

            Transform transChild = child_.transform;
            if (transChild.parent == transParent)
                return false;

            if (transChild.parent == CCApp.Trash.transform)
            {
                //原来在垃圾桶
                CCApp.Trash.Pop(child_, parent_);
            }
            else
            {
                GameObjUtil.ChangeParent(child_, parent_);
            }

            if (child_.layer == 0)
            {
                //这里主动不设置镜头层, 除非儿子是默认层
                child_.layer = parent_ ? parent_.layer : 0;
            }

            //向下广播
            //child_.BroadcastMessage(CCMsgName.UpperParentChange, child_, SendMessageOptions.DontRequireReceiver);  //向下传递
            
            return true;
        }


        /// <summary>
        /// 从父节点移除
        /// </summary>
        /// <param name="child_"></param>
        static public void RemoveFromParent(GameObject child_)
        {
            CCApp.Trash.Push(child_);
        }

        /// <summary>
        /// 是否在垃圾桶
        /// </summary>
        /// <param name="child_"></param>
        /// <returns></returns>
        static public bool IsInTrash(GameObject child_)
        {
            return child_.transform.parent == CCApp.Trash.transform;
        }

        //-------∽-★-∽------∽-★-∽--------∽-★-∽Position相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        //获取位置
        static public Vector3 GetPos(GameObject go_)
        {
            return go_.transform.localPosition;
        }
        
        static public void SetPos(GameObject obj_, Vector3 pos_)
        {
            Transform trans = obj_.transform;
            trans.localPosition = pos_;
        }

        static public void SetPos(GameObject obj_, float x_, float y_, float z_)
        {
            Transform trans = obj_.transform;
            trans.localPosition = new Vector3(x_, y_, z_);
        }

        static public void SetPosX(GameObject go_, float x_)
        {
            SetPos2(go_, x_, go_.transform.localPosition.y);
        }

        static public void SetPosY(GameObject go_, float y_)
        {
            SetPos2(go_, go_.transform.localPosition.x, y_);
        }

        static public void SetPosZ(GameObject go_, float z_)
        {
            SetPos(go_, go_.transform.localPosition.x, go_.transform.localPosition.y, z_);
        }
        
        static public void SetPos2(GameObject go_, Vector2 pos_)
        {
            SetPos2(go_, pos_.x, pos_.y);
        }
        
        static public void SetPos2(GameObject go_, float x_, float y_)
        {
            Transform trans = go_.transform;
            trans.localPosition = new Vector3(x_, y_, trans.localPosition.z);
        }
        

        //-------∽-★-∽------∽-★-∽--------∽-★-∽Scale相关∽-★-∽--------∽-★-∽------∽-★-∽--------//

        static public Vector3 GetScale(GameObject go_)
        {
            return go_.transform.localScale;
        }

        static public void SetScale(GameObject obj_, float scaleX_, float scaleY_, float scaleZ_)
        {
            obj_.transform.localScale = new Vector3(scaleX_, scaleY_, scaleZ_);
        }

        static public void SetScaleX(GameObject obj_, float scaleX_)
        {
            SetScale2(obj_, scaleX_, obj_.transform.localScale.y);
        }

        static public void SetScaleY(GameObject obj_, float scaleY_)
        {
            SetScale2(obj_, obj_.transform.localScale.x, scaleY_);
        }
        

        static public void SetScale2(GameObject go_, Vector2 vec_)
        {
            SetScale2(go_, vec_.x, vec_.y);
        }

        static public void SetScale2(GameObject go_, float scale_)
        {
            SetScale2(go_, scale_, scale_);
        }

        static public void SetScale2(GameObject go_, float scaleX_, float scaleY_)
        {
            SetScale(go_, scaleX_, scaleY_, go_.transform.localScale.z);
        }
        

    


    }

}