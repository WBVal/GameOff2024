using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEngine.XR.XRDisplaySubsystem;

public class ToonShadingRenderFeature : ScriptableRendererFeature
{
	[System.Serializable]
	public class ToonShadingSettings
	{
		public bool IsEnabled = true;
		public RenderPassEvent WhenToInsert = RenderPassEvent.BeforeRenderingPostProcessing;
		public Material material;
	}

	[SerializeField]
	ToonShadingSettings settings = new();

	ToonShadingRenderPass pass;

	public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
	{

		// Gather up and pass any extra information our pass will need.
		// In this case we're getting the camera's color buffer target
		var cameraColorTargetIdent = renderer.cameraColorTargetHandle;
		pass.Setup(cameraColorTargetIdent);

	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(pass);
	}

	public override void Create()
	{
		pass = new ToonShadingRenderPass(settings.material);
		pass.renderPassEvent = settings.WhenToInsert;
	}
}
