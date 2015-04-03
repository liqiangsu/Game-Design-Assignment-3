using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public float DampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public Transform Target;

    private new Camera camera;
    // Use this for initialization
    private void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    private void Update()
    {
        GetMouseInput();
        Move();
    }

    private void GetMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit result;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var isHit = Physics.Raycast(ray, out result, 1000f);
            if (isHit)
            {
                /**
                var targetPosition = result.collider.gameObject.transform.position;
                Vector3 newPosition = new Vector3(targetPosition.x,targetPosition.y, this.transform.position.z);
                this.transform.position = newPosition;**/
                Target = result.collider.transform;
            }
        }
    }

    private void Move()
    {
        if (Target)
        {
            Vector3 point = camera.WorldToViewportPoint(Target.position);
            Vector3 delta = Target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, DampTime);
        }
    }


}