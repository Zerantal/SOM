using System;

namespace Util
{
    /// <summary>
    /// Attribute for marking classes or methods that are incomplete
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class ToDoAttribute : Attribute
    {
        private string message;
        
        public ToDoAttribute()
        {
            message = "No comment entered!";
        }

        /// <summary>
        /// Mark code file with a comment
        /// </summary>
        /// <param name="comment"></param>
        public ToDoAttribute(string comment)
        {
            message = comment;
        }

        /// <summary>
        /// Retrieve comment for TODO entry
        /// </summary>
        public string Comment
        {
            get { return message; }
        }
    }
}