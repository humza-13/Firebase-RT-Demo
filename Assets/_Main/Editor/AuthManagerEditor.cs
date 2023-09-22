using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Phoenix.Firebase.Managers.AuthManager))]
public class AuthManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Create a GUIStyle for the green box text.
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.normal.textColor = Color.grey;
        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.fontSize = 24;
        
        // Create a green box in the Inspector.
        GUIStyle greenBoxStyle = new GUIStyle(GUI.skin.box);
        greenBoxStyle.normal.background = MakeTexture(1, 1, Color.green);

        // Begin a vertical layout group with the green box style.
        EditorGUILayout.BeginVertical(greenBoxStyle);

        // Calculate the size of the label.
        GUIContent labelContent = new GUIContent("Auth Manager");
        Vector2 labelSize = labelStyle.CalcSize(labelContent);

        // Calculate the position to center the label both horizontally and vertically.
        float centerOffsetX = (greenBoxStyle.fixedWidth - labelSize.x) / 2;
        float centerOffsetY = (greenBoxStyle.fixedHeight - labelSize.y) / 2;

        // Create a horizontal layout group to center the label horizontally.
        EditorGUILayout.BeginHorizontal();

        // Create a vertical space to center the label vertically.
        GUILayout.FlexibleSpace();

        // Create a horizontal space to center the label horizontally.
        GUILayout.Space(centerOffsetX);

        // Display the centered label "Auth Manager" using the custom GUIStyle.
        GUILayout.Label("Auth Manager", labelStyle);

        // End the horizontal layout group.
        EditorGUILayout.EndHorizontal();

        // Create a vertical space to center the label vertically.
        GUILayout.FlexibleSpace();

        // End the vertical layout group.
        EditorGUILayout.EndVertical();
    }

    private Texture2D MakeTexture(int width, int height, Color color)
    {
        Color[] pixels = new Color[width * height];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = color;
        }

        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }
}