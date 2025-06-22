using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Rotate : MonoBehaviour
{
    public GameObject cube;
    public float rotationSpeed = 720f; // degrees per second

    void Update() {
        if (cube != null) {
            cube.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }

}
