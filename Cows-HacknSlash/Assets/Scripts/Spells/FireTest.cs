﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTest : MonoBehaviour {

    [SerializeField]
    [Range (1, 45)]
    private float FiringAngle = 45f;

    [SerializeField]
    private float Gravity = 9.8f;

    [SerializeField]
    private float WaitTime = 0f;

    private Vector3 target;
	
    public void Shoot(Transform FirePrefab) {
         StartCoroutine(FireMovement(FirePrefab));
    }

    private IEnumerator FireMovement(Transform FirePrefab) {
        yield return new WaitForSeconds(WaitTime);
        FirePrefab.position = transform.position;

        // getting the mouse input position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero); // using a simple plane cause our camera is on top
        float dist;
        if (plane.Raycast(ray, out dist)) {
            target = ray.GetPoint(dist);
        }

        // Calculate distance to target
        float target_Distance = Vector3.Distance(FirePrefab.position, target);

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float velocity = target_Distance / (Mathf.Sin(2 * FiringAngle * Mathf.Deg2Rad) / Gravity);

        // Extract the X  Y componenent of the velocity
        float x = Mathf.Sqrt(velocity) * Mathf.Cos(FiringAngle * Mathf.Deg2Rad);
        float y = Mathf.Sqrt(velocity) * Mathf.Sin(FiringAngle * Mathf.Deg2Rad);

        // Calculate flight time.
        float flightDutation = target_Distance / 2;

        // Rotate object to face the target.
        FirePrefab.rotation = Quaternion.LookRotation(target - FirePrefab.position);

        float elapsed_time = 0;

        while (elapsed_time < flightDutation) {
            if (FirePrefab != null)
                FirePrefab.Translate(0f, (y - (Gravity * elapsed_time)) * Time.deltaTime, x * Time.deltaTime);
            elapsed_time += Time.deltaTime;
            yield return null;
        }

    }

}
