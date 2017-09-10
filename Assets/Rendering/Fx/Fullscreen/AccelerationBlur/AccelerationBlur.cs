using System;
using UnityEngine;

// This class implements simple ghosting type Motion Blur.
// If Extra Blur is selected, the scene will allways be a little blurred,
// as it is scaled to a smaller resolution.
// The effect works by accumulating the previous frames in an accumulation
// texture.
namespace UnityStandardAssets.ImageEffects
{
    [ExecuteInEditMode]
    [AddComponentMenu("Image Effects/Blur/Acceleration Blur")]
    [RequireComponent(typeof(Camera))]
    public class AccelerationBlur : ImageEffectBase
    {
        public float m_Perturbation = 0.03f;
        public bool m_ExtraBlur = false;
        public Vector2 m_Acceleration = Vector2.zero;

        private RenderTexture m_AccumTexture;

        override protected void Start()
        {
            if (!SystemInfo.supportsRenderTextures)
            {
                enabled = false;
                return;
            }
            base.Start();
        }

        override protected void OnDisable()
        {
            base.OnDisable();
            DestroyImmediate(m_AccumTexture);
        }

        // Called by camera to apply image effect
        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (m_Acceleration == Vector2.zero)
                return;

            // Create the accumulation texture
            if (m_AccumTexture == null || m_AccumTexture.width != source.width || m_AccumTexture.height != source.height)
            {
                DestroyImmediate(m_AccumTexture);
                m_AccumTexture = new RenderTexture(source.width, source.height, 0);
                m_AccumTexture.hideFlags = HideFlags.HideAndDontSave;
                Graphics.Blit(source, m_AccumTexture);
            }

            // If Extra Blur is selected, downscale the texture to 4x4 smaller resolution.
            if (m_ExtraBlur)
            {
                RenderTexture blurbuffer = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
                m_AccumTexture.MarkRestoreExpected();
                Graphics.Blit(m_AccumTexture, blurbuffer);
                Graphics.Blit(blurbuffer, m_AccumTexture);
                RenderTexture.ReleaseTemporary(blurbuffer);
            }

            m_Perturbation = Mathf.Clamp(m_Perturbation, 0.0f, 2.0f);

            // Setup samplers & uniforms
            material.SetTexture("_MainTex", m_AccumTexture);
            material.SetVector("_Acceleration", m_Acceleration);
            material.SetFloat("_Perturbation", m_Perturbation);

            // We are accumulating motion over frames without clear/discard
            // by design, so silence any performance warnings from Unity
            m_AccumTexture.MarkRestoreExpected();

            // Render the image using the motion blur shader
            Graphics.Blit(source, m_AccumTexture, material);
            Graphics.Blit(m_AccumTexture, destination);
        }
    }
}
