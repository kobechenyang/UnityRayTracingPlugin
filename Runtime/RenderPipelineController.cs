using UnityEngine;
using UnityEngine.Rendering;

using System.IO;

namespace PixelsForGlory.RayTracing
{
    public class RenderPipelineController : MonoBehaviour {
        private RenderPipelineAsset _defaultRenderPipelineAsset;
        private RenderPipelineAsset _overrideRenderPipelineAsset;

        private void OnEnable() {
            _defaultRenderPipelineAsset = GraphicsSettings.defaultRenderPipeline;
            _overrideRenderPipelineAsset = ScriptableObject.CreateInstance<RayTracingRenderPipelineAsset>();
            GraphicsSettings.defaultRenderPipeline = _overrideRenderPipelineAsset;
            QualitySettings.renderPipeline = _overrideRenderPipelineAsset;
        }
        private void OnDisable() {
            GraphicsSettings.defaultRenderPipeline = _defaultRenderPipelineAsset;
            QualitySettings.renderPipeline = _defaultRenderPipelineAsset;
            Destroy(_overrideRenderPipelineAsset);
        }
    }
}    