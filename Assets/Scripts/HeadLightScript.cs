using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeadLightScript : MonoBehaviour
{
    private Light _headLight;
    private int hitDistance = 10; 
    void Start()
    {
        _headLight = GetComponentInChildren<Light>();
    }

    private void Update()
    {
        RaycastHit hit;
        Vector3 pos = transform.position;
        pos.y += 1;
        Debug.DrawRay(pos, transform.forward * hitDistance, Color.red);
    if (Physics.BoxCast(pos, new Vector3(0.5f, 0.5f, 0.5f), transform.forward, out hit, transform.rotation, hitDistance))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                if (_headLight.enabled)
            {
                CustomEvent.Trigger(hit.collider.gameObject,"isBlind");
            }
            }
        }
    }

    void OnFlashlight(InputValue value)
    {
        _headLight.enabled = !_headLight.enabled;
    }
    
}
