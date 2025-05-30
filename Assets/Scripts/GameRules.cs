// Vinyl 2025.

using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class GameRules : UdonSharpBehaviour
{
	[Header("Movement")]
	[SerializeField]
	protected float DefaultJumpImpulse;
	[SerializeField]
	protected float DefaultGravityStrength = 1.0f;
	[Space(10.0f)]
	[SerializeField]
	protected float DefaultWalkSpeed = 5.0f;
	[SerializeField]
	protected float DefaultRunSpeed = 5.0f;
	[SerializeField]
	protected float DefaultStrafeSpeed = 5.0f;

	[Header("Voice")]
	[SerializeField]
	protected float DefaultVoiceGain = 15.0f;
	[SerializeField]
	protected float DefaultVoiceDistanceNear;
	[SerializeField]
	protected float DefaultVoiceDistanceFar = 25.0f;
	[SerializeField]
	protected float DefaultVoiceVolumetricRadius = 1.0f;
	[SerializeField]
	protected bool DefaultVoiceUseLowpass = true;

	[Header("Avatar")]
	[SerializeField]
	protected float DefaultAvatarAudioGain = 10.0f;
	[SerializeField]
	protected float DefaultAvatarAudioNearRadius = 40.0f;
	[SerializeField]
	protected float DefaultAvatarAudioFarRadius = 40.0f;
	[SerializeField]
	protected float DefaultAvatarAudioVolumetricRadius = 40.0f;
	[SerializeField]
	protected bool DefaultAvatarAudioForceSpatial;
	[SerializeField]
	protected bool DefaultAvatarAudioUseCustomCurve;

	public void Start()
	{
		if (!Networking.LocalPlayer.IsValid())
		{
			return;
		}

		Networking.LocalPlayer.SetJumpImpulse(DefaultJumpImpulse);
		Networking.LocalPlayer.SetGravityStrength(DefaultGravityStrength);

		Networking.LocalPlayer.SetWalkSpeed(DefaultWalkSpeed);
		Networking.LocalPlayer.SetRunSpeed(DefaultRunSpeed);
		Networking.LocalPlayer.SetStrafeSpeed(DefaultStrafeSpeed);

		Networking.LocalPlayer.SetVoiceGain(DefaultVoiceGain);
		Networking.LocalPlayer.SetVoiceDistanceNear(DefaultVoiceDistanceNear);
		Networking.LocalPlayer.SetVoiceDistanceFar(DefaultVoiceDistanceFar);
		Networking.LocalPlayer.SetVoiceVolumetricRadius(DefaultVoiceVolumetricRadius);
		Networking.LocalPlayer.SetVoiceLowpass(DefaultVoiceUseLowpass);

		Networking.LocalPlayer.SetAvatarAudioGain(DefaultAvatarAudioGain);
		Networking.LocalPlayer.SetAvatarAudioNearRadius(DefaultAvatarAudioNearRadius);
		Networking.LocalPlayer.SetAvatarAudioFarRadius(DefaultAvatarAudioFarRadius);
		Networking.LocalPlayer.SetAvatarAudioVolumetricRadius(DefaultAvatarAudioVolumetricRadius);
		Networking.LocalPlayer.SetAvatarAudioForceSpatial(DefaultAvatarAudioForceSpatial);
		Networking.LocalPlayer.SetAvatarAudioCustomCurve(DefaultAvatarAudioUseCustomCurve);

		Debug.Log("Setup default game rules");
	}
}
