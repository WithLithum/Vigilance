namespace Vigilance.Entities
{
    /// <summary>
    /// Represents the type of backup.
    /// </summary>
    public enum BackupType
    {
        /// <summary>
        /// Fire department. Only responds if a fire is happening.
        /// </summary>
        FireDepartment = 3,
        /// <summary>
        /// Ambulance. Will response in any condition but will be dismissed if there is not dead ped to rescue.
        /// </summary>
        Paramedics = 5,
        /// <summary>
        /// Police department. Will respond in any condition. From 2/3/2021, the police backup is customized.
        /// </summary>
        Police = 7
    }
}
