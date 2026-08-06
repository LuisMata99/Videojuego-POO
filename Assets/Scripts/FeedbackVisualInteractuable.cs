using UnityEngine;

public class FeedbackVisualInteractuable : MonoBehaviour
{
    [SerializeField] private GameObject canvasFeedback;

    private void Awake()
    {
        if (canvasFeedback != null)
        {
            canvasFeedback.SetActive(false);
        }
    }

    public void Encender()
    {
        if (canvasFeedback != null)
        {
            canvasFeedback.SetActive(true);
        }
    }

    public void Apagar()
    {
        if (canvasFeedback == null)
        {
            canvasFeedback.SetActive(false);
        }
    }
}
