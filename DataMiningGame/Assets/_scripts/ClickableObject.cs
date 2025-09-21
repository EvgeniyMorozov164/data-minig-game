using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.Click();

    }
}
