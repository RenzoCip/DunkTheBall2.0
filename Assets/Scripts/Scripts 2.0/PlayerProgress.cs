using System.Collections.Generic;

[System.Serializable]
public class LevelProgress
{
    public int levelNumber;
    public int maxPoints;
    public int stars;
    public bool isUnlocked;
    
    public LevelProgress (int levelNumber, int maxPoints,int stars, bool isUnlocked)
    {
        this.levelNumber = levelNumber;
        this.maxPoints = maxPoints;
        this.stars = stars;
        this.isUnlocked = isUnlocked;
    }
}
[System.Serializable]
public class PlayerProgress
{
    public List<LevelProgress> levels = new List<LevelProgress>();

    public PlayerProgress (int totalLevels)
    {
        for (int i = 0; i < totalLevels; i++)
        {
            bool unlocked = (i==0) ? true: false;// Solo el primer nivel está desbloqueado inicialmente
            levels.Add(new LevelProgress(i +1 , 0, 0,unlocked)); 
        }
    }

    public void UpdateLevelProgress(int levelNumber,int newPoints, int newStars)
    {
        LevelProgress level = levels[levelNumber];
        if (newPoints > level.maxPoints)
        {
            level.maxPoints = newPoints;
            level.stars = newStars;
        }
        if (level.stars > 0 && levelNumber +1 < levels.Count)
        {
            levels[levelNumber +1 ].isUnlocked = true;
        }
    }
}