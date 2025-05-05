using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance {get; private set;}
    
    [SerializeField] private Room starterRoom;
    [SerializeField] public Room currentRoom {get; private set;}
    private float movementTimer = 0f;
    private float movementMaxTime = 1.3f;
    private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private Image healthBar;
    [SerializeField] private BattleState currentState;
    [SerializeField] private float OOCProgression = 0;
    [SerializeField] public float OOCMax = 3;
    private bool isInRange = false;
    [SerializeField] private float EnemyAttackTimer = 0f;
    [SerializeField] private float EnemyAttackMax = 1f;
    [SerializeField] private bool isOnCooldown = true;
    [SerializeField] private float playerAttackTimer = 0f;
    [SerializeField] private float playerAttackMax = 0.5f;
    private int attackDir = 0;
    private void Awake() {
        Instance = this;
    }
    void Start()
    {
        currentRoom = starterRoom;
        health = maxHealth;
        currentState = BattleState.TRAVEL;
    }

    void Update()
    {
        switch (currentState)
        {
            case BattleState.TRAVEL:
                if(currentRoom.CheckDoesHaveEnemy()) {
                    currentState = BattleState.DEFENCE; //! Battle starts
                } else {
                    movementTimer += Time.deltaTime;
                    if(movementTimer > movementMaxTime) {
                        GoNextRoom();
                        movementTimer = 0;
                    }
                }
            break;
            case BattleState.DEFENCE:
                if(!currentRoom.CheckDoesHaveEnemy()) {
                    currentState = BattleState.TRAVEL;
                    EnemyAttackTimer = 0;
                } else if(OOCProgression >= OOCMax) {
                    currentState = BattleState.OUTOFCONTROL;
                    EnemyAttackTimer = 0;
                }

                EnemyAttackTimer += Time.deltaTime;
                if(EnemyAttackTimer >= EnemyAttackMax) {
                    if(isOnCooldown) {
                        attackDir = Random.Range(2, 5);
                        Debug.Log("new attack dir" + attackDir);
                        currentRoom.GetEnemy().FireProjectile((AttackDirection)attackDir);
                        EnemyAttackTimer = 0;
                        isOnCooldown = false;
                    } else {
                        Debug.Log("currentAtackdir = " + (int)GameInput.Instance.currentAttackDir);
                        if(attackDir != 0 && (int)GameInput.Instance.currentAttackDir == attackDir) {
                            Debug.Log("blocked");
                            FindAnyObjectByType<EnemyProjectile>().SliceProjectile();
                            OOCProgression++;
                        } else {
                            currentRoom.GetEnemy().AttackPlayer();
                        }
                        EnemyAttackTimer = 0;
                        isOnCooldown = true;
                    }
                    
                }
            break;
            case BattleState.OUTOFCONTROL:
                if(!currentRoom.CheckDoesHaveEnemy()) {
                    currentState = BattleState.TRAVEL;
                    OOCProgression = 0;
                    isInRange = false;
                    transform.DOMove(new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.4f), 0.5f);
                } else if(OOCProgression <= 0) {
                    currentState = BattleState.DEFENCE;
                    isInRange = false;
                    transform.DOMove(new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.4f), 0.5f);
                }
                if(!isInRange) {
                    transform.DOMove(new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.4f), 0.5f);
                    isInRange = true;
                }
                
                playerAttackTimer += Time.deltaTime;
                if(playerAttackTimer >= playerAttackMax) {
                    if(GameInput.Instance.currentAttackDir != AttackDirection.EMPTY) {
                        currentRoom.GetEnemy().DamageHealth(10);
                        playerAttackTimer = 0;
                        OOCProgression--;
                    }
                }

            break;
            default:
            break;
        }
    }


    public void GoNextRoom() {
        if(currentRoom.nextRoom == null) {
            return;
        }
        
        switch(currentRoom.nextDirection) {
            case Direction.STRAIGHT:
            transform.DOMove(new Vector3(currentRoom.transform.position.x, currentRoom.transform.position.y + 0.5f, currentRoom.transform.position.z), 1);
            break;
            case Direction.RIGHT:
            transform.DOMove(new Vector3(currentRoom.transform.position.x, currentRoom.transform.position.y + 0.5f, currentRoom.transform.position.z),1);
            StartCoroutine(TurnRight());
            
            break;
            case Direction.LEFT:
            transform.DOMove(new Vector3(currentRoom.transform.position.x, currentRoom.transform.position.y + 0.5f, currentRoom.transform.position.z),1);
            StartCoroutine(TurnLeft());
            
            break;
        }
        currentRoom = currentRoom.nextRoom;
        
    }

    IEnumerator TurnRight() {
        yield return new WaitForSeconds(0.8f);
        transform.DORotate(new Vector3(0,transform.rotation.eulerAngles.y + 90,0), 0.3f,RotateMode.FastBeyond360);
    }

    IEnumerator TurnLeft() {
        yield return new WaitForSeconds(0.8f);
        transform.DORotate(new Vector3(0,transform.rotation.eulerAngles.y - 90,0), 0.3f, RotateMode.FastBeyond360);
    }

    public void AttackEnemyOnRoom() {
        currentRoom.GetEnemy().DamageHealth(10);
    }

    public void DamageHealth(float amount) {
        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        healthBar.fillAmount = health/maxHealth;
        Shake.Instance.start = true;
        //TODO animasyon oynat
        if(health <= 0) {
            //TODO oyunu bitir
        }
    }
}

public enum BattleState {
    TRAVEL,
    DEFENCE,
    OUTOFCONTROL
}
