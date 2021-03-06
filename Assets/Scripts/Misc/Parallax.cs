﻿using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour
{
    public float speed = 0.1f;									// Random value. the real value is set at the Unity Inspector.
	private Vector2 offset; 									// SPrite/texture offset. 

    Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    void FixedUpdate()
    {
		offset.Set (Time.time * speed, 0f); 												//new Vector2(Time.time * updated_spd, 0);
        renderer.material.mainTextureOffset = offset;			// We use offset so that the image comes back from right.
    }
}// end of scroll.