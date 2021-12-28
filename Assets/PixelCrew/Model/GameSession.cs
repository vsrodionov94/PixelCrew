using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;

        public PlayerData Data => _data;
        private PlayerData _save;

        public void Awake()
        {
            if (IsSessionExit())
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Save();
                DontDestroyOnLoad(this);
            }
        }

        internal void LoadLastSave()
        {
            _data = _save?.Clone();
        }

        private bool IsSessionExit()
        {
            GameSession[] sessions = FindObjectsOfType<GameSession>();
            foreach (GameSession gameSession in sessions)
            {
                if (gameSession != this)
                    return true;
            }

            return false;
        }

        public void Save()
        {
            _save = _data.Clone();
        }
    }

}
