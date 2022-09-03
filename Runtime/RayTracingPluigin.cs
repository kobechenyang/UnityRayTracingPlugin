using System;
using System.Runtime.InteropServices;

using UnityEngine;

namespace PixelsForGlory.RayTracing
{
    [PluginAttr("RayTracingPlugin")]
    internal static class RayTracingPlugin
    {
        [PluginFunctionAttr("SetShaderFolder")] 
        public static SetShaderFolderDel SetShaderFolder = null;
        public delegate int SetShaderFolderDel(string shaderFolder);

        [PluginFunctionAttr("SetRenderTarget")]
        public static SetRenderTargetDel SetRenderTarget = null;
        public delegate int SetRenderTargetDel(int cameraInstanceId, int unityTextureFormat, int width, int height, IntPtr destination);

        [PluginFunctionAttr("AddSharedMesh")]
        public static AddSharedMeshDel AddSharedMesh = null;
        public delegate int AddSharedMeshDel(int sharedMeshInstanceId, IntPtr vertices, IntPtr normals, IntPtr tangets, IntPtr uvs, int vertexCount, IntPtr indices, int indexCount);

        [PluginFunctionAttr("AddTlasInstance")]
        public static AddTlasInstanceDel AddTlasInstance = null;
        public delegate int AddTlasInstanceDel(int gameObjectInstanceId, int sharedMeshInstanceId, int materialInstanceId, IntPtr l2wMatrix, IntPtr w2lMatrix);

        [PluginFunctionAttr("UpdateTlasInstance")]
        public static UpdateTlasInstanceDel UpdateTlasInstance = null;
        public delegate void UpdateTlasInstanceDel(int gameObjectInstanceId, int materialInstanceId, IntPtr l2wMatrix, IntPtr w2lMatrix);

        [PluginFunctionAttr("RemoveTlasInstance")]
        public static RemoveTlasInstanceDel RemoveTlasInstance = null;
        public delegate void RemoveTlasInstanceDel(int gameObjectInstanceId);

        [PluginFunctionAttr("Prepare")]
        public static PrepareDel Prepare = null;
        public delegate void PrepareDel();

        [PluginFunctionAttr("ResetPipeline")]
        public static ResetPipelineDel ResetPipeline = null;
        public delegate void ResetPipelineDel();
        
        [PluginFunctionAttr("UpdateCamera")]
        public static UpdateCameraDel UpdateCamera = null;
        public delegate void UpdateCameraDel(int cameraInstanceId, IntPtr camPos, IntPtr camDir, IntPtr camUp, IntPtr camSide, float camNear, float camFar, float camFov);

        [PluginFunctionAttr("UpdateSceneData")]
        public static UpdateSceneDataDel UpdateSceneData = null;
        public delegate void UpdateSceneDataDel(IntPtr color);

        [PluginFunctionAttr("AddLight")]
        public static AddLightDel AddLight = null;
        public delegate int AddLightDel(int lightInstanceId, float x, float y, float z, float r, float g, float b, float bounceIntensity, float intensity, float range, float spotAngle, int type, bool enabled);

        [PluginFunctionAttr("UpdateLight")]
        public static UpdateLightDel UpdateLight = null;
        public delegate void UpdateLightDel(int lightInstanceId, float x, float y, float z, float r, float g, float b, float bounceIntensity, float intensity, float range, float spotAngle, int type, bool enabled);

        [PluginFunctionAttr("RemoveLight")]
        public static RemoveLightDel RemoveLight = null;
        public delegate void RemoveLightDel(int lightInstanceId);

        [PluginFunctionAttr("AddTexture")]
        public static AddTextureDel AddTexture = null;
        public delegate int AddTextureDel(int textureInstanceId, IntPtr texture);

        [PluginFunctionAttr("RemoveTexture")]
        public static RemoveTextureDel RemoveTexture = null;
        public delegate void RemoveTextureDel(int textureInstanceId);

        [PluginFunctionAttr("AddMaterial")]
        public static AddMaterialDel AddMaterial = null;
        public delegate int AddMaterialDel(int materialInstanceId,
                                             float albedo_r, float albedo_g, float albedo_b,
                                             float emission_r, float emission_g, float emission_b,
                                             float transmittance_r, float transmittance_g, float transmittance_b,
                                             float metallic,
                                             float roughness,
                                             float indexOfRefraction,
                                             bool albedoTextureSet,
                                             int albedoTextureInstanceId,
                                             bool emissionTextureSet,
                                             int emissionTextureInstanceId,
                                             bool normalTextureSet,
                                             int normalTextureInstanceId,
                                             bool metallicTextureSet,
                                             int metallicTextureInstanceId,
                                             bool roughnessTextureSet,
                                             int roughnessTextureInstanceId,
                                             bool ambientOcclusionTextureSet,
                                             int ambientOcclusionTextureInstanceId);
        [PluginFunctionAttr("UpdateMaterial")]
        public static UpdateMaterialDel UpdateMaterial = null;
        public delegate void UpdateMaterialDel(int materialInstanceId,
                                                 float albedo_r, float albedo_g, float albedo_b,
                                                 float emission_r, float emission_g, float emission_b,
                                                 float transmittance_r, float transmittance_g, float transmittance_b,
                                                 float metallic,
                                                 float roughness,
                                                 float indexOfRefraction,
                                                 bool albedoTextureSet,
                                                 int albedoTextureInstanceId,
                                                 bool emissionTextureSet,
                                                 int emissionTextureInstanceId,
                                                 bool normalTextureSet,
                                                 int normalTextureInstanceId,
                                                 bool metallicTextureSet,
                                                 int metallicTextureInstanceId,
                                                 bool roughnessTextureSet,
                                                 int roughnessTextureInstanceId,
                                                 bool ambientOcclusionTextureSet,
                                                 int ambientOcclusionTextureInstanceId);

        [PluginFunctionAttr("RemoveMaterial")]
        public static RemoveMaterialDel RemoveMaterial = null;
        public delegate void RemoveMaterialDel(int materialInstanceId);

        [StructLayout(LayoutKind.Sequential)]
        public struct RayTracerStatistics
        {
            public uint RegisteredLights;
            public uint RegisteredSharedMeshes;
            public uint RegisteredMeshInstances;
            public uint RegisteredMaterials;
            public uint RegisteredTextures;
            public uint RegisteredRenderTargets;

            public uint DescriptorSetCount;
            public uint AccelerationStuctureCount;
            public uint UniformBufferCount;
            public uint StorageImageCount;
            public uint StorageBufferCount;
            public uint CombinedImageSamplerCount;
        }

        [PluginFunctionAttr("GetRayTracerStatistics")]
        public static GetRayTracerStatisticsDel GetRayTracerStatistics = null;
        public delegate RayTracerStatistics GetRayTracerStatisticsDel();

        // [PluginFunctionAttr("GetEventFunc")]
        // public static GetEventFuncDel GetEventFunc = null;
        // public delegate IntPtr GetEventFuncDel();

        // [PluginFunctionAttr("GetEventAndDataFunc")]
        // public static GetEventAndDataFuncDel GetEventAndDataFunc = null;
        // public delegate IntPtr GetEventAndDataFuncDel();
        [DllImport("RayTracingPlugin")]
        public static extern IntPtr GetEventFunc();
        [DllImport("RayTracingPlugin")]
        public static extern IntPtr GetEventAndDataFunc();

        [PluginFunctionAttr("SetDebugFunction")]
        public static SetDebugFunctionDel SetDebugFunction = null;
        public delegate void SetDebugFunctionDel(IntPtr fp);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void DebugDelegate(string str);
        static void CallBackFunction(string str) { Debug.Log(str); }

        private static string RootDir;

        public static void SetDebug()
        {
            DebugDelegate callback_delegate = new DebugDelegate(CallBackFunction);
            // Convert callback_delegate into a function pointer that can be
            // used in unmanaged code.
            IntPtr intptr_delegate =
                Marshal.GetFunctionPointerForDelegate(callback_delegate);
            // Call the API passing along the function pointer.
            SetDebugFunction(intptr_delegate);
        }

        public static void MonitorShaders(string sourcePath)
        {
            Debug.Log("Monitoring shaders");
            Watcher = new System.IO.FileSystemWatcher();
            Watcher.Path = sourcePath;

            Watcher.NotifyFilter = System.IO.NotifyFilters.LastWrite;
            Watcher.Filter = "*.bin";

            Watcher.Changed += OnChanged;

            // Start watching
            Watcher.EnableRaisingEvents = true;

            var dirInfo = new System.IO.DirectoryInfo(Application.dataPath);
            RootDir = dirInfo.Parent.Parent.FullName;
        }

        private static void OnChanged(object source, System.IO.FileSystemEventArgs e)
        {
            PixelsForGlory.RayTracing.RayTracingPlugin.ResetPipeline();

        }

        public static void StopMonitoringShaders()
        {
            Debug.Log("Stopping monitoring shaders");
            Watcher.Dispose();
        }

        private static System.IO.FileSystemWatcher Watcher;
    }

}
