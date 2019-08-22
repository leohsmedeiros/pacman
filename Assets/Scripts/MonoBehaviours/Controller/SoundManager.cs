using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioClip introSound;
    public AudioClip sirenSound;
    public AudioClip frightenedSound;
    public AudioClip pacmanDeathSound;

    public AudioSource backgroundAudioSource;
    public AudioSource extraLifeAudioSource;


    private void Start() {
        this.backgroundAudioSource = this.GetComponent<AudioSource>();
    }

    private void PlayOnBackgroundAudioSource(AudioClip clip, bool loop) {
        backgroundAudioSource.clip = clip;
        backgroundAudioSource.loop = loop;
        backgroundAudioSource.Play();
    }

    public void PlayIntroSound() => PlayOnBackgroundAudioSource(introSound, false);

    public void PlaySirenSound() => PlayOnBackgroundAudioSource(sirenSound, true);

    public void PlayFrightenedSound() => PlayOnBackgroundAudioSource(frightenedSound, true);

    public void PlayPacManDeathSound() => PlayOnBackgroundAudioSource(pacmanDeathSound, false);

    public void PlayExtraLifeSound() => extraLifeAudioSource.Play();

    public void PauseBackgroundSource() => backgroundAudioSource.Pause();

}
