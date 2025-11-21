using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Game399.Unity
{
    public class PostProcessing : MonoBehaviour
    {
        [SerializeField] private VolumeProfile volumeProfile;

        private Vignette vignette;
        private Bloom bloom;
        private ChromaticAberration chromaticAberration;
        private DepthOfField dof;
        private LensDistortion lensDistortion;
        private void Start()
        {
            if (volumeProfile != null)
            {
                volumeProfile.TryGet(out vignette);
                volumeProfile.TryGet(out bloom);
                volumeProfile.TryGet(out chromaticAberration);
                volumeProfile.TryGet(out dof);
                volumeProfile.TryGet(out lensDistortion);
                bloom.intensity.value = 0f;
                chromaticAberration.intensity.value = 0f;
                lensDistortion.intensity.value = 0f;
                dof.mode.value = 0;
                vignette.intensity.value = 0f;
            }
        }
        
        public void UpdateSobrietyPostProcessing(int sobriety)
        {
            if (sobriety >= 50)
            {
                bloom.intensity.value = 0.3f;
                chromaticAberration.intensity.value = 0f;
                lensDistortion.intensity.value = 0f;
                dof.mode.value = 0;
            } else if (sobriety >= 30)
            {
                bloom.intensity.value = 1f;
                chromaticAberration.intensity.value = 0.4f;
                lensDistortion.intensity.value = -0.4f;
                dof.mode.value = 0;
            } else if (sobriety >= 15)
            {
                bloom.intensity.value = 3f;
                chromaticAberration.intensity.value = 0.8f;
                lensDistortion.intensity.value = -0.5f;
                dof.mode.value = DepthOfFieldMode.Gaussian;
            }
            else
            {
                bloom.intensity.value = 10f;
                chromaticAberration.intensity.value = 1f;
                lensDistortion.intensity.value = -0.6f;
                dof.mode.value = DepthOfFieldMode.Gaussian;
            }
        }

        public void UpdateAffectionPostProcessing(int affection)
        {
            if (affection >= 80)
            {
                vignette.color.value = new Color(1f, 0.4f, 0.7f); // Pink - Love
                vignette.intensity.value = 0.5f;
            }
            else if (affection >= 65)
            {
                vignette.color.value = new Color(1f, 0.6f, 0.6f); // Light red - Very positive
                vignette.intensity.value = 0.4f;
            }
            else if (affection >= 50)
            {
                vignette.color.value = new Color(0.7f, 0.7f, 0.7f); // Grey - Neutral
                vignette.intensity.value = 0f;
            }
            else if (affection >= 30)
            {
                vignette.color.value = Color.orange; // Orange - Dislike
                vignette.intensity.value = 0.4f;
            }
            else
            {
                vignette.color.value = Color.red; // Red - Very Negative
                vignette.intensity.value = 0.5f;
            }
        }
    }
}
