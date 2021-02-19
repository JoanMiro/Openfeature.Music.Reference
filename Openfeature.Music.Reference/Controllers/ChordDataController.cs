namespace Openfeature.Music.Reference.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models;

    /// <summary>
    /// Controller class for all chord and scale data
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ChordDataController : ControllerBase
    {
        private readonly IChordData chordData;
        private readonly ILogger<ChordDataController> logger;

        /// <summary>
        /// Retrieves all chord and scale data
        /// </summary>
        public ChordDataController(IChordData chordData, ILogger<ChordDataController> logger)
        {
            this.chordData = chordData;
            this.logger = logger;
        }

        /// <summary>
        /// Retrieves all chord and scale data
        /// </summary>
        /// <returns>JSON structure</returns>
        [HttpGet]
        public ChordData ChordData()
        {
            return (ChordData)this.chordData;
        }

        /// <summary>
        /// Retrieves all chord data
        /// </summary>
        /// <returns>List of chords</returns>
        [HttpGet("Chords")]
        public IEnumerable<Chord> Chords()
        {
            return this.chordData.Chords;
        }

        /// <summary>
        /// Retrieves all chord data matching the  search term(s)
        /// </summary>
        /// <param name="searchTerm">Text to search for in chord descriptions e.g. maj, sus etc.</param>
        /// <returns>List of chords</returns>
        [HttpGet("Chords/{searchTerm}")]
        public IEnumerable<Chord> Chords(string searchTerm)
        {
            searchTerm = searchTerm.Trim();
            //return this.Chords().Where(chord => chord.Description.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase));
            return NoteSequenceUtilities.SearchDescriptions(this.chordData.Chords, searchTerm);
        }

        /// <summary>
        /// Retrieves all chords (with note values) for the specified root
        /// </summary>
        /// <param name="root">Root note value for chord e.g. C, f#, Ab etc.</param>
        /// <returns>List of chords</returns>
        [HttpGet("ChordNotes/{root}")]
        public IEnumerable<ChordInstance> ChordNotes(string root)
        {
            return this.ChordNotes(root, "Major");
        }

        /// <summary>
        /// Retrieves all chords (with note values) that match the search term(s) for the specified root
        /// </summary>
        /// <param name="root">Root note value for chord e.g. C, f#, Ab etc.</param>
        /// <param name="searchTerm">Text to search for in chord descriptions e.g. maj, sus etc.</param>
        /// <returns>List of chords</returns>
        [HttpGet("ChordNotes/{root}/{searchTerm}")]
        public IEnumerable<ChordInstance> ChordNotes(string root, string searchTerm)
        {
            searchTerm = searchTerm.Trim();
            var chordResults = NoteSequenceUtilities.SearchDescriptions(this.chordData.Chords, searchTerm);
            var chordArray = chordResults as Chord[] ?? chordResults.ToArray();
            var chordInstances = new List<ChordInstance>(chordArray.Length);

            foreach (var chord in chordArray)
            {
                var chordInstance = ChordInstance.Create(
                    chord.Description,
                    chord.Notes,
                    (List<string>)NoteSequenceUtilities.GetNotes(this.chordData, root, chord));

                chordInstances.Add(chordInstance);
            }

            return chordInstances;
        }

        /// <summary>
        /// Retrieves all scale data
        /// </summary>
        /// <returns>List of scales</returns>
        [HttpGet("Scales")]
        public IEnumerable<Scale> Scales()
        {
            return this.chordData.Scales;
        }

        /// <summary>
        /// Retrieves all chord data matching the  search term(s)
        /// </summary>
        /// <param name="searchTerm">Text to search for in scale descriptions e.g. mixolydian, phrygian etc.</param>
        /// <returns>List of chords</returns>
        [HttpGet("Scales/{searchTerm}")]
        public IEnumerable<Scale> Scales(string searchTerm)
        {
            searchTerm = searchTerm.Trim();
            //return this.chordData.Scales.Where(scale => scale.Description.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase));
            return NoteSequenceUtilities.SearchDescriptions(this.chordData.Scales, searchTerm);
        }

        /// <summary>
        /// Retrieves all scales (with note values) for the specified root
        /// </summary>
        /// <param name="root">Root note value for scale e.g. C, f#, Ab etc.</param>
        /// <returns>List of scales</returns>
        [HttpGet("ScaleNotes/{root}")]
        public IEnumerable<ScaleInstance> ScaleNotes(string root)
        {
            return this.ScaleNotes(root, "Major");
        }

        /// <summary>
        /// Retrieves all scales (with note values) that match the search term(s) for the specified root
        /// </summary>
        /// <param name="root">Root note value for scale e.g. C, f#, Ab etc.</param>
        /// <param name="searchTerm">Text to search for in scale descriptions e.g. mixolydian, phrygian etc.</param>
        /// <returns>List of scales</returns>
        [HttpGet("ScaleNotes/{root}/{searchTerm?}")]
        public IEnumerable<ScaleInstance> ScaleNotes(string root, string searchTerm)
        {
            searchTerm = searchTerm.Trim();

            var scaleResults = NoteSequenceUtilities.SearchDescriptions(this.chordData.Scales, searchTerm);
            var scaleArray = scaleResults as Scale[] ?? scaleResults.ToArray();
            var scaleInstances = new List<ScaleInstance>(scaleArray.Length);

            foreach (var scale in scaleArray)
            {
                var scaleInstance = ScaleInstance.Create(
                    scale.Description,
                    scale.Notes,
                    (List<string>)NoteSequenceUtilities.GetNotes(this.chordData, root, scale));

                scaleInstances.Add(scaleInstance);
            }

            return scaleInstances;
        }
    }
}