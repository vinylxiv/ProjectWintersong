// Vinyl 2025.

using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public enum RoleType
{
	None,
	Human,
	Infected
}

public class PlayerNode : UdonSharpBehaviour
{
    [UdonSynced, FieldChangeCallback(nameof(Id))]
	private int _Id;
	public int Id
	{
		get => _Id;
		set
		{
			_Id = value;

			Debug.Log($"Set player to {GetDisplayName()}");
		}
	}
	
    [UdonSynced, FieldChangeCallback(nameof(Role))]
	private RoleType _Role = RoleType.None;
	public RoleType Role
	{
		get => _Role;
		set
		{
			_Role = value;

			Debug.Log($"Set role '{value}' for {GetDisplayName()}");
		}
	}

	public bool IsValid()
	{
		return Id > 0;
	}

	private VRCPlayerApi GetPlayer()
	{
		return VRCPlayerApi.GetPlayerById(Id);
	}

	public string GetDisplayName()
	{
		VRCPlayerApi Player = GetPlayer();
		return Player != null ? Player.displayName : "null";
	}
}
