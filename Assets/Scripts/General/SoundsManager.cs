using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    Lose,
    Hit,
    Shot,
    ShotgunShot,
    NewWave
}

public class SoundsManager : Singleton<SoundsManager>
{
    [SerializeField] private SoundsData[] _soundsDatas;
    private Dictionary<SoundType, AudioClipData> _soundIdPairs = new();

    private WaitForSeconds _waitForCoolDown = new (0.1f);

    protected override void Awake()
    {
        base.Awake();

        foreach (SoundsData soundData in _soundsDatas)
            _soundIdPairs.Add(soundData.SoundType, soundData.AudioClipData);
    }

    public void PlaySound(SoundType soundType)
    {
        if(!_soundIdPairs.ContainsKey(soundType))
        {
            Debug.LogWarning($"There's no audioclip for sound type {soundType}!");
            return;
        }

        if (_soundIdPairs[soundType].IsOnCooldown)
            return;

        _soundIdPairs[soundType].IsOnCooldown = true;
        StartCoroutine(ResetCoolDown(soundType));
        _soundIdPairs[soundType].AudioSource?.PlayOneShot(_soundIdPairs[soundType].AudioClip);
    }

    private IEnumerator ResetCoolDown(SoundType soundType)
    {
        yield return _waitForCoolDown;
        _soundIdPairs[soundType].IsOnCooldown = false;
    }

    [Serializable]
    public class AudioClipData
    {
        public AudioClip AudioClip;
        public AudioSource AudioSource;
        [HideInInspector] public bool IsOnCooldown;
    }

    [Serializable] 
    public class SoundsData
    {
        public SoundType SoundType;
        public AudioClipData AudioClipData;
    }
}