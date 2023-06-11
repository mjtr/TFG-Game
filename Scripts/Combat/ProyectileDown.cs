using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectileDown : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Velocidad de movimiento de la bola de fuego
    [SerializeField] private float downwardForce = 2f; // Fuerza hacia abajo

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Aplicar una velocidad inicial hacia adelante
        rb.velocity = transform.forward * speed;
    }

    private void FixedUpdate()
    {
        // Aplicar una fuerza hacia abajo constante para que la bola de fuego se incline
        rb.AddForce(Vector3.down * downwardForce, ForceMode.Force);
    }
}
