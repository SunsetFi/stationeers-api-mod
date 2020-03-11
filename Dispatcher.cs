
using System;
using System.Collections.Generic;
using UnityEngine;

namespace WebAPI
{
    // From: https://answers.unity.com/questions/305882/how-do-i-invoke-functions-on-the-main-thread.html
    // TODO: Found UnityMainThreadDispatcher referenced in stationeers code, can we use this instead?
    public class Dispatcher : MonoBehaviour
    {
        public static void RunOnMainThread(Action action)
        {
            lock (_backlog)
            {
                _backlog.Add(action);
                _queued = true;
            }
        }

        public static void Initialize()
        {
            if (_instance == null)
            {
                _instance = new GameObject("Dispatcher").AddComponent<Dispatcher>();
                DontDestroyOnLoad(_instance.gameObject);
            }
        }

        private void Update()
        {
            if (_queued)
            {
                lock (_backlog)
                {
                    var tmp = _actions;
                    _actions = _backlog;
                    _backlog = tmp;
                    _queued = false;
                }

                foreach (var action in _actions)
                    action();

                _actions.Clear();
            }
        }

        static Dispatcher _instance;
        static volatile bool _queued = false;
        static List<Action> _backlog = new List<Action>(8);
        static List<Action> _actions = new List<Action>(8);
    }
}