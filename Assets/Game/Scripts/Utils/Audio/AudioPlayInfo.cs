using UnityEngine;

namespace Game.Utils.Audio
{
    [System.Serializable]
    public class AudioPlayInfo
    {
        [field: SerializeField]
        public AudioClip Clip { get; private set; }

        [field: SerializeField]
        public float Volume { get; private set; }

        [field: SerializeField]
        public Vector2 Pitch { get; private set; }
    }

}