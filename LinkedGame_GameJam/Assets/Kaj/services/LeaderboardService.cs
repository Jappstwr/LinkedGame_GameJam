

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Kaj.services
{
    /// <summary>
    /// Data class for rows in leaderboard
    /// </summary>
    [System.Serializable]
    public class LeaderboardRow
    {
        public string user_name;
        public int time_survived;
    }

    [System.Serializable]
    public class LeaderBoardState
    {
        public List<LeaderboardRow> state;
    }

    public class LeaderboardService
    {
        private const string url = "https://mygbjknwsgrbufbxvdrc.supabase.co/rest/v1/";
        private const string publishKey = "sb_publishable_uCLWGyUPoZkXRxjniwJZlA_07gWlf7f";
        private static string Username = "";

        static public void SetUsername(string name)
        {

            Username = name;
        }

        static public IEnumerator SubmitTime(int time)
        {
            // UnityWebRequest www = UnityWebRequest
            //     .PostWwwForm($"{url}leaderboard?select=*&order=time_survived.desc",
            //     form: $"{{\"user_name\": ${Username}, \"time_survived\": {time}}}"
            //     );
            // www.SetRequestHeader("apikey", publishKey);
            WWWForm form = new();
            form.AddField("user_name", Username);
            form.AddField("time_survived", time);
            var www = UnityWebRequest.Post($"{url}leaderboard", form);
            www.SetRequestHeader("apikey", publishKey);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
        }

        static public IEnumerator GetRows(TMP_Text UsernameRow, TMP_Text TimeRow)
        {

            UnityWebRequest www = UnityWebRequest
                .Get($"{url}leaderboard?select=*&order=time_survived.desc");
            www.SetRequestHeader("apikey", publishKey);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Show results as text
                // Debug.Log($"{{{"state"}: {www.downloadHandler.text}}}");
                // Debug.Log($"{{state: \"www.downloadHandler.text\"}}");
                // Debug.Log(JsonUtility.FromJson<LeaderBoardState>($"{{\"state\": {www.downloadHandler.text}}}"));

                var deserializedJson = JsonUtility.FromJson<LeaderBoardState>($"{{\"state\": {www.downloadHandler.text}}}");


                var textToDisplayUsername = "";
                var textToDisplayTime = "";

                foreach (var row in deserializedJson.state)
                {
                    textToDisplayUsername = $"{textToDisplayUsername}{row.user_name}\n";
                    textToDisplayTime = $"{textToDisplayTime}{row.time_survived}\n";
                }
                UsernameRow.text = textToDisplayUsername;
                TimeRow.text = textToDisplayTime;

            }
        }
    }
}