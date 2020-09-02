namespace Openfeature.Music.Reference.Models
{
    using System.Collections.Generic;

    public class Scale : NoteSequence
    {
        protected Scale()
        {
        }

        public static Scale CreateNew()
        {
            return new Scale();
        }

        public static Scale Create(string description, List<int> notes)
        {
            return new Scale
            {
                Notes = notes,
                Description = description
            };
        }
    }
}