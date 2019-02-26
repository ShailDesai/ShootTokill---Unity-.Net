using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerToggle : MonoBehaviour {

    public AudioMixer mixer;//refrence to audio which is want to aaffect
    public string parameter;// volume parameter 
    public GameObject disablimage; // to disable audi variable
  

    private float unmutedaudiolevel;
    private bool ismuted = false;//to check is mute or not

    public void ToggleAudio()
    {
        if (ismuted)
        {
            mixer.GetFloat(parameter, out unmutedaudiolevel);
          
            disablimage.SetActive(true);
            ismuted = true;
        }
        else
        {
            mixer.SetFloat(parameter, unmutedaudiolevel);
            mixer.SetFloat(parameter, -80);
            disablimage.SetActive(false);
            ismuted = false;
        }
    }
}
