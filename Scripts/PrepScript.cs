using UnityEngine;

public class PrepScript : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;          // the collection of possible sprites. 0-1-2-3 E-R-Y-B 
    [SerializeField] float shift = 0.625f;      // exactly .625 because a card is 1.2 wide + .05 margin on sides

    public void SetCards(int[] colors)
    // Sets the sprites for cards under this prep
    {

        // Counts the non-zero cards
        int amount = 0;
        for (int i = 0; i < colors.Length; i++)
        {
            amount += colors[i] == 0 ? 0 : 1;
        }

        // Places and colors the cards
        for (int i = 0; i < transform.childCount; i++)
        {
            // if (colors[i] > 3)
            // {
            //     //Debug.Log("uh-oh");
            //     continue;
            // }


            // Coloring according to the color value
            transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = sprites[colors[i]];

            // Places each card, but accounts only visible.
            // This isbecause the zero-valued, i.e. non-active and invisible cards, 
            // are always placed at the end of the array, end hence there are no empty spots,
            // and everything looks spotless!
            transform.GetChild(i).position = new Vector3(
                (1 - amount + i * 2) * shift,
                transform.position.y,
                transform.position.z
            );
        }
    }
}
