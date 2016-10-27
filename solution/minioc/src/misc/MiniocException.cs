using System;

namespace minioc.misc {
public class MiniocException : Exception {
    public MiniocException(string message) : base(message) {
    }

    public MiniocException(string message, Exception innerException) : base(message, innerException) {
    }
}
}