
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
   [SerializeField]
   private RectTransform thrusterFuelFill;

   [SerializeField]
   private GameObject pauseMenu;

   private PlayerController controller;
   public void SetController(PlayerController _controller) {
      controller=_controller;
   }
   private void Start() {
      PauseMenu.isOn=false;
   }

   private void Update() {
      setFuellAmount(controller.GetThrusterFuelAmount());

      if(Input.GetKeyDown(KeyCode.Escape)) {
         TogglePauseMenu();
      }
   }
   
   public void TogglePauseMenu() {
       pauseMenu.SetActive(!pauseMenu.activeSelf);
       PauseMenu.isOn=pauseMenu.activeSelf;
   }
   void setFuellAmount(float _amount) {
        thrusterFuelFill.localScale=new Vector3(1f,_amount,1f);

   }
}
