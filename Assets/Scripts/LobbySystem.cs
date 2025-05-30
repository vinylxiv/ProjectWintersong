// Vinyl 2025.

using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

public class LobbySystem : UdonSharpBehaviour
{
	[Header("Lobby")]
	
	[SerializeField]
	protected GameManager GameManager;

	[SerializeField]
	protected Button PlayButtonComponent;
	[SerializeField]
	protected Button PlayHostOnlyButtonComponent;

	[SerializeField]
	protected GameObject PlayButtonToggleTextObject;

	[UdonSynced, FieldChangeCallback(nameof(bPlayButtonEnabled))]
	private bool _bPlayButtonEnabled = true;
	public bool bPlayButtonEnabled
	{
		get => _bPlayButtonEnabled;
		set
		{
			_bPlayButtonEnabled = value;

			if (PlayButtonComponent != null)
			{
				PlayButtonComponent.interactable = value;
			}

			Debug.Log($"Setting play button enabled: {value}");
		}
	}

	public void LateUpdate()
	{
		if (PlayHostOnlyButtonComponent == null)
		{
			return;
		}
		
		PlayHostOnlyButtonComponent.interactable = Networking.LocalPlayer.isInstanceOwner;
	}

	public void OnClickedPlayButton()
	{
		if (GameManager == null || !bPlayButtonEnabled)
		{
			return;
		}
		
		GameManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(GameManager.StartGame));
	}

	public void OnClickedPlayHostOnlyButton()
	{
		if (GameManager == null || !Networking.LocalPlayer.isInstanceOwner)
		{
			return;
		}
		
		GameManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(GameManager.StartGame));
	}

	public void OnClickedPlayButtonToggle()
	{
		if (PlayButtonToggleTextObject == null)
		{
			return;
		}

		bPlayButtonEnabled = !bPlayButtonEnabled;

		PlayButtonToggleTextObject.SetActive(bPlayButtonEnabled);
	}
}
