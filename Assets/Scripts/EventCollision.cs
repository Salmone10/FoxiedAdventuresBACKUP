using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCollision : MonoBehaviour
{
    [SerializeField] private UnityEvent _event;
    [SerializeField] private List<string> _tags = new List<string>();

    public void OnCollisionEnter2D(Collision2D collision)
    {
        print(_tags.Contains(collision.gameObject.tag));

        if (_tags.Contains(collision.gameObject.tag))
        {
            _event?.Invoke();
        }
    }
}
