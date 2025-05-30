// Vinyl 2025.

using System;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;

public class GameManager : UdonSharpBehaviour
{
	[Header("Game")]
	[SerializeField]
	protected int NumHumansPerInfected = 2;
	
	[Space(10.0f)]
	
	[SerializeField]
	protected Level[] Levels;
	
	[Space(10.0f)]

	public VRCObjectPool PlayerNodePool;

	private Level CurrentLevel;

	[UdonSynced, FieldChangeCallback(nameof(PlayerIds))]
	private int[] _PlayerIds;
	public int[] PlayerIds
	{
		get => _PlayerIds;
		set
		{
			_PlayerIds = value;

			Debug.Log($"Set players: {_PlayerIds}");
		}
	}

	[UdonSynced, FieldChangeCallback(nameof(InactivePlayerIds))]
	private int[] _InactivePlayerIds;
	public int[] InactivePlayerIds
	{
		get => _InactivePlayerIds;
		set
		{
			_InactivePlayerIds = value;

			Debug.Log($"Set inactive players: {_InactivePlayerIds}");
		}
	}

	[UdonSynced(UdonSyncMode.Linear), FieldChangeCallback(nameof(CurrentLevelIndex))]
	private int _CurrentLevelIndex;
	public int CurrentLevelIndex
	{
		get => _CurrentLevelIndex;
		set
		{
			_CurrentLevelIndex = value;

			CurrentLevel = Levels[CurrentLevelIndex];
			
			Debug.Log($"Set current level index: {_CurrentLevelIndex}");
		}
	}

	[UdonSynced(UdonSyncMode.Linear), FieldChangeCallback(nameof(NumInfected))]
	private int _NumInfected;
	public int NumInfected
	{
		get => _NumInfected;
		set
		{
			_NumInfected = value;

			Debug.Log($"Set number of infected: {_NumInfected}");
		}
	}

	private int[] GetActivePlayerIds()
	{
		int[] Result = new int[1];
		
		foreach (int PlayerId in PlayerIds)
		{
			bool bIsInactive = false;
			
			foreach (int InactivePlayerId in InactivePlayerIds)
			{
				bIsInactive |= PlayerId == InactivePlayerId;
				break;
			}

			if (!bIsInactive)
			{
				Result = Result.AddToArray(PlayerId);
			}
		}
		
		return Result;
	}

	private void AddPlayerId(int PlayerId)
	{
		PlayerIds = PlayerIds.AddToArray(PlayerId);
		Debug.Log($" - Added to player array: {PlayerIds.Length} players");
	}
	
	private void RemovePlayerId(int PlayerId)
	{
		PlayerIds = PlayerIds.RemoveFromArray(PlayerId);
		Debug.Log($" - Removed from player array: {PlayerIds.Length} players");
	}
	
	public void AddInactivePlayerId(int PlayerId)
	{
		InactivePlayerIds = InactivePlayerIds.AddToArray(PlayerId);
		Debug.Log($" - Added to inactive player array: {InactivePlayerIds.Length} inactive players");
	}
	
	public void RemoveInactivePlayerId(int PlayerId)
	{
		InactivePlayerIds = InactivePlayerIds.RemoveFromArray(PlayerId);
		Debug.Log($" - Removed from inactive player array: {InactivePlayerIds.Length} inactive players");
	}

	public override void OnPlayerJoined(VRCPlayerApi Player)
	{
		if (Player == null || !Player.IsValid())
		{
			return;
		}

		Debug.Log($"Player joined: {Player.displayName}");

		AddPlayerId(Player.playerId);
		AddInactivePlayerId(Player.playerId);
		
		RequestSerialization();
	}

	public override void OnPlayerLeft(VRCPlayerApi Player)
	{
		if (Player == null || !Player.IsValid())
		{
			return;
		}

		Debug.Log($"Player left: {Player.displayName}");
		
		RemovePlayerId(Player.playerId);
		RemoveInactivePlayerId(Player.playerId);
		
		RequestSerialization();
	}

	public void StartGame()
	{
		if (!Networking.LocalPlayer.isInstanceOwner)
		{
			return;
		}

		Debug.Log("Starting Game");

		CurrentLevelIndex = UnityEngine.Random.Range(0, Levels.Length - 1);
		RequestSerialization();

		SetupPlayerNodes();

		SpawnPlayers();
	}

	private void SpawnPlayers()
	{
		Debug.Log("Spawning players");

		if (CurrentLevel == null)
		{
			return;
		}

		foreach (int PlayerId in PlayerIds)
		{
			VRCPlayerApi Player = VRCPlayerApi.GetPlayerById(PlayerId);
			if (Player == null || !Player.IsValid())
			{
				continue;
			}

			int InactivePlayerIdx = Array.IndexOf(InactivePlayerIds, PlayerId);
			if (InactivePlayerIdx >= 0)
			{
				continue;
			}

			Transform[] CurrentLevelSpawns = CurrentLevel.GetSpawns();
			Transform ChosenSpawn = CurrentLevelSpawns[UnityEngine.Random.Range(0, CurrentLevelSpawns.Length - 1)];
			if (ChosenSpawn == null)
			{
				continue;
			}
			
			Debug.Log($" - Attempting to spawn: {Player.displayName}");

			AssignRoleToPlayer(Player);

			Player.TeleportTo(ChosenSpawn.position, ChosenSpawn.rotation);
		}
	}

	private void SetupPlayerNodes()
	{
		Debug.Log("Setting up player nodes");
		
		int[] ActivePlayerIds = GetActivePlayerIds();

		for (int Idx = 0; Idx < PlayerNodePool.Pool.Length; ++Idx)
		{
			PlayerNode PlayerNode = PlayerNodePool.Pool[Idx].GetComponent<PlayerNode>();
			if (PlayerNode == null)
			{
				continue;
			}

			PlayerNode.Id = Idx < ActivePlayerIds.Length ? ActivePlayerIds[Idx] : 0;
			PlayerNode.Role = RoleType.None;
			PlayerNode.RequestSerialization();
			
			Debug.Log($" - Registered player node for: {PlayerNode.GetDisplayName()}");
		}
	}

	private void AssignRoleToPlayer(VRCPlayerApi Player)
	{
		if (Player == null || !Player.IsValid())
		{
			return;
		}
		
		Debug.Log($"Assigning role for: {Player.displayName}");

		int[] ActivePlayerIds = GetActivePlayerIds();

		Debug.Log($" - Temp active player set created with length of: {ActivePlayerIds.Length}");

		if (NumInfected < (NumHumansPerInfected / ActivePlayerIds.Length))
		{
			Debug.Log(" - There is an infected spot left, randomly assigning");

			foreach (GameObject PlayerNodeGameObject in PlayerNodePool.Pool)
			{
				PlayerNode PlayerNode = PlayerNodeGameObject.GetComponent<PlayerNode>();
				if (PlayerNode == null || !Player.IsValid())
				{
					continue;
				}
				
				int RandomInt = UnityEngine.Random.Range(0, 1);
				PlayerNode.Role = RandomInt == 1 ? RoleType.Infected : RoleType.Human;
				PlayerNode.RequestSerialization();
			
				Debug.Log($" - Assigning role '{(RandomInt == 1 ? "Infected" : "Human")}' to: {Player.displayName}");
			
				if (RandomInt == 1)
				{
					Debug.Log(" - Attempting to increment out number of infected");

					++NumInfected;
					RequestSerialization();
				}
			}
		}
		else
		{
			Debug.Log(" - There are no infected spots left, assigning to human");

			foreach (GameObject PlayerNodeGameObject in PlayerNodePool.Pool)
			{
				PlayerNode PlayerNode = PlayerNodeGameObject.GetComponent<PlayerNode>();
				if (PlayerNode == null || !Player.IsValid())
				{
					continue;
				}

				PlayerNode.Role = RoleType.Human;
				PlayerNode.RequestSerialization();
			}
		}
	}
}
