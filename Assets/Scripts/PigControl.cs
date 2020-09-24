using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

public class PigControl : MonoBehaviour
{
    public SpriteAtlas atlas;
    public float up_angle;
    public float speed;
    Vector3 upVector;

    public Joystick joystick;

    Rigidbody2D rb;
    [SerializeField]
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GetComponent<SpriteRenderer>().sprite = atlas.GetSprite("pig0");
        upVector = Quaternion.Euler(0f, 0f, up_angle) * Vector3.up; 
    }

    void Update()
    {
        float v = joystick.Vertical;
        float h = joystick.Horizontal;
        rb.velocity = (upVector.normalized * v + Vector3.right * h) * Time.deltaTime * speed;

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y); // control z value for correct hiding pig behind stones

        if (Input.touchCount > 0)
        {
            Touch[] touches = Input.touches;

            if (touches.Any(x => Camera.main.ScreenToWorldPoint(x.position).x > 0f 
                                && x.phase == TouchPhase.Began))    //any 'began' touch on right part of the screen
            {
                Vector3 pos = CalculateBombPos(transform.position);
                Debug.Log(pos);
                GameObject bomb = new GameObject("bomb");
                bomb.transform.position = new Vector3(pos.x, pos.y, transform.position.z);
                var sr = bomb.AddComponent<SpriteRenderer>();
                sr.sprite = atlas.GetSprite("bomb");
                sr.sortingLayerName = "Objects";
            }           
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    private Vector3 CalculateBombPos(Vector3 pigpos)
    {
        List<Vector3> collisions = GetComponentInChildren<TriggerCounter>().GetCollisions();
        
        if(collisions.Count == 4)  // if there are 4 stones near to pig, bomb position is center between two diagonal placed stones
        {
            Vector3 top_right = collisions.Find(pos => pos.x < transform.position.x && pos.y < transform.position.y);
            Vector3 down_left = collisions.Find(pos => pos.x > transform.position.x && pos.y > transform.position.y);
            return Vector3.Lerp(top_right, down_left, 0.5f);
        }
        else if (collisions.Count > 1)                                  //if there are 2, just center between them. 
        {                                                               //It is possible to have 3 near stones, so just ignore 3rd
            return Vector3.Lerp(collisions[0], collisions[1], 0.5f);
        }


        return pigpos;
    }

    
}
