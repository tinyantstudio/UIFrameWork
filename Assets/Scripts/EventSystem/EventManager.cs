using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace TinyFrameWork
{

    /// <summary>
    /// http://www.willrmiller.com/?p=87#comment-267
    /// Event Manager.
    /// </summary>
    public class EventManager
    {
        private Dictionary<string, Delegate> mEventDic = new Dictionary<string, Delegate>();
        public Dictionary<string, Delegate> EventDic
        {
            get { return this.mEventDic; }
            private set { }
        }

        public bool ContainsEvent(string eventType)
        {
            return this.mEventDic.ContainsKey(eventType);
        }

        public void CleanUp()
        {
            mEventDic.Clear();
        }

        private void CheckAddingNewListener(string eventType, Delegate listener)
        {
            if (!this.mEventDic.ContainsKey(eventType))
                this.mEventDic.Add(eventType, null);

            Delegate tmpDelegate = this.mEventDic[eventType];
            if (tmpDelegate != null && tmpDelegate.GetType() != listener.GetType())
            {
                throw new Exception(
                    string.Format("try to add incorrect eventtype {0}. needed listener type is {1}, given listener type is {2}.",
                    eventType,
                    tmpDelegate.GetType(),
                    listener.GetType()));
            }
        }

        private bool CheckRemovingListener(string eventType, Delegate listener)
        {
            bool result = false;
            if (!this.mEventDic.ContainsKey(eventType))
                result = false;
            else
            {
                Delegate tmpDelegate = this.mEventDic[eventType];
                if (tmpDelegate != null && tmpDelegate.GetType() != listener.GetType())
                {
                    throw new Exception(
                        string.Format("try to remove incorrect eventtype {0}. needed listener type is {1}, given listener type is {2}.",
                        eventType,
                        tmpDelegate.GetType(),
                        listener.GetType()));
                }
                result = true;
            }
            return result;
        }

        public void AddEventListener(string eventType, Action listener)
        {
            CheckAddingNewListener(eventType, listener);
            this.mEventDic[eventType] = (Action)Delegate.Combine((Action)this.mEventDic[eventType], listener);
        }

        public void AddEventListener<T>(string eventType, Action<T> listener)
        {
            CheckAddingNewListener(eventType, listener);
            this.mEventDic[eventType] = (Action<T>)Delegate.Combine((Action<T>)this.mEventDic[eventType], listener);
        }

        public void AddEventListener<T, U>(string eventType, Action<T, U> listener)
        {
            CheckAddingNewListener(eventType, listener);
            this.mEventDic[eventType] = (Action<T, U>)Delegate.Combine((Action<T, U>)this.mEventDic[eventType], listener);
        }

        public void AddEventListener<T, U, K>(string eventType, Action<T, U, K> listener)
        {
            CheckAddingNewListener(eventType, listener);
            this.mEventDic[eventType] = (Action<T, U, K>)Delegate.Combine((Action<T, U, K>)this.mEventDic[eventType], listener);
        }

        public void RemoveEventListener(string eventType, Action listener)
        {
            if (CheckRemovingListener(eventType, listener))
                this.mEventDic[eventType] = (Action)Delegate.Remove((Action)this.mEventDic[eventType], listener);
        }

        public void RemoveEventListener<T>(string eventType, Action<T> listener)
        {
            if (CheckRemovingListener(eventType, listener))
            {
                Delegate tmpDelegate = (Action<T>)Delegate.Remove((Action<T>)this.mEventDic[eventType], listener);
                // remove the last delegate will return null.
                this.mEventDic[eventType] = tmpDelegate;
            }
        }

        public void RemoveEventListener<T, U>(string eventType, Action<T, U> listener)
        {
            if (CheckRemovingListener(eventType, listener))
                this.mEventDic[eventType] = (Action<T, U>)Delegate.Remove((Action<T, U>)this.mEventDic[eventType], listener);
        }

        public void RemoveEventListener<T, U, K>(string eventType, Action<T, U, K> listener)
        {
            if (CheckRemovingListener(eventType, listener))
                this.mEventDic[eventType] = (Action<T, U, K>)Delegate.Remove((Action<T, U, K>)this.mEventDic[eventType], listener);
        }

        public void TriggerEvent(string eventType)
        {
            Delegate targetDelegate = null;
            if (this.mEventDic.TryGetValue(eventType, out targetDelegate))
            {
                if (targetDelegate == null)
                    return;

                Delegate[] invocationList = targetDelegate.GetInvocationList();
                for (int i = 0; i < invocationList.Length; i++)
                {
                    if (invocationList[i].GetType() != typeof(Action))
                        throw new Exception(string.Format("triggerEvnet {0} error : types of parameters are not match.", eventType));

                    Action action = (Action)invocationList[i];
                    try
                    {
                        action();
                    }
                    catch (System.Exception ex)
                    {
                        string exMsg = ex.ToString();
                        Debug.LogError(exMsg);
                    }
                }
            }
        }

        public void TriggerEvent<T>(string eventType, T params01)
        {
            Delegate targetDelegate = null;
            if (this.mEventDic.TryGetValue(eventType, out targetDelegate))
            {
                if (targetDelegate == null)
                    return;

                Delegate[] invocationList = targetDelegate.GetInvocationList();
                for (int i = 0; i < invocationList.Length; i++)
                {
                    if (invocationList[i].GetType() != typeof(Action<T>))
                        throw new Exception(string.Format("triggerEvnet {0} error : types of parameters are not match. Needed type {1}, given p1 type {2}.",
                            eventType, invocationList[i].GetType(),
                            params01.GetType()));

                    Action<T> action = (Action<T>)invocationList[i];
                    try
                    {
                        action(params01);
                    }
                    catch (System.Exception ex)
                    {
                        string exMsg = ex.ToString();
                        Debug.LogError(exMsg);
                    }
                }
            }
        }

        public void TriggerEvent<T, U>(string eventType, T params01, U params02)
        {
            Delegate targetDelegate = null;
            if (this.mEventDic.TryGetValue(eventType, out targetDelegate))
            {
                if (targetDelegate == null)
                    return;

                Delegate[] invocationList = targetDelegate.GetInvocationList();
                for (int i = 0; i < invocationList.Length; i++)
                {
                    if (invocationList[i].GetType() != typeof(Action<T, U>))
                        throw new Exception(string.Format(
                            "triggerEvnet {0} error : types of parameters are not match. Needed type{1}, given p1 p2 type [{2}], [type{3}].",
                            eventType,
                            invocationList[i].GetType(),
                            params01.GetType(),
                            params02.GetType()));

                    Action<T, U> action = (Action<T, U>)invocationList[i];
                    try
                    {
                        action(params01, params02);
                    }
                    catch (System.Exception ex)
                    {
                        string exMsg = ex.ToString();
                        Debug.LogError(exMsg);
                    }
                }
            }
        }

        public void TriggerEvent<T, U, K>(string eventType, T params01, U params02, K params03)
        {
            Delegate targetDelegate = null;
            if (this.mEventDic.TryGetValue(eventType, out targetDelegate))
            {
                if (targetDelegate == null)
                    return;

                Delegate[] invocationList = targetDelegate.GetInvocationList();
                for (int i = 0; i < invocationList.Length; i++)
                {
                    if (invocationList[i].GetType() != typeof(Action<T, U, K>))
                        throw new Exception(
                            string.Format("triggerEvnet {0} error : types of parameters are not match. Needed type{1}, given p1 p2 p3 type [{2}], [type{3}],[4].",
                            eventType,
                            invocationList[i].GetType(),
                            params01.GetType(),
                            params02.GetType(),
                            params03.GetType()));

                    Action<T, U, K> action = (Action<T, U, K>)invocationList[i];
                    try
                    {
                        action(params01, params02, params03);
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError(ex.Message);
                    }
                }
            }
        }
    }

}
