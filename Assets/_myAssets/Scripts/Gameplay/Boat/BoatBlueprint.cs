using UnityEngine;

public class BoatBlueprint : MonoBehaviour
{
    public MeshRenderer boatRenderer;
    public MeshFilter boatMeshFilter;

    // Array to hold the boat meshes
    public Mesh[] boatMeshes; // Assign in the Inspector

    // Current index of the selected boat
    private int currentBoatIndex = 0;

    private void Start()
    {
        // Initialize with the first boat mesh
        ChangeBoatMesh(currentBoatIndex);
    }

    public void ChangeBoatMesh(int index)
    {
        // Validate index
        if (index < 0 || index >= boatMeshes.Length)
            return;

        currentBoatIndex = index;
        boatMeshFilter.mesh = boatMeshes[currentBoatIndex];
    }

    public void NextBoat()
    {
        currentBoatIndex = (currentBoatIndex + 1) % boatMeshes.Length;
        ChangeBoatMesh(currentBoatIndex);
    }

    public void PreviousBoat()
    {
        currentBoatIndex = (currentBoatIndex - 1 + boatMeshes.Length) % boatMeshes.Length;
        ChangeBoatMesh(currentBoatIndex);
    }
}
