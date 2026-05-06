namespace McsCoreLib.Tools.Algorithm
{
    public class AxisTool
    {
        /// <summary>
        ///  依据夹爪旋转速度计算开关盖Z轴旋转速度
        /// </summary>
        /// <param name="mDistance">Z轴导程</param>
        /// <param name="jawSpeed">夹爪旋转速度</param>
        /// <param name="mHousePitch">外盖螺纹距离</param>
        /// /// <param name="mPluse">Z轴旋转1圈的脉冲数</param>
        /// <returns></returns>
        public static float GetZAxisSpeed(float mDistance, float jawSpeed, float mHousePitch, int mPluse = 10000)
        {
            if (mDistance > 0)
            {
                var d = jawSpeed * mHousePitch / mDistance * mPluse;
                return d;
            }
            else return 0;
        }

        /// <summary>
        ///  依据外盖螺距获取Z轴旋转圈数
        /// </summary>
        /// <param name="mHousePitch">外盖螺距</param>
        /// <param name="mDistance">Z轴导程</param>
        /// <param name="mCounter">开盖圈数</param>
        /// <returns></returns>
        public static float GetZAxisDis(float mHousePitch, float mDistance, float mCounter)
        {
            if (mDistance > 0)
            {
                return mHousePitch * mCounter / mDistance;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        ///  依据导程获取电机旋转圈数
        /// </summary>
        /// <param name="mDistance">导程 mm</param>
        /// <param name="mPos">位置 mm</param>
        /// <returns></returns>
        public static float AxisDis(float mDistance, float mPos)
        {
            if (mDistance > 0)
            {
                return mPos / mDistance;
            }
            else return 0;
        }

        /// <summary>
        /// 依据导程获取实际位置
        /// </summary>
        /// <param name="mDistance"></param>
        /// <param name="mDis"></param>
        /// <returns></returns>
        public static float AxisPos(float mDistance, float mDis)
        {
            return mDistance * mDis;
        }

        /// <summary>
        ///  速度转脉冲数
        /// </summary>
        /// <param name="mSpeed">转速RPM</param>
        /// <param name="mPluse">每圈脉冲数</param>
        /// <returns></returns>
        public static int AxisSpeed(float mSpeed, int mPluse = 10000)
        {
            return (int)mSpeed * mPluse;
        }
    }
}