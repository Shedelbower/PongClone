using UnityEngine;
using TMPro;

public class FlashingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _flashingText;
    [SerializeField] private Gradient _gradient;
    [SerializeField] private float _speed = 1.0f;

    private float _timer = 0.0f;

    private void Update()
    {
        _timer += Time.deltaTime;

        float t = (Mathf.Sin(_timer * _speed) + 1) / 2;

        _flashingText.color = _gradient.Evaluate(t);
    }
}
