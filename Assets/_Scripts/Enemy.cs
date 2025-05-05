using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Room room;
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private GameObject slashEffect;
    private Animator animator;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject firePoint;
    private void Awake() {
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        health = maxHealth;
        switch (room.nextDirection) {
            case Direction.STRAIGHT:
            transform.position = new Vector3(room.transform.position.x, room.transform.position.y + 0.4f, room.transform.position.z + 0.4f);
            break;
            case Direction.RIGHT:
            transform.position = new Vector3(room.transform.position.x - 0.4f, room.transform.position.y+ 0.4f, room.transform.position.z);
            transform.rotation = Quaternion.Euler(0,90,0);
            break;
            case Direction.LEFT:
            transform.position = new Vector3(room.transform.position.x + 0.4f, room.transform.position.y + 0.4f, room.transform.position.z);
            transform.rotation = Quaternion.Euler(0,-90,0);
            break;
        }
        
        room.AddEnemyToArray(this);
    }


    void Update()
    {
        if(PlayerMovement.Instance.currentRoom == room) {
            animator.SetTrigger("EnemyReact");
        }
    }

    public void FireProjectile(AttackDirection direction) {
        animator.SetTrigger("EnemyAttack");
        StartCoroutine(Attack(direction));

    }

    IEnumerator Attack(AttackDirection direction) {
        yield return new WaitForSeconds(1f);
        Instantiate(projectile, firePoint.transform.position, Quaternion.identity).GetComponent<EnemyProjectile>().direction = direction;

    }

    public void AttackPlayer() {
        PlayerMovement.Instance.DamageHealth(3);
    }

    public void DamageHealth(float amount) {
        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        
        //transform.DOPunchPosition(new Vector3(transform.position.x + 0.01f, transform.position.y, transform.position.z ),0.2f, 5,0,false);
        Instantiate(slashEffect);
        Shake.Instance.start = true;
        //HitStop.Instance.Stop(1f);
        //TODO animasyon oynat
        animator.SetTrigger("EnemyDead");
        if(health <= 0) {
            room.RemoveEnemyFromArray(this);
            gameObject.SetActive(false);
        }
    }
}
