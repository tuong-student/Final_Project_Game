using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NOOD
{
    public static class InstanceHolder
    {
        public static Dictionary<Type, object> InstanceDic = new Dictionary<Type, object>();

        public static void AddToDic(object instance)
        {
            if(InstanceDic.ContainsKey(instance.GetType()))
            {
                InstanceDic[instance.GetType()] = instance;
            }
            else
            {
                InstanceDic.Add(instance.GetType(), instance);
            }
        }
        public static void RemoveFromDic(object instance)
        {
            if(InstanceDic.ContainsKey(instance.GetType()))
            {
                InstanceDic.Remove(instance.GetType());
            }
        }
        public static T GetInstance<T>()
        {
            return (T)InstanceDic[typeof(T)];
        }
    }

    public class MonoBehaviorInstance<T> : MonoBehaviour 
    {
        public static readonly object lockObject = new object();
        public Type type;

        public static T Instance
        {
            get
            {
                lock(lockObject)
                {
                    return InstanceHolder.GetInstance<T>();
                }
            }
        }

        protected virtual void Awake()
        {
            InstanceHolder.AddToDic(this);
        }
    }

}

