using System.Collections.Generic;  
using UnityEngine;
using System;
using System.IO;

namespace Managers
{
    public static class SaveKeys
    {
        public const string QUEST = "quest";
        public const string SAVESLOT = "saveslot";
        public const string OBJECTIVES = "objectives";
        public const string INVENTORY = "inventory";
    }

    [Serializable]
    public struct SaveData
    {
        public List<string> InventoryKeys;
        public List<string> Objectives;
        public List<string> Diaries;
        public List<string> Files;
        public int Saveslot;
        public int Quest;
        public int Day;
        public Dictionary<string, int> PersistentObjectStates;
        public string PreviousLevel;
        public Vector3 CarPosition;
        public Vector3 CarEulerRotation;
    }
    public static class SaveDataManager
    {
        static SaveDataManager()
        {
            CachedSaveData = new SaveData()
            {
                InventoryKeys = new List<string>(),
                Files = new List<string>(),
                Objectives = new List<string>(),
                PersistentObjectStates = new Dictionary<string, int>(),
                Diaries = new List<string>()
            };
        }

        public static List<string> InventoryKeys => CachedSaveData.InventoryKeys;

        public static List<string> Objectives => CachedSaveData.Objectives;

        public static Dictionary<string, int> PersistentObjectStates => CachedSaveData.PersistentObjectStates;

        /// <summary>
        /// This is a copy of the save data that you load when you begin playing
        /// it is altered as you play and then saved when you finish a quest
        /// when it is saved, it writes to disk, but this doesnt not happen
        /// until a quest is completed - aka if you found an item and then quit
        /// you would not get that item on next boot up unless you hit a checkpoint
        /// this could end up pissing people off, so maybe certain objects
        /// can be saved outside of outside of checkpoints, but we wont worry for now
        /// </summary>
        public static SaveData CachedSaveData;

        public static void SetPreviousLevel(string level)
        {
            CachedSaveData.PreviousLevel = level;
        }
        
        /// <summary>
        /// the save slot being used this session
        /// </summary>
        private static int _saveslot;
        public static void SetCurrentSaveSlot(int slot)
        {
            _saveslot = slot;
        }
        
        public static SaveData LoadSaveSlot(int saveSlot)
        {
            _saveslot = saveSlot;
            var x = LoadData(saveSlot);
            var loadData = JsonUtility.FromJson<SaveData>(x);
            return loadData;
        }

        public static void SaveCarLocation(Transform trans)
        {
            CachedSaveData.CarPosition = trans.position;
            CachedSaveData.CarEulerRotation = trans.rotation.eulerAngles;
        }

        public static void CacheQuest(int quest)
        {
            CachedSaveData.Quest = quest;

        }
        
        public static async void SaveObjectStateData(string key, int value)
        {
            if (!CachedSaveData.PersistentObjectStates.TryAdd(key, value))
            {
                CachedSaveData.PersistentObjectStates[key] = value;
            }

            SaveStringData(CachedSaveData);
        }

        public static bool SlotIsUsed(int slot)
        {
            return true;
        }

        public static int LoadData(string key)
        {
            if(CachedSaveData.PersistentObjectStates.ContainsKey(key))
                return CachedSaveData.PersistentObjectStates[key];
            return 0;
        }
        
        private static string _path = Path.Combine(Application.persistentDataPath, "saveGame");
    
        public static void SaveStringData(SaveData saveData)
        {
            var json = JsonUtility.ToJson(saveData); 
            File.WriteAllText($"{_path}_{saveData.Saveslot}", json);
        }

        public static string LoadData(int slot)
        {
            var path = $"{_path}_{slot}";
            var data = File.ReadAllText(path);
            return data;
        }

        public static void CacheDay(int day)
        {
            CachedSaveData.Day = day;
        }

        public static bool LoadedInitialQuest { get; set; }
    }
}

