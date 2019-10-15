using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Trophies.SwissDigital
{
    /// <summary>
    /// Helper class that plays a video on a RawImage texture.
    /// </summary>
    [RequireComponent(typeof(RawImage))]
    [RequireComponent(typeof(VideoPlayer))]
    public class RawImageVideoPlayer : MonoBehaviour
    {
        /// <summary>
        /// The raw image where the video will be played.
        /// </summary>
        public RawImage RawImage;

        /// <summary>
        /// The video player component to be played.
        /// </summary>
        public VideoPlayer VideoPlayer;

        private Texture m_RawImageTexture;

        public bool IsPlaying { get; private set; }

        /// <summary>
        /// The Unity Start() method.
        /// </summary>
        public void Start()
        {
            IsPlaying = false;
            VideoPlayer.enabled = false;
            m_RawImageTexture = RawImage.texture;
            VideoPlayer.prepareCompleted += _PrepareCompleted;
        }

        public void PlayVideo()
        {
            IsPlaying = true;

            VideoPlayer.enabled = true;
            VideoPlayer.Play();
        }

        public void StopVideo()
        {
            IsPlaying = false;

            VideoPlayer.Stop();
            RawImage.texture = m_RawImageTexture;
            VideoPlayer.enabled = false;
        }

        private void _PrepareCompleted(VideoPlayer player)
        {
            RawImage.texture = player.texture;
        }
    }
}