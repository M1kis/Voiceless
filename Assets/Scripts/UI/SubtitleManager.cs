using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // Asegúrate de importar el namespace de TextMeshPro

public class AudioSubtitleManagerTMP : MonoBehaviour
{
    [Header("Referencias")]
    public AudioSource audioSource;
    public TMP_Text subtitleText;  // Cambiado a TMP_Text
    public TextAsset srtFile;

    private List<SubtitleLine> subtitles = new List<SubtitleLine>();
    private int currentIndex = 0;

    void Start()
    {
        if (srtFile != null)
            ParseSRT(srtFile.text);
        else
            Debug.LogError("No se asignó el archivo .srt");
    }

    void Update()
    {
        if (audioSource.isPlaying && currentIndex < subtitles.Count)
        {
            float currentTime = audioSource.time;
            SubtitleLine line = subtitles[currentIndex];

            if (currentTime >= line.startTime && currentTime <= line.endTime)
            {
                subtitleText.text = line.text;
            }
            else if (currentTime > line.endTime)
            {
                subtitleText.text = "";
                currentIndex++;
            }
        }
    }

    void ParseSRT(string srt)
    {
        subtitles.Clear();
        string[] blocks = srt.Split(new string[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string block in blocks)
        {
            string[] lines = block.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length >= 3)
            {
                string timeLine = lines[1].Trim();
                string[] times = timeLine.Split(new[] { " --> " }, StringSplitOptions.None);

                float start = (float)ConvertToSeconds(times[0]);
                float end = (float)ConvertToSeconds(times[1]);

                string text = string.Join("\n", lines, 2, lines.Length - 2);

                subtitles.Add(new SubtitleLine
                {
                    startTime = start,
                    endTime = end,
                    text = text
                });
            }
        }
    }

    double ConvertToSeconds(string time)
    {
        time = time.Replace(',', '.');
        TimeSpan ts;
        if (TimeSpan.TryParse(time, out ts))
            return ts.TotalSeconds;
        else
            return 0;
    }

    class SubtitleLine
    {
        public float startTime;
        public float endTime;
        public string text;
    }
}
