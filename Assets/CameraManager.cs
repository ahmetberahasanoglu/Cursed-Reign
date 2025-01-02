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
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        // GameObject player = GameObject.Find("Player");
        //  if (player != null)
        //   {
        //      virtualCamera.Follow = player.transform;
        //  }
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
        if (scene.name == "CreditsScene")
        {
            Destroy(gameObject);
        }
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {

            virtualCamera.Follow = player.transform;
        }
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
            virtualCamera.m_Lens.OrthographicSize = 8f;
        }
        else if (sceneName == "level2")
        {
            PolygonCollider2D level2Collider = GameObject.Find("level1CameraLimit").GetComponent<PolygonCollider2D>();
            confiner.m_BoundingShape2D = level2Collider;
            confiner.enabled = true;
        }
        else if (sceneName == "level3")
        {
            PolygonCollider2D level3Collider = GameObject.Find("level1CameraLimit").GetComponent<PolygonCollider2D>();
            confiner.m_BoundingShape2D = level3Collider;
            confiner.enabled = true;
        }
        else if (sceneName == "level4")
        {
            PolygonCollider2D level4Collider = GameObject.Find("level1CameraLimit").GetComponent<PolygonCollider2D>();
            confiner.m_BoundingShape2D = level4Collider;
            confiner.enabled = true;
        }
        else if (sceneName == "level5")
        {
            PolygonCollider2D level5Collider = GameObject.Find("level1CameraLimit").GetComponent<PolygonCollider2D>();
            confiner.m_BoundingShape2D = level5Collider;
            confiner.enabled = true;
        }
        else if (sceneName == "Door1")
        {
            PolygonCollider2D door1Collider = GameObject.Find("CameraLimit").GetComponent<PolygonCollider2D>();
            confiner.m_BoundingShape2D = door1Collider;
            confiner.enabled = true;
        }
        else if (sceneName == "Door2")
        {
            PolygonCollider2D door2Collider = GameObject.Find("CameraLimit").GetComponent<PolygonCollider2D>();
            confiner.m_BoundingShape2D = door2Collider;
            confiner.enabled = true;
        }
        else if (sceneName == "Door3")
        {
            PolygonCollider2D door3Collider = GameObject.Find("CameraLimit").GetComponent<PolygonCollider2D>();
            confiner.m_BoundingShape2D = door3Collider;
            confiner.enabled = true;
        }
        else if (sceneName == "FirstScene")
        {
            PolygonCollider2D door2Collider = GameObject.Find("CameraLimit1").GetComponent<PolygonCollider2D>();
            confiner.m_BoundingShape2D = door2Collider;
            confiner.enabled = true;
            virtualCamera.m_Lens.OrthographicSize = 6f;
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
