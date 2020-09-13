using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour {

	public static SoundManager instance;

	public AudioSource musicAS;
	public AudioSource effectAS;
	public Slider volume;

	private bool _isMuteM = false;
	private bool _isMuteE = false;

	void Awake()
	{
		instance = GetComponent<SoundManager>();
	}

	public void MuteAS(int posAudioSource)
	{
		if (posAudioSource == 0)
		{

			if (_isMuteM == true)
			{
				musicAS.mute = false;
				_isMuteM = false;
			}
			else 
			{
				musicAS.mute = true;
				_isMuteM = true;
			}
		}
		else if (posAudioSource == 1)
		{

			if (_isMuteE == false)
			{
				effectAS.mute = true;
				_isMuteE = true;
			}
			else
			{
				effectAS.mute = false;
				_isMuteE = false;
			}
		}
		
	}

	public void Volume()
	{
		musicAS.volume = volume.value;
		effectAS.volume = volume.value;
	}

	
}
