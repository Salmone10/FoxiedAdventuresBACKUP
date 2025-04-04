using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class InteractComponent : MonoBehaviour
{
    [SerializeField] private UnityEvent _event;

    public void InteractEvent() 
    {
        _event?.Invoke();
    }
}
