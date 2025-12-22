using Arkanoid.Entities;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Arkanoid.Game
{
    [Serializable]
    [XmlRoot("ArkanoidSave")]
    public class GameSaveData
    {
        [XmlElement("Score")]
        public int Score { get; set; }

        [XmlElement("Lives")]
        public int Lives { get; set; }

        [XmlElement("BallPosition")]
        public Vector2Data BallPosition { get; set; }

        [XmlElement("BallVelocity")]
        public Vector2Data BallVelocity { get; set; }

        [XmlElement("PaddlePosition")]
        public Vector2Data PaddlePosition { get; set; }

        [XmlArray("Blocks")]
        [XmlArrayItem("Block")]
        public List<BlockSaveData> Blocks { get; set; }

        [XmlElement("SaveTime")]
        public DateTime SaveTime { get; set; }

        [XmlElement("CurrentLevel")]
        public int CurrentLevel { get; set; }

        [XmlElement("LevelBallSpeed")]
        public float LevelBallSpeed { get; set; }

        [XmlElement("LevelPaddleSpeed")]
        public float LevelPaddleSpeed { get; set; }

        [XmlElement("LevelBallRadius")]
        public float LevelBallRadius { get; set; }

        [XmlElement("LevelPaddleWidth")]
        public float LevelPaddleWidth { get; set; }

        [XmlElement("LevelBackgroundColor")]
        public ColorData LevelBackgroundColor { get; set; }

        public GameSaveData()
        {
            Blocks = new List<BlockSaveData>();
        }
    }

    [Serializable]
    public class BlockSaveData
    {
        [XmlElement("Position")]
        public Vector2Data Position { get; set; }

        [XmlElement("Type")]
        public string Type { get; set; }

        [XmlElement("IsDestroyed")]
        public bool IsDestroyed { get; set; }

        [XmlElement("HitPoints")]
        public int HitPoints { get; set; }
    }

    [Serializable]
    public class Vector2Data
    {
        [XmlAttribute("X")]
        public float X { get; set; }

        [XmlAttribute("Y")]
        public float Y { get; set; }

        public Vector2Data() { }

        public Vector2Data(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}