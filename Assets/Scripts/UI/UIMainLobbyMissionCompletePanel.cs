using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMainLobbyMissionCompletePanel : MonoBehaviour
{
    [SerializeField] Image _rewardIconImage;
    [SerializeField] TextMeshProUGUI _rewardDescriptionText;
    [SerializeField] Transform _iconGroupTransform;
    [SerializeField] UIMainLobbyMissionPanelView _missionPanelView;

    public void ShowPanel(Sprite rewardIcon, string rewardDescription)
    {
        _rewardIconImage.sprite = rewardIcon;
        _rewardDescriptionText.text = rewardDescription;

        _iconGroupTransform.gameObject.SetActive(false);
        _rewardIconImage.enabled = true;
        gameObject.SetActive(true);
    }

    public void ShowPanel(Dictionary<RewardType, Sprite> icons, Dictionary<RewardType, string> rewardsMsg)
    {
        foreach (var icon in icons)
        {
            GameObject obj = new GameObject("Image");
            obj.transform.parent = _iconGroupTransform;
            obj.transform.localScale = Vector3.one;


            Image iconImage = obj.AddComponent<Image>();
            iconImage.sprite = icon.Value;
            iconImage.preserveAspect = true;
        }

        _rewardDescriptionText.text = null;
        foreach (var msg in rewardsMsg)
        {
            if (_rewardDescriptionText.text == null)
                _rewardDescriptionText.text = msg.Value;
            else
                _rewardDescriptionText.text += "\n" + msg.Value;
        }

        _rewardIconImage.enabled = false;
        _iconGroupTransform.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void OnConfirm()
    {
        for (int i = 0; i < _iconGroupTransform.childCount; i++)
        {
            Destroy(_iconGroupTransform.GetChild(i).gameObject);
        }

        gameObject.SetActive(false);
        _missionPanelView.OnRewardAllComplete();
    }
}
