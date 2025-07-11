using UnityEngine;

public class CardPlacementScript : MonoBehaviour
{
    public Player playerScript;
    public int color;
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mPos.x, mPos.y, 0f);
        }
        if (Input.GetMouseButtonUp(0))
        {
            bool left = (playerScript.transform.position.x - transform.position.x) > 0;
            playerScript.MoveCard(color, left);
            Destroy(gameObject);
        }
    }
}
