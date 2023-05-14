using UnityEngine;
using Mirror; 
using System.Collections;

public class Player : NetworkBehaviour
{   
    [SyncVar] // Added hook to update client when death status changes
    private bool _isDead = false;

    public bool isDead => _isDead; // Using C# 6 expression-bodied property

    [SerializeField]
    private float maxHealth = 100f;

    [SyncVar] // Added hook to update client when health changes
    private float currentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;

    private bool[] wasEnabledOnStart; 

    public void Setup() 

    {
        CmdBroadCastNewPlayerSetup();
    }
    [Command]
    private void CmdBroadCastNewPlayerSetup() {
        RpcSetupPlayerOnAllClients();
    }
    [ClientRpc]
    private void RpcSetupPlayerOnAllClients() {
        wasEnabledOnStart = new bool[disableOnDeath.Length]; // Fixed syntax error on length property
        for (int i = 0; i < disableOnDeath.Length; i++) 
        { 
            wasEnabledOnStart[i] = disableOnDeath[i].enabled;
        }

        SetDefaults();
    }
    public void SetDefaults() 
    {
        _isDead = false; // Fixed syntax error, accessing private field directly
        currentHealth = maxHealth;

        for (int i = 0; i < disableOnDeath.Length; i++) 
        {
            disableOnDeath[i].enabled = wasEnabledOnStart[i];
        }

        Collider col = GetComponent<Collider>();
        if (col != null) 
        {
            col.enabled = true;
        }
    }

    [ClientRpc] 
    public void RpcTakeDamage(float damage) 
    {
        if (_isDead) // Fixed syntax error, accessing private field directly
        {
            return;
        }
        currentHealth -= damage;
        Debug.Log(transform.name + " a maintenant " + currentHealth + " point de vie");
        if (currentHealth <= 0) 
        {
            Die();
        }
    }
    private IEnumerator Respawn() {
        yield return new WaitForSeconds(3f);
        SetDefaults();
        Transform spawnPoint=NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation=spawnPoint.rotation;
    }
    private void Die() 
    {
        _isDead = true; // Fixed syntax error, accessing private field directly
        for (int i = 0; i < disableOnDeath.Length; i++) 
        {
            disableOnDeath[i].enabled = false;
        }
        Collider col=GetComponent<Collider>();
        if(col!=null) {
            col.enabled=false;
        }
        Debug.Log(transform.name +"& ete eliminé");
        StartCoroutine(Respawn());
    }
    private void Update() {
        if(!isLocalPlayer) {
            return ;
        }
            if(Input.GetKeyDown(KeyCode.K)) {
                RpcTakeDamage(999);
            }
        
    }


}
