namespace Tdd.FrameworkWrappers.Lib
{
    public interface ILogger
    {
        void Log(LogLevelEnum level, string message);
    }
}