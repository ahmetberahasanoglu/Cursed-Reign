using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineConfiner2D confiner;
    public static CameraManager instance;

    private void Start()
    {
        confiner = virtualCamera.GetComponent<CinemachineConfiner2D>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (PlayerMovement.instance != null)
        {
            virtualCamera.Follow = PlayerMovement.instance.transform;
        }

        // Sahne yüklendiðinde confiner'ý sahnede bul ve güncelle
        UpdateConfinerForScene(scene.name);
    }

    private void UpdateConfinerForScene(string sceneName)
    {
        // Sahne adlarýna göre collider'larý dinamik olarak bul
        if (sceneName == "level1")
        {
            PolygonCollider2D door1Collider = GameObject.Find("level1CameraLimit").GetComponent<PolygonCollider2D>();
            confiner.m_BoundingShape2D = door1Collider;
            confiner.enabled = true;
        }
        else if (sceneName == "Door1")
        {
            PolygonCollider2D door2Collider = GameObject.Find("CameraLimit").GetComponent<PolygonCollider2D>();
            confiner.m_BoundingShape2D = door2Collider;
            confiner.enabled = true;
        }
        else
        {
            confiner.enabled = false;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
