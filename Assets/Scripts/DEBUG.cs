using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D c)
    {
        Debug.Log($"COLLISION: my {c.otherCollider.name} (layer {LayerMask.LayerToName(c.otherCollider.gameObject.layer)}, trigger={c.otherCollider.isTrigger}) " + $"vs their {c.collider.name} (layer {LayerMask.LayerToName(c.collider.gameObject.layer)}, trigger={c.collider.isTrigger})");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log($"TRIGGER: myCollider? (on {name}) vs {col.name} (layer {LayerMask.LayerToName(col.gameObject.layer)}, trigger={col.isTrigger})");
    }

    private void Awake()
    {
        int p = LayerMask.NameToLayer("player");
        int pr = LayerMask.NameToLayer("projectile");
        Physics2D.IgnoreLayerCollision(p, pr, true);
    }
}
