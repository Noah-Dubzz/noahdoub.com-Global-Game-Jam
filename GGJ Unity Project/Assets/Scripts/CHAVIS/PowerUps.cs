using TMPro;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField] MeshRenderer puImageRenderer;
    [SerializeField] TextMeshPro puTextRenderer;

    private PowerUpsSO puInfo;
    public void SetUp(PowerUpsSO powerUp)
    {
        puInfo = powerUp;
        puImageRenderer.material = powerUp.powerUpImage;
        puTextRenderer.text = powerUp.powerUpTxt;
    }

    private void OnMouseDown()
    {
        PowerUpManager.Instance.SelectPowerUp(puInfo);
        Hero.Instance.ApplyPU(puInfo);
        Time.timeScale = 1f;
    }
}
