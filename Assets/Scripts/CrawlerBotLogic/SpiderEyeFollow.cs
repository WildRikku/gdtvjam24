using UnityEngine;

public class SpiderEyeFollow : MonoBehaviour
{
    private Camera mainCamera;
    public Transform eyeCenter; 
    public float radius = 1f;
    public SpiderController spiderController;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!spiderController.isActive) return;
        
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, eyeCenter.position.z - mainCamera.transform.position.z));
        Vector3 direction = mousePosition - eyeCenter.position;
        direction.z = 0; 
        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
