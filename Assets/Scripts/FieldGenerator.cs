using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldGenerator : MonoBehaviour
{
    [SerializeField] GameObject fieldPrefab;
    [SerializeField] GameObject fieldParent;
    GameObject generatedField;

    void Start()
    {
        GenerateField(3, 10, new Vector2(0, -10));
    }

    void GenerateField(int rows, int columns, Vector2 startingPos)
    {

        for(int row = 0; row < rows; row++)
        {
            for(int column = 0; column < columns; column++)
            {
                generatedField = Instantiate(fieldPrefab, startingPos + new Vector2(column, row), Quaternion.identity, fieldParent.transform);
                generatedField.name = "Field: " + row + "_" + column;
            }
        }
    }
}
