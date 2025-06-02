using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
   [SerializeField] private AudioClip _som;

   private void TocarSom()
   {
      SoundFXManager.Instance.PlaySoundFXClip(_som, this.transform, 1f);
   }
}
