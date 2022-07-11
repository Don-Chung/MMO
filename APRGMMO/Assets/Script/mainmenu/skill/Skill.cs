using ARPGCommon.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Basic,
    Skill
}

public enum PosType
{
    Basic, One = 1, Two = 2, Three = 3
}

public class Skill
{
    private int id;
    private string name;
    private string icon;
    private PlayerType playerType;
    private SkillType skillType;
    private PosType posType;
    private int coldTime;
    private int damage;
    private int level = 1;

    private SkillDB skillDB;

    public void Sync(SkillDB skillDB)
    {
        this.skillDB = skillDB;
        Level = skillDB.Level;
    }

    public SkillDB SkillDB
    {
        get { return skillDB; }
        set { skillDB = value; }
    }

    public int Id { get => id; set => id = value; }
    public string Name { get => name; set => name = value; }
    public string Icon { get => icon; set => icon = value; }
    public PlayerType PlayerType { get => playerType; set => playerType = value; }
    public SkillType SkillType { get => skillType; set => skillType = value; }
    public PosType PosType { get => posType; set => posType = value; }
    public int ColdTime { get => coldTime; set => coldTime = value; }
    public int Damage { get => damage; set => damage = value; }
    public int Level { get => level; set => level = value; }

    public void Upgrade()
    {
        level++;
        if(skillDB == null)
        {
            skillDB = new SkillDB();
            skillDB.Level = Level;
            skillDB.SkillID = id;
        }
        else
        {
            skillDB.Level = Level;
        }
    }
}
