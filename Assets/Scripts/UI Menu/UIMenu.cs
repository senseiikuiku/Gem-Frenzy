using UnityEngine;

public class UIMenu : MonoBehaviour
{
    public GameObject settingPanel;

    private void Start()
    {
        settingPanel.SetActive(false);
    }

    public void ToggleSettingPanel()
    {
        settingPanel.SetActive(!settingPanel.activeSelf);
    }
}
