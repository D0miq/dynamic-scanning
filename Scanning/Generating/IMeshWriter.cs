﻿namespace Generating
{
    /// <summary>
    /// An instance of the <see cref="IMeshWriter"/> interface represents a writer.
    /// It should be able to write <paramref name="mesh"/> into some kind of output stream.
    /// </summary>
    /// <seealso cref="ObjMeshWriter"/>
    public interface IMeshWriter
    {
        /// <summary>
        /// Writes the given <paramref name="mesh"/> into output stream.
        /// </summary>
        /// <param name="mesh">The given mesh that is written into output stream.</param>
        /// <param name="fileName">Name of an output file.</param>
        void WriteMesh(Mesh mesh, string fileName);
    }
}
