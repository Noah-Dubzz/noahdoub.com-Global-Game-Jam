using CHAVIS;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField] SpriteRenderer puImageRenderer;
    [SerializeField] TextMeshPro puTextRenderer;

    private PowerUpsSO puInfo;
    public void SetUp(PowerUpsSO powerUp)
    {
        puInfo = powerUp;
        puImageRenderer.sprite = powerUp.image;
        puTextRenderer.text = powerUp.description;
    }

    private void OnMouseDown()
    {
        PowerUpManager.Instance.SelectPowerUp(puInfo);
        PlayerMovement.Instance.ApplyPU(puInfo);
        Time.timeScale = 1f;
    }
}
