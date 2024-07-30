using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    float speed;
    float sensitivity;

    private float yaw = 0f;
    private float pitch = 0f;

    Camera cam;
    private void Awake()
    {
        cam = GetComponent<Camera>();

        speed = PlayerPrefs.GetFloat("camSpeed");
        if (speed == 0)
        {
            speed = 0.5f;
            PlayerPrefs.SetFloat("camSpeed", 0.5f);
        }

        sensitivity = PlayerPrefs.GetFloat("camSens");
        if (sensitivity == 0)
        {
            sensitivity = 0.5f;
            PlayerPrefs.SetFloat("camSens", 0.5f);
        }
    }
   
    void Update()
    {
        Vector3 move = Vector3.zero;
        if(Input.GetKey(KeyCode.W))
            move += Vector3.forward * speed;
        if (Input.GetKey(KeyCode.S))
            move -= Vector3.forward * speed;
        if (Input.GetKey(KeyCode.D))
            move += Vector3.right * speed;
        if (Input.GetKey(KeyCode.A))
            move -= Vector3.right * speed;
        if (Input.GetKey(KeyCode.E))
            move += Vector3.up * speed;
        if (Input.GetKey(KeyCode.Q))
            move -= Vector3.up * speed;
        
        transform.Translate(move);

        if (transform.position.y < 0)
        {
            Vector3 newPos = transform.position;
            newPos.y = 0;
            transform.position = newPos;
        }


        if (Input.GetMouseButton(1))
        {
            yaw += sensitivity * Input.GetAxis("Mouse X");
            pitch -= sensitivity * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        }

        transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * 2f, Space.Self);
    }
}
