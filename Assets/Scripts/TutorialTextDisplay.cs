using UnityEngine;

public class TutorialTextDisplay : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (Transform child in transform.parent.transform)
        {
            if (!child.name.Equals("AppearenceZone"))
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        foreach(Transform child in transform.parent.transform)
        {
            if (! child.name.Equals("TriggerZone"))
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
