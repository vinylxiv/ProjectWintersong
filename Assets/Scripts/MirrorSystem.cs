// Vinyl 2025.

using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

public class MirrorSystem : UdonSharpBehaviour
{
	[Header("Mirror")]
	
	[SerializeField]
	protected Transform MirrorTransform;
	[SerializeField]
	protected VRC_MirrorReflection MirrorComponent;
	[SerializeField]
	protected TMPro.TextMeshProUGUI EnableDisableTextComponent;
	[SerializeField]
	protected TMPro.TextMeshProUGUI HQLQTextComponent;
	
	[Space(10.0f)]
	
	[SerializeField]
	protected string EnableText;
	[SerializeField]
	protected string DisableText;
	
	[SerializeField]
	protected string HQText;
	[SerializeField]
	protected string LQText;
	
	[Space(10.0f)]
	
	[SerializeField]
	protected LayerMask HQLayerMask;
	[SerializeField]
	protected LayerMask LQLayerMask;
	
	private bool bMirrorEnabled;
	private bool bHQMirrorEnabled;

	public void Start()
    {
		if (MirrorTransform == null || MirrorComponent == null || EnableDisableTextComponent == null || HQLQTextComponent == null)
		{
			return;
		}

		MirrorTransform.gameObject.SetActive(false);
		MirrorComponent.m_ReflectLayers = LQLayerMask;
		EnableDisableTextComponent.text = EnableText;
		HQLQTextComponent.text = HQText;

		bMirrorEnabled = false;
		bHQMirrorEnabled = false;

		UpdateMirror();
	}

	public void OnClickedToggleMirrorButton()
	{
		if (MirrorTransform == null || MirrorComponent == null || EnableDisableTextComponent == null || HQLQTextComponent == null)
		{
			return;
		}

		bMirrorEnabled = !bMirrorEnabled;

		UpdateMirror();
	}

	public void OnClickedToggleMirrorQualityButton()
	{
		if (MirrorTransform == null || MirrorComponent == null || EnableDisableTextComponent == null || HQLQTextComponent == null)
		{
			return;
		}

		bHQMirrorEnabled = !bHQMirrorEnabled;

		UpdateMirror();
	}

	private void UpdateMirror()
	{
		MirrorTransform.gameObject.SetActive(bMirrorEnabled);
		EnableDisableTextComponent.text = bMirrorEnabled ? DisableText : EnableText;

		MirrorComponent.m_ReflectLayers = bHQMirrorEnabled ? HQLayerMask : LQLayerMask;
		HQLQTextComponent.text = bHQMirrorEnabled ? HQText : LQText;
	}
}
