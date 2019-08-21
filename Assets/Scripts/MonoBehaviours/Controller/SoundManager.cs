using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioClip sirenSound;
    public AudioClip frightenedSound;
    public AudioClip pacmanDeathSound;

    public AudioSource backgroundAudioSource;
    public AudioSource extraLifeAudioSource;


    private void Start() {
        this.backgroundAudioSource = this.GetComponent<AudioSource>();
    }

    public void PlaySirenSound() {
        backgroundAudioSource.loop = true;

        backgroundAudioSource.clip = sirenSound;
        backgroundAudioSource.Play();
    }

    public void PlayFrightenedSound() {
        backgroundAudioSource.loop = true;

        backgroundAudioSource.clip = frightenedSound;
        backgroundAudioSource.Play();
    }

    public void PlayPacManDeathSound() {
        backgroundAudioSource.loop = false;

        backgroundAudioSource.clip = pacmanDeathSound;
        backgroundAudioSource.Play();
    }

    public void PlayExtraLifeSound() {
        extraLifeAudioSource.Play();
    }

    public void PauseBackgroundSource() {
        backgroundAudioSource.Pause();
    }



}
