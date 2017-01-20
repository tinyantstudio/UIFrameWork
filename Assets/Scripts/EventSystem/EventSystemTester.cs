using UnityEngine;
using System.Collections;


namespace TinyFrameWork
{
    /// <summary>
    /// Event example 事件系统使用
    /// 1. 继承IEventListener接口(IEventListener)
    /// 2. 初始化注册消息(register event when init.)
    /// 3. 退出注销消息(unregister event when quit)
    /// 4. Behaviour类型，一般在OnEnable和OnDisable中完成事件处理
    /// </summary>
    public class EventSystemTester : MonoBehaviour, IEventListener
    {

        void OnEnable ()
        {
            RegisterEvent();
        }

        void OnDisable ()
        {
            UnRegisterEvent();
        }
        /// <summary>
        /// register the target event message, set the call back method with params and event name.
        /// </summary>
        public void RegisterEvent ()
        {
            EventDispatcher.GetInstance().MainEventManager.AddEventListener<string>(EventId.TestUserInput, this.OnUserInput);
        }

        /// <summary>
        /// unregister the target event message.
        /// </summary>
        public void UnRegisterEvent ()
        {
            EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<string>(EventId.TestUserInput, this.OnUserInput);
        }

        private void OnUserInput ( string msg )
        {
            Debug.Log("[on User input message:" + msg + "]");
        }

        void Update ()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // trigger the event.
                EventDispatcher.GetInstance().MainEventManager.TriggerEvent<string>(EventId.TestUserInput, "Hello World!");
            }
        }
    }
}
