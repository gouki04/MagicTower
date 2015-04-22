using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NAT
{
    public interface ICoroutine
    {
        void ExecuteCoroutine(IEnumerator routine);
    }

    public class Coroutine
    {
        private static ICoroutine mImpl;

        public static void SetCoroutineImpl(ICoroutine coroutine_impl)
        {
            mImpl = coroutine_impl;
        }

        public static void Start(IEnumerator routine)
        {
            mImpl.ExecuteCoroutine(routine);
        }
    }
}
