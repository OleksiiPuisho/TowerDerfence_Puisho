namespace Helpers
{
    public static class EventAggregator
    {
        public static void Subscribe<T>(System.Action<object, T> eventCallBack)
        {
            Event<T>.EventCallback += eventCallBack;
        }
        public static void Unsubscribe<T>(System.Action<object, T> eventCallBack)
        {
            Event<T>.EventCallback -= eventCallBack;
        }
        public static void Post<T>(object sender, T eventData)
        {
            Event<T>.Post(sender, eventData);
        }
        private static class Event<T>
        {
            public static System.Action<object, T> EventCallback;

            public static void Post(object sender, T eventData)
            {
                EventCallback?.Invoke(sender, eventData);
            }
        }

    }
}

