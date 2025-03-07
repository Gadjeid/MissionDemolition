using UnityEngine;
using UnityEngine.UI;

public class UIButton_View : MonoBehaviour
{
    public FollowCam.eView viewToSwitchTo;

    public Button button;

    void Start()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        FollowCam.SWITCH_VIEW(viewToSwitchTo);
    }
}