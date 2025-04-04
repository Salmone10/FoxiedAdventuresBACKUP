using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTriggerStay : MonoBehaviour
{
    [SerializeField] private UnityEvent _event;
    [SerializeField] private List<string> _tags = new List<string>();

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (_tags.Contains(collision.gameObject.tag))
        {
            _event?.Invoke();
        }
    }
}
