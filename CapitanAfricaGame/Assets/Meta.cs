﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Meta : MonoBehaviour
{
    public CarController carController;
    private void OnTriggerEnter2D(Collider2D other) {
         SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }
}
