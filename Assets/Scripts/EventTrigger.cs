using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent _event;
    [SerializeField] private List<string> _tags = new List<string>();

    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (_tags.Contains(collision.gameObject.tag))
        {
            _event?.Invoke();
        }
    }
}
