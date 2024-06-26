﻿using UnityEngine;

namespace Avrahamy.Audio {
    /// <summary>
    /// Use this message to play an audio and get an AudioInstance reference that
    /// can be used to manipulate the audio source. This should always be used
    /// to play music or other looping audio.
    /// </summary>
    public class RequestPlayManagedAudioMessage {
        public const float PLAY_TO_THE_END = -1f;

        private AudioEvent audioEvent;
        private Vector3? position;
        private float delay;
        private float startTime;
        private float endTime;
        private float fadeInDuration;
        private bool isLooping;
        private AudioInstance audioInstance;

        public AudioEvent Event {
            get {
                return audioEvent;
            }
        }

        public Vector3? Position {
            get {
                return position;
            }
        }

        public float Delay {
            get {
                return delay;
            }
        }

        public float StartTime {
            get {
                return startTime;
            }
            set {
                startTime = value;
            }
        }

        public float EndTime {
            get {
                return endTime;
            }
            set {
                endTime = value;
            }
        }

        public float FadeInDuration {
            get {
                return fadeInDuration;
            }
            set {
                fadeInDuration = value;
            }
        }

        public bool IsLooping {
            get {
                return isLooping;
            }
        }

        public AudioInstance PlayedAudioInstance {
            get {
                return audioInstance;
            }
            set {
                audioInstance = value;
            }
        }

        public RequestPlayManagedAudioMessage(AudioEvent audioEvent, Vector3 position, bool isLooping = true, float fadeInDuration = -1f, float delay = 0f) {
            this.audioEvent = audioEvent;
            this.position = position;
            this.delay = delay;
            this.startTime = 0f;
            this.endTime = AudioTrack.PLAY_TO_THE_END;
            this.fadeInDuration = fadeInDuration;
            this.isLooping = isLooping;
        }

        public RequestPlayManagedAudioMessage(AudioEvent audioEvent, bool isLooping = true, float fadeInDuration = -1f, float delay = 0f) {
            this.audioEvent = audioEvent;
            this.delay = delay;
            this.startTime = 0f;
            this.endTime = AudioTrack.PLAY_TO_THE_END;
            this.fadeInDuration = fadeInDuration;
            this.isLooping = isLooping;
        }

        public RequestPlayManagedAudioMessage(AudioEvent audioEvent, float endTime) {
            this.audioEvent = audioEvent;
            this.delay = 0f;
            this.startTime = 0f;
            this.endTime = endTime;
            this.fadeInDuration = -1f;
            this.isLooping = false;
        }

        public RequestPlayManagedAudioMessage(AudioEvent audioEvent, float startTime, float endTime, float fadeInDuration = -1f) {
            this.audioEvent = audioEvent;
            this.delay = 0f;
            this.startTime = startTime;
            this.endTime = endTime;
            this.fadeInDuration = fadeInDuration;
            this.isLooping = false;
        }
    }
}
