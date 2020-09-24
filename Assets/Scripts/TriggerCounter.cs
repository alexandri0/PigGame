using System.Collections.Generic;
using UnityEngine;


//this class counts stones near to pig
[RequireComponent(typeof(BoxCollider2D))]
public class TriggerCounter : MonoBehaviour
{
    [SerializeField]
    List<Vector3> collision_with = new List<Vector3>();

    public List<Vector3> GetCollisions()
    {
        return collision_with;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Trigger")
            collision_with.Add(collision.gameObject.transform.position);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Trigger")
            collision_with.Remove(collision.gameObject.transform.position);
    }
}
