using UnityEngine;

public class RotateAroundZ : MonoBehaviour {
    public float rotationSpeed = 10f; // Drehgeschwindigkeit in Grad pro Sekunde

    private void Update() {
        // Rotiert das GameObject um die Z-Achse
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}