using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CoroutineComponent : MonoBehaviour, NAT.ICoroutine
{
    void Start()
    {
        NAT.Coroutine.SetCoroutineImpl(this);
    }

    #region ICoroutine 成员

    public void ExecuteCoroutine(System.Collections.IEnumerator routine)
    {
        StartCoroutine(routine);
    }

    #endregion
}
