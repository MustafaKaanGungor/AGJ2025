using System.Collections;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerMovement.Instance.currentRoom == gameObject.GetComponent<Room>()) {
            StartCoroutine(OpenDoor());
        }
    }

    IEnumerator OpenDoor() {
        yield return new WaitForSeconds(2f);
        GetComponent<Animator>().SetTrigger("OpenDoor");
    }
}
