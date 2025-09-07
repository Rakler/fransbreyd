using UnityEngine;

public class ControlsUI : MonoBehaviour
{
    [Header("Assign the panel you want to toggle")]
    public GameObject targetPanel;

    private void Start()
    {
        // Force the panel to be hidden at the beginning
        if (targetPanel != null)
        {
            targetPanel.SetActive(false);
        }
    }

    public void TogglePanel()
    {
        if (targetPanel != null)
        {
            targetPanel.SetActive(!targetPanel.activeSelf);
        }
        else
        {
            Debug.LogWarning("No panel assigned to PanelToggle!");
        }
    }
}
