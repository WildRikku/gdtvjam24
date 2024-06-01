using UnityEngine;

namespace DTerrain {
    /// <summary>
    /// Example script: Spawining Unity gameobject with collider to show that DTerrain works with Unity Colliders 2D.
    /// Press B to spawn a ball.
    /// </summary>
    public class MouseBallSpawn : MonoBehaviour {
        [SerializeField]
        private GameObject ball = null;

        private void Update() {
            CreateBall();
        }

        public void CreateBall() {
            if (Input.GetKeyDown(KeyCode.B)) {
                Vector3 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mPos.z = 0;
                Instantiate(ball, mPos, new(0, 0, 0, 0));
            }
        }
    }
}