using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] shapes;

    public bool canSpawn;

    public static bool gameStarted;

    public static bool gameOver = false;


    private void Awake()
    {
        gameStarted = false;
    }

    private void Start()
    {
        canSpawn = false;

    }

    private void Update()
    {
        if (Input.anyKeyDown && !gameStarted)
        {
            gameStarted = true;
            canSpawn = true;
        }

        SpawnShape();
    }

    void SpawnShape()
    {

        if (canSpawn && !gameOver)
        {
            int index = Random.Range(0, shapes.Length);

            GameObject shape = Instantiate(shapes[index], transform.position, Quaternion.identity);
           

            if (index != 3)
            {
                shape.GetComponent<ShapeController>().canRotate = true;
            }

            GameManager.Instance.objectList.Add(shape.GetComponent<ShapeController>());

            GameManager.Instance.DisplayPreview();

            canSpawn = false;
        }
    }
}
