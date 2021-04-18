using System.Collections;

[System.Serializable]
public class SessionData
{
    public SceneIndex lastLevel;
    public bool tutorialCompleted;
    public bool isNewSave;


    public SessionData()
    {

    }

    public SessionData(bool tutorialCompleted, bool isNewSave, SceneIndex level)
    {
        this.isNewSave = isNewSave;
        this.tutorialCompleted = tutorialCompleted;
        lastLevel = level;
    }

    public void Reset()
    {
        lastLevel = SceneIndex.Tutorial;
        isNewSave = true;
        tutorialCompleted = false;
    }
}
