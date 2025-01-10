using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FullScreenRenderPassFeature : ScriptableRendererFeature
{
    [SerializeField]
    private Material material;

    class CustomRenderPass : ScriptableRenderPass
    {
        [SerializeField]
        private Material material;
        RTHandle tempTexture, sourceTexture;

        public CustomRenderPass(Material material) : base()
        {
            this.material = material;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            sourceTexture = renderingData.cameraData.renderer.cameraColorTargetHandle;

            tempTexture =
                RTHandles.Alloc(
                    new RenderTargetIdentifier("_TempTexture"),
                    name: "_TempTexture");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer commandBuffer = CommandBufferPool.Get("Full Screen Render Feature");

            RenderTextureDescriptor targetDescriptor =
                renderingData.cameraData.cameraTargetDescriptor;

            targetDescriptor.depthBufferBits = 0;

            commandBuffer.GetTemporaryRT(
                Shader.PropertyToID(tempTexture.name), targetDescriptor, FilterMode.Bilinear);

            // Apply the material to the source texture and blit it to the target
            Blit(commandBuffer, sourceTexture, tempTexture, material);
            Blit(commandBuffer, tempTexture, sourceTexture);

            context.ExecuteCommandBuffer(commandBuffer);
            CommandBufferPool.Release(commandBuffer);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            tempTexture.Release();
        }
    }

    CustomRenderPass m_ScriptablePass;

    public override void Create()
    {
        m_ScriptablePass = new CustomRenderPass(this.material);

        // Before render
        m_ScriptablePass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_ScriptablePass);
    }
}