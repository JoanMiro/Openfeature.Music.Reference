namespace Openfeature.Music.Reference.Models
{
    using System.Collections.Generic;

    public abstract class NoteSequence
    {
        public List<int> Notes { get; set; }

        public string Description { get; set; }

    }
}