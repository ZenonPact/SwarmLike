using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Info")]
    [SerializeField] private float playerMovementSpeed= 10f;
    Vector3 directionToLook;
    CharacterController player;
    Vector3 velocity;
    Quaternion lookRotation;
    Animator playerAnim;

    [Header("Fireball Info")]
    [SerializeField] private GameObject fireBallPrefab;
    [SerializeField] private float fireBallSpeed = 20f;
    public UnityEvent OnFireballEvent;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<CharacterController>();
        playerAnim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        player.Move(velocity*Time.deltaTime*playerMovementSpeed);
        velocity.y = Physics.gravity.y;

        // if(velocity.y==0 && velocity.x ==0)
        // {
        //     playerAnim.SetBool("isRunning",false);
        // }
        
    }

    public void OnMovement(InputAction.CallbackContext _value)
    {
        Vector2 inputMovement = _value.ReadValue<Vector2>();
        velocity.z = inputMovement.y;
        velocity.x = inputMovement.x;

        Vector3 directionToRun = transform.forward * inputMovement.x + transform.right*inputMovement.y;
        playerAnim.SetBool("isRunning",true);
        playerAnim.SetFloat("Horizontal", directionToRun.x);
        playerAnim.SetFloat("Vertical", directionToRun.z);

        if(_value.canceled)
        {
            playerAnim.SetBool("isRunning",false);
        }
    }

    public void Attack(InputAction.CallbackContext _value)
    {
        playerAnim.SetBool("isAttacking", true);
    }

    public void ThrowFireball()
    {
        GameObject fireball = Instantiate(fireBallPrefab, transform.position + transform.forward * 1.5f, transform.rotation);
        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * fireBallSpeed;
        playerAnim.SetBool("isAttacking", false);
    }

    public void CallThrowFireballEvent()
    {
        OnFireballEvent.Invoke();
    }

    public void LookAtCursor(InputAction.CallbackContext _value)
    {
        Vector2 cursorScreenPosition = _value.ReadValue<Vector2>();
       

        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(cursorScreenPosition);
        float hitDist = 0.0f;


        if (playerPlane.Raycast(ray, out hitDist))
        {
            Vector3 cursorWorldPosition = ray.GetPoint(hitDist);

            Vector3 directionToLook = cursorWorldPosition - transform.position;
            directionToLook.y = 0;

            if (directionToLook != Vector3.zero)
            {
                lookRotation = Quaternion.LookRotation(directionToLook);
                transform.rotation = lookRotation;
            }
        }
    }
   
}
