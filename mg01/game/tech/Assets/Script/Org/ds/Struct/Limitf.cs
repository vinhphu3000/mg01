/* ==============================================================================
 * Limitf
 * @author jr.zeng
 * 2017/8/10 19:26:22
 * ==============================================================================*/
 

namespace mg.org
{
    public struct Limitf
    {
        public float min;
        public float max;
        

        public Limitf(float min_=0, float max_=1)
        {
            min = min_;
            max = max_;
        }


        public float Clamp(float value_)
        {
            if (value_ < min)
                return min;
            if (value_ > max)
                return max;
            return value_;
        }

        public float ClampNotZero(float value_)
        {
            if (min == 0 && max == 0)
                return value_;

            if (value_ < min)
                return min;
            if (value_ > max)
                return max;
            return value_;
        }



        public bool isZero
        {
            get { return min == 0 && max == 0; }
        }

        public static bool operator == (Limitf lhs, Limitf rhs)
        {
            return lhs.min == rhs.min && lhs.max == rhs.max;
        }

        public static bool operator !=(Limitf lhs, Limitf rhs)
        {
            return lhs.min != rhs.min || lhs.max != rhs.max;
        }

        //重写Equals方法
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if ((obj.GetType().Equals(this.GetType())) == false)
                return false;

            Limitf temp = (Limitf)obj;
            return this.min.Equals(temp.min) && this.max.Equals(temp.max);

        }

        //重写GetHashCode方法（重写Equals方法必须重写GetHashCode方法，否则发生警告
        public override int GetHashCode()
        {
            return this.min.GetHashCode() + this.max.GetHashCode();
        }

    }

}