using UnityEngine;
using UnityEditor;

public class StoryEditor : EditorWindow
{
    private enum View { List, Beat }

    private Vector2 scroll = new Vector2();
    private int currentIndex = -1;
    private View view;

    [MenuItem("Tool/Show Story Editor")]
    public static void ShowStoryEditor()
    {
        GetWindow(typeof(StoryEditor));
    }

    //sets up gui
    void OnGUI()
    {
        StoryData data = StoryData.LoadData();//Get story data from editor
        SerializedObject dataObj = new SerializedObject(data);//turn data in to an object
        SerializedProperty beatList = dataObj.FindProperty("beats");//get beats

        //note* look up editor api 
        EditorGUILayout.BeginVertical();
        scroll = EditorGUILayout.BeginScrollView(scroll);


        if (view == View.Beat && currentIndex != -1)
        {
            OnGUI_BeatView(beatList, currentIndex);
        }
        else
        {
            OnGUI_ListView(beatList);
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        dataObj.ApplyModifiedProperties();
    }
    //defines list view
    private void OnGUI_ListView(SerializedProperty beatList)
    {
        EditorGUILayout.BeginVertical();

        if (beatList.arraySize == 0)
        {
            AddBeat(beatList, 1, "First Story Beat");
        }

        for (int count = 0; count < beatList.arraySize; ++count)
        {
            SerializedProperty arrayElement = beatList.GetArrayElementAtIndex(count);
            SerializedProperty text = arrayElement.FindPropertyRelative("text");
            SerializedProperty id = arrayElement.FindPropertyRelative("id");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(id.intValue.ToString());

            if (GUILayout.Button("Edit"))
            {
                view = View.Beat;
                currentIndex = count;
                break;
            }

            if (GUILayout.Button("Delete"))
            {
                beatList.DeleteArrayElementAtIndex(count);
                break;
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(text);
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
    }
    //defines how beats are displayed on the gui
    private void OnGUI_BeatView(SerializedProperty beatList, int index)
    {
        SerializedProperty arrayElement = beatList.GetArrayElementAtIndex(index);
        SerializedProperty choiceList = arrayElement.FindPropertyRelative("dialogueChoices");
        SerializedProperty text = arrayElement.FindPropertyRelative("text");
        SerializedProperty id = arrayElement.FindPropertyRelative("id");
        //To easily toggle if beat leads to a transition
        SerializedProperty isEnd = arrayElement.FindPropertyRelative("endBeat");

        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("Beat ID: " + id.intValue.ToString());

        //Displays wether beat is a trigger to a transition
        text.stringValue = EditorGUILayout.TextArea(text.stringValue, GUILayout.Height(200));

        GUILayout.Label("\nIs Transition Trigger");
        isEnd.boolValue = EditorGUILayout.Toggle(isEnd.boolValue,GUILayout.Height(10f), GUILayout.Width(10f));
      
        OnGUI_BeatViewDecision(choiceList, beatList);

        EditorGUILayout.EndVertical();

        if (GUILayout.Button("Return to Beat List", GUILayout.Height(50)))
        {
            view = View.List;
            currentIndex = -1;
        }
    }
    //defines gow beats decisions are displayed
    private void OnGUI_BeatViewDecision(SerializedProperty choiceList, SerializedProperty beatList)
    {
        EditorGUILayout.BeginHorizontal();

        for (int count = 0; count < choiceList.arraySize; ++count)
        {
            OnGUI_BeatViewChoice(choiceList, count, beatList);
        }

        if (GUILayout.Button((choiceList.arraySize == 0 ? "Add Choice" : "Add Another Choice"), GUILayout.Height(100)))
        {
            int newBeatId = FindUniqueId(beatList);
            AddBeat(beatList, newBeatId);
            AddChoice(choiceList, newBeatId);
        }

        EditorGUILayout.EndHorizontal();
    }

    private void OnGUI_BeatViewChoice(SerializedProperty choiceList, int index, SerializedProperty beatList)
    {
        SerializedProperty arrayElement = choiceList.GetArrayElementAtIndex(index);

        SerializedProperty text = arrayElement.FindPropertyRelative("text");
        SerializedProperty beatId = arrayElement.FindPropertyRelative("targetBeatID");

        EditorGUILayout.BeginVertical();

        GUILayout.Label("\nChoice text");
        text.stringValue = EditorGUILayout.TextArea(text.stringValue, GUILayout.Height(50));

   
        SerializedProperty Result = arrayElement.FindPropertyRelative("resultName");

        //Consequence evaluated
        GUILayout.Label("Result Name");
        Result.stringValue = EditorGUILayout.TextArea(Result.stringValue, GUILayout.Height(20f), GUILayout.Width(200f));

        EditorGUILayout.LabelField("Leads to Beat ID: " + beatId.intValue.ToString());
           
        if (GUILayout.Button("Go to Beat", GUILayout.Height(20f), GUILayout.Width(100f)))
        {
            currentIndex = FindIndexOfBeatId(beatList, beatId.intValue);
            GUI.FocusControl(null);
            Repaint();
        }
 
        EditorGUILayout.EndVertical();
    }

    //finds beat with unique ID
    private int FindUniqueId(SerializedProperty beatList)
    {
        int result = 1;

        while (IsIdInList(beatList, result))
        {
            ++result; 
        }

        return result;
    }

    private bool IsIdInList(SerializedProperty beatList, int beatId)
    {
        bool result = false;

        for (int count = 0; count < beatList.arraySize && !result; ++count)
        {
            SerializedProperty arrayElement = beatList.GetArrayElementAtIndex(count);
            SerializedProperty id = arrayElement.FindPropertyRelative("id");
            result = id.intValue == beatId;
        }

        return result;
    }

    private int FindIndexOfBeatId(SerializedProperty beatList, int beatId)
    {
        int result = -1;

        for (int count = 0; count < beatList.arraySize; ++count)
        {
            SerializedProperty arrayElement = beatList.GetArrayElementAtIndex(count);
            SerializedProperty id = arrayElement.FindPropertyRelative("id");
            if (id.intValue == beatId)
            {
                result = count;
                break;
            }
        }

        return result;
    }

    private void AddBeat(SerializedProperty beatList, int beatId, string initialText = "New Story Beat")
    {
        int index = beatList.arraySize;
        beatList.arraySize += 1;
        SerializedProperty arrayElement = beatList.GetArrayElementAtIndex(index);
        SerializedProperty text = arrayElement.FindPropertyRelative("text");
        SerializedProperty id = arrayElement.FindPropertyRelative("id");

        text.stringValue = initialText;
        id.intValue = beatId;
    }

    private void AddChoice(SerializedProperty choiceList, int beatId, string initialText = "New Beat Choice")
    {
        int index = choiceList.arraySize;
        choiceList.arraySize += 1;
        SerializedProperty arrayElement = choiceList.GetArrayElementAtIndex(index);
        SerializedProperty text = arrayElement.FindPropertyRelative("text");
        SerializedProperty nextId = arrayElement.FindPropertyRelative("targetBeatID");

        text.stringValue = initialText;
        nextId.intValue = beatId;
    }
}
