namespace CoreServiceLib.Services
{
    public class Signal
    {
        private bool isOld = false;
        private bool isDelayOn = false;
        private readonly bool isDelay = false;
        private readonly int delayTime = 0;
        private DateTime _time;
        public bool SiganlOn { get; private set; }

        public Signal()
        {
            SiganlOn = false;
            isDelayOn = false;
            isOld = false;
        }

        public Signal(int mdelayTime = 0)
        {
            SiganlOn = false;
            isDelayOn = false;

            isDelay = true;
            delayTime = mdelayTime;
        }

        public void ResetValue()
        {
            SiganlOn = false;
            isDelayOn = false;
        }

        public void SetValue(bool mValue)
        {
            if (isDelay)
            {
                if (isDelayOn)
                {
                    Console.WriteLine($"Delay is on{delayTime} -- {(DateTime.Now - _time).TotalSeconds}");
                }
                if (mValue != isOld && mValue)
                {
                    isDelayOn = true;
                    _time = DateTime.Now;
                }
                else if (isDelayOn && (DateTime.Now - _time).TotalSeconds >= delayTime)
                {
                    SiganlOn = true;
                    isDelayOn = false;
                }
                else if (isDelayOn && !mValue)
                {
                    isDelayOn = false;
                }
            }
            else
            {
                if (mValue != isOld && mValue)
                {
                    SiganlOn = true;
                }
            }

            isOld = mValue;
        }
    }

    public class Signal<T>
    {
        private T _Value;
        public bool SiganlOn { get; private set; }

        public void ResetValue()
        {
            SiganlOn = false;
        }

        public void SetValue(T mValue)
        {
            if (_Value != null && !_Value.Equals(mValue))
            {
                SiganlOn = true;
            }
            _Value = mValue;
        }
    }
}