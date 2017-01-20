// #define ENABLE_UNITY_EVENT

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#if ENABLE_UNITY_EVENT
using UnityEngine.Events;
using UnityEngine.EventSystems;
#endif

namespace TinyFrameWork
{
    // 1. Use Unity's UnityEvent (Unity3d 4.6.9 version or later Unity3d version)
    // 2. Use C# Delegate
    // Two Way Compare result : http://jacksondunstan.com/articles/3335

    /// <summary>
    /// Event System With UnityEvent
    /// </summary>
#if ENABLE_UNITY_EVENT

    public class EventT<T> : UnityEvent<T>
    {
    }

    public class EventTT<T0, T1> : UnityEvent<T0, T1>
    {
    }

    public class EventTTT<T0, T1, T2> : UnityEvent<T0, T1, T2>
    {
    }

    public class EventTTTT<T0, T1, T2, T3> : UnityEvent<T0, T1, T2, T3>
    {
    }
    public class EventManagerWithUnityEvent
    {
        private Dictionary<int, UnityEventBase> _dicEvents = new Dictionary<int, UnityEventBase>();

        private void LogTypeError ( int eventId, object listener, string handleName )
        {
            Debug.LogError(string.Format("## Event Id {0}, [{1}] Wrong Listener Type {2}, needed Type {3}.",
                eventId,
                handleName,
                listener.GetType(),
                _dicEvents[eventId].GetType()));
        }

        #region Void 
        public void AddEventListener ( EventId evtId, UnityAction listener )
        {
            int eventId = (int) evtId;
            if (!_dicEvents.ContainsKey(eventId))
            {
                UnityEvent newEvent = new UnityEvent();
                newEvent.AddListener(listener);
                _dicEvents[eventId] = newEvent;
            }
            else
            {
                UnityEvent uEvent = _dicEvents[eventId] as UnityEvent;
                if (uEvent != null)
                {
                    uEvent.AddListener(listener);
                }
                else
                {
                    LogTypeError(eventId, listener, EventSystemDefine.dicHandleType[0]);
                }
            }
        }

        public void RemoveEventListener ( EventId evtId, UnityAction listener )
        {
            int eventId = (int) evtId;
            if (!_dicEvents.ContainsKey(eventId))
                return;
            else
            {
                UnityEvent uEvent = _dicEvents[eventId] as UnityEvent;
                if (uEvent != null)
                {
                    uEvent.RemoveListener(listener);
                }
                else
                {
                    LogTypeError(eventId, listener, EventSystemDefine.dicHandleType[1]);
                }
            }
        }

        public void TriggerEvent ( EventId evtId )
        {
            int eventId = (int) evtId;
            if (!_dicEvents.ContainsKey(eventId))
            {
                return;
            }
            else
            {
                UnityEvent uEvent = _dicEvents[eventId] as UnityEvent;
                if (uEvent != null)
                    uEvent.Invoke();
                else
                {
                    Debug.LogError(string.Format("## Event Trigger need Type {0} ", _dicEvents[eventId].GetType()));
                }
            }
        }
        #endregion

        #region One param
        public void AddEventListener<T> ( EventId evtId, UnityAction<T> listener )
        {
            int eventId = (int) evtId;
            if (!_dicEvents.ContainsKey(eventId))
            {
                EventT<T> newEvent = new EventT<T>();
                newEvent.AddListener(listener);
                _dicEvents[eventId] = newEvent;
            }
            else
            {
                EventT<T> uEvent = _dicEvents[eventId] as EventT<T>;
                if (uEvent != null)
                {
                    uEvent.AddListener(listener);
                }
                else
                {
                    LogTypeError(eventId, listener, EventSystemDefine.dicHandleType[0]);
                }
            }
        }

        public void RemoveEventListener<T> ( EventId evtId, UnityAction<T> listener )
        {
            int eventId = (int) evtId;
            if (!_dicEvents.ContainsKey(eventId))
                return;
            else
            {
                EventT<T> uEvent = _dicEvents[eventId] as EventT<T>;
                if (uEvent != null)
                {
                    uEvent.RemoveListener(listener);
                }
                else
                {
                    LogTypeError(eventId, listener, EventSystemDefine.dicHandleType[1]);
                }
            }
        }

        public void TriggerEvent<T> ( EventId evtId, T p0 )
        {
            int eventId = (int) evtId;
            if (!_dicEvents.ContainsKey(eventId))
            {
                return;
            }
            else
            {
                EventT<T> uEvent = _dicEvents[eventId] as EventT<T>;
                if (uEvent != null)
                    uEvent.Invoke(p0);
                else
                {
                    Debug.LogError(string.Format("## Event Trigger need Type {0}", _dicEvents[eventId].GetType()));
                }
            }
        }
        #endregion

        #region Two params
        public void AddEventListener<T0, T1> ( EventId evtId, UnityAction<T0, T1> listener )
        {
            int eventId = (int) evtId;
            if (!_dicEvents.ContainsKey(eventId))
            {
                EventTT<T0, T1> newEvent = new EventTT<T0, T1>();
                newEvent.AddListener(listener);
                _dicEvents[eventId] = newEvent;
            }
            else
            {
                EventTT<T0, T1> uEvent = _dicEvents[eventId] as EventTT<T0, T1>;
                if (uEvent != null)
                {
                    uEvent.AddListener(listener);
                }
                else
                {
                    LogTypeError(eventId, listener, EventSystemDefine.dicHandleType[0]);
                }
            }
        }

        public void RemoveEventListener<T0, T1> ( EventId evtId, UnityAction<T0, T1> listener )
        {
            int eventId = (int) evtId;
            if (!_dicEvents.ContainsKey(eventId))
                return;
            else
            {
                EventTT<T0, T1> uEvent = _dicEvents[eventId] as EventTT<T0, T1>;
                if (uEvent != null)
                {
                    uEvent.RemoveListener(listener);
                }
                else
                {
                    LogTypeError(eventId, listener, EventSystemDefine.dicHandleType[1]);
                }
            }
        }

        public void TriggerEvent<T0, T1> ( EventId evtId, T0 p0, T1 p1 )
        {
            int eventId = (int) evtId;
            if (!_dicEvents.ContainsKey(eventId))
            {
                return;
            }
            else
            {
                EventTT<T0, T1> uEvent = _dicEvents[eventId] as EventTT<T0, T1>;
                if (uEvent != null)
                    uEvent.Invoke(p0, p1);
                else
                {
                    Debug.LogError(string.Format("## Event Trigger need Type {0}", _dicEvents[eventId].GetType()));
                }
            }
        }
        #endregion

        #region Three params
        public void AddEventListener<T0, T1, T2> ( EventId evtId, UnityAction<T0, T1, T2> listener )
        {
            int eventId = (int) evtId;
            if (!_dicEvents.ContainsKey(eventId))
            {
                EventTTT<T0, T1, T2> newEvent = new EventTTT<T0, T1, T2>();
                newEvent.AddListener(listener);
                _dicEvents[eventId] = newEvent;
            }
            else
            {
                EventTTT<T0, T1, T2> uEvent = _dicEvents[eventId] as EventTTT<T0, T1, T2>;
                if (uEvent != null)
                {
                    uEvent.AddListener(listener);
                }
                else
                {
                    LogTypeError(eventId, listener, EventSystemDefine.dicHandleType[0]);
                }
            }
        }

        public void RemoveEventListener<T0, T1, T2> ( EventId evtId, UnityAction<T0, T1, T2> listener )
        {
            int eventId = (int) evtId;
            if (!_dicEvents.ContainsKey(eventId))
            {
                return;
            }
            else
            {
                EventTTT<T0, T1, T2> uEvent = _dicEvents[eventId] as EventTTT<T0, T1, T2>;
                if (uEvent != null)
                {
                    uEvent.RemoveListener(listener);
                }
                else
                {
                    LogTypeError(eventId, listener, EventSystemDefine.dicHandleType[1]);
                }
            }
        }

        public void TriggerEvent<T0, T1, T2> ( EventId evtId, T0 p0, T1 p1, T2 p2 )
        {
            int eventId = (int) evtId;
            if (_dicEvents.ContainsKey(eventId))
            {
                return;
            }
            else
            {
                EventTTT<T0, T1, T2> uEvent = _dicEvents[eventId] as EventTTT<T0, T1, T2>;
                if (uEvent != null)
                {
                    uEvent.Invoke(p0, p1, p2);
                }
                else
                {
                    Debug.LogError(string.Format("## Event Trigger need Type {0}", _dicEvents[eventId].GetType()));
                }
            }
        }
        #endregion
    }
#endif

    /// <summary>
    /// Event System With C# Delegate
    /// </summary>
    public class EventManager
    {
        private Dictionary<int, Delegate> _dicEvents = new Dictionary<int, Delegate>();

        private void LogTypeError ( EventId eventId, HandleType handleType, Delegate targetEventType, Delegate listener )
        {
            Debug.LogError(string.Format("## Event Id {0}, [{1}] Wrong Listener Type {2}, needed Type {3}.", eventId.ToString(),
                EventSystemDefine.dicHandleType[(int) handleType],
                targetEventType.GetType(),
                listener.GetType()));
        }

        private bool CheckAddEventListener ( EventId eventId, Delegate listener )
        {
            if (!this._dicEvents.ContainsKey((int) eventId))
                this._dicEvents.Add((int) eventId, null);
            Delegate tmDelegate = this._dicEvents[(int) eventId];
            if (tmDelegate != null && tmDelegate.GetType() != listener.GetType())
            {
                LogTypeError(eventId, HandleType.Add, _dicEvents[(int) eventId], listener);
                return false;
            }
            return true;
        }

        private bool CheckRemoveEventListener ( EventId eventId, Delegate listener )
        {
            if (!_dicEvents.ContainsKey((int) eventId))
                return false;

            Delegate tmpDel = _dicEvents[(int) eventId];
            if (tmpDel != null && tmpDel.GetType() != listener.GetType())
            {
                LogTypeError(eventId, HandleType.Remove, _dicEvents[(int) eventId], listener);
                return false;
            }
            return true;
        }

        #region Void
        public void AddEventListener ( EventId eventId, Action listener )
        {
            if (CheckAddEventListener(eventId, listener))
            {
                Delegate del = this._dicEvents[(int) eventId];
                _dicEvents[(int) eventId] = (Action) Delegate.Combine((Action) del, listener);
            }
        }

        public void RemoveEventListener ( EventId eventId, Action listener )
        {
            if (CheckRemoveEventListener(eventId, listener))
            {
                Delegate del = _dicEvents[(int) eventId];
                _dicEvents[(int) eventId] = Delegate.Remove(del, listener);
            }
        }

        public void TriggerEvent ( EventId eventId )
        {
            Delegate del = null;
            if (_dicEvents.TryGetValue((int) eventId, out del))
            {
                if (del == null)
                    return;

                Delegate[] invocationList = del.GetInvocationList();
                for (int i = 0; i < invocationList.Length; i++)
                {
                    Action action = invocationList[i] as Action;
                    if (action == null)
                    {
                        Debug.LogErrorFormat("## Trigger Event {0} Parameters type [void] are not match  target type : {1}.", eventId.ToString(), invocationList[i].GetType());
                        return;
                    }
                    action();
                }
            }
        }

        #endregion

        #region One param
        public void AddEventListener<T> ( EventId eventId, Action<T> listener )
        {
            if (CheckAddEventListener(eventId, listener))
            {
                Delegate del = _dicEvents[(int) eventId];
                _dicEvents[(int) eventId] = (Action<T>) Delegate.Combine((Action<T>) del, listener);
            }
        }

        public void RemoveEventListener<T> ( EventId eventId, Action<T> listener )
        {
            if (CheckRemoveEventListener(eventId, listener))
            {
                Delegate del = _dicEvents[(int) eventId];
                _dicEvents[(int) eventId] = Delegate.Remove(del, listener);
            }
        }

        public void TriggerEvent<T> ( EventId eventId, T p )
        {
            Delegate del = null;
            if (_dicEvents.TryGetValue((int) eventId, out del))
            {
                if (del == null)
                    return;

                Delegate[] invocationList = del.GetInvocationList();
                for (int i = 0; i < invocationList.Length; i++)
                {
                    Action<T> action = invocationList[i] as Action<T>;
                    if (action == null)
                    {
                        Debug.LogErrorFormat("## Trigger Event {0} Parameters type [ {1} ] are not match  target type : {2}. ",
                            eventId.ToString(),
                            p.GetType(),
                            invocationList[i].GetType());
                        return;
                    }
                    action(p);
                }
            }
        }
        #endregion

        #region Two params
        public void AddEventListener<T0, T1> ( EventId eventId, Action<T0, T1> listener )
        {
            if (CheckAddEventListener(eventId, listener))
            {
                Delegate del = _dicEvents[(int) eventId];
                _dicEvents[(int) eventId] = (Action<T0, T1>) Delegate.Combine((Action<T0, T1>) del, listener);
            }
        }

        public void RemoveEventListener<T0, T1> ( EventId eventId, Action<T0, T1> listener )
        {
            if (CheckRemoveEventListener(eventId, listener))
            {
                Delegate del = _dicEvents[(int) eventId];
                _dicEvents[(int) eventId] = Delegate.Remove(del, listener);
            }
        }

        public void TriggerEvent<T0, T1> ( EventId eventId, T0 p0, T1 p1 )
        {
            Delegate del = null;
            if (_dicEvents.TryGetValue((int) eventId, out del))
            {
                if (del == null)
                    return;

                Delegate[] invocationList = del.GetInvocationList();
                for (int i = 0; i < invocationList.Length; i++)
                {
                    Action<T0, T1> action = invocationList[i] as Action<T0, T1>;
                    if (action == null)
                    {
                        Debug.LogErrorFormat("## Trigger Event {0} Parameters type [ {1}, {2}] are not match  target type : {3}.",
                            eventId.ToString(),
                            p0.GetType(),
                            p1.GetType(),
                            invocationList[i].GetType());
                        return;
                    }
                    action(p0, p1);
                }
            }
        }
        #endregion

        #region Thress params
        public void AddEventListener<T0, T1, T2> ( EventId eventId, Action<T0, T1, T2> listener )
        {
            if (CheckAddEventListener(eventId, listener))
            {
                Delegate del = _dicEvents[(int) eventId];
                _dicEvents[(int) eventId] = (Action<T0, T1, T2>) Delegate.Combine((Action<T0, T1, T2>) del, listener);
            }
        }

        public void RemoveEventListener<T0, T1, T2> ( EventId eventId, Action<T0, T1, T2> listener )
        {
            if (CheckRemoveEventListener(eventId, listener))
            {
                Delegate del = _dicEvents[(int) eventId];
                _dicEvents[(int) eventId] = Delegate.Remove(del, listener);
            }
        }

        public void TriggerEvent<T0, T1, T2> ( EventId eventId, T0 p0, T1 p1, T2 p2 )
        {
            Delegate del = null;
            if (_dicEvents.TryGetValue((int) eventId, out del))
            {
                if (del == null)
                    return;
                Delegate[] invocationList = del.GetInvocationList();
                for (int i = 0; i < invocationList.Length; i++)
                {
                    Action<T0, T1, T2> action = invocationList[i] as Action<T0, T1, T2>;
                    if (action == null)
                    {
                        Debug.LogErrorFormat("## Trigger Event {0} Parameters type [{1}, {2}, {3}] are not match  target type : {4}.",
                            eventId.ToString(),
                            p0.GetType(),
                            p1.GetType(),
                            p2.GetType(),
                            invocationList[i].GetType());
                        return;
                    }
                    action(p0, p1, p2);
                }
            }
        }
        #endregion
    }
}
