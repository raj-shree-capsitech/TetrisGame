using UnityEngine;

public class SpawnTetro : MonoBehaviour
{
    public GameObject[] Tetro;
    public static bool hasgamestart = false;
    public void StartSpawn()
    {   
        hasgamestart = true;
        SpawnTetromino();
    }
    public void SpawnTetromino()
    {
        if (!hasgamestart) return;
        GameObject spwantetro = Instantiate(Tetro[Random.Range(0, Tetro.Length)], transform.position, Quaternion.identity);

        TetrominoMove move = spwantetro.GetComponent<TetrominoMove>();

        if (!move.ValidMove())
        {
            Destroy(spwantetro);
            UIManager.instance.GameOver();
            return;
        }
    }

    public void StopSpawner()
    {
        hasgamestart = false;
    }
}
