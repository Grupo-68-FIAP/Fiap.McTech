namespace Fiap.McTech.Domain.Entities
{
    /// <summary>
    /// Represents a base entity in the system.
    /// </summary>
    public abstract class EntityBase
    {
        /// <summary>
        /// Gets the unique identifier for the entity.
        /// </summary>
        public Guid Id { get; private set; } = Guid.NewGuid();

        /// <summary>
        /// Gets the date and time when the entity was created.
        /// </summary>
        public DateTime CreatedDate { get; private set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the date and time when the entity was last updated.
        /// </summary>
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Determines whether the entity is valid.
        /// </summary>
        /// <returns>True if the entity is valid; otherwise, false.</returns>
        public abstract bool IsValid();
    }
}
