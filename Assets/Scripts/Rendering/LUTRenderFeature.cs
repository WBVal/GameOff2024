using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LUTRenderFeature : ScriptableRendererFeature
{
	[System.Serializable]
	public class MyFeatureSettings
	{
		// we're free to put whatever we want here, public fields will be exposed in the inspector
		public bool IsEnabled = true;
		public RenderPassEvent WhenToInsert = RenderPassEvent.BeforeRenderingPostProcessing;
		public Material MaterialToBlit;
	}

	// MUST be named "settings" (lowercase) to be shown in the Render Features inspector
	public MyFeatureSettings settings = new MyFeatureSettings();

	RenderTargetHandle renderTextureHandle;
	LUTRenderPass myRenderPass;

	public override void Create()
	{
		myRenderPass = new LUTRenderPass(
		  "My custom pass",
		  settings.WhenToInsert,
		  settings.MaterialToBlit
		);
	}

	public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
	{

		// Gather up and pass any extra information our pass will need.
		// In this case we're getting the camera's color buffer target
		var cameraColorTargetIdent = renderer.cameraColorTargetHandle;
		myRenderPass.Setup(cameraColorTargetIdent);

	}

	// called every frame once per camera
	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (!settings.IsEnabled)
		{
			// we can do nothing this frame if we want
			return;
		}
		// Ask the renderer to add our pass.
		// Could queue up multiple passes and/or pick passes to use
		renderer.EnqueuePass(myRenderPass);
	}
}
