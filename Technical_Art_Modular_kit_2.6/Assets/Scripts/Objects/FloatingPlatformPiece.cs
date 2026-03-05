using UnityEngine;

public class FloatingPlatformPiece : MonoBehaviour
{
    [Header("Floating Settings")]
    public float amplitude = 0.25f;   // qué tanto sube y baja
    public float frequency = 1.0f;    // qué tan rápido flota
    public bool useLocalPosition = true;

    [Header("Smoothing (optional)")]
    public float freezeLerpSpeed = 12f; // qué tan rápido se “congela” suave

    private Vector3 startPos;
    private bool frozen = false;
    private float timeOffset;

    void Awake()
    {
        // guardamos la posición inicial para flotar alrededor de ella
        startPos = useLocalPosition ? transform.localPosition : transform.position;

        // para que no todas las piezas floten idéntico (se ve más natural)
        timeOffset = Random.Range(0f, 1000f);
    }

    void Update()
    {
        if (frozen)
        {
            // Mantenerse quieto en la posición base (con suavizado)
            Vector3 current = useLocalPosition ? transform.localPosition : transform.position;
            Vector3 target = startPos;

            Vector3 newPos = Vector3.Lerp(current, target, Time.deltaTime * freezeLerpSpeed);

            if (useLocalPosition) transform.localPosition = newPos;
            else transform.position = newPos;

            return;
        }

        // Flotación vertical
        float y = Mathf.Sin((Time.time + timeOffset) * frequency) * amplitude;
        Vector3 offset = new Vector3(0f, y, 0f);

        if (useLocalPosition)
            transform.localPosition = startPos + offset;
        else
            transform.position = startPos + offset;
    }

    public void SetFrozen(bool shouldFreeze)
    {
        frozen = shouldFreeze;

        // si se “descongela”, vuelve a flotar alrededor de su base original
        // (startPos sigue siendo el centro de flotación)
    }

    // Si quieres que el punto base se “recalcule” en runtime:
    public void RecalibrateBasePosition()
    {
        startPos = useLocalPosition ? transform.localPosition : transform.position;
    }
}