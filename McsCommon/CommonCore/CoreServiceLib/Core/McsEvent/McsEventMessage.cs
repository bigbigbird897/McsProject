namespace CoreServiceLib.Core.McsEvent
{
    public class McsEventMessage
    {
        private static readonly McsEventMessage _instance = new();
        private readonly IEventAggregator mEventAggregator;

        public static McsEventMessage Instance
        { get { return _instance; } }

        private McsEventMessage()
        {
            mEventAggregator = ContainerLocator.Container.Resolve<IEventAggregator>();
        }

        /// <summary>
        ///  消息发布
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="message"></param>
        public void Publish<T>(T message)
        {
            mEventAggregator?.GetEvent<PubSubEvent<T>>().Publish(message);
        }

        /// <summary>
        ///  消息订阅
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="action">执行方法</param>
        /// <param name="option">方法执行线程选择</param>
        /// <param name="keepSubscriberReferenceAlive">强引用</param>
        public void Subscribe<T>(Action<T> action, ThreadOption option = ThreadOption.PublisherThread, bool keepSubscriberReferenceAlive = false)
        {
            _ = mEventAggregator?.GetEvent<PubSubEvent<T>>().Subscribe(action, option, keepSubscriberReferenceAlive, null);
        }

        /// <summary>
        ///  消息订阅
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="action">执行方法</param>
        /// <param name="option">方法执行线程选择</param>
        /// <param name="filter">过滤器</param>
        /// <param name="keepSubscriberReferenceAlive">强引用</param>
        public void Subscribe<T>(Action<T> action, Predicate<T> filter, ThreadOption option = ThreadOption.PublisherThread, bool keepSubscriberReferenceAlive = false)
        {
            _ = mEventAggregator?.GetEvent<PubSubEvent<T>>().Subscribe(action, option, keepSubscriberReferenceAlive, filter);
        }

        /// <summary>
        ///  取消订阅
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="action">执行方法</param>
        public void UnSubscribe<T>(Action<T> action)
        {
            mEventAggregator?.GetEvent<PubSubEvent<T>>().Unsubscribe(action);
        }
    }
}