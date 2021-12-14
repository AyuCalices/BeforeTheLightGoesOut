using UnityEngine;

namespace Features.Character_Namespace.Scripts
{
    public class CameraPositionBehaviour : MonoBehaviour
    {
        [SerializeField] private float CameraDistance = -2f;
        private Transform mainCameraTransform;
        private void Awake()
        {
            mainCameraTransform = Camera.main.transform;
        }
        private void Update()
        {
            mainCameraTransform.position = new Vector3(transform.position.x, transform.position.y, CameraDistance);
        }
    }
}
