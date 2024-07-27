using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public static class SaveLoadManager
{
    public static void SavePlayer(Player currentPlayer)
    {
        string pathToFile = Application.persistentDataPath + "/statistics.olmp";

        if (File.Exists(pathToFile))
        {
            List<PlayerData> playersData = LoadPlayers();
            List<PlayerData> tempPlayersData = new List<PlayerData>(playersData);

            // Kontrol: Aynı isimli oyuncu zaten listede mi?
            if (playersData.Exists(p => p.playerName == currentPlayer.Name))
            {
                Debug.LogWarning("Player with the same name already exists in the list.");
                return;
            }

            foreach (PlayerData player in playersData)
            {
                if (currentPlayer.MaxJumpScore >= player.playerScore)
                {
                    SwapPlayer(pathToFile, playersData, player, currentPlayer, tempPlayersData);
                    break;
                }
            }
        }
        else
        {
            CreateEmptyPlayers();
            // Tekrar SavePlayer çağırarak ilk oyuncuyu kaydedin
            SavePlayer(currentPlayer);
        }
    }

    public static void SwapPlayer(
        string pathToFile,
        List<PlayerData> playersData,
        PlayerData player,
        Player currentPlayer,
        List<PlayerData> tempPlayersData)
    {
        BinaryFormatter bFormatter = new BinaryFormatter();
        FileStream fStream = new FileStream(pathToFile, FileMode.Create);

        int indexSwap = playersData.IndexOf(player);

        PlayerData newRecordPlayer = new PlayerData();
        newRecordPlayer.playerName = currentPlayer.Name;
        newRecordPlayer.playerScore = (int)currentPlayer.MaxJumpScore;

        tempPlayersData.Insert(indexSwap, newRecordPlayer);
        tempPlayersData.RemoveAt(tempPlayersData.Count - 1);

        bFormatter.Serialize(fStream, tempPlayersData);
        fStream.Close();
    }

    public static List<PlayerData> LoadPlayers()
    {
        string pathToFile = Application.persistentDataPath + "/statistics.olmp";

        if (File.Exists(pathToFile))
        {
            BinaryFormatter bFormatter = new BinaryFormatter();
            FileStream fStream = new FileStream(pathToFile, FileMode.Open);

            List<PlayerData> playersData = (List<PlayerData>)bFormatter.Deserialize(fStream);
            fStream.Close();

            return playersData;
        }
        return CreateEmptyPlayers();
    }

    public static PlayerData? LoadBestPlayer()
    {
        string pathToFile = Application.persistentDataPath + "/statistics.olmp";

        if (File.Exists(pathToFile))
        {
            List<PlayerData> playersData = LoadPlayers();
            return playersData[0];
        }
        return null;
    }

    public static List<PlayerData> CreateEmptyPlayers()
    {
        string pathToFile = Application.persistentDataPath + "/statistics.olmp";

        BinaryFormatter bFormatter = new BinaryFormatter();
        FileStream fStream = new FileStream(pathToFile, FileMode.Create);

        List<PlayerData> playersData = new List<PlayerData>();

        for (int i = 0; i < 10; i++)
        {
            PlayerData playerData = new PlayerData
            {
                playerName = "unknownPlayer" + (i + 1),
                playerScore = 10 - i
            };
            playersData.Add(playerData);
        }

        bFormatter.Serialize(fStream, playersData);
        fStream.Close();

        return playersData;
    }

    public static bool isDataFileExists()
    {
        string pathToFile = Application.persistentDataPath + "/statistics.olmp";
        return File.Exists(pathToFile);
    }

    public static bool isPlayerBeatRecord(Player currentPlayer)
    {
        List<PlayerData> playersData = LoadPlayers();
        foreach (PlayerData player in playersData)
        {
            if (currentPlayer.MaxJumpScore >= player.playerScore)
            {
                return true;
            }
        }
        return false;
    }
}
