#version 460
#extension GL_EXT_ray_tracing : enable
#extension GL_GOOGLE_include_directive : require
#extension GL_EXT_nonuniform_qualifier : require

#include "../Vulkan/ShaderConstants.h"

layout(set = DESCRIPTOR_SET_FACE_DATA,         binding = DESCRIPTOR_BINDING_FACE_DATA,         std430) readonly buffer FacesBuffer { ShaderFaceData Faces[]; } FacesArray[];
layout(set = DESCRIPTOR_SET_VERTEX_ATTRIBUTES, binding = DESCRIPTOR_BINDING_VERTEX_ATTRIBUTES, std430) readonly buffer VertexAttributesBuffer { ShaderVertexAttributeData VertexAttributes[]; } VertexAttributesArray[];
layout(set = DESCRIPTOR_SET_INSTANCE_DATA,     binding = DESCRIPTOR_BINDING_INSTANCE_DATA,     std430) readonly buffer InstanceDataBuffer { ShaderInstanceData InstanceData; } InstanceDataArray[];
layout(set = DESCRIPTOR_SET_MATERIALS_DATA,    binding = DESCRIPTOR_BINDING_MATERIALS_DATA,    std430) readonly buffer MaterialDataBuffer { ShaderMaterialData MaterialData; } MaterialDataArray[];

layout(location = LOCATION_PRIMARY_RAY) rayPayloadInEXT ShaderRayPayload PrimaryRay;
                                        hitAttributeEXT vec2 HitAttribs;

void main() {
    // The hitAttributeEXT (i.e. the barycentric coordinates) as provided by the built-in triangle-ray intersection test. 
    const vec3 barycentrics = vec3(1.0f - HitAttribs.x - HitAttribs.y, HitAttribs.x, HitAttribs.y);

    // Get data associated with this hit
    ShaderFaceData face = FacesArray[nonuniformEXT(gl_InstanceCustomIndexEXT)].Faces[gl_PrimitiveID];

    ShaderVertexAttributeData v0 = VertexAttributesArray[nonuniformEXT(gl_InstanceCustomIndexEXT)].VertexAttributes[face.index0];
    ShaderVertexAttributeData v1 = VertexAttributesArray[nonuniformEXT(gl_InstanceCustomIndexEXT)].VertexAttributes[face.index1];
    ShaderVertexAttributeData v2 = VertexAttributesArray[nonuniformEXT(gl_InstanceCustomIndexEXT)].VertexAttributes[face.index2];

    ShaderInstanceData instance = InstanceDataArray[nonuniformEXT(gl_InstanceCustomIndexEXT)].InstanceData;
    ShaderMaterialData material = MaterialDataArray[instance.materialIndex].MaterialData;

    // Using the barycentric coordinates, find the uv for this hit
    const vec2 uv = BaryLerp(v0.uv.xy, v1.uv.xy, v2.uv.xy, barycentrics);

    // Get the face normal
    vec3 normal = (instance.objectToWorldNormal * vec4(normalize(BaryLerp(v0.normal.xyz, v1.normal.xyz, v2.normal.xyz, barycentrics)), 0.0f)).xyz;
    // if(mat.normalIndex != -1)
    // {
    //     // Adapted from
    //     // https://learnopengl.com/Advanced-Lighting/Normal-Mapping

    //     // If we have a normal map, calculate the Tangent/Bi-tangent/Normal matrix to transform
    //     // the tangent space normal map into world space
    //     vec3 faceTangent = BaryLerp(v0.tangent.xyz, v1.tangent.xyz, v2.tangent.xyz, barycentrics);

    //     vec3 t = normalize(faceTangent);
    //     vec3 b = normalize(cross(normal, faceTangent));
    //     vec3 n = normal;

    //     mat3 tbn = mat3(t, b, n);

    //     vec3 texNormal = textureLod(TexturesArray[nonuniformEXT(mat.normalIndex)], uv, 0.0f).rgb;
    //     texNormal = texNormal * 2.0f - 1.0f;

    //     normal = normalize(tbn * texNormal);
    // }
    
    // // If we have a roughness map, use it. Otherwise take from material
    float roughness = material.roughness;
    // if(mat.roughIndex != -1)
    // {
    //     roughness = textureLod(TexturesArray[nonuniformEXT(mat.roughIndex)], uv, 0.0f).rgb;
    // }
    // else
    // {
    //     roughness = vec3(mat.roughness);
    // }

    // // Take the albedo value from the material 
    vec4 albedo = material.albedo;
    // if(mat.albedoIndex != -1)
    // {
    //     // Add texture contribution if available 
    //     texel *= textureLod(TexturesArray[nonuniformEXT(mat.albedoIndex)], uv, 0.0f).rgb;
    // }
    // texel += mat.emission.rgb;

    vec4 emission = material.emission;

    float metallic = material.metallic;
    // if(mat.reflIndex != -1)
    // {
    //     // If the have an metallic map, get value here
    //     // Only read the r value since it should be a grayscale value
    //     metallic = textureLod(TexturesArray[nonuniformEXT(mat.reflIndex)], uv, 0.0f).r;
    // }


    float ambientOcclusion = 1.0f;
    // vec3 ao = vec3(1.0f);
    // if(mat.aoIndex != -1)
    // {
    //     // If the have an ambient occulsion map, get value here
    //     ao = textureLod(TexturesArray[nonuniformEXT(mat.aoIndex)], uv, 0.0f).rgb;
    // }

    // Return payload to gen shader
    PrimaryRay.albedo               = albedo;
    PrimaryRay.emission             = emission;
    PrimaryRay.metallic             = metallic;
    PrimaryRay.roughness            = roughness;
    PrimaryRay.indexOfRefraction    = material.indexOfRefraction;
    PrimaryRay.ambientOcclusion     = ambientOcclusion;
    PrimaryRay.materialIndex        = instance.materialIndex;
    
    PrimaryRay.normal = normal;
    PrimaryRay.distance = gl_HitTEXT;
}

