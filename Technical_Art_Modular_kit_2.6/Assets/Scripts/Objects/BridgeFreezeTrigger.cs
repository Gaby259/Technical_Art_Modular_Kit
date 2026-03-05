using UnityEngine;

public class BridgeFreezeTrigger : MonoBehaviour
{
    [Header("Who triggers it")]
    public string playerTag = "Player";

    [Header("Pieces to freeze")]
    public FloatingPlatformPiece[] pieces;

    private int playersInside = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        playersInside++;
        FreezeAll(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        playersInside = Mathf.Max(0, playersInside - 1);

        // si ya no hay jugador adentro, vuelven a flotar
        if (playersInside == 0)
            FreezeAll(false);
    }

    private void FreezeAll(bool freeze)
    {
        if (pieces == null) return;

        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] != null)
                pieces[i].SetFrozen(freeze);
        }
    }
}