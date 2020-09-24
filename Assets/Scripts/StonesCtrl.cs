using UnityEngine;
using UnityEngine.U2D;
public class StonesCtrl : MonoBehaviour
{
    public SpriteAtlas atlas;
    public Transform stones_start_point;
    [Range(0,3)]
    public float stones_offsetX = 1.75f;
    [Range(0, 3)]
    public float stones_offsetY = 1.6f;
    [Range(0, 2)]
    public float incline = 0.13f;

    private Vector3[,] stones_pos;
    int col_cnt = 8;
    int row_cnt = 4;

    private string levelname = "level";
    private string stonename = "stone";

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = atlas.GetSprite(levelname);
        InitStones();
    }

    //Creating a grid with stones.
    //For easy bomb position calculating added an empty layers with triggers on four sides around stones.
    private void InitStones()
    {       
        stones_pos = new Vector3[col_cnt + 1, row_cnt + 1];
        float valueY = stones_start_point.position.y - stones_offsetY;
        for (int y = 0; y < row_cnt + 2; y++)
        {
            float valueX = (stones_start_point.position.x - stones_offsetX) + incline * y;
            for (int x = 0; x < col_cnt + 2; x++)
            {
                Vector3 pos = new Vector3(valueX, valueY, valueY);      // added Z value same as y for hiding pig behind stones
                if (x > 0 && x < col_cnt + 1 && y > 0 && y < row_cnt + 1)
                {
                    stones_pos[x, y] = pos;
                    CreateStone(stones_pos[x, y]);
                }
                else
                {
                    GameObject empty = new GameObject("Empty");
                    empty.transform.position = pos;
                    CreateTrigger(empty);
                }
                valueX += stones_offsetX;
            }
            valueY += stones_offsetY;
        }     
    }

    private void CreateStone(Vector3 position)
    {
        string name = "Stone";
        GameObject stone = new GameObject(name);
        stone.transform.position = position;
        stone.tag = name;
        var sr = stone.AddComponent<SpriteRenderer>();
        sr.sprite = atlas.GetSprite(stonename);
        sr.sortingLayerName = "Objects";

        var collider = stone.AddComponent<EdgeCollider2D>(); // barrier collider
        collider.points = new Vector2[2] { new Vector2(-0.2f, -0.3f), new Vector2(0.2f, -0.3f) };
        collider.edgeRadius = 0.2f;

        CreateTrigger(stone);
        
    }

    private void CreateTrigger(GameObject parent)
    {
        GameObject trigger = new GameObject("Trigger");
        trigger.tag = "Trigger";
        trigger.transform.SetParent(parent.transform, false);
        var collider = trigger.AddComponent<BoxCollider2D>(); // trigger collider
        collider.size = new Vector2(1.4f, 1.6f);
        collider.isTrigger = true;
    }
}
