namespace Openfeature.Music.Reference.Models
{

    using Controllers;

    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Xml;

    /// <summary>
    /// Openfeature Chord Data
    /// </summary>
    public class ChordData : IChordData
    {
        /// <summary>
        /// Chord Data Constructor
        /// </summary>
        public ChordData()
        {
            this.Chords = new List<Chord>();
            this.Scales = new List<Scale>();
            this.NoteNames = new List<string>();

            this.LoadData();
        }

        private void LoadData()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("Openfeature.Music.Reference.Data.ChordData.xml");
            var chordsDoc = new XmlDocument();
            chordsDoc.Load(stream);

            foreach (var childElement in chordsDoc.SelectNodes("descendant::Chord").Cast<XmlElement>())
            {
                var description = childElement.SelectSingleNode("descendant::Description").InnerText;
                var noteList = childElement.SelectNodes("descendant::NoteIndex").Cast<XmlNode>().Select(x => int.Parse(x.InnerText));
                var chord = Chord.Create(description, noteList.ToList());
                this.Chords.Add(chord);
            }

            foreach (var childElement in chordsDoc.SelectNodes("descendant::Scale").Cast<XmlElement>())
            {
                var description = childElement.SelectSingleNode("descendant::Description").InnerText;
                var noteList = childElement.SelectNodes("descendant::NoteIndex").Cast<XmlNode>().Select(x => int.Parse(x.InnerText));
                var scale = Scale.Create(description, noteList.ToList());
                this.Scales.Add(scale);
            }

            foreach (var childElement in chordsDoc.SelectNodes("descendant::NoteName").Cast<XmlElement>())
            {
                var noteName = childElement.InnerText;
                this.NoteNames.Add(noteName);
            }
        }

        public List<string> NoteNames { get; }

        public List<Chord> Chords { get; }

        public List<Scale> Scales { get; }
    }
}