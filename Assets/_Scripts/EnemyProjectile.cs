using DG.Tweening;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private Vector3 TOP, BOTTOM, RIGHT, LEFT;
    public AttackDirection direction;
    private Vector3 targetTransform;
    private float deathTimer = 0;
    private float deathMax = 2;
    private bool hasSliced = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TOP = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z - 0.3f);
        BOTTOM = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z - 0.3f);
        RIGHT = new Vector3(transform.position.x + 0.3f, transform.position.y, transform.position.z - 0.3f);
        LEFT = new Vector3(transform.position.x - 0.2f, transform.position.y, transform.position.z - 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (direction)
        {
            case AttackDirection.TOP:
            //transform.position = Vector3.Slerp(transform.position, TOP, 1f);
            targetTransform = TOP;
            break;
            case AttackDirection.BOTTOM:
            //transform.position = Vector3.Slerp(transform.position, BOTTOM, 1f);
            targetTransform = BOTTOM;
            break;
            case AttackDirection.RIGHT:
            //transform.position = Vector3.Slerp(transform.position, RIGHT, 1f);
            targetTransform = RIGHT;
            break;
            case AttackDirection.LEFT:
            //transform.position = Vector3.Slerp(transform.position, LEFT, 1f);
            targetTransform = LEFT;
            break;
            default:
            break;
        }
        var tween = transform.DOMove(targetTransform, 1f, false);

        if(!hasSliced) {
            deathTimer += Time.deltaTime;
            if(deathTimer >= deathMax) {
                tween.Kill();
                Destroy(gameObject);
            }
        } else {
            tween.Kill();
            GetComponent<Animator>().SetTrigger("Sliced");
            Destroy(gameObject,0.5f);
        }
        
    }

    public void SliceProjectile() {
        hasSliced = true;
    }

    
}
