using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectionZone : MonoBehaviour
{
    Collider2D col;
    public List<Collider2D> detectedColliders = new List<Collider2D>();
    // Start is called before the first frame update
    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        detectedColliders.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedColliders.Remove(collision);
    }
}
