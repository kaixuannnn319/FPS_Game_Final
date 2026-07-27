using System.IO;
using UnityEditor;
using UnityEngine;

public static class StoryDataGenerator
{
    private const string OutputFolder = "Assets/BaoXiaoxiao/UI/StoryData";

    [MenuItem("Tools/Story System/Generate 20 Stories")]
    public static void GenerateStories()
    {
        Directory.CreateDirectory(OutputFolder);
        AssetDatabase.Refresh();

        string[] stories =
        {
            "The doors are not locked to keep us inside.\n\nThey are locked to keep whatever walks the halls from finding us.",

            "Third night without sleep.\n\nWe hear something enormous moving beneath the castle. Each step shakes dust from the ceiling.",

            "His Majesty no longer asks how to preserve his health.\n\nHe asks only whether death itself can be defeated.",

            "The king ordered me to study blood, ancient magic, and the remains of forbidden creatures.\n\nI told him immortality was not medicine. He told me to continue.",

            "By royal decree, the Festival of Light shall continue for seven nights.\n\nMusic, food, and games will be offered to honor the Guardian Spirit.",

            "Mother said the Guardian Spirit protects good children.\n\nThen why did everyone start screaming when the castle bells rang?",

            "Three seals. Three locks placed upon mankind's greatest gift.\n\nWhy should a spirit decide how long a king is permitted to live?",

            "Five ships attempted to leave Sanctoria today.\n\nBefore sunset, all five returned from the opposite side of the island.",

            "I sailed north until the island vanished behind me.\n\nThe fog closed around the boat, and moments later I was facing the same harbor again.",

            "THE KING SAVES SANCTORIA AGAIN\n\nThe Demonlord disappeared before sunrise after His Majesty rode alone into the eastern valley.",

            "No witnesses were permitted near the battlefield.\n\nThe royal guard declared the area unsafe and removed all remains before morning.",

            "Bread, candles, medicine, weapons. Everything is gone.\n\nPeople no longer trade for comfort. They trade only for one more night of survival.",

            "The sickness begins with whispers.\n\nThen comes anger, confusion, and fear. By the third day, they no longer recognize their own families.",

            "We were ordered to protect the market, but the creatures never tire.\n\nThe lizard warrior fights as though battle is the only thing it remembers.",

            "The three Sacred Seals were never treasures.\n\nThey were promises between the Guardian Spirit and the people of Sanctoria.",

            "The king demanded access to the sacred chambers.\n\nWe refused him. That night, royal soldiers broke down the doors.",

            "The king placed all three seals around the altar and began the forbidden ritual.\n\nThe light that followed was brighter than the sun, yet it gave no warmth.",

            "The Guardian's cry passed through every wall on the island.\n\nThen the barrier rose from the sea, and the people began to change.",

            "I was promised eternity.\n\nInstead, the island has become my prison, and every voice outside my chamber speaks my name with hatred.",

            "What was taken must be returned.\n\nWhen the three seals stand together once more, the path beyond the barrier shall open."
        };

        for (int i = 0; i < stories.Length; i++)
        {
            string assetPath = $"{OutputFolder}/Story_{i + 1:00}.asset";

            StoryData storyData =
                AssetDatabase.LoadAssetAtPath<StoryData>(assetPath);

            if (storyData == null)
            {
                storyData = ScriptableObject.CreateInstance<StoryData>();
                AssetDatabase.CreateAsset(storyData, assetPath);
            }

            storyData.SetContent(stories[i]);
            EditorUtility.SetDirty(storyData);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Selection.activeObject =
            AssetDatabase.LoadAssetAtPath<DefaultAsset>(OutputFolder);

        EditorUtility.FocusProjectWindow();

        EditorUtility.DisplayDialog(
            "Story System",
            "20 story assets were created successfully.",
            "OK"
        );
    }
}
