using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    void Update()
    {
        float movSpeed = m_cameraMovementSpeed;
        float rotSpeed = m_cameraRotationSpeed;

        if(Input.GetKey(KeyCode.LeftShift))
        {
            movSpeed *= 2.0f;
            rotSpeed *= 2.0f;
        }

        if(Input.GetMouseButton(0))
        {
            Vector3 rotation = transform.eulerAngles;
            if (rotation.x > 180.0f) rotation.x -= 360.0f;

            rotation.y += (Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime);
            rotation.x -= (Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime);
            
            rotation.x = Mathf.Clamp(rotation.x, -90.0f, 90.0f);
            rotation.z = 0.0f;

            transform.eulerAngles = rotation;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = Vector3.zero;

        if (horizontal != 0.0f || vertical != 0.0f)
        {
            movement += (transform.right * horizontal);
            movement += (transform.forward * vertical);
        }

        if(Input.GetKey(KeyCode.Q))
        {
            movement += Vector3.up;
        }

        if(Input.GetKey(KeyCode.E))
        {
            movement += Vector3.down;
        }

        if(movement != Vector3.zero)
        {
            movement *= (movSpeed * Time.deltaTime);
            transform.position += movement;
        }
    }

    [SerializeField] private float m_cameraMovementSpeed = 10.0f;
    [SerializeField] private float m_cameraRotationSpeed = 100.0f;
}
