using UnityEngine;
using System.Collections;
public class GameInput : MonoBehaviour
{
    public static GameInput Instance{ get; private set;}
    private PlayerInputActions inputAction;
    public AttackDirection currentAttackDir = AttackDirection.EMPTY;
    private bool directionLock = false;
    [SerializeField] private Animator swordAnimator;
    private float directionTimer = 0f;
    private float directionMaxx = 2f;
    private void Awake() {
        Instance = this;
        inputAction = new PlayerInputActions();
        inputAction.Player.Enable();
    }

    private void Update() {
        
        Debug.Log("mouse delta = " + inputAction.Player.MouseDelta.ReadValue<Vector2>());
        Vector2 mouseDelta = inputAction.Player.MouseDelta.ReadValue<Vector2>();
        if(directionLock == false) {
            if(mouseDelta.x >= 20) {
                directionLock = true;
                currentAttackDir = AttackDirection.RIGHT;
                swordAnimator.SetTrigger("Right");
            } else if(mouseDelta.x <= -20) {
                directionLock = true;
                currentAttackDir = AttackDirection.LEFT;
                swordAnimator.SetTrigger("Left");
            } else if(mouseDelta.y >= 20) {
                directionLock = true;
                currentAttackDir = AttackDirection.TOP;
                swordAnimator.SetTrigger("Top");
            } else if(mouseDelta.y <= -20) {
                directionLock = true;
                currentAttackDir = AttackDirection.BOTTOM;
                swordAnimator.SetTrigger("Down");
            }
        }

        if(directionLock == true) {
            switch (currentAttackDir)
            {
                case AttackDirection.RIGHT:
                    if(mouseDelta.x <= 2) {

                        directionTimer += Time.deltaTime;
                        if(directionTimer >= directionMaxx) {
                            currentAttackDir = AttackDirection.EMPTY;
                            directionLock = false;
                            directionTimer = 0f;
                            Debug.Log("heyo");
                        }
                    }
                break;
                case AttackDirection.LEFT:
                    if(mouseDelta.x >= -2) {
                        directionTimer += Time.deltaTime;
                        if(directionTimer >= directionMaxx) {
                            currentAttackDir = AttackDirection.EMPTY;
                            directionLock = false;
                            directionTimer = 0f;
                            Debug.Log("heyo");
                        }
                    }
                break;
                case AttackDirection.TOP:
                    if(mouseDelta.y <= 2) {
                        directionTimer += Time.deltaTime;
                        if(directionTimer >= directionMaxx) {
                            currentAttackDir = AttackDirection.EMPTY;
                            directionLock = false;
                            directionTimer = 0f;
                            Debug.Log("heyo");
                        }
                    }
                break;
                case AttackDirection.BOTTOM:
                    if(mouseDelta.y >= -2) {
                        directionTimer += Time.deltaTime;
                        if(directionTimer >= directionMaxx) {
                            currentAttackDir = AttackDirection.EMPTY;
                            directionLock = false;
                            directionTimer = 0f;
                            Debug.Log("heyo");
                        }
                    }
                break;
                default:
                currentAttackDir = AttackDirection.EMPTY;
                directionLock = false;
                Debug.Log("heyo");
                break;
            }
        }
    }

    IEnumerator resetAttack() {
        yield return new WaitForSeconds(2f);
        currentAttackDir = AttackDirection.EMPTY;
        directionLock = false;
        Debug.Log("heyo");
    }
}

public enum AttackDirection {
    EMPTY = 0,
    TOP = 1,
    BOTTOM = 2,
    LEFT = 3,
    RIGHT = 4
}