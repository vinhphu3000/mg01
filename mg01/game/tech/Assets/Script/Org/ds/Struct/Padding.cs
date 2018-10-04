/* ==============================================================================
 * 外框
 * @author jr.zeng
 * 2017/8/31 17:39:42
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org
{
    public struct Padding
    {
        public float left;
        public float top;
        public float right;
        public float bottom;
        
        public Padding(float left_ = 0, float top_ = 0, float right_ = 0, float bottom_ = 0)
        {
            left = left_;
            top = top_;
            right = right_;
            bottom = bottom_;
        }

        public static bool operator ==(Padding lhs, Padding rhs)
        {
            return lhs.left == rhs.left && 
                lhs.top == rhs.top && 
                lhs.right == rhs.right && 
                lhs.bottom == rhs.bottom;
        }

        public static bool operator !=(Padding lhs, Padding rhs)
        {
            return lhs.left != rhs.left || 
                lhs.top != rhs.top || 
                lhs.right != rhs.right || 
                lhs.bottom != rhs.bottom;
        }

        //重写Equals方法
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if ((obj.GetType().Equals(this.GetType())) == false)
                return false;

            Padding temp = (Padding)obj;
            return this.left.Equals(temp.left) && 
                    this.top.Equals(temp.top) &&
                    this.right.Equals(temp.right) &&
                    this.bottom.Equals(temp.bottom);

        }

        //重写GetHashCode方法（重写Equals方法必须重写GetHashCode方法，否则发生警告
        public override int GetHashCode()
        {
            return this.left.GetHashCode() + 
                    this.top.GetHashCode() + 
                    this.right.GetHashCode() + 
                    this.bottom.GetHashCode();
        }

    }

}