namespace Openfeature.Music.Reference.Models
{
    using System.Collections.Generic;

    public class ChordInstance : Chord
    {
        public List<string> NoteNames { get; set; }

        public string RootName { get; private set; }

        public string DisplayName => $"{this.RootName} {this.Description}";

        public static ChordInstance Create(string description, List<int> notes, List<string> noteNames)
        {
            return new ChordInstance
            {
                RootName = noteNames[0],
                Notes = notes,
                NoteNames = noteNames,
                Description = description
            };
        }
    }
}