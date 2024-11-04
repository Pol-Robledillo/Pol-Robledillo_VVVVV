using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject spawnPoint;
    public float time = 2f;
    public GameObject projectile;
    private Stack<GameObject> stack;
    private float projectileSpeed = 5f;
    public bool direction;
    public AudioClip shootSound;
    // Start is called before the first frame update
    void Start()
    {
        stack = new Stack<GameObject>();
        if (direction)
        {
            this.transform.localScale = new Vector3(-1, 1, 1);
        }
        StartCoroutine(Shoot());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Push(GameObject obj)
    {
        obj.SetActive(false);
        stack.Push(obj);
    }
    public GameObject Pop()
    {
        GameObject obj = stack.Pop();
        obj.SetActive(true);
        obj.transform.position = spawnPoint.transform.position;
        return obj;
    }
    public GameObject Peek()
    {
        return stack.Peek();
    }
    private IEnumerator Shoot()
    {
        this.GetComponent<Animator>().SetTrigger("Attack");
        gameObject.GetComponent<AudioSource>().PlayOneShot(shootSound);
        if (stack.Count != 0)
        {
            Pop();
        }
        else
        {
            GameObject bullet = Instantiate(projectile, spawnPoint.transform.position, Quaternion.identity);
            bullet.GetComponent<Projectile>().shooter = this;
            bullet.GetComponent<Projectile>().speed = direction ? projectileSpeed : -projectileSpeed;
            bullet.GetComponent<Projectile>().transform.localScale = new Vector3(direction ? 1 : -1, 1, 1);
        }
        yield return new WaitForSeconds(time);
        yield return Shoot();
    }
}
