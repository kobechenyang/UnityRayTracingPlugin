using UnityEngine;
using UnityEngine.Rendering;

namespace PixelsForGlory.RayTracing
{
    [CreateAssetMenu(menuName = "Rendering/Ray Tracing Render Pipeline")]
    public class RayTracingRenderPipelineAsset : RenderPipelineAsset
    {
        protected override RenderPipeline CreatePipeline()
        {
            PixelsForGlory.RayTracing.RayTracingPlugin.SetShaderFolder(System.IO.Path.Combine(Application.dataPath, "Plugins", "RayTracing", "x86_64"));
            PixelsForGlory.RayTracing.RayTracingPlugin.MonitorShaders(System.IO.Path.Combine(Application.dataPath, "..", "..", "PluginSource", "source", "PixelsForGlory", "Shaders"));
            PixelsForGlory.RayTracing.RayTracingPlugin.Prepare();
            return new RayTracingRenderPipeline();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            PixelsForGlory.RayTracing.RayTracingPlugin.StopMonitoringShaders();
        }
    }
}    