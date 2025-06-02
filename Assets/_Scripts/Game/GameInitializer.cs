using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [Scene] [SerializeField] private int _gameScene;

    // private async void Start()
    // {
    //     BindObjects();
    //     //Show Loading Screen
    //     await InitializeObjects();
    //     await CreateObjects();
    //     
    //     PrepareGame();
    //     //Hide Loading Screen
    //
    //     await BeginGame();
    // }
    //
    // private void BindObjects()
    // {
    //     //Instantiate and bind
    // }
    //
    // private async UniTask InitializeObjects()
    // {
    //     //Initialize Objects
    // }
    //
    // private async UniTask CreateObjects()
    // {
    //     //Create Objects
    // }
    //
    // private void PrepareGame()
    // {
    //     //Prepare Game
    // }
    //
    // private async UniTask BeginGame()
    // {
    //     //Begin Game
    // }
}
