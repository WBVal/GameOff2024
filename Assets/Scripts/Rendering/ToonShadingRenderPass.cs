using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ToonShadingRenderPass : ScriptableRenderPass
{
	private Material toonMat;
	private RenderTargetIdentifier source;
	private RenderTargetHandle tempTexture;

	public ToonShadingRenderPass(Material mat) : base()
	{
		this.toonMat = mat;
		tempTexture.Init("_TempToonFeature");
	}

	// This isn't part of the ScriptableRenderPass class and is our own addition.
	// For this custom pass we need the camera's color target, so that gets passed in.
	public void Setup(RenderTargetIdentifier cameraColorTargetIdent)
	{
		this.source = cameraColorTargetIdent;
	}
	public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
	{
		RenderTextureDescriptor cameraTextureDesc = renderingData.cameraData.cameraTargetDescriptor;
		cameraTextureDesc.depthBufferBits = 0;
		cmd.GetTemporaryRT(tempTexture.id, cameraTextureDesc, FilterMode.Bilinear);
	}

	public void SetSource(RenderTargetIdentifier source)
	{
		this.source = source;
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		CommandBuffer cmd = CommandBufferPool.Get("ToonFeature");

		Blit(cmd, source, tempTexture.Identifier(), toonMat, 0);
		Blit(cmd, tempTexture.Identifier(), source);

		context.ExecuteCommandBuffer(cmd);
		CommandBufferPool.Release(cmd);
	}

	public override void OnCameraCleanup(CommandBuffer cmd)
	{
		cmd.ReleaseTemporaryRT(tempTexture.id);
	}
}
