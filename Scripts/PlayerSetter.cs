
using UnityEngine;
using Mirror;
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]
public class PlayerSetter : NetworkBehaviour
{
    [SerializeField]
   Behaviour[] componentsToDisable;
    Camera sceneCamera;
    [SerializeField] 
    private string remoteLayerName="RemotePlayer";
    [SerializeField]
    private GameObject playerUIPrefab;
    private GameObject playerUIInstance;
   private void Start() {
    // Boucle sur tous les components qui sont autre que le jour actual et les desactiver pour qu'on peut avoir un seul 
    // marcher sans 
        if(!isLocalPlayer) {
            DisableComponent();
            AssignRemotePlayer();
        }
        else {
            sceneCamera=Camera.main;
           if(sceneCamera !=null) {
            sceneCamera.gameObject.SetActive(false);
           }
           //Creation du UI Local
           playerUIInstance=Instantiate(playerUIPrefab);

           PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
           if(ui==null) {
                Debug.LogError("PlayerUI Component is null");
           }
           else {
            ui.SetController(GetComponent<PlayerController>());
           }
           GetComponent<Player>().Setup();
        }
        //Changer le nom du joeur pour qu'on puisse detecter qui a touché qui dans le system de tir 
        
        
   }
   public override void OnStartClient()  {
        //Quand le serveur start , on va enrigistrer les joueurs present dans la scene
        base.OnStartClient();
        string netId=GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();

        GameManager.RegisterPlayer(netId,player);
   }

    private void DisableComponent() {
        for(int i=0;i<componentsToDisable.Length;i++) {
                componentsToDisable[i].enabled=false;
            }
    }
    private void AssignRemotePlayer() {
        gameObject.layer=LayerMask.NameToLayer(remoteLayerName);
    }
    private void onDisble() {
        Destroy(playerUIInstance);
        if(sceneCamera!=null) {
            //Set la camera a la position initale quand on quitte la partie
            sceneCamera.gameObject.SetActive(true);
        }

        GameManager.UnregisterPlayer(transform.name);
   }
}
