using System;
using System.Collections.ObjectModel;

 
    public static class ICollectionExtension
    {
        /// <summary>
        /// 查找满足条件的对象
        /// </summary>
        /// <typeparam name="T">数据项</typeparam>
        /// <param name="list">集合</param>
        /// <param name="predicate">判断条件</param>
        /// <returns>存在返回对象,不存在返回null</returns>
        public static T Find<T>(this ObservableCollection<T> list, Predicate<T> predicate)
        {
            foreach (var item in list)
            {
                if (predicate(item))
                {
                    return item;
                }
            }
            return default(T);
        }

        /// <summary>
        /// 查找满足条件的对象集合
        /// </summary>
        /// <typeparam name="T">数据项</typeparam>
        /// <param name="list">集合</param>
        /// <param name="predicate">判断条件</param>
        /// <returns>返回满足条件的对象集合</returns>
        public static ObservableCollection<T> FindCollection<T>(this ObservableCollection<T> list, Predicate<T> predicate)
        {
            ObservableCollection<T> data = new ObservableCollection<T>();
            foreach (var item in list)
            {
                if (predicate(item))
                {
                    data.Add(item);
                }
            }
            return data;
        }

        /// <summary>
        /// 查找满足条件的对象是否存在
        /// </summary>
        /// <typeparam name="T">数据项</typeparam>
        /// <param name="list">集合</param>
        /// <param name="predicate">判断条件</param>
        /// <returns>true为存在,false为不存在</returns>
        public static bool Exists<T>(this ObservableCollection<T> list, Predicate<T> predicate)
        {
            foreach (var item in list)
            {
                if (predicate(item))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 查找满足条件的对象是否存在
        /// </summary>
        /// <typeparam name="T">数据项</typeparam>
        /// <param name="list">集合</param>
        /// <param name="t">要查找的对象</param>
        /// <returns>true为存在,false为不存在</returns>
        public static bool Exists<T>(this ObservableCollection<T> list, T t)
        {
            foreach (var item in list)
            {
                if (item.Equals(t))
                {
                    return true;
                }
            }
            return false;
        }
    }
 
