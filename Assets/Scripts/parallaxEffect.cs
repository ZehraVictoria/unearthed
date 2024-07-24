using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallaxEffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;

    Vector2 startingPos; // Starting pos for parallax game obj
    float startingZ; // start z value  of parallax game obj
    float zdistanceFromTarget => transform.position.z - followTarget.transform.position.z; 
    Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startingPos; // distance that cam has moved from starting pos of parallax obj
    float parallaxFactor => Mathf.Abs(zdistanceFromTarget) / clippingPlane; // the further obj from player, the faster parallaxEffect obj will move (drag its z value closer to target to make slower)
    float clippingPlane => (cam.transform.position.z + (zdistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        startingZ = transform.position.z;

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPosition = startingPos + camMoveSinceStart * parallaxFactor;
        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
    }
}
