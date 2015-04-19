using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoroutineComponent : MonoBehaviour
{
    public void LateUpdate()
    {
        CoroutineController.Instance.Update(Time.deltaTime);
    }
}
