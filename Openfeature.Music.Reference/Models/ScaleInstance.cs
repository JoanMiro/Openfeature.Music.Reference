namespace Openfeature.Music.Reference.Models
{
    using System.Collections.Generic;

    public class ScaleInstance : Scale
    {
        public List<string> NoteNames { get; set; }

        public string RootName { get; private set; }

        public string DisplayName => $"{this.RootName} {this.Description}";

        public static ScaleInstance Create(string description, List<int> notes, List<string> noteNames)
        {
            return new ScaleInstance
            {
                RootName = noteNames[0],
                Notes = notes,
                NoteNames = noteNames,
                Description = description
            };
        }
    }
}