using UnityEngine;

public class SpiderEyeFollow : MonoBehaviour
{
    private Camera mainCamera;
    public Transform eyeCenter; 
    public float radius = 1f;
    public SpiderController spiderController;

    public Material activSpiderMat;
    public Material inactivSpiderMat;
    public MeshRenderer sphereRenderer;

    private void Awake()
    {
        mainCamera = Camera.main;
        sphereRenderer.material = inactivSpiderMat;
    }

    void Update()
    {
        if (!spiderController.isActive)
        {
            sphereRenderer.material = inactivSpiderMat;
            return;
        }

        Vector3 position = eyeCenter.position;
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, position.z - mainCamera.transform.position.z));
        Vector3 direction = mousePosition - position;
        direction.z = 0; 
        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            sphereRenderer.material = activSpiderMat;
        }
    }
}
