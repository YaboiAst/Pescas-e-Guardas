using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
   public static SoundFXManager Instance;

   [SerializeField] private AudioSource _soundFXObjectPrefab;

   private void Awake()
   {
      if (Instance && Instance != this)
      {
         Destroy(gameObject);
         return;
      }

      Instance = this;
   }

   public void PlaySoundFXClip(AudioClip audioClip,Transform spawnTransform ,float volume)
   {
      AudioSource audioSource = ObjectPoolManager.SpawnObject(
         _soundFXObjectPrefab, 
         spawnTransform, 
         Quaternion.identity, 
         ObjectPoolManager.PoolType.SoundFX);

      audioSource.clip = audioClip;

      audioSource.volume = volume;
      
      audioSource.Play();

      float clipLength = audioSource.clip.length;

      StartCoroutine(ObjectPoolManager.ReturnObjectToPool(audioSource.gameObject, clipLength, ObjectPoolManager.PoolType.SoundFX));
   }
   
   public void PlayRandomSoundFXClip(AudioClip[] audioClips,Transform spawnTransform ,float volume)
   {
      int rand = Random.Range(0, audioClips.Length);
      
      AudioSource audioSource = ObjectPoolManager.SpawnObject(
         _soundFXObjectPrefab, 
         spawnTransform, 
         Quaternion.identity, 
         ObjectPoolManager.PoolType.SoundFX);

      audioSource.clip = audioClips[rand];

      audioSource.volume = volume;
      
      audioSource.Play();

      float clipLength = audioSource.clip.length;

      StartCoroutine(ObjectPoolManager.ReturnObjectToPool(
         audioSource.gameObject, 
         clipLength, 
         ObjectPoolManager.PoolType.SoundFX));
   }
}