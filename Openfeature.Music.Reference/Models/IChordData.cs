namespace Openfeature.Music.Reference.Models
{
    using System.Collections.Generic;

    public interface IChordData
    {
        List<Chord> Chords { get; }
        List<Scale> Scales { get; }
        List<string> NoteNames { get; }
    }
}