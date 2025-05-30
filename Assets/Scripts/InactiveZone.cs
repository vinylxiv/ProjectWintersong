// Vinyl 2025.

using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class InactiveZone : UdonSharpBehaviour
{
	[Header("Inactive Zone")]
	[SerializeField]
	protected GameManager GameManager;

	public override void OnPlayerTriggerEnter(VRCPlayerApi Player)
	{
		if (GameManager == null)
		{
			return;
		}
		
		GameManager.AddInactivePlayerId(Player.playerId);
	}

	public override void OnPlayerTriggerExit(VRCPlayerApi Player)
	{
		if (GameManager == null)
		{
			return;
		}
		
		GameManager.RemoveInactivePlayerId(Player.playerId);
	}
}
