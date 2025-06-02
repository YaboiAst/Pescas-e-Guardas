using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public static WindowManager Instance;

    [SerializeField] private List<WindowController> _allWindows = new List<WindowController>();

    [SerializeField] private List<WindowController> _currentOpenWindows = new List<WindowController>();

    public bool AnyWindowOpen() => _currentOpenWindows.Count > 0;

    private void Awake()
    {
        if (Instance)
        {
            Debug.LogWarning("More than one instance of the Window Manager. Destroying the new instance");
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Start() => UpdateAllWindows();

    private void UpdateAllWindows()
    {
        WindowController[] allWindows = FindObjectsByType<WindowController>(FindObjectsInactive.Exclude,FindObjectsSortMode.None) as WindowController[];

        foreach (WindowController window in allWindows!)
            _allWindows.Add(window);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_currentOpenWindows.Count >= 1)
                _currentOpenWindows[^1].CloseWindow();
        }
    }

    public void CloseAllWindows()
    {
        for (int i = _currentOpenWindows.Count - 1; i >= 0; i--) 
            _currentOpenWindows[i].CloseWindow();
    }

    public void AddWindow(WindowController window)
    {
        if (!_currentOpenWindows.Contains(window)) 
            _currentOpenWindows?.Add(window);
    }

    public IEnumerator RemoveWindow(WindowController window)
    {
        yield return new WaitForEndOfFrame();
        if (_currentOpenWindows.Contains(window)) 
            _currentOpenWindows.Remove(window);
    }
}