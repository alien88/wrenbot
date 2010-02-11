using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using WrenLib;

namespace WrenBot.Types
{
    /// <summary>
    /// Darkages Map Object
    /// </summary>
    public class Map
    {
        /// <summary>
        /// Default Map Constructor
        /// </summary>
        public Map()
        {
            Width = 0;
            Height = 0;
            BaseMatrix = new byte[0, 0];
            Entities = new Dictionary<uint, MapEntity>();
            BlockPoints = new List<Point>();
        }

        public BotClient Socket;

        /// <summary>
        /// System Root Directory
        /// </summary>
        public static string Dir { get { return Environment.GetEnvironmentVariable("SystemRoot").Substring(0, 1); } }

        /// <summary>
        /// Darkages Folder Directory
        /// </summary>
        public static string DAFolder
        {
            get
            {
                
                if (System.IO.File.Exists("MapsPath.txt"))
                    return System.IO.File.ReadAllText("MapsPath.txt");
                if (Directory.Exists(@"!:\Program Files (x86)\KRU\Dark Ages".Replace("!", Dir)))
                    return @"!:\Program Files (x86)\KRU\Dark Ages".Replace("!", Dir);
                return @"!:\Program Files\KRU\Dark Ages".Replace("!", Dir);
            }
        }

        /// <summary>
        /// Default Map Constructor
        /// </summary>
        /// <param name="Number">Map Number</param>
        /// <param name="Width">Map Width</param>
        /// <param name="Height">Map Height</param>
        public Map(ushort Number, ushort Width, ushort Height)
        {
            try
            {
                System.IO.FileStream FS = System.IO.File.OpenRead(DAFolder + @"\Maps\lod" + Number.ToString() + ".map");
                System.IO.BinaryReader Reader = new System.IO.BinaryReader(FS);
                BaseMatrix = new byte[Width, Height];
                for (int y = 0; y < Height; y++)
                    for (int x = 0; x < Width; x++)
                    {
                        Reader.ReadUInt16();
                        BaseMatrix[x, y] = (byte)(CheckSOTP(Reader.ReadUInt16(), Reader.ReadUInt16()) ? 0x00 : 0x01);
                    }
                Entities = new Dictionary<uint, MapEntity>();
            }
            catch { }
        }

        /// <summary>
        /// Load Map Matrix
        /// </summary>
        /// <returns>Boolean: Has Map Loaded Sucessfully?</returns>
        public bool LoadMatrix()
        {
            try
            {
                System.IO.FileStream FS = System.IO.File.OpenRead(DAFolder + @"\Maps\lod" + Number.ToString() + ".map");
                System.IO.BinaryReader Reader = new System.IO.BinaryReader(FS);
                BaseMatrix = new byte[Width, Height];
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        Reader.ReadUInt16();
                        BaseMatrix[x, y] = (byte)(CheckSOTP(Reader.ReadUInt16(), Reader.ReadUInt16()) ? 0x00 : 0x01);
                    }
                }
                // Horizontal Top And Bottom Border Block
                for (int x = 0; x < Width; x++)
                {
                    if (BaseMatrix[x, 0] != 0x00)
                        BaseMatrix[x, 0] = 0x06;
                    if (BaseMatrix[x, 1] != 0x00)
                        BaseMatrix[x, 1] = 0x06;
                    if (BaseMatrix[x, Height - 1] != 0x00)
                        BaseMatrix[x, Height - 1] = 0x06;
                    if (BaseMatrix[x, Height - 2] != 0x00)
                        BaseMatrix[x, Height - 2] = 0x06;
                }
                // Vertical Left And Right Border Block
                for (int y = 0; y < Height; y++)
                {
                    if (BaseMatrix[0, y] != 0x00)
                        BaseMatrix[0, y] = 0x30;
                    if (BaseMatrix[1, y] != 0x00)
                        BaseMatrix[1, y] = 0x30;
                    if (BaseMatrix[Width - 1, y] != 0x00)
                        BaseMatrix[Width - 1, y] = 0x30;
                    if (BaseMatrix[Width - 2, y] != 0x00)
                        BaseMatrix[Width - 2, y] = 0x30;
                }
                FS.Close();
                return true;
            }
            catch (ObjectDisposedException)
            {
                return true;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Map Not Processed yet, Try Pressing F5 ingame.");
                return false;
            }
        }

        /// <summary>
        /// Map Number
        /// </summary>
        public ushort Number { get; set; }

        /// <summary>
        /// Map Width
        /// </summary>
        public ushort Width { get; set; }

        /// <summary>
        /// Map Height
        /// </summary>
        public ushort Height { get; set; }

        #region SOTP
        private static byte[] SOTP;
        private static void LoadSOTP()
        {
            FileStream FS = File.Open("Sotp", FileMode.Open);
            SOTP = new Byte[(int)FS.Length];
            FS.Read(SOTP, 0, SOTP.Length);
            FS.Close();
        }
        private static bool CheckSOTP(ushort Left, ushort Right)
        {
            if (SOTP == null)
                LoadSOTP();
            if (Left == 0 && Right == 0)
                return false;
            else if (Left == 0)
                try
                {
                    return SOTP[Right - 1] == 0 ? false : true;
                }
                catch { return true; }
            else if (Right == 0)
                try
                {
                    return SOTP[Left - 1] == 0 ? false : true;
                }
                catch { return true; }
            else try
                {
                    return SOTP[Left - 1] == 0 && SOTP[Right - 1] == 0 ? false : true;
                }
                catch { return true; }
        }
        #endregion

        /// <summary>
        /// Block Points List
        /// </summary>
        public List<Point> BlockPoints { get; set; }

        /// <summary>
        /// Base Map Matrix
        /// </summary>
        public byte[,] BaseMatrix { get; set; }

        /// <summary>
        /// Monster Heuristics (Used For Walk-To-Back)
        /// </summary>
        public byte MonsterHeuristic = 0x10;

        /// <summary>
        /// Current Back Matrix (Used For Walk-To-Back)
        /// </summary>
        public byte[,] CurrentBackMatrix
        {
            get
            {
                try
                {
                    byte[,] Matrix = new byte[Width + 1, Height + 1];
                    for (int y = 0; y < Height; y++)
                        for (int x = 0; x < Width; x++)
                            Matrix[x, y] = BaseMatrix[x, y];
                    MapEntity[] MapEntities = EntityList.ToArray();
                    foreach (MapEntity Ent in MapEntities)
                    {
                        if (Ent.EntityType == MapEntity.Type.Player || Ent.EntityType == MapEntity.Type.Monster || Ent.EntityType == MapEntity.Type.NPC)
                            if (Ent.EntityType == MapEntity.Type.Monster)
                            {
                                if (!(Ent as Monster).IsPet)
                                    Matrix[Ent.Location.X, Ent.Location.Y] = 0x00;
                            }
                            else
                                Matrix[Ent.Location.X, Ent.Location.Y] = 0x00;
                    }


                    foreach (MapEntity Ent in MapEntities)
                        if (Ent.EntityType == MapEntity.Type.Player || Ent.EntityType == MapEntity.Type.Monster || Ent.EntityType == MapEntity.Type.NPC)
                        {
                            switch (Ent.Location.Direction)
                            {
                                case FaceDirection.Up:
                                    {
                                        //   (X)
                                        //   (M)
                                        if (Ent.Location.Y - 1 > 0 && Ent.Location.Y - 1 < Height)
                                        {
                                            if (Matrix[Ent.Location.X, Ent.Location.Y - 1] != 0x00)
                                                Matrix[Ent.Location.X, Ent.Location.Y - 1] = MonsterHeuristic;
                                        }
                                        // (X) (X)
                                        //   (M)
                                        if (Ent.Location.Y - 1 > 0 && Ent.Location.Y - 1 < Height &&
                                            Ent.Location.X - 1 > 0 && Ent.Location.X - 1 < Width)
                                        {
                                            if (Matrix[Ent.Location.X - 1, Ent.Location.Y - 1] != 0x00)
                                                Matrix[Ent.Location.X - 1, Ent.Location.Y - 1] = MonsterHeuristic;
                                        }
                                        if (Ent.Location.Y - 1 > 0 && Ent.Location.Y - 1 < Height &&
                                            Ent.Location.X + 1 > 0 && Ent.Location.X + 1 < Width)
                                        {
                                            if (Matrix[Ent.Location.X + 1, Ent.Location.Y - 1] != 0x00)
                                                Matrix[Ent.Location.X + 1, Ent.Location.Y - 1] = MonsterHeuristic;
                                        }


                                        // (X(M)X)
                                        if (Ent.Location.X - 1 > 0 && Ent.Location.X - 1 < Width)
                                        {
                                            if (Matrix[Ent.Location.X - 1, Ent.Location.Y] != 0x00)
                                                Matrix[Ent.Location.X - 1, Ent.Location.Y] = MonsterHeuristic;
                                        }
                                        if (Ent.Location.X + 1 > 0 && Ent.Location.X + 1 < Width)
                                        {
                                            if (Matrix[Ent.Location.X + 1, Ent.Location.Y] != 0x00)
                                                Matrix[Ent.Location.X + 1, Ent.Location.Y] = MonsterHeuristic;
                                        }
                                    } break;
                                case FaceDirection.Down:
                                    {
                                        //   (M)
                                        //   (X)
                                        if (Ent.Location.Y + 1 > 0 && Ent.Location.Y + 1 < Height)
                                        {
                                            if (Matrix[Ent.Location.X, Ent.Location.Y + 1] != 0x00)
                                                Matrix[Ent.Location.X, Ent.Location.Y + 1] = MonsterHeuristic;
                                        }

                                        //   (M)
                                        // (X) (X)
                                        if (Ent.Location.Y + 1 > 0 && Ent.Location.Y + 1 < Height &&
                                            Ent.Location.X - 1 > 0 && Ent.Location.X - 1 < Width)
                                        {
                                            if (Matrix[Ent.Location.X - 1, Ent.Location.Y + 1] != 0x00)
                                                Matrix[Ent.Location.X - 1, Ent.Location.Y + 1] = MonsterHeuristic;
                                        }
                                        if (Ent.Location.Y + 1 > 0 && Ent.Location.Y + 1 < Height &&
                                            Ent.Location.X + 1 > 0 && Ent.Location.X + 1 < Width)
                                        {
                                            if (Matrix[Ent.Location.X + 1, Ent.Location.Y + 1] != 0x00)
                                                Matrix[Ent.Location.X + 1, Ent.Location.Y + 1] = MonsterHeuristic;
                                        }

                                        // (X(M)X)
                                        if (Ent.Location.X - 1 > 0 && Ent.Location.X - 1 < Width)
                                        {
                                            if (Matrix[Ent.Location.X - 1, Ent.Location.Y] != 0x00)
                                                Matrix[Ent.Location.X - 1, Ent.Location.Y] = MonsterHeuristic;
                                        }
                                        if (Ent.Location.X + 1 > 0 && Ent.Location.X + 1 < Width)
                                        {
                                            if (Matrix[Ent.Location.X + 1, Ent.Location.Y] != 0x00)
                                                Matrix[Ent.Location.X + 1, Ent.Location.Y] = MonsterHeuristic;
                                        }
                                    } break;
                                case FaceDirection.Left:
                                    {
                                        // (X(M)
                                        if (Ent.Location.X - 1 > 0 && Ent.Location.X - 1 < Width)
                                        {
                                            if (Matrix[Ent.Location.X - 1, Ent.Location.Y] != 0x00)
                                                Matrix[Ent.Location.X - 1, Ent.Location.Y] = MonsterHeuristic;
                                        }

                                        // (X)
                                        //   (M)
                                        // (X)
                                        if (Ent.Location.X - 1 > 0 && Ent.Location.X - 1 < Width &&
                                            Ent.Location.Y - 1 > 0 && Ent.Location.Y - 1 < Height)
                                        {
                                            if (Matrix[Ent.Location.X - 1, Ent.Location.Y - 1] != 0x00)
                                                Matrix[Ent.Location.X - 1, Ent.Location.Y - 1] = MonsterHeuristic;
                                        }
                                        if (Ent.Location.X - 1 > 0 && Ent.Location.X - 1 < Width &&
                                            Ent.Location.Y - 1 > 0 && Ent.Location.Y + 1 < Height)
                                        {
                                            if (Matrix[Ent.Location.X - 1, Ent.Location.Y + 1] != 0x00)
                                                Matrix[Ent.Location.X - 1, Ent.Location.Y + 1] = MonsterHeuristic;
                                        }

                                        //   (X)
                                        //   (M)
                                        //   (X)
                                        if (Ent.Location.Y - 1 > 0 && Ent.Location.Y - 1 < Height)
                                        {
                                            if (Matrix[Ent.Location.X, Ent.Location.Y - 1] != 0x00)
                                                Matrix[Ent.Location.X, Ent.Location.Y - 1] = MonsterHeuristic;
                                        }
                                        if (Ent.Location.Y + 1 > 0 && Ent.Location.Y + 1 < Height)
                                        {
                                            if (Matrix[Ent.Location.X, Ent.Location.Y + 1] != 0x00)
                                                Matrix[Ent.Location.X, Ent.Location.Y + 1] = MonsterHeuristic;
                                        }
                                    } break;
                                case FaceDirection.Right:
                                    {
                                        //   (M)X)
                                        if (Ent.Location.X + 1 > 0 && Ent.Location.X + 1 < Width)
                                        {
                                            if (Matrix[Ent.Location.X + 1, Ent.Location.Y] != 0x00)
                                                Matrix[Ent.Location.X + 1, Ent.Location.Y] = MonsterHeuristic;
                                        }

                                        //     (X)
                                        //   (M)
                                        //     (X)
                                        if (Ent.Location.X + 1 > 0 && Ent.Location.X + 1 < Width &&
                                            Ent.Location.Y - 1 > 0 && Ent.Location.Y - 1 < Height)
                                        {
                                            if (Matrix[Ent.Location.X + 1, Ent.Location.Y - 1] != 0x00)
                                                Matrix[Ent.Location.X + 1, Ent.Location.Y - 1] = MonsterHeuristic;
                                        }
                                        if (Ent.Location.X + 1 > 0 && Ent.Location.X + 1 < Width &&
                                            Ent.Location.Y + 1 > 0 && Ent.Location.Y + 1 < Height)
                                        {
                                            if (Matrix[Ent.Location.X + 1, Ent.Location.Y + 1] != 0x00)
                                                Matrix[Ent.Location.X + 1, Ent.Location.Y + 1] = MonsterHeuristic;
                                        }

                                        //   (X)
                                        //   (M)
                                        //   (X)
                                        if (Ent.Location.Y - 1 > 0 && Ent.Location.Y - 1 < Height)
                                        {
                                            if (Matrix[Ent.Location.X, Ent.Location.Y - 1] != 0x00)
                                                Matrix[Ent.Location.X, Ent.Location.Y - 1] = MonsterHeuristic;
                                        }
                                        if (Ent.Location.Y + 1 > 0 && Ent.Location.Y + 1 < Height)
                                        {
                                            if (Matrix[Ent.Location.X, Ent.Location.Y + 1] != 0x00)
                                                Matrix[Ent.Location.X, Ent.Location.Y + 1] = MonsterHeuristic;
                                        }
                                    } break;
                            }
                        }
                    return Matrix;
                }
                catch { return null; }
            }
        }

        /// <summary>
        /// Current Map Matrix
        /// </summary>
        public byte[,] CurrentMatrix
        {
            get
            {
                    byte[,] Matrix = new byte[Width + 1, Height + 1];
                    for (int y = 0; y < Height; y++)
                        for (int x = 0; x < Width; x++)
                            Matrix[x, y] = BaseMatrix[x, y];
                    MapEntity[] MapEntities = EntityList.ToArray();
                    foreach (MapEntity Ent in MapEntities)
                        if (Ent.EntityType == MapEntity.Type.Player || Ent.EntityType == MapEntity.Type.Monster || Ent.EntityType == MapEntity.Type.NPC)
                            Matrix[Ent.Location.X, Ent.Location.Y] = 0x00;
                 /*
                    switch (Socket.Aisling.Location.Direction)
                    {
                        case FaceDirection.Up:
                            {
                                if (Matrix[Socket.Aisling.Location.X, Socket.Aisling.Location.Y + 1] != 0x00)
                                    Matrix[Socket.Aisling.Location.X, Socket.Aisling.Location.Y + 1] = 0x30;
                            } break;
                        case FaceDirection.Right:
                            {
                                if (Matrix[Socket.Aisling.Location.X - 1, Socket.Aisling.Location.Y] != 0x00)
                                    Matrix[Socket.Aisling.Location.X - 1, Socket.Aisling.Location.Y] = 0x30;
                            } break;
                        case FaceDirection.Down:
                            {
                                if (Matrix[Socket.Aisling.Location.X, Socket.Aisling.Location.Y - 1] != 0x00)
                                    Matrix[Socket.Aisling.Location.X, Socket.Aisling.Location.Y - 1] = 0x30;
                            } break;
                        case FaceDirection.Left:
                            {
                                if (Matrix[Socket.Aisling.Location.X + 1, Socket.Aisling.Location.Y] != 0x00)
                                    Matrix[Socket.Aisling.Location.X + 1, Socket.Aisling.Location.Y] = 0x30;
                            } break;
                    }
                  * 
                  */
                    return Matrix;

            }
            set { }
        }

        /// <summary>
        /// Current Map Entitiy Dictionary
        /// </summary>
        public Dictionary<uint, MapEntity> Entities { get; set; }

        /// <summary>
        /// Current Number Of Attack Tries
        /// </summary>
        public int AttackTries { get; set; }

        /// <summary>
        /// Map Entity List (Used For Sorting And Filtering)
        /// </summary>
        public List<MapEntity> EntityList
        {
            get
            {
                try
                {
                    
                    MapEntity[] MapEntities = null;
                    lock (Entities)
                    {
                        MapEntities = new MapEntity[Entities.Count];
                        Entities.Values.CopyTo(MapEntities, 0);
                    }
                    return new List<MapEntity>(MapEntities);
                }
                catch { return null; }
            }
        }

        /// <summary>
        /// Find Entities On Map Matching Predicate
        /// </summary>
        /// <param name="Predicate">Predicate To Match</param>
        /// <returns>List Of Entities Matching Predicate</returns>
        public List<MapEntity> FindEntities(Predicate<MapEntity> Predicate)
        {
            try
            {
                return EntityList.FindAll(Predicate);
            }
            catch { return null; }
        }
    }
}
