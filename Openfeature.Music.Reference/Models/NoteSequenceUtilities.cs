namespace Openfeature.Music.Reference.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class NoteSequenceUtilities
    {
        private static readonly Dictionary<string, string> variants = new Dictionary<string, string>
        {
            { "second", "2nd" },
            { "fourth", "4th" },
            { "fifth", "5th" },
            { "sixth", "6th" },
            { "seventh", "7th" },
            { "ninth", "9th" },
            { "eleventh", "11th" },
            { "thirteenth", "13th" }
        };

        private static Dictionary<string, string> reversedVariants = variants.ToDictionary(v => v.Value, v => v.Key);

        public static IEnumerable<string> GetNotes(IChordData chordData, string root, NoteSequence results)
        {
            var rootNote = chordData.NoteNames.First(name => name.FindRoot(root.CorrectCase()));

            var rootNoteIndex = chordData.NoteNames.IndexOf(rootNote);
            var notes = new List<string>();

            foreach (var noteNumber in results.Notes)
            {
                notes.Add(chordData.NoteNames[(rootNoteIndex + noteNumber) % chordData.NoteNames.Count]);
            }

            // notes[0] = root.CorrectCase();
            return notes;
        }

        public static IEnumerable<T> SearchDescriptions<T>(List<T> noteSequences, string searchTerm) where T : NoteSequence
        {
            var searchTermList = searchTerm.Split(' ');
            var searchResults = noteSequences.Where(
                noteSequence => searchTermList.All(
                    termText => noteSequence.Description.ContainsWithVariants(termText))).ToArray();
            var results = new T[searchResults.Length];
            Array.Copy(searchResults, results, searchResults.Length);

            return results;
        }

        public static IEnumerable<T> ExactDescription<T>(List<T> noteSequences, string searchTerm) where T : NoteSequence
        {
            searchTerm = searchTerm.Trim();

            var searchResults = noteSequences
                                .Where(noteSequence => noteSequence.Description.Equals(searchTerm, StringComparison.InvariantCultureIgnoreCase))
                                .ToArray();

            var results = new T[searchResults.Length];
            Array.Copy(searchResults, results, searchResults.Length);

            return results;
        }

        public static bool FindRoot(this string noteName, string searchTerm)
        {
            searchTerm = searchTerm.Trim();
            if (searchTerm.Length == 1)
            {
                return noteName == searchTerm;
            }

            if (searchTerm.Length == 2)
            {
                return noteName.Contains(searchTerm);
            }

            return false;
        }

        private static string CorrectCase(this string noteName)
        {
            var upperNoteName = noteName.Substring(0, 1).ToUpper();
            var accidental = noteName.Length > 1 ? noteName.Substring(1, 1) : string.Empty;
            return upperNoteName + accidental;
        }

        private static bool ContainsWithVariants(this string target, string value, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
        {
            var valueLower = value.ToLower();
            var found = false;
            
            foreach (var variant in variants)
            {
                if (variant.Key.Contains(valueLower))
                {
                    if (found == false)
                    {
                        found = target.Contains(variant.Value, comparison);
                    }
                }
            }

            return target.Contains(valueLower, comparison) || found;
        }
    }
}