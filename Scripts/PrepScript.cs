using UnityEngine;

public class PrepScript : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    public void SetCards(int[] colors)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (colors[i] > 3)
            {
                Debug.Log("uh-oh");
                continue;
            }

            transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = sprites[colors[i]];
        }
    }
}
