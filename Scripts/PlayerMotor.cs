
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera cam; 
    [SerializeField]
    private float Jump=20f;
    [SerializeField]
    private bool isJumping=false;
    private Vector3 thrusterVelocity ;
    [SerializeField]
    private float cameraRotationLimit=85f;
    //La velocity doit etre toujours recupperer et stocker dans une variable de meme nom
    private Vector3 velocity;
    private Vector3 rotation;
    private float cameraRotationX=0f;
    private float currentCameraRotationX=0f;
    //declaration de rigidbody
    private Rigidbody rb;
    private void Start() {
        //recuperation de rigidbody
        rb=GetComponent<Rigidbody>();
       
    }
    public void Rotate(Vector3 _rotation) {
        rotation=_rotation;
    }
    public void rotateCamera(float _cameraRotationX) {
        cameraRotationX=_cameraRotationX;
    }
    public void Move(Vector3 _velocity) {
        //passage de velocity calculer dans player controller vers la velocity de ce class
        velocity=_velocity;
    }
    private void FixedUpdate() {
        PerformMovement();
        PerformRotation();
        PerformJump();
        
    }
    private void PerformMovement() {
        //La condition est mise pour qu'on retour un vector.zero (pas de movement) , on n'effectue rien
        if(velocity!=Vector3.zero) {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
        //Deplacementt sur l'axe Y
        if(thrusterVelocity != Vector3.zero ) {
            rb.AddForce(thrusterVelocity * Time.fixedDeltaTime, ForceMode.Acceleration); 
        }
    }
    private void PerformRotation() {
        if(rotation!=Vector3.zero) {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation) );
        }
        //CAMERA ROTATION
        //Calcul de rotation de camera et blockage a un limite
        currentCameraRotationX += cameraRotationX ; 
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX,-cameraRotationLimit,cameraRotationLimit);
        cam.transform.localEulerAngles= new Vector3(-currentCameraRotationX,0f,0f);
    }
    private void PerformJump() {
        
        if(Input.GetButton("Jump")) {
            isJumping=true;
            rb.AddForce(transform.up * 20f );
        }
        isJumping=false;
    }
    public void ApplyThruster(Vector3 _thrusterVelocity) {
        thrusterVelocity = _thrusterVelocity ; 
    }
}
