using UnityEngine;
using UnityEngine.VFX;

public class SnowTrigger : MonoBehaviour
{
    [SerializeField] private VisualEffect snowVFX;

    private void Start()
    {
        if (snowVFX != null)
            snowVFX.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (snowVFX != null)
                snowVFX.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (snowVFX != null)
                snowVFX.Stop();
        }
    }
}