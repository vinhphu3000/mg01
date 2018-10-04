/* ==============================================================================
 * Size
 * @author jr.zeng
 * 2017/8/31 18:55:10
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org
{
    public struct Size
    {
        public float width;
        public float height;

        public Size(float width_ = 0, float height_ = 0)
        {
            width = width_;
            height = height_;
        }

        public bool isZero
        {
            get { return width == 0 && height == 0; }
        }

        public static bool operator ==(Size lhs, Size rhs)
        {
            return lhs.width == rhs.width && lhs.height == rhs.height;
        }

        public static bool operator !=(Size lhs, Size rhs)
        {
            return lhs.width != rhs.width || lhs.height != rhs.height;
        }

        //重写Equals方法
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if ((obj.GetType().Equals(this.GetType())) == false)
                return false;

            Size temp = (Size)obj;
            return this.width.Equals(temp.width) && this.height.Equals(temp.height);

        }

        //重写GetHashCode方法（重写Equals方法必须重写GetHashCode方法，否则发生警告
        public override int GetHashCode()
        {
            return this.width.GetHashCode() + this.height.GetHashCode();
        }

    }

}