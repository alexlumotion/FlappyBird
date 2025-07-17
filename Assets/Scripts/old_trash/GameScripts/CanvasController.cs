using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public GameObject menuPanel;


    // Update is called once per frame
    void Update()
    {
        menuPanel.SetActive(GameManager.Instance.GameState != GameState.Playing);
    }
}
