
using UnityEngine;
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 10;
    

    private PlayerMotor motor;

    [SerializeField]
    private float sensitivity = 3f;
    [SerializeField]
    private float thrusterForce=1000f;
    [Header("Joint Options")]
    [SerializeField]
    private float jointSpring=20f;
    [SerializeField]
    private float jointMaxForce=50f;

    private ConfigurableJoint joint;
    [SerializeField]
    private float tgrusterFuelBurnSpeed=1f;
    [SerializeField]
    private float thrusterFuelRegenSpeed=0.3f;
    private float thursterFuelAmount=1f;

    public float GetThrusterFuelAmount() {
        return thursterFuelAmount;
    }

    private void Start() {
        //Importation de PlayerController Class de l'autre fichier
        motor=GetComponent<PlayerMotor>();
        joint=GetComponent<ConfigurableJoint>();
        SetJointSettings(jointSpring);
    }
    private void Update() {
        if(PauseMenu.isOn) {
            if(Cursor.lockState!=CursorLockMode.None) {
                Cursor.lockState=CursorLockMode.None;
            }
            motor.Move(Vector3.zero);
            motor.Rotate(Vector3.zero);
            motor.rotateCamera(0f);
            motor.ApplyThruster(Vector3.zero);

            return;
        }
        if(Cursor.lockState!=CursorLockMode.Locked) {
            Cursor.lockState=CursorLockMode.Locked;
        }
        //Calculer la vitesse de mouvement d'un joeur;
        float xMovement= Input.GetAxisRaw("Horizontal") ;
        float zMovement= Input.GetAxisRaw("Vertical");

        //Calcul de movement horizontal et vertical
        Vector3 moveHorizontal = transform.right * xMovement ;
        Vector3 moveVertical = transform.forward * zMovement ;
        //Calcul de movement total
        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed ;
        //Appel a la fonction de PlayerMotor qui move le joueur
        motor.Move(velocity);
        //Calcul de rotation de joeur en un vecteur 3
        float yRotate=Input.GetAxisRaw("Mouse X");
        Vector3 rotation=new Vector3(0,yRotate,0)* sensitivity ;
        motor.Rotate(rotation);
        //Calcul de rotation de camero un vecteur 3
        float xRotate=Input.GetAxisRaw("Mouse Y");
        float camerarotation=xRotate* sensitivity ;
        motor.rotateCamera(camerarotation);
        //Calcul de la force du jetpack / thruster
        Vector3 thrusterVelocity = Vector3.zero;

        //Aplique la variable thrusterforce / utilisation de jetpack
        if(Input.GetButton("Jump") && thursterFuelAmount>0) {
            thursterFuelAmount-=tgrusterFuelBurnSpeed * Time.deltaTime;
            if(thursterFuelAmount>=0.001f) {
                thrusterVelocity= Vector3.up * thrusterForce;
                SetJointSettings(0f);
            }  
        }
        else {
            thursterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;
            SetJointSettings(jointSpring);
        }
        thursterFuelAmount=Mathf.Clamp(thursterFuelAmount,0f,1f);

        motor.ApplyThruster(thrusterVelocity);
    }
    private void SetJointSettings(float _jointSpring) {
        joint.yDrive = new JointDrive
        {
            positionSpring=_jointSpring , maximumForce=jointMaxForce
        };
    }

    
}
