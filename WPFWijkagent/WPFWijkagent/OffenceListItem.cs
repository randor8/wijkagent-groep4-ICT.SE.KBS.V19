using WijkagentModels;

namespace WijkagentWPF
{
    /// <summary>
    /// class for converting offences to the needed list items
    /// </summary>
    public class OffenceListItem
    {
        public Offence Offence { get; private set; }

        /// <summary>
        /// inits the offence list item so it can be used to display in a list
        /// </summary>
        /// <param name="offence"> the offence db item</param>
        public OffenceListItem(Offence offence)
        {
            Offence = offence;
        }

        /// <summary>
        /// creates a string representation of the object
        /// </summary>
        /// <returns> the string representation of the object</returns>
        public override string ToString() => $"{Offence.Description}, {Offence.DateTime}";
    }
}
