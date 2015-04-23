namespace Utils
{
    /// <summary>
    /// 单例模板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : new()
    {
        static protected T mInstance = default(T);

        protected Singleton()
        {
        }

        static public T Instance
        {
            get { return GetInstance(); }
        }

        static public T GetInstance()
        {
            if (mInstance == null)
                mInstance = new T();

            return mInstance;
        }
    } 
}